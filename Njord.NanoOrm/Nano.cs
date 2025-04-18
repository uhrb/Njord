using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Njord.NanoOrm
{
    public class Nano : INano
    {
        private readonly string _connectionString;
        private readonly ConcurrentDictionary<string, string> _tables = [];
        private readonly ConcurrentDictionary<string, string> _selectors = [];
        private readonly ConcurrentDictionary<string, Func<SqliteDataReader, object?>[]> _propertySqlReaders = [];
        private readonly ConcurrentDictionary<string, Action<object, object?>[]> _propertySetters = [];
        private readonly ConcurrentDictionary<string, Func<object, object?>[]> _propertyReaders = [];
        private readonly ConcurrentDictionary<string, string[]> _properties = [];
        private readonly ConcurrentDictionary<string, string> _primaryKeys = [];

        private enum SqliteTypes
        {
            INTEGER,
            TEXT,
            BOOLEAN,
            DOUBLE
        }

        public Nano(IOptions<NanoOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public void MapTable<T>() where T : class
        {
            var typeInfo = typeof(T);
            var tableKey = typeInfo.Name;
            // add schema version?
            var sbSchema = new StringBuilder($"CREATE TABLE IF NOT EXISTS {tableKey} (");

            var array = typeInfo.GetRuntimeProperties().Where(_ => _.GetCustomAttribute<CompilerGeneratedAttribute>() == null).OrderBy(_ => _.Name).ToArray();
            var lstIndex = new List<PropertyInfo>();
            var lstPropertySqlReaders = new List<Func<SqliteDataReader, object?>>();
            var lstPropertyReaders = new List<Func<object, object?>>();
            var lstPropertySetters = new List<Action<object, object?>>();
            var lstProperties = new List<string>();

            var sbUpsertConflict = new StringBuilder();
            var sbIndexes = new StringBuilder();

            for (int i = 0; i < array.Length; i++)
            {
                var item = array[i];
                lstProperties.Add(item.Name);
                var (sqliteType, isNotNull) = GetSqliteType(item);
                var postfix = isNotNull ? " NOT NULL" : string.Empty;
                sbSchema.Append($"{item.Name} {sqliteType} {postfix}");
                var bi = i;
                lstPropertySqlReaders.Add((_) => ReadByTypeBased(_, bi, sqliteType));
                lstPropertySetters.Add((_, __) => SetByTypeBased(_, __, item, sqliteType));
                lstPropertyReaders.Add(item.GetValue);

                var isPrimaryKey = item.GetCustomAttribute<PrimaryKeyAttribute>() != null;
                var isUnique = item.GetCustomAttribute<UniqueAttribute>() != null;
                var isIndexed = item.GetCustomAttribute<IndexAttribute>() != null;

                if (isPrimaryKey)
                {
                    sbSchema.Append(" PRIMARY KEY");
                    _primaryKeys.AddOrUpdate(tableKey, item.Name, (_, __) => item.Name);
                }
                else
                {
                    sbUpsertConflict.Append($"{item.Name} = ##TEMPLATE_{item.Name}##");
                }
                if (isUnique)
                {
                    sbSchema.Append(" UNIQUE");
                }

                if (isIndexed)
                {
                    var middle = string.Empty;
                    if (isUnique)
                    {
                        middle = "UNIQUE";
                    }
                    sbIndexes.AppendLine($"CREATE {middle} INDEX IF NOT EXISTS idx_{typeInfo.Name}_{item.Name} ON {typeInfo.Name} ({item.Name});");
                }
                if (i < array.Length - 1)
                {
                    sbSchema.Append(',');
                    if (!isPrimaryKey)
                    {
                        sbUpsertConflict.Append(',');
                    }
                }
                else
                {
                    sbSchema.Append(");");
                }
            }

            var propertyList = string.Join(",", lstProperties.ToArray());
            var paramPropertyList = string.Join(",", lstProperties.Select(_ => $"@{_}"));
            var createSql = $"{sbSchema}\n{sbIndexes}";
            var selectSql = $"SELECT {propertyList} FROM {tableKey}";

            _tables.AddOrUpdate(tableKey, createSql, (_, __) => createSql);
            _selectors.AddOrUpdate(tableKey, selectSql, (_, __) => selectSql);

            var propSqlReaders = lstPropertySqlReaders.ToArray();
            var propSetters = lstPropertySetters.ToArray();
            var propReaders = lstPropertyReaders.ToArray();
            var properties = lstProperties.ToArray();
            _propertySqlReaders.AddOrUpdate(tableKey, propSqlReaders, (_, __) => propSqlReaders);
            _propertySetters.AddOrUpdate(tableKey, propSetters, (_, __) => propSetters);
            _propertyReaders.AddOrUpdate(tableKey, propReaders, (_, __) => propReaders);
            _properties.AddOrUpdate(tableKey, properties, (_, __) => properties);
        }

        public async Task EnsureTableAsync<T>() where T : class
        {
            var tableKey = typeof(T).Name;
            if (_tables.TryGetValue(tableKey, out string? creator))
            {
                using (var con = new SqliteConnection(_connectionString))
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = creator;
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        await con.CloseAsync();
                    }
                }
            }
        }

        public async IAsyncEnumerable<T> Query<T>(Expression<Func<T, bool>> whereClause)
        {
            var tableKey = typeof(T).Name;

            if (!_selectors.TryGetValue(tableKey, out var selector))
            {
                throw new KeyNotFoundException(tableKey);
            }

            if (!_propertySqlReaders.TryGetValue(tableKey, out var propertyReaders))
            {
                throw new KeyNotFoundException(tableKey);
            }

            if (!_propertySetters.TryGetValue(tableKey, out var propertySetters))
            {
                throw new KeyNotFoundException(tableKey);
            }

            var parameterNames = new List<string>();
            var evaluated = new Dictionary<string, object>();

            var sqlWhere = NanoHelpers.Convert(whereClause, parameterNames, evaluated, "query");
            sqlWhere = sqlWhere.Replace($"{parameterNames[0]}.", string.Empty);

            var sql = $"{selector} WHERE {sqlWhere}";

            using (var con = new SqliteConnection(_connectionString))
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    foreach (var param in evaluated)
                    {
                        cmd.Parameters.AddWithValue(param.Key, HandleDbTypedValue(param.Value));
                    }
                    await con.OpenAsync();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            yield return ReadRow<T>(reader, propertyReaders, propertySetters);
                        }
                    }
                    await con.CloseAsync();
                }
            }
        }

        /// <summary>
        /// Performs upsert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions">
        /// Each is expression to evaluate conflict case, first argument is oldValue, second is newValue
        /// </param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<T> UpsertAsync<T>(params FieldSetter<T>[] fieldSetters) where T : class
        {
            var tableKey = typeof(T).Name;
            if (fieldSetters.Length == 0)
            {
                throw new InvalidOperationException("Expressions for upsert cant be null");
            }
            if (!_propertySqlReaders.TryGetValue(tableKey, out var propertySqlReaders))
            {
                throw new KeyNotFoundException(tableKey);
            }
            if (!_propertySetters.TryGetValue(tableKey, out var propertySetters))
            {
                throw new KeyNotFoundException(tableKey);
            }
            if (!_properties.TryGetValue(tableKey, out var properties))
            {
                throw new KeyNotFoundException(tableKey);
            }
            if (!_primaryKeys.TryGetValue(tableKey, out var primaryKey))
            {
                throw new KeyNotFoundException(tableKey);
            }

            var dicFields = new Dictionary<string, object?>();
            var dicEvaluatedParams = new Dictionary<string, object>();
            var dicCaseClauses = new Dictionary<string, string>();

            for (int i = 0; i < fieldSetters.Length; i++)
            {
                var fieldSetter = fieldSetters[i];
                var prefix = $"p{i}_";
                var expressionParameterNames = new List<string>();
                var evaluatedParameters = new Dictionary<string, object>();
                var fieldName = fieldSetter.Name(null);
                var fieldValue = fieldSetter.Value;
                var sqlCaseClause = string.Empty;
                if (fieldSetter.OnConflict != null)
                {
                    sqlCaseClause = NanoHelpers.Convert(fieldSetter.OnConflict, expressionParameterNames, evaluatedParameters, prefix);
                    // first is always oldOne, second - newOne
                    var oldOneName = expressionParameterNames[0];
                    var newOneName = expressionParameterNames[1];
                    sqlCaseClause = sqlCaseClause.Replace($"{oldOneName}.", string.Empty).Replace($"{newOneName}.", "@");

                } 
                else
                {
                    // direct set
                    sqlCaseClause = $"@{fieldName}";
                }
                if (fieldName != primaryKey)
                {
                    dicCaseClauses.Add(fieldName, $"{fieldName} = {sqlCaseClause}");
                }

                dicFields.Add(fieldName, fieldValue);

                // adding more params with value
                foreach (var evaluatedParameter in evaluatedParameters)
                {
                    dicEvaluatedParams.Add(evaluatedParameter.Key, evaluatedParameter.Value);
                }

            }

            var fieldsNames = string.Join(",", dicFields.Keys);
            var fieldsParams = string.Join(",", dicFields.Keys.Select(_ => $"@{_}"));
            var fieldClauses = string.Join(",", dicCaseClauses.Values);
            var returning = string.Join(",", properties);

            var cmdInsert = $"INSERT INTO {tableKey} ({fieldsNames}) VALUES ({fieldsParams}) ON CONFLICT ({primaryKey}) DO UPDATE SET {fieldClauses} RETURNING {returning}";

            using (var con = new SqliteConnection(_connectionString))
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = cmdInsert;
                    foreach (var item in dicEvaluatedParams)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    foreach (var item in dicFields)
                    {
                        cmd.Parameters.AddWithValue($"@{item.Key}", item.Value ?? DBNull.Value);
                    }

                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        var vsl = ReadRow<T>(reader, propertySqlReaders, propertySetters);
                        await reader.CloseAsync();
                        await con.CloseAsync();
                        return vsl;
                    }
                }
            }

        }

        private static T ReadRow<T>(SqliteDataReader reader, Func<SqliteDataReader, object?>[] propertySqlReaders, Action<object, object?>[] propertySetters)
        {
            var type = typeof(T);
            T instance = (T)Activator.CreateInstance(type)!;
            for (int i = 0; i < propertySqlReaders.Length; i++)
            {
                var propReader = propertySqlReaders[i];
                var propSetter = propertySetters[i];
                var value = propReader(reader);
                propSetter(instance, value);
            }

            return instance;
        }



        private static object HandleDbTypedValue(object? value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            var type = value.GetType();

            return type switch
            {
                _ when type == typeof(DateTime) => ((DateTime)value).ToString(DateTimeFormatInfo.InvariantInfo.UniversalSortableDateTimePattern),
                _ when type == typeof(DateTime?) => ((DateTime?)value).Value.ToString(DateTimeFormatInfo.InvariantInfo.UniversalSortableDateTimePattern),
                _ when type.BaseType == typeof(Enum) => (int)value,
                _ => value
            };
        }

        private static object? ReadByTypeBased(SqliteDataReader r, int order, SqliteTypes type)
        {
            if (r.IsDBNull(order))
            {
                return null;
            }
            return type switch
            {
                SqliteTypes.INTEGER => r.GetInt32(order),
                SqliteTypes.TEXT => r.GetString(order),
                SqliteTypes.BOOLEAN => r.GetBoolean(order),
                SqliteTypes.DOUBLE => r.GetDouble(order),
                _ => throw new NotSupportedException(),
            };
        }

        private static void SetByTypeBased(object instance, object? value, PropertyInfo item, SqliteTypes sqliteType)
        {
            if (value == null)
            {
                item.SetValue(instance, value);
            }
            else
            {
                switch (sqliteType)
                {
                    case SqliteTypes.INTEGER:
                        var type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType;
                        if (type.IsEnum)
                        {
                            item.SetValue(instance, Enum.ToObject(type, (int)value));
                        }
                        else
                        {
                            item.SetValue(instance, value);
                        }
                        break;

                    case SqliteTypes.BOOLEAN:
                    case SqliteTypes.DOUBLE:
                        item.SetValue(instance, value);
                        break;

                    case SqliteTypes.TEXT:
                        if (item.PropertyType == typeof(DateTime?) || item.PropertyType == typeof(DateTime))
                        {
                            item.SetValue(instance, DateTime.Parse((string)value));
                        }
                        else
                        {
                            item.SetValue(instance, value);
                        }
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        private static (SqliteTypes, bool) GetSqliteType(PropertyInfo info)
        {
            var isNotNull = false;
            var realType = Nullable.GetUnderlyingType(info.PropertyType);
            if (realType == null)
            {
                realType = info.PropertyType;
                if (realType != typeof(string))
                {
                    isNotNull = true;
                }
            }

            SqliteTypes sqliteType = realType switch
            {
                _ when realType == typeof(int) => SqliteTypes.INTEGER,
                _ when realType == typeof(double) => SqliteTypes.DOUBLE,
                _ when realType == typeof(string) => SqliteTypes.TEXT,
                _ when realType == typeof(bool) => SqliteTypes.BOOLEAN,
                _ when realType == typeof(DateTime) => SqliteTypes.TEXT,
                _ when realType.BaseType == typeof(Enum) => SqliteTypes.INTEGER,
                _ => throw new NotSupportedException(info.PropertyType.FullName)
            };

            return (sqliteType, isNotNull);
        }
    }
}

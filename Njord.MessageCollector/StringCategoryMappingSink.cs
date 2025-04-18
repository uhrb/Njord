using DuckDB.NET.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Njord.Ais.MessageProcessing;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.RegularExpressions;

namespace Njord.MessageCollector
{
    public class StringCategoryMappingSink : IMessageSink<RawAisMessage>
    {
        private readonly string _connectionString;
        private readonly List<Regex> _extractors;
        private readonly bool _appendMode;
        private readonly string _tableName;
        private readonly string _inserter;
        private readonly string? _notMatchCategory;
        private readonly int _perCategoryLimit;
        private readonly bool _countInDatabase;
        private readonly ILogger<StringCategoryMappingSink> _logger;
        private readonly ConcurrentDictionary<string, long> _countPerCategory;
        private readonly Meter _meter;
        private readonly Counter<long> _processedCounter;
        private readonly Counter<long> _skippedCounter;

        public StringCategoryMappingSink(ILogger<StringCategoryMappingSink> logger, IOptions<StringCategoryMappingSinkOptions> options, IMeterFactory meterFactory)
        {
            _connectionString = string.IsNullOrEmpty(options.Value.ConnectionString) ? throw new ArgumentNullException(nameof(options.Value.ConnectionString)) : options.Value.ConnectionString;
            _appendMode = options.Value.AppendMode;
            _tableName = string.IsNullOrEmpty(options.Value.TableName) ? throw new ArgumentNullException(nameof(options.Value.TableName)) : options.Value.TableName;
            _inserter = $"INSERT INTO {_tableName}(TimeZ,Category,Value,Valid) VALUES(CURRENT_TIMESTAMP,$category,$value,TRUE)";
            _notMatchCategory = options.Value.NotMatchCategory;
            _perCategoryLimit = options.Value.PerCategoryLimit;
            _countInDatabase = options.Value.CountInDatabaseForAppend;
            _countPerCategory = [];
            if (options.Value.CategoryExtractors == null)
            {
                throw new ArgumentNullException(nameof(options.Value.CategoryExtractors));
            }
            else
            {
                _extractors = [];
                foreach (var extractor in options.Value.CategoryExtractors)
                {
                    _extractors.Add(new Regex(extractor));
                }
            }

            _logger = logger;
            var fName = typeof(StringCategoryMappingSink).FullName!;
            _meter = meterFactory.Create(fName, "v1.0");
            _processedCounter = _meter.CreateCounter<long>($"{fName}.messagesProcessed");
            _skippedCounter = _meter.CreateCounter<long>($"{fName}.skippedMessages");
        }

        public async Task InitAsync()
        {
            using (var con = new DuckDBConnection(_connectionString))
            {
                using (var cmd = con.CreateCommand())
                {
                    var sb = new StringBuilder();
                    if (!_appendMode)
                    {
                        sb.AppendLine($"DROP TABLE IF EXISTS {_tableName};");
                        sb.AppendLine($"DROP SEQUENCE IF EXISTS {_tableName}_sequence;");
                    }

                    sb.AppendLine($"CREATE SEQUENCE IF NOT EXISTS {_tableName}_sequence START WITH 1 INCREMENT BY 1;");
                    sb.AppendLine($"CREATE TABLE IF NOT EXISTS {_tableName}(Id INTEGER PRIMARY KEY default nextval('{_tableName}_sequence'), TimeZ TIMESTAMPTZ NOT NULL,Category STRING NOT NULL, Value JSON NOT NULL, Valid BOOLEAN);");
                    cmd.CommandText = sb.ToString();
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await con.CloseAsync();
                }
            }

            if (_appendMode && _countInDatabase)
            {
                using (var con = new DuckDBConnection(_connectionString))
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT DISTINCT Category, COUNT(Category) FROM {_tableName} GROUP BY Category";
                        await con.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var category = reader.GetString(0);
                                var count = reader.GetInt32(1);
                                _processedCounter.Add(count, [new("category", category)]);
                                _countPerCategory.AddOrUpdate(category, count, (k, v) => count);
                            }
                        }
                    }
                }
            }
        }

        public async Task PutAsync(RawAisMessage message, CancellationToken token)
        {
            var str = Encoding.UTF8.GetString(message.RawData.Span);
            var category = _notMatchCategory;
            foreach (var extractor in _extractors)
            {
                var match = extractor.Match(str);
                if (match.Success)
                {
                    category = match.Groups[1].Value;
                    break;
                }
            }

            if (string.IsNullOrEmpty(category))
            {
                if (!string.IsNullOrEmpty(_notMatchCategory))
                {
                    _logger.LogWarning("Category matching logic fail");
                }

                return;
            }

            if (_perCategoryLimit > 0)
            {
                if (_countPerCategory.TryGetValue(category, out long val))
                {
                    if (val >= _perCategoryLimit)
                    {
                        _skippedCounter.Add(1, [new("category", category)]);
                        return;
                    }
                }
            }

            using (var con = new DuckDBConnection(_connectionString))
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = _inserter;
                    cmd.Parameters.Add(new DuckDBParameter("category", category));
                    cmd.Parameters.Add(new DuckDBParameter("value", str));
                    await con.OpenAsync(token);
                    await cmd.ExecuteNonQueryAsync(token);
                    await con.CloseAsync();
                }
            }
            _countPerCategory.AddOrUpdate(category, 1, (k, v) => v + 1);
            _processedCounter.Add(1, [new("category", category)]);
        }
    }
}

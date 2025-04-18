using Microsoft.Extensions.Options;
using Moq;
using Njord.NanoOrm.Tests.Data;
using Njord.NanoOrm.Tests.Models;
using Xunit;
using Assert = Xunit.Assert;

namespace Njord.NanoOrm.Tests
{
    public class NanoOrmTests
    {
        [Theory]
        [ClassData(typeof(SimpleOrmModelSet))]
        public async Task UpsertWorkingInsert(OrmTestModel model)
        {
            var orm = await GetOrmAsync<OrmTestModel>();
            var result = await orm.UpsertAsync(GetOrmModelSetters(model));
            Assert.NotNull(result);
            Assert.Equivalent(model, result, true);
        }

        [Theory]
        [ClassData(typeof(UpsertUpdatesModelSet))]
        public async Task UpsertUpdates(OrmTestModel insert, OrmTestModel update, OrmTestModel expected)
        {
            var orm = await GetOrmAsync<OrmTestModel>();
            await orm.UpsertAsync(GetOrmModelSetters(insert));
            var result = await orm.UpsertAsync(GetOrmModelSetters(update));
            Assert.Equivalent(expected, result, true);  
        }

        private static FieldSetter<OrmTestModel>[] GetOrmModelSetters(OrmTestModel model)
        {
            return [
                new FieldSetter<OrmTestModel> {
                    Name = _ => nameof(_.Integer),
                    Value = model.Integer,
                    OnConflict = (oldOne, newOne) => newOne.Integer,
                },
                new FieldSetter<OrmTestModel> {
                    Name = _ => nameof(_.String),
                    Value = model.String,
                    OnConflict = (oldOne, newOne) => newOne.String ?? oldOne.String,
                },
                new FieldSetter<OrmTestModel> {
                    Name = _ => nameof(_.Double),
                    Value = model.Double,
                    OnConflict = (oldOne, newOne) => newOne.Double ?? oldOne.Double,
                },
                new FieldSetter<OrmTestModel> {
                    Name = _ => nameof(_.DateTime),
                    Value = model.DateTime,
                    OnConflict = (oldOne, newOne) => newOne.DateTime ?? oldOne.DateTime,
                },
                new FieldSetter<OrmTestModel> {
                    Name = _ => nameof(_.Enumeration),
                    Value = model.Enumeration,
                    OnConflict = (oldOne, newOne) => newOne.Enumeration ?? oldOne.Enumeration,
                }
            ];
        }

        private static async Task<INano> GetOrmAsync<T>() where T : class
        {
            var fName = Path.GetTempFileName();
            var moqOptions = new Mock<IOptions<NanoOptions>>();
            moqOptions.Setup(_ => _.Value).Returns(new NanoOptions { ConnectionString = $"Data Source={fName}" });
            var orm = new Nano(moqOptions.Object);
            orm.MapTable<T>();
            await orm.EnsureTableAsync<T>();
            return orm;
        }

        private static void AssertType<T>(T vsl, Action<object?> defaultAction, params (string, Action)[] except)
        {
            Assert.NotNull(vsl);
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var excepts = except.Where(_ => _.Item1 == property.Name).ToArray();
                if (excepts.Length == 0)
                {
                    var value = property.GetValue(vsl);
                    defaultAction(value);
                }
                else
                {
                    foreach (var exceptClause in excepts)
                    {
                        exceptClause.Item2();
                    }

                }
            }
        }
    }
}
using Njord.NanoOrm.Tests.Models;
using Xunit;
using Assert = Xunit.Assert;

namespace Njord.NanoOrm.Tests
{

    public class NanoOrmHelpersTests
    {
        [Fact]
        public void HandlesBasicSelect()
        {
            var parameters = new Dictionary<string, object>();
            var names = new List<string>();
            var sql = NanoHelpers.Convert((OrmHelpersTestModel _) => _.Date >= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)), names, parameters, "p");
            Assert.NotEmpty(parameters);
            Assert.Equal("(_.Date >= @p1)", sql);
        }

        [Fact]
        public void ReplaceLogicCorrect()
        {
            var parameters = new Dictionary<string, object>();
            var names = new List<string>();
            var sql = NanoHelpers.Convert((OrmHelpersTestModel _) => _.Date >= DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)), names, parameters, "p");
            Assert.Single(parameters);
            Assert.Single(names);
            Assert.Equal("(_.Date >= @p1)", sql);
            var newSql = sql.Replace($"{names[0]}.", string.Empty);
            Assert.Equal("(Date >= @p1)", newSql);
        }

        [Fact]
        public void HandleNullNotNull()
        {
            var parameters = new Dictionary<string, object>();
            var names = new List<string>();
            var sql = NanoHelpers.Convert((OrmHelpersTestModel _) => _.Date != null && _.Date == null, names, parameters, "p");
            Assert.NotEmpty(parameters);
            Assert.Equal("(_.Date IS NOT NULL AND _.Date IS NULL)", sql);
        }

        [Fact()]
        public void TernaryOperatorTest()
        {
            var parameters = new Dictionary<string, object>();
            var names = new List<string>();
            var sql = NanoHelpers.Convert((OrmHelpersTestModel oldOne, OrmHelpersTestModel newOne) => newOne.String ?? oldOne.String, names, parameters, string.Empty);
            Assert.Equal("COALESCE(newOne.String, oldOne.String)", sql);
        }
    }
}
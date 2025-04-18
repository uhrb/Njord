using Njord.NanoOrm.Tests.Models;
using System.Collections;

namespace Njord.NanoOrm.Tests.Data
{
    internal class UpsertUpdatesModelSet : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {

            var utcOne = DateTime.UtcNow;
            // nulls
            yield return [
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
            ];

            // direct update when no value
            yield return [
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = utcOne, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = utcOne, Double = null, Enumeration = null, String = null },
            ];
            yield return [
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = 1.5, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = 1.5, Enumeration = null, String = null },
            ];
            yield return [
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = OrmTestModelEnum.Four, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = OrmTestModelEnum.Four, String = null },
            ];
            var stringOne = Guid.NewGuid().ToString();
            yield return [
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = stringOne },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = stringOne },
            ];

            // update with saving value
            yield return [
                new OrmTestModel { Integer = 1, DateTime = utcOne, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = utcOne, Double = null, Enumeration = null, String = null },
            ];
            yield return [
                new OrmTestModel { Integer = 1, DateTime = null, Double = 1.8, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = 1.8, Enumeration = null, String = null },
            ];
            yield return [
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = OrmTestModelEnum.Three, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = OrmTestModelEnum.Three, String = null },
            ];
            yield return [
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = stringOne },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = null },
                new OrmTestModel { Integer = 1, DateTime = null, Double = null, Enumeration = null, String = stringOne },
            ];
            // random updates
            var stringTwo = Guid.NewGuid().ToString();
            var dateTwo = DateTime.UtcNow.AddDays(1);
            yield return [
                new OrmTestModel { Integer = 1, DateTime = utcOne, Double = 1.6, Enumeration = OrmTestModelEnum.One, String = stringOne },
                new OrmTestModel { Integer = 1, DateTime = dateTwo, Double = 3.56, Enumeration = null, String = stringTwo },
                new OrmTestModel { Integer = 1, DateTime = dateTwo, Double = 3.56, Enumeration = OrmTestModelEnum.One, String = stringTwo },
            ];


        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
using Njord.NanoOrm.Tests.Models;
using System.Collections;

namespace Njord.NanoOrm.Tests.Data
{
    internal class SimpleOrmModelSet : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [new OrmTestModel { DateTime = DateTime.UtcNow, Double = 3.0, Enumeration = OrmTestModelEnum.One, Integer = 1, String = Guid.NewGuid().ToString() }];
            yield return [new OrmTestModel { DateTime = null, Double = 0.2, Enumeration = OrmTestModelEnum.Two, Integer = 234234, String = Guid.NewGuid().ToString() }];
            yield return [new OrmTestModel { DateTime = DateTime.UtcNow, Double = null, Enumeration = OrmTestModelEnum.Three, Integer = 1, String = Guid.NewGuid().ToString() }];
            yield return [new OrmTestModel { DateTime = DateTime.UtcNow, Double = 0.55, Enumeration = null, Integer = 1, String = Guid.NewGuid().ToString() }];
            yield return [new OrmTestModel { DateTime = DateTime.UtcNow, Double = 16.25, Enumeration = OrmTestModelEnum.Four, Integer = 1, String = null }];
            yield return [new OrmTestModel { DateTime = null, Double = null, Enumeration = null, Integer = 1, String = Guid.NewGuid().ToString() }];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
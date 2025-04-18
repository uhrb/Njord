namespace Njord.NanoOrm.Tests.Models
{
    public record OrmTestModel
    {
        [PrimaryKey, Unique, Index]
        public int Integer { get; set; }
        public string? String { get; set; }
        public double? Double { get; set; }
        public DateTime? DateTime { get; set; }
        public OrmTestModelEnum? Enumeration { get; set; }
    }

    public enum OrmTestModelEnum : int
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }
}

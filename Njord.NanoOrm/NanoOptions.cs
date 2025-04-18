namespace Njord.NanoOrm
{
    public record NanoOptions
    {
        public required string ConnectionString { get; init; }
    }
}

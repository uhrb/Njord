namespace Njord.MessageCollector
{
    public record StringCategoryMappingSinkOptions
    {
        public required string ConnectionString { get; init; }
        public required IEnumerable<string> CategoryExtractors { get; init; }
        public required bool AppendMode { get; init; }
        public required string TableName { get; init; }
        public required string? NotMatchCategory { get; init; }
        public required int PerCategoryLimit { get; init; }
        public required bool CountInDatabaseForAppend { get; init; }
    }
}

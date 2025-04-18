namespace Njord.NanoOrm
{
    internal record ConversionState
    {
        public required Dictionary<string, object> EvaluatedParameters { get; init; }
        public required List<string> ExpressionParameters { get; init; }
        public required string EvaluatePrefix { get; init; }
    }
}

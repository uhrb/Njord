using System.Linq.Expressions;

namespace Njord.NanoOrm
{
    public record FieldSetter<T>
    {
        public required Func<T?, string> Name { get; init; }
        public required object? Value { get; init; }
        public required Expression<Func<T, T, object?>>? OnConflict { get; init; }
    }
}

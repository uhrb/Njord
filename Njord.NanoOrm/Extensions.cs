using System.Linq.Expressions;

namespace Njord.NanoOrm
{
    public static class Extensions
    {
        public static UpsertBuilder<T> CreateUpsert<T>(this INano orm) where T : class
        {
            return new UpsertBuilder<T>(orm);
        }

        public static UpsertBuilder<T> WithPrimaryKey<T>(this UpsertBuilder<T> builder, Func<T?, string> name, object value) where T : class
        {
            builder.List.Add(new FieldSetter<T> { Name = name, Value = value, OnConflict = null });
            return builder;
        }

        public static UpsertBuilder<T> WithColumnConflict<T>(this UpsertBuilder<T> builder, Func<T?, string> name, object? value, Expression<Func<T, T, object?>> onConflict) where T : class
        {
            builder.List.Add(new FieldSetter<T> { Name = name, Value =value, OnConflict = onConflict });
            return builder;
        }

        public static UpsertBuilder<T> WithColumnSet<T>(this UpsertBuilder<T> builder, Func<T?, string> name, object? value) where T : class
        {
            // need somethign to overwrite
            builder.List.Add(new FieldSetter<T> { Name = name, Value = value, OnConflict = null });
            return builder;
        }


        public static async Task<T> ExecuteAsync<T>(this UpsertBuilder<T> builder) where T : class
        {
            return await builder.NanoOrm.UpsertAsync<T>([.. builder.List]);
        }
    }
}

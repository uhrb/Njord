using System.Linq.Expressions;

namespace Njord.NanoOrm
{
    public interface INano
    {
        Task EnsureTableAsync<T>() where T : class;
        void MapTable<T>() where T : class;
        IAsyncEnumerable<T> Query<T>(Expression<Func<T, bool>> expression);
        Task<T> UpsertAsync<T>(params FieldSetter<T>[] fieldSetters) where T : class;
    }
}
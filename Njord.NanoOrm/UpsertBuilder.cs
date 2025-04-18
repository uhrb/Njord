namespace Njord.NanoOrm
{
    public record UpsertBuilder<T>
    {
        internal IList<FieldSetter<T>> List { get; init; }
        internal INano NanoOrm { get; init; }

        internal UpsertBuilder(INano orm)
        {
            List = [];
            NanoOrm = orm;
        }
    }
}

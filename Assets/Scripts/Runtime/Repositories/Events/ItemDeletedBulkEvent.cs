namespace Obert.Common.Runtime.Repositories.Events
{
    public sealed class ItemDeletedBulkEvent<TData>
    {
        public ItemDeletedBulkEvent(int startingIndex, TData[] items)
        {
            StartingIndex = startingIndex;
            Items = items;
        }

        public int StartingIndex { get; }
        public TData[] Items { get; }
    }
}
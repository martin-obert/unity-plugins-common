namespace Obert.Common.Runtime.Repositories.Events
{
    public sealed class ItemCreatedBulkEvent<TData>
    {
        public ItemCreatedBulkEvent(int startingStartingIndex, TData[] items)
        {
            StartingIndex = startingStartingIndex;
            Items = items;
        }

        public int StartingIndex { get; }
        public TData[] Items { get; }
    }
}
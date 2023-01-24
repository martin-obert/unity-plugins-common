using System;

namespace Obert.Common.Runtime.Repositories.Events
{
    [Serializable]
    public sealed class ItemMovedEvent<TData>
    {
        public ItemMovedEvent(int oldIndex, int newIndex, TData item)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            Item = item;
        }

        public int OldIndex { get; }
        public int NewIndex { get; }
        public TData Item { get; }
    }
}
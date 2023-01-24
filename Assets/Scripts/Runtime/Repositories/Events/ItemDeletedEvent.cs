using System;

namespace Obert.Common.Runtime.Repositories.Events
{
    [Serializable]
    public sealed class ItemDeletedEvent<TData>
    {
        public ItemDeletedEvent(int index, TData item)
        {
            Index = index;
            Item = item;
        }

        public int Index { get; }
        public TData Item { get; }
    }
}
using System;

namespace Obert.Common.Runtime.Repositories.Events
{
    [Serializable]
    public sealed class ItemUpdatedEvent<TData>
    {
        public ItemUpdatedEvent(int index, TData item)
        {
            Index = index;
            Item = item;
        }

        public int Index { get; }
        public TData Item { get; }
    }
}
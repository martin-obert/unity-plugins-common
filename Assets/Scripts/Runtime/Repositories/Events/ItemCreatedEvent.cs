using System;

namespace Obert.Common.Runtime.Repositories.Events
{
    [Serializable]
    public sealed class ItemCreatedEvent<TData>
    {
        public ItemCreatedEvent(int index, TData item)
        {
            Index = index;
            Item = item;
        }

        public int Index { get; }
        public TData Item { get; }
    }
}
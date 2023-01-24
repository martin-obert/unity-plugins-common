using System;

namespace Obert.Common.Runtime.Repositories.Events
{
    public interface IObservableRepository<TData>
    {
        IObservable<ItemCreatedEvent<TData>> ItemCreated { get; }
        IObservable<ItemCreatedBulkEvent<TData>> BulkCreated { get; }
        IObservable<ItemDeletedBulkEvent<TData>> BulkDeleted { get; }
        IObservable<ItemMovedEvent<TData>> ItemMoved { get; }
        IObservable<ItemUpdatedEvent<TData>> ItemUpdated { get; }
        IObservable<ItemDeletedEvent<TData>> ItemDeleted { get; }
    }
}
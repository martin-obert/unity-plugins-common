using System;

namespace Obert.Common.Runtime.Repositories
{
    public interface IRepositoryBase : IReadOnlyRepositoryBase
    {
        
    }
    
    public interface IRepository<TData> : IReadOnlyRepository<TData>, IRepositoryBase, IDisposable
    {
        void AddSingle(TData item, bool allowDuplicates = false);
        void UpdateSingle(TData item);
        void DeleteSingle(TData item);

        void Save();
    }
}
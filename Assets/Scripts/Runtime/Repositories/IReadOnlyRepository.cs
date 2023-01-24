using System;
using System.Collections.Generic;

namespace Obert.Common.Runtime.Repositories
{
    public interface IReadOnlyRepositoryBase
    {
        
    }
    
    public interface IReadOnlyRepository<out TData> : IReadOnlyRepositoryBase
    {
        TData FirstOrDefault(Func<TData, bool> search);
        IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = int.MaxValue, int skip = 0);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories
{
    public class ScriptableRepositoryBase : ScriptableObject
    {
        
    }
    
    public abstract class ScriptableObjectsRepository<TData> : ScriptableRepositoryBase, IReadOnlyRepository<TData>
    {
        [SerializeField] private TData[] items;

        public TData FirstOrDefault(Func<TData, bool> search) => items.FirstOrDefault(search);

        public IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = int.MaxValue, int skip = 0)
        {
            var predicate = search ?? (_ => true);
            
            return items.Where(predicate).Skip(skip).Take(limit);
        }

    }
}
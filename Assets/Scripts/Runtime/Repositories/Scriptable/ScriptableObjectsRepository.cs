using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories.Scriptable
{
    public abstract class ScriptableObjectsRepository<TData> : ScriptableRepositoryBase, IReadOnlyRepository<TData>
    {
        [SerializeField] private TData[] items;

        public TData FirstOrDefault(Func<TData, bool> search = null) => items.FirstOrDefault(search ?? (_ => true));

        public IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = int.MaxValue, int skip = 0)
        {
            var predicate = search ?? (_ => true);

            return items.Where(predicate).Skip(skip).Take(limit);
        }

        public int Count() => items.Length;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Obert.Common.Runtime.Repositories.Scriptable;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories.Components
{
    public abstract class ScriptableRepository<TData> : ScriptableRepositoryBase, IReadOnlyRepository<TData>
    {
        [SerializeField] private TData[] data;

        public TData FirstOrDefault(Func<TData, bool> search) => data.FirstOrDefault(search);

        public IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = Int32.MaxValue, int skip = 0) =>
            data.Where(search ?? (_ => true)).Skip(skip).Take(limit);

        public int Count() => data.Length;
    }
}
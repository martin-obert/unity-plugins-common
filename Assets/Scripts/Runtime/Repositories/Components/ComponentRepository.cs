﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories.Components
{
    public abstract class ComponentRepository<TData> : ReadOnlyMonoRepositoryBase, IReadOnlyRepository<TData>
    {
        [SerializeField] private TData[] items;

        protected virtual IEnumerable<TData> Items => items;
        
        protected virtual void Awake()
        {
            items ??= Array.Empty<TData>();
        }

        public virtual TData FirstOrDefault(Func<TData, bool> search = null) => Items.FirstOrDefault(search ?? (_ => true));

        public IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = int.MaxValue, int skip = 0)
        {
            var predicate = search ?? (_ => true);
            return Items.Where(predicate).Skip(skip).Take(limit);
        }

        public override int Count() => items.Length;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obert.Common.Runtime.Repositories.Components
{
    public sealed class InMemoryRepository<TData> : IRepository<TData>
    {
        private readonly IList<TData> _data;

        public InMemoryRepository(TData[] data = null)
        {
            _data = new List<TData>(data ?? Array.Empty<TData>());
        }

        public TData FirstOrDefault(Func<TData, bool> search = null) => _data.FirstOrDefault(search ?? (_ => true));

        public IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = int.MaxValue, int skip = 0) =>
            _data.Where(search ?? (_ => true)).Skip(skip).Take(limit);

        public void Dispose()
        {
            ClearAll();
        }

        public void AddSingle(TData item, bool allowDuplicates = false)
        {
            if (allowDuplicates)
            {
                _data.Add(item);
                return;
            }

            if (_data.Contains(item)) return;
            _data.Add(item);
        }

        public void UpdateSingle(TData item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            
            for (var i = 0; i < _data.Count; i++)
            {
                var data = _data[i];
                if(!item.Equals(data))continue;
                _data[i] = item;
            }
        }

        public void DeleteSingle(TData item)
        {
            _data.Remove(item);
        }

        public void ClearAll()
        {
            _data.Clear();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public int Count() => _data.Count;
    }
}
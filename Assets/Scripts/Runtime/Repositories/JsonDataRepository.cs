using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Obert.Common.Runtime.Repositories
{
    public class JsonDataRepository<TData> : IRepository<TData>
    {
        public DataLoadStrategy LoadStrategy { get; protected set; } = DataLoadStrategy.Once;

        private readonly string _fileName;
        private readonly string _filePath;

        public JsonDataRepository(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }

        protected string FullFilePath => Path.Combine(_filePath, _fileName);

        private readonly List<TData> _items = new();

        private void InitData()
        {
            if (LoadStrategy == DataLoadStrategy.Once && _items.Any()) return;

            LoadItems();
        }

        protected virtual void LoadItems()
        {
            if (!File.Exists(FullFilePath))
            {
                return;
            }

            var text = File.ReadAllText(FullFilePath);

            _items.AddRange(Deserialize(text));
        }

        protected virtual IEnumerable<TData> Deserialize(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? Array.Empty<TData>() : JsonConvert.DeserializeObject<TData[]>(input);
        }

        protected virtual string Serialize(TData[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            return JsonConvert.SerializeObject(items);
        }


        public TData FirstOrDefault(Func<TData, bool> search)
        {
            InitData();
            return _items.FirstOrDefault(search);
        }

        public IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = int.MaxValue, int skip = 0)
        {
            InitData();
            var predicate = search ?? (_ => true);

            return _items.Where(predicate).Skip(skip).Take(limit);
        }

        public void AddSingle(TData item, bool allowDuplicates = false)
        {
            InitData();
            if (!allowDuplicates && _items.Contains(item)) return;
            _items.Add(item);
        }

        public void UpdateSingle(TData item)
        {
            InitData();
            var index = _items.IndexOf(item);
            if (index < 0)
                return;
            _items[index] = item;
        }

        public void DeleteSingle(TData item)
        {
            InitData();
            _items.Remove(item);
        }

        public void Save()
        {
            var text = Serialize(_items.ToArray());
            File.WriteAllText(FullFilePath, text);
        }

        public void Dispose()
        {
            Save();
        }
    }
}
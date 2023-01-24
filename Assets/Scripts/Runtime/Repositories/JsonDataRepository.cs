using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Obert.Common.Runtime.Repositories.Events;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories
{
    public class JsonDataRepository<TData> : IRepository<TData>, IObservableRepository<TData>
    {
        private readonly string _fileName;
        private readonly string _filePath;
        private readonly ObservableItem<ItemCreatedEvent<TData>> _itemCreated = new();
        private readonly ObservableItem<ItemMovedEvent<TData>> _itemMoved = new();
        private readonly ObservableItem<ItemUpdatedEvent<TData>> _itemUpdated = new();
        private readonly ObservableItem<ItemDeletedEvent<TData>> _itemDeleted = new();
        private readonly ObservableItem<ItemDeletedBulkEvent<TData>> _itemDeletedBulk = new();
        private readonly ObservableItem<ItemCreatedBulkEvent<TData>> _itemCreatedBulk = new();

        public DataLoadStrategy LoadStrategy { get; protected set; } = DataLoadStrategy.Once;

        public IObservable<ItemCreatedEvent<TData>> ItemCreated => _itemCreated;
        public IObservable<ItemCreatedBulkEvent<TData>> BulkCreated => _itemCreatedBulk;
        public IObservable<ItemDeletedBulkEvent<TData>> BulkDeleted => _itemDeletedBulk;
        public IObservable<ItemMovedEvent<TData>> ItemMoved => _itemMoved;
        public IObservable<ItemUpdatedEvent<TData>> ItemUpdated => _itemUpdated;
        public IObservable<ItemDeletedEvent<TData>> ItemDeleted => _itemDeleted;

        public JsonDataRepository(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }

        protected string FullFilePath => Path.Combine(_filePath, _fileName);

        private readonly List<TData> _items = new();

        public void InitData()
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

            for (var index = 0; index < _items.Count; index++)
            {
                var item = _items[index];
                _itemCreated.OnNext(new ItemCreatedEvent<TData>(index, item));
            }
        }

        protected virtual IEnumerable<TData> Deserialize(string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? Array.Empty<TData>()
                : JsonConvert.DeserializeObject<TData[]>(input);
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
            _itemCreated?.OnNext(new ItemCreatedEvent<TData>(_items.Count - 1, item));
        }

        public void UpdateSingle(TData item)
        {
            InitData();
            var index = _items.IndexOf(item);
            if (index < 0)
                return;
            _items[index] = item;
            _itemUpdated?.OnNext(new ItemUpdatedEvent<TData>(index, item));
        }

        public void DeleteSingle(TData item)
        {
            InitData();
            var index = _items.IndexOf(item);
            if (index < 0) return;
            _items.RemoveAt(index);
            _itemDeleted?.OnNext(new ItemDeletedEvent<TData>(index, item));
        }

        public void ClearAll()
        {
            if(!_items.Any()) return;
            var array = _items.ToArray();
            
            _items.Clear();
            _itemDeletedBulk.OnNext(new ItemDeletedBulkEvent<TData>(0, array));
        }

        public void Save()
        {
            var text = Serialize(_items.ToArray());
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }

            Debug.Log($"{nameof(TData)} saved at: {FullFilePath}");
            File.WriteAllText(FullFilePath, text);
        }

        public void Dispose()
        {
            _itemCreated?.OnCompleted();
            _itemCreated?.Dispose();
            
            _itemMoved?.OnCompleted();
            _itemMoved?.Dispose();
            
            _itemDeleted?.OnCompleted();
            _itemDeleted?.Dispose();

            _itemCreatedBulk?.OnCompleted();
            _itemCreatedBulk?.Dispose();
            
            _itemUpdated?.OnCompleted();
            _itemUpdated?.Dispose();
        
            _itemDeletedBulk?.OnCompleted();
            _itemDeletedBulk?.Dispose();
        }
    }
}
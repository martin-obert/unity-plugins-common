using System;
using System.Collections.Generic;
using System.IO;
using Obert.Common.Runtime.Repositories.Events;
using Obert.Common.Runtime.Repositories.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Obert.Common.Runtime.Repositories.Components
{
    public abstract class JsonComponentRepository<TData> : MonoRepositoryBase, IRepository<TData>
    {
        public enum DataInitStrategy
        {
            Manual = 0,
            OnAwake = 1,
            OnStart = 2
        }

        [Header("Settings")] [SerializeField] private bool saveOnDisable;

        [SerializeField] private string jsonFileName;

        [SerializeField, Tooltip("Relative to - Application.persistentDataPath")]
        private string relativePath;

        [SerializeField] private DataInitStrategy initStrategy;

        private JsonDataRepository<TData> _repository;

        [Header("Events"), Space(10)] [Header("Single Item")]
        public UnityEvent<ItemCreatedEvent<TData>> onItemCreated;

        public UnityEvent<ItemDeletedEvent<TData>> onItemDeleted;
        public UnityEvent<ItemUpdatedEvent<TData>> onItemUpdated;

        [Header("Bulk")] public UnityEvent<ItemCreatedBulkEvent<TData>> onItemCreatedBulk;
        public UnityEvent<ItemDeletedBulkEvent<TData>> onItemDeletedBulk;


        private readonly DisposableContainer _disposableContainer = new();


        private void Awake()
        {
            var filePath = Path.Combine(Application.persistentDataPath, relativePath);

            _repository = new JsonDataRepository<TData>(new FileProvider(jsonFileName, filePath));

            Subscribe();

            if (initStrategy == DataInitStrategy.OnAwake)
                LoadData();
        }

        private void Subscribe()
        {
            _repository.ItemCreated.Subscribe(new ItemObserver<ItemCreatedEvent<TData>>
            {
                Next = e => onItemCreated?.Invoke(e)
            }).AddTo(_disposableContainer);

            _repository.BulkCreated.Subscribe(new ItemObserver<ItemCreatedBulkEvent<TData>>
            {
                Next = e => onItemCreatedBulk?.Invoke(e)
            }).AddTo(_disposableContainer);

            _repository.ItemDeleted.Subscribe(new ItemObserver<ItemDeletedEvent<TData>>
            {
                Next = e => onItemDeleted?.Invoke(e)
            }).AddTo(_disposableContainer);

            _repository.BulkDeleted.Subscribe(new ItemObserver<ItemDeletedBulkEvent<TData>>
            {
                Next = e => onItemDeletedBulk?.Invoke(e)
            }).AddTo(_disposableContainer);

            _repository.ItemUpdated.Subscribe(new ItemObserver<ItemUpdatedEvent<TData>>
            {
                Next = e => onItemUpdated?.Invoke(e)
            }).AddTo(_disposableContainer);
        }

        private void Start()
        {
            if (initStrategy == DataInitStrategy.OnStart)
                LoadData();
        }

        public void LoadData()
        {
            _repository.InitData();
        }

        private void OnDestroy()
        {
            Dispose();

            onItemCreated?.RemoveAllListeners();
            onItemCreatedBulk?.RemoveAllListeners();

            onItemDeleted?.RemoveAllListeners();
            onItemDeletedBulk?.RemoveAllListeners();

            _disposableContainer.Dispose();
        }

        private void OnDisable()
        {
            if (saveOnDisable)
                _repository.Save();
        }

        public TData FirstOrDefault(Func<TData, bool> search = null)
            => _repository.FirstOrDefault(search ?? (_ => true));

        public IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = Int32.MaxValue, int skip = 0)
            => _repository.Many(search, limit, skip);

        public void Dispose() => _repository.Dispose();

        public void AddSingle(TData item, bool allowDuplicates = false)
            => _repository.AddSingle(item, allowDuplicates);

        public void UpdateSingle(TData item)
            => _repository.UpdateSingle(item);

        public void DeleteSingle(TData item)
            => _repository.DeleteSingle(item);

        public void ClearAll()
            => _repository.ClearAll();

        public void Save()
            => _repository.Save();

        public override int Count() => _repository.Count();
    }
}
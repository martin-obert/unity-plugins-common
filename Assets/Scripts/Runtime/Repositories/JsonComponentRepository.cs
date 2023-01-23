using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories
{
    public abstract class JsonComponentRepository<TData> : MonoRepositoryBase, IRepository<TData>
    {
        [SerializeField] private string jsonFileName;

        [SerializeField, Tooltip("Relative to - Application.streamingAssetsPath")]
        private string relativePath;

        private JsonDataRepository<TData> _repository;

        private void Awake()
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, relativePath);
            _repository = new JsonDataRepository<TData>(jsonFileName, filePath);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public TData FirstOrDefault(Func<TData, bool> search)
            => _repository.FirstOrDefault(search);

        public IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = Int32.MaxValue, int skip = 0)
            => _repository.Many(search, limit, skip);

        public void Dispose() => _repository.Dispose();

        public void AddSingle(TData item, bool allowDuplicates = false)
            => _repository.AddSingle(item, allowDuplicates);

        public void UpdateSingle(TData item)
            => _repository.UpdateSingle(item);

        public void DeleteSingle(TData item)
            => _repository.DeleteSingle(item);

        public void Save() 
            => _repository.Save();
    }
}
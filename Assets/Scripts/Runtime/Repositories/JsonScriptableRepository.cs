using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories
{
    public abstract class JsonScriptableRepository<TData> : ScriptableRepositoryBase, IRepository<TData>
    {
        [SerializeField] private string jsonFileName;

        [SerializeField, Tooltip("Relative to - Application.streamingAssetsPath")]
        private string relativePath;
        
        private JsonDataRepository<TData> _repository;

        private IRepository<TData> Repository
        {
            get
            {
                if (_repository != null) return _repository;
                
                var filePath = Path.Combine(Application.streamingAssetsPath, relativePath);
                
                _repository = new JsonDataRepository<TData>(jsonFileName, filePath);

                return _repository;
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public TData FirstOrDefault(Func<TData, bool> search)
            => Repository.FirstOrDefault(search);

        public IEnumerable<TData> Many(Func<TData, bool> search = null, int limit = Int32.MaxValue, int skip = 0)
            => Repository.Many(search, limit, skip);

        public void Dispose() => Repository?.Dispose();

        public void AddSingle(TData item, bool allowDuplicates = false)
            => Repository.AddSingle(item, allowDuplicates);

        public void UpdateSingle(TData item)
            => Repository.UpdateSingle(item);

        public void DeleteSingle(TData item)
            => Repository.DeleteSingle(item);

        public void Save() 
            => Repository.Save();
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories.Scriptable
{
    public abstract class JsonScriptableRepository<TData> : ScriptableRepositoryBase, IRepository<TData>
    {
        [SerializeField] private string jsonFileName;

        [SerializeField, Tooltip("Relative to - Application.persistentDataPath")]
        private string relativePath;

        private JsonDataRepository<TData> _repository;

        [SerializeField] private bool saveOnDestroy;

        private IRepository<TData> Repository
        {
            get
            {
                if (_repository != null) return _repository;

                var filePath = Path.Combine(Application.persistentDataPath, relativePath);

                _repository = new JsonDataRepository<TData>(new FileProvider(jsonFileName, filePath));

                return _repository;
            }
        }

        private void OnDestroy()
        {
            if (saveOnDestroy)
            {
                _repository.Save();
            }

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

        public void ClearAll() => Repository.ClearAll();

        public void Save()
            => Repository.Save();

        public int Count() => Repository.Count();
    }
}
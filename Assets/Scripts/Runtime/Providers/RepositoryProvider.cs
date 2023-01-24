using System;
using System.Linq;
using Obert.Common.Runtime.Extensions;
using Obert.Common.Runtime.Repositories;
using Obert.Common.Runtime.Repositories.Components;
using UnityEngine;

namespace Obert.Common.Runtime.Providers
{
    public class RepositoryProvider : MonoBehaviour
    {
        [SerializeField] private ScriptableObject[] scriptableRepositories;
        private MonoRepositoryBase[] _monoRepositories;
        private ReadOnlyMonoRepositoryBase[] _readOnlyMonoRepositories;

        private IRepositoryBase[] Repositories { get; set; }
        private IReadOnlyRepositoryBase[] ReadOnlyRepositories { get; set; }

        public static RepositoryProvider Instance { get; private set; }

        public IRepository<TData> ProvideRepositoryFor<TData>()
        {
            var repository = Repositories.OfType<IRepository<TData>>().FirstOrDefault();
            return repository;
        }

        public IReadOnlyRepository<TData> ProvideReadOnlyRepositoryFor<TData>()
        {
            var repository = ReadOnlyRepositories.OfType<IReadOnlyRepository<TData>>().FirstOrDefault();
            return repository;
        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (!ReferenceEquals(Instance, this))
                {
                    Destroy(this);
                    return;
                }
            }

            var readOnlySelf = this.GetInterfacesOfType<IReadOnlyRepositoryBase>();
            var readOnlyChildren = transform.GetChildrenInterfacesOfType<IReadOnlyRepositoryBase>();

            ReadOnlyRepositories = readOnlySelf.Union(readOnlyChildren).ToArray();
            Repositories = ReadOnlyRepositories.OfType<IRepositoryBase>().ToArray();

            DontDestroyOnLoad(gameObject);
        }
    }
}
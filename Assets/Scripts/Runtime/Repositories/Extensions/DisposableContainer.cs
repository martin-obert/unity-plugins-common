using System;
using System.Collections.Generic;

namespace Obert.Common.Runtime.Repositories.Extensions
{
    public sealed class DisposableContainer : IDisposable
    {
        private readonly IList<IDisposable> _container = new List<IDisposable>();

        public void Add(IDisposable disposable)
        {
            _container.Add(disposable);
        }

        public void Dispose()
        {
            foreach (var disposable in _container)
            {
                disposable?.Dispose();
            }
        }
    }
}
using System;

namespace Obert.Common.Runtime.Repositories.Extensions
{
    public static class DisposableExtensions
    {
        public static void AddTo(this IDisposable source, DisposableContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (source == null) return;
            container.Add(source);
        }
    }
}
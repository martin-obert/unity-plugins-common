using System;
using System.Collections.Generic;
using System.Linq;
using Obert.Common.Runtime.Exceptions;

namespace Obert.Common.Runtime.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ThrowIfEmptyOrNull<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (!source.Any()) throw new CollectionEmptyException(nameof(source));
        }
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source) => !IsNullOrEmpty(source);
    }
}
using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Obert.Common.Runtime.Exceptions
{
    public class CollectionEmptyException : ArgumentNullException
    {
        public CollectionEmptyException()
        {
        }

        protected CollectionEmptyException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CollectionEmptyException(string message) : base(message)
        {
        }

        public CollectionEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
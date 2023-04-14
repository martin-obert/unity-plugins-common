using System;
using UnityEngine;

namespace Obert.Common.Runtime.Logging
{
    public sealed class Logger<T> : Logger, ILogger<T>
    {
        public Logger() : base(typeof(T))
        {
        }
    }

    public class Logger : ILogger
    {
        private static ILogger _instance;
        private readonly Type _type;

        protected Logger(Type type = null)
        {
            _type = type;
        }

        public static ILogger<T> CreateFor<T>()
        {
            return new Logger<T>();
        }

        public static ILogger Instance
        {
            get => _instance ??= new Logger();
            set => _instance = value;
        }


        public void Log(string message)
        {
            if (_type == null)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogFormat("{0} - {1}", _type.FullName, message);
        }

        public void LogError(string message)
        {
            if (_type == null)
            {
                Debug.LogError(message);
                return;
            }

            Debug.LogErrorFormat("{0} - {1}", _type.FullName, message);
        }

        public void LogWarning(string warning)
        {
            if (_type == null)
            {
                Debug.LogWarning(warning);
                return;
            }

            Debug.LogWarningFormat("{0} - {1}", _type.FullName, warning);
        }

        public void LogException(Exception e)
        {
            if (_type == null)
            {
                Debug.LogException(e);
                return;
            }

            Debug.LogErrorFormat("{0} - {1}", _type.FullName, e);
        }

        public void Dispose()
        {
        }
    }
}
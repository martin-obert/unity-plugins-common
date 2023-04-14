using System;

namespace Obert.Common.Runtime.Logging
{
    public interface ILogger<T> : ILogger
    {
        
    }
    
    public interface ILogger : IDisposable
    {
        void Log(string message);
        void LogError(string message);
        void LogWarning(string warning);
        void LogException(Exception e);
    }
}
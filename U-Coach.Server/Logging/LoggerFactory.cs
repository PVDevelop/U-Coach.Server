
using System;

namespace PVDevelop.UCoach.Server.Logging
{
    public static class LoggerFactory
    {
        public static ILogger CreateLogger<T>()
        {
            return CreateLogger(typeof(T));
        }

        public static ILogger CreateLogger(Type sourceType)
        {
            return new Logger(sourceType.AssemblyQualifiedName);
        }
    }
}

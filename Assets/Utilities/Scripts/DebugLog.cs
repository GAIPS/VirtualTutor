using System.Collections.Generic;
using Antlr4.Runtime.Misc;

namespace Utilities
{
    public interface IDebugLogger
    {
        void Log(object message);
        void Warn(object message);
        void Err(object message);
    }

    public class DebugLog
    {
        private static readonly List<IDebugLogger> loggers = new List<IDebugLogger>();

        private DebugLog()
        {
        }

        public static void Add(IDebugLogger logger)
        {
            loggers.Add(logger);
        }

        public static void Clean()
        {
            loggers.Clear();
        }

        public static void Err(object message)
        {
            foreach (var logger in loggers)
            {
                if (logger == null) continue;
                logger.Err(message);
            }
        }

        public static void Log(object message)
        {
            foreach (var logger in loggers)
            {
                if (logger == null) continue;
                logger.Log(message);
            }
        }

        public static void Warn(object message)
        {
            foreach (var logger in loggers)
            {
                if (logger == null) continue;
                logger.Warn(message);
            }
        }
    }
}
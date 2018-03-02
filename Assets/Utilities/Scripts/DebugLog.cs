namespace Utilities
{
    public interface DebugLogger
    {
        void Log(object message);
        void Warn(object message);
        void Err(object message);
    }

    public class DebugLog
    {
        public static DebugLogger logger;

        private DebugLog() { }

        public static void Err(object message)
        {
            if (logger == null) return;
            logger.Err(message);
        }

        public static void Log(object message)
        {
            if (logger == null) return;
            logger.Log(message);
        }

        public static void Warn(object message)
        {
            if (logger == null) return;
            logger.Warn(message);
        }
    }
}

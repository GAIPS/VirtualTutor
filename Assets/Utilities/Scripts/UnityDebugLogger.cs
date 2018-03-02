
using UnityEngine;

namespace Utilities
{
    public class UnityDebugLogger : DebugLogger
    {
        public void Err(object message)
        {
            Debug.LogError(message);
        }

        public void Log(object message)
        {
            Debug.Log(message);
        }

        public void Warn(object message)
        {
            Debug.LogWarning(message);
        }
    }
}

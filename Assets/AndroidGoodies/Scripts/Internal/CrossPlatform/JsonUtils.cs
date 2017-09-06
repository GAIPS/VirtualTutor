#if UNITY_ANDROID
using System.Collections.Generic;

namespace DeadMosquito.AndroidGoodies.Internal {
    public static class JsonUtils
    {
        public static T GetValue<T>(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
            {
                return (T) dic[key];
            }

            return default(T);
        }

        public static string GetStr(this Dictionary<string, object> dic, string key)
        {
            return dic.GetValue<string>(key);
        }
    }
}
#endif
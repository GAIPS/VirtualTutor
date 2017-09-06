#if UNITY_ANDROID
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal {
    static class AndroidUri
    {
        public static AndroidJavaObject Parse(string uriString)
        {
            using (var uriClass = new AndroidJavaClass(C.UriClass))
            {
                return uriClass.CallStaticAJO("parse", uriString);
            }
        }

        public static AndroidJavaObject FromFile(string filePath)
        {
            using (var uriClass = new AndroidJavaClass(C.UriClass))
            {
                return uriClass.CallStaticAJO("fromFile", AGUtils.NewJavaFile(filePath));
            }
        }
    }
}
#endif
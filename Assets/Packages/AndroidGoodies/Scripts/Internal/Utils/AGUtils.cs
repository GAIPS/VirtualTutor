#if UNITY_ANDROID
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal {
    public static class AGUtils
    {
        static AndroidJavaObject _activity;

        public static AndroidJavaObject Activity
        {
            get
            {
                if (_activity == null)
                {
                    var unityPlayer = new AndroidJavaClass(C.ComUnity3DPlayerUnityPlayer);
                    _activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
                return _activity;
            }
        }

        public static AndroidJavaObject ActivityDecorView
        {
            get
            {
                return Window.Call<AndroidJavaObject>("getDecorView");
            }
        }

        public static AndroidJavaObject Window
        {
            get { return Activity.Call<AndroidJavaObject>("getWindow"); }
        }

        public static AndroidJavaObject PackageManager
        {
            get { return Activity.CallAJO("getPackageManager"); }
        }

        public static AndroidJavaObject ContentResolver
        {
            get { return AGUtils.Activity.CallAJO("getContentResolver"); }
        }

        public static bool HasSystemFeature(string feature)
        {
            using (var pm = PackageManager)
            {
                return pm.CallBool("hasSystemFeature", feature);
            }
        }

        public static long CurrentTimeMillis
        {
            get
            {
                using (var system = new AndroidJavaClass(C.JavaLangSystem))
                {
                    return system.CallStaticLong("currentTimeMillis");
                }
            }
        }

        #region reflection

        public static AndroidJavaObject ClassForName(string className)
        {
            using (var clazz = new AndroidJavaClass(C.JavaLangClass))
            {
                return clazz.CallStaticAJO("forName", className);
            }
        }

        public static AndroidJavaObject Cast(this AndroidJavaObject source, string destClass)
        {
            using (var destClassAJC = ClassForName(destClass))
            {
                return destClassAJC.Call<AndroidJavaObject>("cast", source);
            }
        }

        public static bool IsJavaNull(this AndroidJavaObject ajo)
        {
            return ajo.GetRawObject().ToInt32() == 0;
        }

        #endregion

        public static bool IsNotAndroidCheck()
        {
            bool isAndroid = Application.platform == RuntimePlatform.Android;

            if (isAndroid)
            {
                GoodiesSceneHelper.Init();
            }

            return !isAndroid;
        }

        public static void RunOnUiThread(Action action)
        {
            Activity.Call("runOnUiThread", new AndroidJavaRunnable(action));
        }

        public static void StartActivity(AndroidJavaObject intent, Action fallback = null)
        {
            try
            {
                Activity.Call("startActivity", intent);
            }
            catch (AndroidJavaException exception)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogWarning("Could not start the activity with " + intent.JavaToString() + ": " + exception.Message);
                }
                if (fallback != null)
                    fallback();
            }
            finally
            {
                intent.Dispose();
            }
        }

        public static void StartActivityWithChooser(AndroidJavaObject intent, string chooserTitle)
        {
            try
            {
                AndroidJavaObject jChooser = intent.CallStaticAJO("createChooser", intent, chooserTitle);
                Activity.Call("startActivity", jChooser);
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogWarning("Could not start the activity with " + intent.JavaToString() + ": " + exception.Message);
            }
            finally
            {
                intent.Dispose();
            }
        }

        public static void SendBroadcast(AndroidJavaObject intent)
        {
            Activity.Call("sendBroadcast", intent);
        }

        public static AndroidJavaObject GetMainActivityClass()
        {
            var packageName = AGDeviceInfo.GetApplicationPackage();
            using (PackageManager)
            {
                var launchIntent = PackageManager.CallAJO("getLaunchIntentForPackage", packageName);
                try
                {
                    var className = launchIntent.CallAJO("getComponent").CallStr("getClassName");
                    return ClassForName(className);
                }
                catch (Exception e)
                {
                    if (Debug.isDebugBuild)
                    {
                        Debug.LogWarning("Unable to find Main Activity Class: " + e.Message);
                    }
                    return null;
                }
            }
        }

        public static AndroidJavaObject NewJavaFile(string path)
        {
            return new AndroidJavaObject("java.io.File", path);
        }

        #region images
        public static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D tex2D, ImageFormat format = ImageFormat.PNG)
        {
            byte[] encoded = tex2D.Encode(format);
            using (var bf = new AndroidJavaClass(C.AndroidGraphicsBitmapFactory))
            {
                return bf.CallStaticAJO("decodeByteArray", encoded, 0, encoded.Length);
            }
        }

        /// <summary>
        /// Loads Texture2D from URI
        /// </summary>
        /// <returns>The from URI internal.</returns>
        /// <param name="uri">URI.</param>
        public static Texture2D TextureFromUriInternal(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            using (var resolver = ContentResolver)
            {
                var uriAjo = AndroidUri.Parse(uri);
                AndroidJavaObject bitmapAjo;
                try
                {
                    var stream = resolver.CallAJO("openInputStream", uriAjo);
                    using (var c = new AndroidJavaClass(C.AndroidGraphicsBitmapFactory))
                    {
                        bitmapAjo = c.CallStaticAJO("decodeStream", stream);
                    }

                    var compressFormatPng = new AndroidJavaClass(C.AndroidGraphicsBitmapCompressFormat).GetStatic<AndroidJavaObject>("PNG");
                    var outputStream = new AndroidJavaObject(C.JavaIoBytearrayOutputStream);
                    bitmapAjo.CallBool("compress", compressFormatPng, 100, outputStream);
                    byte[] buffer = outputStream.Call<byte[]>("toByteArray");

                    var tex = new Texture2D(2, 2);
                    tex.LoadImage(buffer);
                    return tex;
                }
                catch (Exception ex)
                {
                    if (Debug.isDebugBuild)
                    {
                        Debug.LogException(ex);
                    }
                    return null;
                }
            }
        }

        #endregion
    }
}
#endif

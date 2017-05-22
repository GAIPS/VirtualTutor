#if UNITY_ANDROID
using UnityEngine;
using System;
using System.IO;

namespace DeadMosquito.AndroidGoodies.Internal
{
    public static class CommonUtils
    {
        public static byte[] Encode(this Texture2D tex, ImageFormat format)
        {
            return format == ImageFormat.PNG ? tex.EncodeToPNG() : tex.EncodeToJPG();
        }

        public static Texture2D TextureFromFile(string path)
        {
            Texture2D tex = null;
            if (File.Exists(path))
            {
                var fileData = File.ReadAllBytes(path);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);
            }
            return tex;
        }

        static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToMillisSinceEpoch(this DateTime date)
        {
            return (long)(date - Jan1st1970.ToLocalTime()).TotalMilliseconds;
        }
    }
}
#endif

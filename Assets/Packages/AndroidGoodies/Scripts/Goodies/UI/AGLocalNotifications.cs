// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGLocalNotifications.cs
//

#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies {
    /// <summary>
    /// Class to show local notifications.
    /// </summary>
    public static class AGLocalNotifications
    {
        private const int DEFAULT_SOUND = 0x00000001;
        private const int DEFAULT_VIBRATE = 0x00000002;
        private const int DEFAULT_LIGHTS = 0x00000004;
        private const int DEFAULT_ALL = -1;

        private const string CompatBuilderClass = "android.support.v4.app.NotificationCompat$Builder";

        /// <summary>
        /// Shows the local notification. Upon click it will open the game if the game is not in foreground.
        /// </summary>
        /// <param name="title">Set the title (first row) of the notification, in a standard notification.</param>
        /// <param name="text">Set the text (second row) of the notification, in a standard notification.</param>
        /// <param name="when">Set the time that the event occurred. Notifications in the panel are sorted by this time.</param>
        /// <param name="tickerText">Set the text that is displayed in the status bar when the notification first arrives.</param>
        /// <param name="iconName">Icon drawable name. Icon must be included in the app in res/drawable folder</param>
        public static void ShowNotification(string title, string text,
                                            DateTime? when = null, string tickerText = null, string iconName = null)
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            using (var builderAJO = new AndroidJavaObject(CompatBuilderClass, AGUtils.Activity))
            {
                var intent = new AndroidIntent(AGUtils.GetMainActivityClass());
                var pendingIntent = AndroidPendingIntent.GetActivity(intent.AJO);

                builderAJO.CallAJO("setContentTitle", title);
                builderAJO.CallAJO("setContentText", text);
                builderAJO.CallAJO("setAutoCancel", true);

                builderAJO.CallAJO("setWhen", (when ?? DateTime.Now).ToMillisSinceEpoch());

                int icon = string.IsNullOrEmpty(iconName) ? R.UnityLauncherIcon : R.GetAppDrawableId(iconName);
                builderAJO.CallAJO("setSmallIcon", icon);

                builderAJO.CallAJO("setDefaults", DEFAULT_ALL);
                builderAJO.CallAJO("setContentIntent", pendingIntent);
                //builderAJO.CallAJO("setContentInfo", "Info");
                if (!string.IsNullOrEmpty(tickerText))
                {
                    builderAJO.CallAJO("setTicker", tickerText);
                }

                AGSystemService.NotificationService.Call("notify", 1, builderAJO.CallAJO("build"));
                intent.Dispose();
                pendingIntent.Dispose();
            }
        }
    }
}
#endif

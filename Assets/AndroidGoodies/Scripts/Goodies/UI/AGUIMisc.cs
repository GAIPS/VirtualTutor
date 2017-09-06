// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGUIMisc.cs
//

#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies.Internal;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies {
    public static class AGUIMisc
    {
        #region toast

        /// <summary>
        /// Toast length.
        /// </summary>
        public enum ToastLength
        {
            /// <summary>
            /// The short toast duration.
            /// </summary>
            Short = 0,
            /// <summary>
            /// The long toast duration.
            /// </summary>
            Long = 1
        }

        /// <summary>
        /// Shows the toast with specified text.
        /// </summary>
        /// <param name="text">Text to display on toast.</param>
        /// <param name="length">Duration to show.</param>
        public static void ShowToast(string text, ToastLength length = ToastLength.Short)
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            AGUtils.RunOnUiThread(() =>
                {
                    using (var toast = new AndroidJavaClass(C.AndroidWidgetToast))
                    {
                        var toastInstance = toast.CallStaticAJO("makeText", AGUtils.Activity, text, (int)length);
                        toastInstance.Call("show");
                    }
                }
            );
        }

        #endregion

        #region immersive_mode

        private const int SYSTEM_UI_FLAG_LAYOUT_STABLE = 0x00000100;
        private const int SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION = 0x00000200;
        private const int SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN = 0x00000400;
        private const int SYSTEM_UI_FLAG_HIDE_NAVIGATION = 0x00000002;
        private const int SYSTEM_UI_FLAG_FULLSCREEN = 0x00000004;
        private const int SYSTEM_UI_FLAG_IMMERSIVE = 0x00000800;
        private const int SYSTEM_UI_FLAG_IMMERSIVE_STICKY = 0x00001000;

        private const int ImmersiveFlagNonSticky = SYSTEM_UI_FLAG_LAYOUT_STABLE
                                                   | SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                                                   | SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                                                   | SYSTEM_UI_FLAG_HIDE_NAVIGATION// hide nav bar
                                                   | SYSTEM_UI_FLAG_FULLSCREEN// hide status bar
                                                   | SYSTEM_UI_FLAG_IMMERSIVE;

        private const int ImmersiveFlagSticky = SYSTEM_UI_FLAG_LAYOUT_STABLE
                                                | SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                                                | SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                                                | SYSTEM_UI_FLAG_HIDE_NAVIGATION// hide nav bar
                                                | SYSTEM_UI_FLAG_FULLSCREEN// hide status bar
                                                | SYSTEM_UI_FLAG_IMMERSIVE_STICKY;

        // Enables Immersive Full-Screen Mode on Android device
        // Unity 5 has added immersive mode by default, so if your using Unity 5 or above, this method is redundant.
        public static void EnableImmersiveMode(bool sticky = true)
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            GoodiesSceneHelper.IsInImmersiveMode = true;
            int mode = sticky ? ImmersiveFlagSticky : ImmersiveFlagNonSticky;

            AGUtils.RunOnUiThread(
                () =>
                {
                    using (var decorView = AGUtils.ActivityDecorView)
                    {
                        decorView.Call("setSystemUiVisibility", mode);
                    }
                });
        }

        #endregion
    }
}
#endif

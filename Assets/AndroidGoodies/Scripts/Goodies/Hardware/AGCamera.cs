// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGCamera.cs
//

#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
    /// <summary>
    /// Methods to interact with device camera.
    /// </summary>
    public static class AGCamera
    {
        /// <summary>
        /// Check if device has camera
        /// </summary>
        /// <returns><c>true</c>, if device has camera, <c>false</c> otherwise.</returns>
        public static bool DeviceHasCamera()
        {
            return AGDeviceInfo.SystemFeatures.HasSystemFeature(AGDeviceInfo.SystemFeatures.FEATURE_CAMERA);
        }

        /// <summary>
        /// Check if device has frontal camera
        /// </summary>
        /// <returns><c>true</c>, if device has frontal camera, <c>false</c> otherwise.</returns>
        public static bool DeviceHasFrontalCamera()
        {
            return AGDeviceInfo.SystemFeatures.HasSystemFeature(AGDeviceInfo.SystemFeatures.FEATURE_CAMERA_FRONT);
        }

        /// <summary>
        /// Check if device has camera with autofocus
        /// </summary>
        /// <returns><c>true</c>, if device has camera with autofocus, <c>false</c> otherwise.</returns>
        public static bool DeviceHasCameraWithAutoFocus()
        {
            return DeviceHasCamera() && AGDeviceInfo.SystemFeatures.HasSystemFeature(AGDeviceInfo.SystemFeatures.FEATURE_CAMERA_AUTOFOCUS);
        }

        /// <summary>
        /// Check if device has camera with flashlight
        /// </summary>
        /// <returns><c>true</c>, if device has camera with flashlight, <c>false</c> otherwise.</returns>
        public static bool DeviceHasCameraWithFlashlight()
        {
            return DeviceHasCamera() && AGDeviceInfo.SystemFeatures.HasSystemFeature(AGDeviceInfo.SystemFeatures.FEATURE_CAMERA_FLASH);
        }

        #region take_photo
        /// <summary>
        /// Required permissions:
        ///     <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
        /// 
        /// Launches the camera app to take a photo and returns resulting Texture2D in callback. The photo is also saved to the device gallery.
        /// 
        /// IMPORTANT! : You don't need any permissions to use this method. It works using Android intent.
        /// </summary>
        /// <param name="onSuccess">On success callback. Image is received as callback parameter</param>
        /// <param name="onError">On error/cancel callback.</param>
        /// <param name="maxSize">Max image size. If provided image will be downscaled.</param>
        /// <param name="shouldGenerateThumbnails">Whether thumbnail images will be generated. Used to show small previews of image.</param>
        public static void TakePhoto(Action<ImagePickResult> onSuccess, Action<string> onError, 
            ImageResultSize maxSize = ImageResultSize.Original, bool shouldGenerateThumbnails = true)
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            if (onSuccess == null)
            {
                throw new ArgumentNullException("onSuccess", "Success callback cannot be null");
            }

            _onSuccessAction = onSuccess;
            _onCancelAction = onError;

            AGUtils.RunOnUiThread(() => AGActivityUtils.TakePhoto(maxSize, shouldGenerateThumbnails));
        }

        static Action<ImagePickResult> _onSuccessAction;
        static Action<string> _onCancelAction;

        // Invoked by UnityPlayer.SendMessage
        internal static void OnSuccessTrigger(string imageCallbackJson)
        {
            if (_onSuccessAction != null)
            {
                var image = ImagePickResult.FromJson(imageCallbackJson);
                _onSuccessAction(image);
            }
        }

        // Invoked by UnityPlayer.SendMessage
        internal static void OnErrorTrigger(string errorMessage)
        {
            if (_onCancelAction != null)
            {
                _onCancelAction(errorMessage);
                _onCancelAction = null;
            }
        }
        #endregion
    }
}
#endif

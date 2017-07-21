// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGGallery.cs
//

#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
    /// <summary>
    /// Methods to interact with device gallery.
    /// </summary>
    public static class AGGallery
    {
        static Action<ImagePickResult> _onSuccessAction;
        static Action<string> _onCancelAction;

        /// <summary>
        /// Picks the image from gallery.
        /// </summary>
        /// <param name="onSuccess">On success callback. Image is received as callback parameter</param>
        /// <param name="onError">On error callback.</param>
        /// <param name="maxSize">Max image size. If provided image will be downscaled.</param>
        /// <param name="shouldGenerateThumbnails">Whether thumbnail images will be generated. Used to show small previews of image.</param>
        public static void PickImageFromGallery(Action<ImagePickResult> onSuccess, Action<string> onError,
            ImageResultSize maxSize = ImageResultSize.Original, bool shouldGenerateThumbnails = true)
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            Check.Argument.IsNotNull(onSuccess, "onSuccess");

            _onSuccessAction = onSuccess;
            _onCancelAction = onError;

            AGActivityUtils.PickPhotoFromGallery(maxSize, shouldGenerateThumbnails);
        }

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

        /// <summary>
        /// Saves the image to android gallery.
        /// </summary>
        /// <returns>The image to save to the gallery.</returns>
        /// <param name="texture2D">Texture2D to save.</param>
        /// <param name="title">Title.</param>
        /// <param name="folder">Inner folder in Pictures directory. Must be a valid folder name</param>
        /// <param name="imageFormat">Image format.</param>
        public static void SaveImageToGallery(Texture2D texture2D, string title, string folder = null,
            ImageFormat imageFormat = ImageFormat.PNG)
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            AGFileUtils.SaveImageToGallery(texture2D, title, folder, imageFormat);
        }
    }
}

#endif

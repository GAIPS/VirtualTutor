#if UNITY_ANDROID
using UnityEngine;
using DeadMosquito.AndroidGoodies;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Linq;
using System.IO;

namespace AndroidGoodiesExamples
{
    public class OtherGoodiesTest : MonoBehaviour
    {
        public Image image;
        public InputField input;

        Texture2D _lastTakenScreenshot;

        void Start()
        {
            input.text = "com.twitter.android";
        }

        #region toast
        public void OnShortToastClick()
        {
            AGUIMisc.ShowToast("hello short!");
        }

        public void OnLongToastClick()
        {
            AGUIMisc.ShowToast("hello long!", AGUIMisc.ToastLength.Long);
        }
        #endregion

        #region maps
        public void OnOpenMapClick()
        {
            AGMaps.OpenMapLocation(47.6f, -122.3f, 9);
        }

        public void OnOpenMapLabelClick()
        {
            AGMaps.OpenMapLocationWithLabel(47.6f, -122.3f, "My Label");
        }

        public void OnOpenMapAddress()
        {
            AGMaps.OpenMapLocation("1st & Pike, Seattle");
        }
        #endregion

        public void OnEnableImmersiveMode()
        {
            AGUIMisc.EnableImmersiveMode();
        }

        public void OnVibrate()
        {
            if (!AGVibrator.HasVibrator())
            {
                Debug.LogWarning("This device does not have vibrator");
            }

            AGVibrator.Vibrate(500);
        }

        public void OnVibratePattern()
        {
            if (!AGVibrator.HasVibrator())
            {
                Debug.LogWarning("This device does not have vibrator");
            }

            // Start without a delay
            // Each element then alternates between vibrate, sleep, vibrate, sleep...
            long[] pattern = {0, 100, 1000, 300, 200, 100, 500, 200, 100};

            AGVibrator.VibratePattern(pattern);
        }

        #region screenshot
        public void OnSaveScreenshotToGallery()
        {
            StartCoroutine(TakeScreenshot(Screen.width, Screen.height));
        }

        public IEnumerator TakeScreenshot(int width, int height)
        {
            yield return new WaitForEndOfFrame();
            var texture = new Texture2D(width, height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();
            _lastTakenScreenshot = texture;
            var imageTitle = "Screenshot-" + System.DateTime.Now.ToString("yy-MM-dd-hh-mm-ss");
            const string folderName = "Goodies";
            AGFileUtils.SaveImageToGallery(_lastTakenScreenshot, imageTitle, folderName, ImageFormat.JPEG);
            AGUIMisc.ShowToast(imageTitle + " saved to gallery");
        }
        #endregion

        public void OnFlashlightOn()
        {
            AGFlashLight.Enable();
        }

        public void OnFlashlightOff()
        {
            AGFlashLight.Disable();
        }

        public void SendLocalPushNotification()
        {
            AGLocalNotifications.ShowNotification("Notification Title", "Notification Text", DateTime.Now);
        }

        public void WatchYoutubeVideo()
        {
            const string videoId = "mZqjmyyJkQc";
            AGApps.WatchYoutubeVideo(videoId);
        }

        public void OpenInstagramProfile()
        {
            const string profileId = "tarasleskivlviv";
            AGApps.OpenInstagramProfile(profileId);
        }

        public void OpenTwitterProfile()
        {
            const string profileId = "Taras_Leskiv";
            AGApps.OpenTwitterProfile(profileId);
        }

        public void OpenFacebookProfile()
        {
            const string profileId = "4"; // Mark Zuckerberg
            AGApps.OpenFacebookProfile(profileId);
        }

        public void OpenOtherApp()
        {
            var package = input.text;
            AGApps.OpenOtherAppOnDevice(package, () => AGUIMisc.ShowToast("Could not launch " + package));
        }

        public void UninstallOtherApp()
        {
            var package = input.text;
            AGApps.UninstallApp(package);
        }

        public void InstallApkFromSDCard()
        {
            // NOTE: In order to test this method apk file must exist on file system.
            // To test this method you put 'test.apk' file into your downloads folder
            const string apkFileName = "/test.apk";
            var filePath = Path.Combine(AGEnvironment.ExternalStorageDirectoryPath, AGEnvironment.DirectoryDownloads) +
                           apkFileName;
            Debug.Log("Unstalling APK: " + filePath + ", file exists?: " + File.Exists(filePath));

            AGApps.InstallApkFileFromSDCard(filePath);
        }

        public void OnPickContactFromAddressBook()
        {
            AGContacts.PickContact(
                pickedContact =>
                {
                    var msg = string.Format("Picked contact: {0}, photo URI: {1}, emails: {2}, phones: {3}",
                        pickedContact.DisplayName,
                        pickedContact.PhotoUri,
                        string.Join(",", pickedContact.Emails.ToArray()),
                        string.Join(",", pickedContact.Phones.ToArray())
                        );

                    Debug.Log(msg);
                    AGUIMisc.ShowToast(msg);

                    if (!string.IsNullOrEmpty(pickedContact.PhotoUri)) // Not all contacts have image
                    {
                        var contactPicture = AGFileUtils.ImageUriToTexture2D(pickedContact.PhotoUri);
                        image.sprite = SpriteFromTex2D(contactPicture);
                    }
                },
                failureReason =>
                {
                    AGUIMisc.ShowToast("Picking contact failed: " + failureReason);
                });
        }

        public void OnPickGalleryImage()
        {
            // Whether to generate thumbnails
            var shouldGenerateThumbnails = true;

            // if image is larger it will be downscaled to the max size proportionally
            var imageResultSize = ImageResultSize.Max512;
            AGGallery.PickImageFromGallery(
                selectedImage =>
                {
                    var imageTexture2D = selectedImage.LoadTexture2D();

                    string msg = string.Format("{0} was loaded from gallery with size {1}x{2}",
                        selectedImage.OriginalPath, imageTexture2D.width, imageTexture2D.height);
                    AGUIMisc.ShowToast(msg);
                    Debug.Log(msg);
                    image.sprite = SpriteFromTex2D(imageTexture2D);

                    // Clean up
                    Resources.UnloadUnusedAssets();
                },
                errorMessage => AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage),
                imageResultSize, shouldGenerateThumbnails);
        }

        public void OnTakePhoto()
        {
            // Whether to generate thumbnails
            var shouldGenerateThumbnails = false;

            // if image is larger it will be downscaled to the max size proportionally
            var imageResultSize = ImageResultSize.Max1024; 
            AGCamera.TakePhoto(
                selectedImage =>
                {
                    // Load received image into Texture2D
                    var imageTexture2D = selectedImage.LoadTexture2D();
                    string msg = string.Format("{0} was taken from camera with size {1}x{2}",
                        selectedImage.DisplayName, imageTexture2D.width, imageTexture2D.height);
                    AGUIMisc.ShowToast(msg);
                    Debug.Log(msg);
                    image.sprite = SpriteFromTex2D(imageTexture2D);

                    // Clean up
                    Resources.UnloadUnusedAssets();
                },
                error => AGUIMisc.ShowToast("Cancelled taking photo from camera: " + error), imageResultSize, shouldGenerateThumbnails);
        }

        /// <summary>
        /// Example how to request for runtime permissions
        /// </summary>
        public void OnRequestPermissions()
        {
            // Don't forget to also add the permissions you need to manifest!
            var permissions = new[]
            {
                AGPermissions.WRITE_CONTACTS,
                AGPermissions.CALL_PHONE,
                AGPermissions.ACCESS_FINE_LOCATION,
                AGPermissions.READ_CALENDAR
            };

            // Filter permissions so we don't request already granted permissions,
            // otherwise if the user denies already granted permission the app will be killed
            var nonGrantedPermissions = permissions.ToList()
                .Where(x => !AGPermissions.IsPermissionGranted(x))
                .ToArray();

            if (nonGrantedPermissions.Length == 0)
            {
                var message = "User already granted all these permissions: " + string.Join(",", permissions);
                Debug.Log(message);
                AGUIMisc.ShowToast(message);
                return;
            }

            // Finally request permissions user has not granted yet and log the results
            AGPermissions.RequestPermissions(permissions, results =>
            {
                // Process results of requested permissions
                foreach (var result in results)
                {
                    string msg = string.Format("Permission [{0}] is [{1}], should show explanation?: {2}",
                            result.Permission, result.Status, result.ShouldShowRequestPermissionRationale);
                    Debug.Log(msg);
                    AGUIMisc.ShowToast(msg);
                    if (result.Status == AGPermissions.PermissionStatus.Denied)
                    {
                        // User denied permission, now we need to find out if he clicked "Do not show again" checkbox
                        if (result.ShouldShowRequestPermissionRationale)
                        {
                            // User just denied permission, we can show explanation here and request permissions again
                            // or send user to settings to do so
                        }
                        else
                        {
                            // User checked "Do not show again" checkbox or permission can't be granted.
                            // We should continue with this permission denied
                        }
                    }
                }
            });
        }

        static Sprite SpriteFromTex2D(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}

#endif
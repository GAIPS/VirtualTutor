#if UNITY_ANDROID
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal {
    public static class AGSystemService
    {
        private const string VIBRATOR_SERVICE = "vibrator";
        private const string VibratorClass = "android.os.Vibrator";
        private static AndroidJavaObject _vibratorService;

        private const string LOCATION_SERVICE = "location";
        private const string LocationManagerClass = "android.location.LocationManager";
        private static AndroidJavaObject _locationService;

        private const string CONNECTIVITY_SERVICE = "connectivity";
        private const string ConnectivityManagerClass = "android.net.ConnectivityManager";
        private static AndroidJavaObject _connectivityService;

        private const string WIFI_SERVICE = "wifi";
        private const string WifiManagerClass = "android.net.wifi.WifiManager";
        private static AndroidJavaObject _wifiService;

        private const string TELEPHONY_SERVICE = "phone";
        private const string TelephonyManagerClass = "android.telephony.TelephonyManager";
        private static AndroidJavaObject _telephonyService;

        private const string NOTIFICATION_SERVICE = "notification";
        private const string NotificationManagerClass = "android.app.NotificationManager";
        private static AndroidJavaObject _notificationService;

        private const string CAMERA_SERVICE = "camera";
        private const string CameraManagerClass = "android.hardware.camera2.CameraManager";
        private static AndroidJavaObject _cameraService;

        public static AndroidJavaObject VibratorService
        {
            get
            {
                if (_vibratorService == null)
                {
                    _vibratorService = GetSystemService(VIBRATOR_SERVICE, VibratorClass);
                }
                return _vibratorService;
            }
        }

        public static AndroidJavaObject LocationService
        {
            get
            {
                if (_locationService == null)
                {
                    _locationService = GetSystemService(LOCATION_SERVICE, LocationManagerClass);
                }
                return _locationService;
            }
        }

        public static AndroidJavaObject ConnectivityService
        {
            get
            {
                if (_connectivityService == null)
                {
                    _connectivityService = GetSystemService(CONNECTIVITY_SERVICE, ConnectivityManagerClass);
                }
                return _connectivityService;
            }
        }

        public static AndroidJavaObject WifiService
        {
            get
            {
                if (_wifiService == null)
                {
                    _wifiService = GetSystemService(WIFI_SERVICE, WifiManagerClass);
                }
                return _wifiService;
            }
        }

        public static AndroidJavaObject TelephonyService
        {
            get
            {
                if (_telephonyService == null)
                {
                    _telephonyService = GetSystemService(TELEPHONY_SERVICE, TelephonyManagerClass);
                }
                return _telephonyService;
            }
        }

        public static AndroidJavaObject NotificationService
        {
            get
            {
                if (_notificationService == null)
                {
                    _notificationService = GetSystemService(NOTIFICATION_SERVICE, NotificationManagerClass);
                }
                return _notificationService;
            }
        }

        public static AndroidJavaObject CameraService
        {
            get
            {
                if (_cameraService == null)
                {
                    _cameraService = GetSystemService(CAMERA_SERVICE, CameraManagerClass);
                }
                return _cameraService;
            }
        }

        private static AndroidJavaObject GetSystemService(string name, string serviceClass)
        {
            try
            {
                var serviceObj = AGUtils.Activity.CallAJO("getSystemService", name);
                return serviceObj.Cast(serviceClass);
            }
            catch (Exception e)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogWarning("Failed to get " + name + " service. Error: " + e.Message);
                }
                return null;
            }
        }
    }
}
#endif
#if UNITY_ANDROID

using UnityEngine;

public class AndroidCalendarPlugin
{

    private static AndroidCalendarPlugin _instance;

    public static AndroidCalendarPlugin Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AndroidCalendarPlugin();
            }
            return _instance;
        }
    }
    private AndroidJavaClass calendar;

    private bool _hasPermissions;

    private AndroidCalendarPlugin()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
        
        calendar = new AndroidJavaClass("pt.inesc_id.ricardorodrigues.androidcalendarplugin.CalendarPlugin");
        calendar.Call("SetContext", context);
    }

    public bool AskPermissions()
    {
        return _hasPermissions || (_hasPermissions = calendar.Call<bool>("AskPermissions"));
    }
    
    public string Echo(string message)
    {
        AndroidJavaClass echo = new AndroidJavaClass("pt.inesc_id.ricardorodrigues.androidcalendarplugin.Echo");
        string somestring = echo.CallStatic<string>("Shout", "Hello from Unity");
        Debug.Log(somestring);
        return somestring;
    }
}

#endif
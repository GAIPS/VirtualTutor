using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    [SerializeField] private Text _text;

    private AndroidCalendarPlugin _calendar;

    // Use this for initialization
    void Start()
    {
        if (_text == null) return;
        _calendar = AndroidCalendarPlugin.Instance;
#if UNITY_ANDROID

        if (!_calendar.AskPermissions())
        {
            _text.text = "Permissions Denied";
        }
        _text.text = _calendar.Echo("Hello from Unity");
#else
		_text.text = "Not on Android";
#endif
    }
}
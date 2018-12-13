using HookControl;
using UnityEngine;
using Utilities;

public class MoodleLoginController : IControl
{
    public GameObject MenuPrefab;
    private Control _control;
    private LoginHook _hook;
    private WebManager _webManager;

    public GameObject Instance
    {
        get { return _control.Instance; }
    }

    public MoodleLoginController(WebManager webManager)
    {
        _webManager = webManager;
    }

    public string GetName()
    {
        return "LoginMenu";
    }

    public ShowResult Show()
    {
//        var storage = PersistentDataStorage.Instance;
//        var state = storage.GetState();

        if (!MenuPrefab)
        {
            DebugLog.Warn("Unable to show Login Menu. No menu prefab set.");
            return ShowResult.FAIL;
        }

        if (_control == null)
            _control = new Control(MenuPrefab);

        var result = _control.Show();
        if (result == ShowResult.FAIL)
        {
            DebugLog.Warn("Unable to show Activity Menu.");
            return result;
        }

        if (result == ShowResult.FIRST)
        {
            _hook = _control.Instance.GetComponent<LoginHook>();
            if (!_hook)
            {
                DebugLog.Warn("Unable to retrieve Login Hook.");
                return ShowResult.FAIL;
            }
        }

        _hook.Title = "Moodle Login";
        _hook.ConnectFunction = (username, password) =>
        {
            DebugLog.Log("Login made with username " + username + " and password " + password);
            if (!_webManager)
            {
                DebugLog.Err("Web Manager not defined.");
                return;
            }

            _webManager.Login(username, password, success =>
            {
                DebugLog.Log("Logged in successefully? " + success.ToString());
                if (!success)
                {
                    _hook.CompleteLogin("O nome de utlizador ou password estão incorrectos.");
                    return;
                }
                
                var state = PersistentDataStorage.Instance.GetState();

                state["UserID"] = username;
                PersistentDataStorage.Instance.SaveState();
                
                Disable();
//                _webManager.RetrieveData(username, (percentage, message) =>
//                {
//                    DebugLog.Log("Progress: " + percentage * 100f + " | " + message);
//                    if (percentage == 1f)
//                    {
//                        _hook.CompleteLogin();
//                        Disable();
//                    }
//                });
            });
        };

        return result;
    }

    public void Destroy()
    {
        if (_control == null) return;
        _control.Destroy();
    }

    public void Disable()
    {
        if (_control == null) return;
        _control.Disable();
    }

    public bool IsVisible()
    {
        if (_control == null) return false;

        return _control.IsVisible();
    }
}
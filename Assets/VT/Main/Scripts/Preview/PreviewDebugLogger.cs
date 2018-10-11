using System;
using System.Collections;
using System.Collections.Generic;
using HookControl;
using UnityEngine;
using Utilities;

public class PreviewDebugLogger : MonoBehaviour, IDebugLogger
{
    [SerializeField] private GameObject _yesNoPrefab;
    private Control _yesNoObject;
    private YesNoHook _hook;

    public Preview Preview;
    private bool _playing;

    public void Log(object message)
    {
        // Intentionally left empty
    }

    public void Warn(object message)
    {
        ShowMessage(message);
    }

    public void Err(object message)
    {
        var exception = message as Exception;
        if (exception != null)
        {
            ShowMessage(exception.Message);
        }
        else
        {
            ShowMessage(message);
        }
    }

    private void ShowMessage(object message)
    {
        if (!_yesNoPrefab)
        {
            return;
        }

        if (_yesNoObject == null)
        {
            _yesNoObject = new Control(_yesNoPrefab);
        }

        var show = _yesNoObject.Show();
        if (show == ShowResult.FIRST || _hook == null)
        {
            _hook = _yesNoObject.Instance.GetComponent<YesNoHook>();
            _hook.NoText = "Restart";
            _hook.NoFunction = HideRestart;
            _hook.YesText = "Continue";
            _hook.YesFunction = HideContinue;
        }

        if (show == ShowResult.FIRST || show == ShowResult.OK)
        {
            _hook.Message = message.ToString();

            if (Preview != null)
            {
                _playing = Preview.Playing;
                Preview.Playing = false;
            }
        }
    }

    private void HideRestart()
    {
        _yesNoObject.Disable();
        ReloadLevel.ReloadStatic();
    }

    private void HideContinue()
    {
        _yesNoObject.Disable();
        if (Preview != null) Preview.Playing = _playing;
    }
}
using System;
using HookControl;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class LoginHook : MonoBehaviour
{
    [SerializeField] private InputField _username;
    [SerializeField] private InputField _password;

    [SerializeField] private GameObject _loadingTextObj;
    [SerializeField] private GameObject _confirmButtonObj;

    [SerializeField] private Text _titleText;

    public string Title
    {
        get
        {
            if (_titleText)
            {
                return _titleText.text;
            }

            return string.Empty;
        }
        set
        {
            if (_titleText)
            {
                _titleText.text = value;
            }
        }
    }

    [SerializeField] private Text _connectText;

    public string ConnectText
    {
        get
        {
            if (_connectText)
            {
                return _connectText.text;
            }

            return string.Empty;
        }
        set
        {
            if (_connectText)
            {
                _connectText.text = value;
            }
        }
    }

    [SerializeField] private Text _errorText;

    private string Error
    {
        get
        {
            if (_errorText)
            {
                return _errorText.text;
            }

            return string.Empty;
        }
        set
        {
            if (_errorText)
            {
                _errorText.text = value ?? string.Empty;
                _errorText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }
    }

    private void Start()
    {
        CompleteLogin();
        if (_errorText)
        {
            _errorText.gameObject.SetActive(false);
        }
    }

    public LoginFunc ConnectFunction;

    public void UiConnect()
    {
        if (!_username || !_password)
        {
            DebugLog.Err("Username or Password Input Field missing.");
            return;
        }

        if (ConnectFunction != null)
        {
            ConnectFunction(_username.text, _password.text);
            if (_loadingTextObj && _confirmButtonObj)
            {
                _loadingTextObj.SetActive(true);
                _confirmButtonObj.SetActive(false);
            }
            Error = null;
        }
    }

    public void CompleteLogin(string error = null)
    {
        if (_loadingTextObj && _confirmButtonObj)
        {
            _loadingTextObj.SetActive(false);
            _confirmButtonObj.SetActive(true);
        }
        Error = error;
    }
}
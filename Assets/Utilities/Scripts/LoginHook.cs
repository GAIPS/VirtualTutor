using HookControl;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class LoginHook : MonoBehaviour {
	[SerializeField] private InputField _username;
	[SerializeField] private InputField _password;

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
		}
	}
}

using HookControl;
using UnityEngine;
using UnityEngine.UI;

public class YesNoHook : Hook
{
    [SerializeField] private Text _message;

    public string Message
    {
        get
        {
            if (_message)
            {
                return _message.text;
            }

            return string.Empty;
        }
        set
        {
            if (_message)
            {
                _message.text = value;
            }
        }
    }

    [SerializeField] private Text _noText;

    public string NoText
    {
        get
        {
            if (_noText)
            {
                return _noText.text;
            }

            return string.Empty;
        }
        set
        {
            if (_noText)
            {
                _noText.text = value;
            }
        }
    }

    public VoidFunc NoFunction;

    public void UiNo()
    {
        if (NoFunction != null)
        {
            NoFunction();
        }
    }

    [SerializeField] private Text _yesText;

    public string YesText
    {
        get
        {
            if (_yesText)
            {
                return _yesText.text;
            }

            return string.Empty;
        }
        set
        {
            if (_yesText)
            {
                _yesText.text = value;
            }
        }
    }

    public VoidFunc YesFunction;

    public void UiYes()
    {
        if (YesFunction != null)
        {
            YesFunction();
        }
    }
}
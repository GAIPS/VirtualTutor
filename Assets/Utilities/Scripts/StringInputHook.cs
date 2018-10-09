using HookControl;
using UnityEngine;
using UnityEngine.UI;

public class StringInputHook : MonoBehaviour
{
    [SerializeField] private InputField _inputField;

    [SerializeField] private Text _titleText;

    private string _name;

    public StringFunc OnSubmit;

    public void SetTitle(string title)
    {
        if (_titleText)
        {
            _titleText.text = title;
        }
    }

    public void SetName(string submitedName)
    {
        _name = submitedName;
        if (_inputField)
        {
            _inputField.text = submitedName;
        }
    }

    public void UpdateString(string submitedName)
    {
        _name = submitedName;
    }

    public void Submit()
    {
        if (OnSubmit != null)
        {
            OnSubmit(_name);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Submit();
        }
    }
}
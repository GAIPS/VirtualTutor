using System.IO;
using HookControl;
using SFB;
using UnityEngine;
using UnityEngine.UI;

public class FileInputHook : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    [SerializeField] private Text _titleText;

    private string _name;
    private string _title;
    
    public string DialogDirectory { get; set; }
    public string DialogExtensions { get; set; }

    public StringFunc OnSubmit;

    public void SetTitle(string title)
    {
        _title = title;
        if (_titleText)
        {
            _titleText.text = title;
        }
    }

    public void SetName(string submitedName)
    {
        _name = submitedName;
#if UNITY_STANDALONE_OSX
        _name = _name.Replace("file://", "");
#endif
        if (_inputField)
        {
            _inputField.text = submitedName;
        }
    }

    public void UpdateString(string submitedName)
    {
        _name = submitedName;
    }

    public void BrowseFiles()
    {
        string title = string.IsNullOrEmpty(_title) ? "Select file" : _title;
        string directory = string.IsNullOrEmpty(DialogDirectory) ? string.Empty : DialogDirectory;
        string extensions = string.IsNullOrEmpty(DialogExtensions) ? string.Empty : DialogExtensions;
        string[] files = StandaloneFileBrowser.OpenFilePanel(title, directory, extensions, false);

        if (files.Length > 0 && !string.IsNullOrEmpty(files[0]))
        {
            SetName(files[0]);
        }
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
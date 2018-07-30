using System;
using HookControl;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMenuHook : Hook
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Button _confirmButton;
    private VoidFunc _onConfirm;

    public Tabs Tabs;
    public MetricsTab MetricsTab;
    public CheckpointsTab CheckpointsTab;

    public VoidFunc OnConfirm
    {
        get { return _onConfirm; }
        set
        {
            _onConfirm = value;
            if (_confirmButton && _onConfirm != null)
            {
                _confirmButton.onClick.RemoveAllListeners();
                _confirmButton.onClick.AddListener(() => OnConfirm());
            }
        }
    }

    public bool OnConfirmVisibility
    {
        get
        {
            if (!_confirmButton) return false;
            return _confirmButton.gameObject.activeSelf;
        }
        set
        {
            if (!_confirmButton) return;
            _confirmButton.gameObject.SetActive(value);
        }
    }

    public string Title
    {
        get { return _titleText.text; }
        set
        {
            if (!value.Equals(_titleText.text))
                _titleText.text = value;
        }
    }
}
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
        get { return this._titleText.text; }
        set
        {
            if (!value.Equals(this._titleText.text))
                this._titleText.text = value;
        }
    }

//    private void Start()
//    {
//        if (CheckpointsTab == null) return;
//
//        CheckpointsTab.AddCheckpoint(new Checkpoint
//        {
//            Type = Checkpoint.CType.Checkbox,
//            Name = "Test",
//            Date = DateTime.Now,
//            Effort = .5f,
//            Importance = .7f,
//            CheckboxDone = false
//        });
//
//        if (MetricsTab == null) return;
//
//        MetricsTab.OnEaseSlider = value => Debug.Log("Ease Slider: " + value);
//        MetricsTab.OnImportanceSlider = value => Debug.Log("Importance Slider: " + value);
//        MetricsTab.OnLikeSlider = value => Debug.Log("Like Slider: " + value);
//    }
}
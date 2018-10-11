using System;
using HookControl;
using SimpleJSON;
using UnityEngine;

public class ActivityMenuController : IControl
{
    public GameObject MenuPrefab;
    private Control _control;
    private ActivityMenuHook _hook;

    public GameObject Instance
    {
        get { return _control.Instance; }
    }

    public string GetName()
    {
        return "ActivityMenu";
    }

    public ShowResult Show()
    {
        var storage = PersistentDataStorage.Instance;
        var state = storage.GetState();

        if (state["Current"].AsObject["Activity"] == null)
        {
            Debug.LogWarning("Unable to show Activity Menu. No Current Activity placed.");
            return ShowResult.FAIL;
        }

        string currentActivity = state["Current"].AsObject["Activity"];
        if (string.IsNullOrEmpty(currentActivity) || state["Activities"].AsObject[currentActivity] == null)
        {
            Debug.LogWarning("Unable to show Activity Menu. No Current Activity found.");
            return ShowResult.FAIL;
        }

        JSONNode activity = state["Activities"].AsObject[currentActivity];

        if (!MenuPrefab)
        {
            Debug.LogWarning("Unable to show Activity Menu. No menu prefab set.");
            return ShowResult.FAIL;
        }

        if (_control == null)
            _control = new Control(MenuPrefab);

        var result = _control.Show();
        if (result == ShowResult.FAIL)
        {
            Debug.LogWarning("Unable to show Activity Menu.");
            return result;
        }

        if (result == ShowResult.FIRST)
        {
            _hook = _control.Instance.GetComponent<ActivityMenuHook>();
        }

        _hook.Title = activity["Name"];
        _hook.OnConfirm = () =>
        {
            // TODO Set hook variables.
            Debug.Log("On Confirm pressed");
            Disable();
        };

        if (_hook.CheckpointsTab != null)
        {
            foreach (var checkpointPair in activity["Checkpoints"].AsArray)
            {
                var checkpoint = checkpointPair.Value;
                Checkpoint.CType type;
                if (EnumUtils.TryParse(checkpoint["Type"], out type))
                {
                    float? score = null;
                    if (checkpoint["EvaluationScore"] != null)
                    {
                        score = checkpoint["EvaluationScore"];
                    }
                    _hook.CheckpointsTab.AddCheckpoint(new Checkpoint
                    {
                        Type = type,
                        Name = checkpoint["Name"],
                        Date = DateTime.Now, // TODO Convert string date to date
                        Effort = checkpoint["Effort"],
                        Importance = checkpoint["Importance"],
                        CheckboxDone = checkpoint["CheckboxDone"] ?? false,
                        EvaluationScore = score
                    });
                }
            }
        }

        if (_hook.MetricsTab != null)
        {
            _hook.MetricsTab.OnEaseSlider = value => Debug.Log("Ease Slider: " + value);
            _hook.MetricsTab.OnImportanceSlider = value => Debug.Log("Importance Slider: " + value);
            _hook.MetricsTab.OnLikeSlider = value => Debug.Log("Like Slider: " + value);
        }

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
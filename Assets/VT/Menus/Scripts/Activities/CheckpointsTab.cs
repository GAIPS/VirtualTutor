using System.Collections.Generic;
using HookControl;
using UnityEngine;
using VT;

public class CheckpointsTab : MonoBehaviour
{
    [SerializeField] private GameObject _checkpointPrefab;
    [SerializeField] private GameObject _listObject;

    public List<Control> CheckpointsControls { get; set; }

    public void AddCheckpoint(Checkpoint checkpoint)
    {
        Control buttonControl = new Control(_checkpointPrefab);
        ShowResult result = buttonControl.Show();
        if (result == ShowResult.FIRST)
        {
            if (CheckpointsControls == null)
                CheckpointsControls = new List<Control>();
            CheckpointsControls.Add(buttonControl);
            if (_listObject)
            {
                buttonControl.instance.transform.SetParent(_listObject.transform);
            }

            CheckpointHook checkpointHook = buttonControl.instance.GetComponent<CheckpointHook>();
            if (checkpointHook != null)
            {
                checkpointHook.Set(checkpoint);
            }
            else
            {
                Debug.LogWarning("No Checkpoint Hook found.");
            }
        }
    }
}
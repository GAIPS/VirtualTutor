using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

[RequireComponent(typeof(AnimationHook))]
public class InputIdMenu : MonoBehaviour
{
    private int _id;

    [SerializeField] private GameObject _confirmButton;

    public void OnChange(string idString)
    {
        if (string.IsNullOrEmpty(idString))
        {
            if (_confirmButton) _confirmButton.SetActive(false);
            return;
        }

        if (_confirmButton) _confirmButton.SetActive(true);

        _id = Convert.ToInt32(idString);
    }

    public void OnConfirm()
    {
        // Save value to history
        if (SystemManager.instance != null)
        {
            var state = PersistentDataStorage.Instance.GetState();

            state["UserID"] = _id;
            PersistentDataStorage.Instance.SaveState();

            Debug.Log("User ID: " + state["UserID"]);
        }

        GetComponent<AnimationHook>().Hide();
    }

    private void Start()
    {
        if (_confirmButton)
        {
            _confirmButton.SetActive(false);
        }
    }
}
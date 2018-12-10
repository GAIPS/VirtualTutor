using System;
using UnityEngine;

[RequireComponent(typeof(AnimationHook))]
public class InputVisitsMenu : MonoBehaviour
{
    private float _visits;

    [SerializeField] private GameObject _confirmButton;

    public void OnChange(string gradeString)
    {
        if (string.IsNullOrEmpty(gradeString))
        {
            if (_confirmButton) _confirmButton.SetActive(false);
            return;
        }

        if (_confirmButton) _confirmButton.SetActive(true);

        _visits = Convert.ToSingle(gradeString);
        if (_visits > 50)
        {
            _visits = 50;
        }
        else if (_visits < 0)
        {
            _visits = 0;
        }
    }

    public void OnConfirm()
    {
        // Save value to history
        if (SystemManager.instance != null)
        {
            var state = PersistentDataStorage.Instance.GetState();

            state["Visits"].AsArray.Add(_visits);
            PersistentDataStorage.Instance.SaveState();
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
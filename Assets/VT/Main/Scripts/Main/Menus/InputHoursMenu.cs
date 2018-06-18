using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationHook))]
public class InputHoursMenu : MonoBehaviour
{
    private float _hours;

    [SerializeField] private GameObject _confirmButton;

    public void OnChange(string gradeString)
    {
        if (string.IsNullOrEmpty(gradeString))
        {
            if (_confirmButton) _confirmButton.SetActive(false);
            return;
        }

        if (_confirmButton) _confirmButton.SetActive(true);

        _hours = Convert.ToSingle(gradeString);
        if (_hours > 16)
        {
            _hours = 16;
        }
        else if (_hours < 0)
        {
            _hours = 0;
        }
    }

    public void OnConfirm()
    {
        // Save value to history
        if (SystemManager.instance != null)
        {
            History history = SystemManager.instance.History;

            List<float> hours = history.Get<List<float>>("hours");
            if (hours == null)
            {
                hours = new List<float>();
                history.Set("hours", hours);
            }

            hours.Add(_hours);


            List<float> test = history.Get<List<float>>("hours");
            foreach (var f in test)
            {
                Debug.Log("Hour log: " + f);
            }
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
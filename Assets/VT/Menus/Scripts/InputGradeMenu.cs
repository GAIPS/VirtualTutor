using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

[RequireComponent(typeof(AnimationHook))]
public class InputGradeMenu : MonoBehaviour
{
    private float _grade;

    [SerializeField] private GameObject _confirmButton;

    public void OnChange(string gradeString)
    {
        if (string.IsNullOrEmpty(gradeString))
        {
            if (_confirmButton) _confirmButton.SetActive(false);
            return;
        }

        if (_confirmButton) _confirmButton.SetActive(true);

        _grade = Convert.ToSingle(gradeString);
        if (_grade > 20)
        {
            _grade = 20;
        }
        else if (_grade < 0)
        {
            _grade = 0;
        }
    }

    public void OnConfirm()
    {
        // Save value to history
        if (SystemManager.instance != null)
        {
            var state = PersistentDataStorage.Instance.GetState();

            state["Grades"].AsArray.Add(_grade);
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
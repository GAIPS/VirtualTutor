using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AnimationHook))]
public class InputSliderMenu : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private string _fieldName = "Default";

    public void OnConfirm()
    {
        // Save value to history
        if (SystemManager.instance != null)
        {
            var state = PersistentDataStorage.Instance.GetState();

            var values = state["DailyTask"].AsObject["InputSubjective"].AsObject[DateTime.Now.ToShortDateString()].AsObject;
            values[_fieldName] = Convert.ToInt32(_slider.value);

            PersistentDataStorage.Instance.SaveState();
        }

        GetComponent<AnimationHook>().Hide();
    }

    private void Start()
    {
        if (_slider == null)
        {
            Debug.LogWarning("Some prefabs are empty.");
            return;
        }

        var state = PersistentDataStorage.Instance.GetState();
        var emotionCoherence = state["DailyTask"].AsObject["InputSubjective"].AsObject;
        for (int i = emotionCoherence.Count - 1; i >= 0; i--)
        {
            JSONNode values = emotionCoherence[i];
            if (values[_fieldName] != null)
            {
                _slider.value = values[_fieldName];
                break;
            }
        }
    }
}
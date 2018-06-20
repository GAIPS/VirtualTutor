using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AnimationHook))]
public class InputSubjectiveMenu : MonoBehaviour
{
    [SerializeField] private Slider _sliderChallenge;
    [SerializeField] private Slider _sliderEnjoyment;
    [SerializeField] private Slider _sliderImportance;

    public void OnConfirm()
    {
        // Save value to history
        if (SystemManager.instance != null)
        {
            var state = PersistentDataStorage.Instance.GetState();

            JSONObject values = new JSONObject();
            values["Challenge"] = Convert.ToInt32(_sliderChallenge.value);
            values["Enjoyment"] = Convert.ToInt32(_sliderEnjoyment.value);
            values["Importance"] = Convert.ToInt32(_sliderImportance.value);
            state["InputSubjective"].AsObject[DateTime.Now.ToShortDateString()] = values;

            PersistentDataStorage.Instance.SaveState();
        }

        GetComponent<AnimationHook>().Hide();
    }

    private void Start()
    {
        if (_sliderChallenge == null || _sliderEnjoyment == null || _sliderImportance == null)
        {
            Debug.LogWarning("Some prefabs are empty.");
            return;
        }

        var state = PersistentDataStorage.Instance.GetState();
        var emotionCoherence = state["InputSubjective"].AsObject;
        if (emotionCoherence.Count > 0 && emotionCoherence[emotionCoherence.Count - 1] != null)
        {
            JSONNode values = emotionCoherence[emotionCoherence.Count - 1];
            _sliderChallenge.value = values["Challenge"];
            _sliderEnjoyment.value = values["Enjoyment"];
            _sliderImportance.value = values["Importance"];
        }
    }
}
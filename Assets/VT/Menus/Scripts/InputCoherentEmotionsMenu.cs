using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AnimationHook))]
public class InputCoherentEmotionsMenu : MonoBehaviour
{
    private int _emotion;

    [SerializeField] private Slider _slider;

    public void OnChange(float value)
    {
        _emotion = Convert.ToInt32(value);
    }

    public void OnConfirm()
    {
        // Save value to history
        if (SystemManager.instance != null)
        {
            var state = PersistentDataStorage.Instance.GetState();

            state["EmotionCoherence"].AsObject[DateTime.Now.ToShortDateString()] = _emotion;

            PersistentDataStorage.Instance.SaveState();
        }

        GetComponent<AnimationHook>().Hide();
    }

    private void Start()
    {
        if (_slider != null)
        {
            var state = PersistentDataStorage.Instance.GetState();
            var emotionCoherence = state["EmotionCoherence"].AsObject;
            if (emotionCoherence.Count > 0 && emotionCoherence[emotionCoherence.Count - 1] != null )
            {
                _slider.value = emotionCoherence[emotionCoherence.Count - 1];
            }
            OnChange(_slider.value);
        }
    }
}
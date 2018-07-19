using System.Collections;
using System.Collections.Generic;
using HookControl;
using UnityEngine;
using UnityEngine.UI;

public class MetricsTab : MonoBehaviour
{
    [SerializeField] private Slider _easySlider;
    [SerializeField] private Slider _likeSlider;
    [SerializeField] private Slider _importanceSlider;
    private FloatFunc _onLikeSlider;
    private FloatFunc _onEaseSlider;
    private FloatFunc _onImportanceSlider;

    public FloatFunc OnEaseSlider
    {
        get { return _onEaseSlider; }
        set
        {
            _onEaseSlider = value;
            if (_easySlider && OnEaseSlider != null)
            {
                _easySlider.onValueChanged.RemoveAllListeners();
                _easySlider.onValueChanged.AddListener(listener => OnEaseSlider(listener));
            }
        }
    }


    public FloatFunc OnLikeSlider
    {
        get { return _onLikeSlider; }
        set
        {
            _onLikeSlider = value;

            if (_likeSlider && OnLikeSlider != null)
            {
                _likeSlider.onValueChanged.RemoveAllListeners();
                _likeSlider.onValueChanged.AddListener(listener => OnLikeSlider(listener));
            }
        }
    }

    public FloatFunc OnImportanceSlider
    {
        get { return _onImportanceSlider; }
        set
        {
            _onImportanceSlider = value;
            if (_importanceSlider && OnImportanceSlider != null)
            {
                _importanceSlider.onValueChanged.RemoveAllListeners();
                _importanceSlider.onValueChanged.AddListener(listener => OnImportanceSlider(listener));
            }
        }
    }
}
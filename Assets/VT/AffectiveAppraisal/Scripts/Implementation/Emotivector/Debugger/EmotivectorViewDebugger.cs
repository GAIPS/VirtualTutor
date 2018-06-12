using System;
using System.Collections.Generic;
using System.Linq;
using HookControl;
using UnityEngine;
using UnityEngine.UI;

public class EmotivectorViewDebugger : MonoBehaviour
{
    [SerializeField] private GameObject _valuePrefab;
    [SerializeField] private GameObject _expectedValuePrefab;

    [SerializeField] private RectTransform _content;

    [SerializeField] private InputField _valueInput;
    [SerializeField] private Button _addPredictButton;

    [SerializeField] private Text _expectancyLog;

    private Emotivector _emotivector;

    public void SetEmotivector(Emotivector emotivector)
    {
        _emotivector = emotivector;
        UpdateGraph();
    }

    public void UpdateGraph()
    {
        if (_emotivector == null || _content == null || _valuePrefab == null || _expectedValuePrefab == null) return;

        ClearTransform(_content.transform);
        _content.sizeDelta = Vector2.zero;

        float x = 0;
        float width = 0;
        List<float> values = _emotivector.GetValues(),
            predictions = _emotivector.GetPredictions();
        foreach (float value in values)
        {
            AddElement(_valuePrefab, x, value, out width);

            x += width * 1.5f;
        }

        x = width * 1.5f;
        foreach (float prediction in predictions)
        {
            AddElement(_expectedValuePrefab, x, prediction, out width);

            x += width * 1.5f;
        }

        ResizeContentWidth(x);
    }

    private void ResizeContentWidth(float x)
    {
        if (x > _content.rect.width)
        {
            _content.sizeDelta = new Vector2 {x = x - _content.rect.width, y = _content.sizeDelta.y};
        }
    }

    public void AddElement(GameObject prefab, float x, float y, out float width)
    {
        Control control = new Control(prefab);
        if (control.Show() == ShowResult.FIRST)
        {
            control.instance.transform.SetParent(_content);
            var rect = control.instance.GetComponent<RectTransform>();

            float yUi = (_content.rect.height - rect.rect.height) * y;

            rect.anchoredPosition = new Vector2 {x = x, y = yUi};

            width = rect.rect.width;
        }
        else
            width = 0;
    }

    private void ClearTransform(Transform contentTransform)
    {
        foreach (Transform child in contentTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        if (_valueInput == null || _expectedValuePrefab == null || _content == null || _valueInput == null ||
            _addPredictButton == null || _expectancyLog == null)
        {
            Debug.LogWarning("Some of the needed prefabs or components are NULL. This object may not work properly!");
        }

        if (_addPredictButton != null)
        {
            _addPredictButton.onClick.AddListener(delegate
            {
                if (_emotivector == null)
                {
                    Debug.LogWarning("Emotivector is NULL");
                    return;
                }

                float value = Convert.ToSingle(_valueInput.text);
                _emotivector.AddValue(value);
                _expectancyLog.text = _emotivector.ComputeExpectancy().ToString();
                _emotivector.Predict();
                UpdateGraph();

                Debug.Log("Last Prediction Value: " + _emotivector.GetPredictions().Last());
            });
        }
    }
}
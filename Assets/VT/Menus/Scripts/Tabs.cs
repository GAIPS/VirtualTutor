using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tabs : MonoBehaviour
{
    [SerializeField] private List<Toggle> _toggles;
    [SerializeField] private List<RectTransform> _tabObjects;

    private int _activeIndex = -1;
    private Coroutine _activeCoroutine;

    private Vector2 _anchor;
    
    public float Speed = 2;

    void Start()
    {
        for (int i = 0; i < _toggles.Count; i++)
        {
            var i1 = i;
            _toggles[i].onValueChanged.AddListener(toggle =>
            {
                if (toggle)
                {
                    OnTabClick(i1);
                }
            });
        }

        _anchor = _tabObjects[0].anchorMin;
        _activeIndex = 0;
    }

    public void SwitchTab(int index)
    {
        if (_toggles.Count <= index) return;
        _toggles[index].isOn = true;
    }

    private void OnTabClick(int index)
    {
        if (_activeIndex == index) return;

        if (_tabObjects == null || _tabObjects.Count <= index) return;

        RectTransform previousTab = _tabObjects[_activeIndex];
        _activeIndex = index;
        RectTransform selectedTab = _tabObjects[_activeIndex];

        UpdatePosition(selectedTab, previousTab);
    }

    private void UpdatePosition(RectTransform selectedTab, RectTransform previousTab)
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
        }

        _activeCoroutine = StartCoroutine(UpdatePositionLerp(_anchor, selectedTab, previousTab));
    }

    private IEnumerator UpdatePositionLerp(Vector2 anchor, RectTransform active, RectTransform previous)
    {
        while (Math.Abs((active.anchorMin - anchor).magnitude) > 0.01f)
        {
            var deltaActive = active.anchorMax - active.anchorMin;
            var deltaPrevious = previous.anchorMax - previous.anchorMin;

            var selectedTabPos = active.anchorMin;
            active.anchorMin = Vector2.Lerp(active.anchorMin, anchor, Time.deltaTime * Speed);
            previous.anchorMin = active.anchorMin - (selectedTabPos - previous.anchorMin);

            active.anchorMax = active.anchorMin + deltaActive;
            previous.anchorMax = previous.anchorMin + deltaPrevious;
            yield return null;
        }

        _activeCoroutine = null;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tabs : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _tabObjects;

    private int _activeIndex = -1;
    private Coroutine _activeCoroutine;

    private Vector2 _anchor;

    public void OnToggleSwitched(int index)
    {
        if (_tabObjects == null || _tabObjects.Count <= index) return;


        if (_activeIndex < 0)
        {
            _anchor = _tabObjects[0].anchorMin;
            _activeIndex = 0;
        }

        if (_activeIndex == index) return;

        RectTransform previousTab = _tabObjects[_activeIndex];
        _activeIndex = index;

        RectTransform selectedTab = _tabObjects[index];

        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
        }

        _activeCoroutine = StartCoroutine(UpdatePosition(_anchor, selectedTab, previousTab));
    }

    private IEnumerator UpdatePosition(Vector2 anchor, RectTransform active, RectTransform previous)
    {
        while (Math.Abs((active.anchorMin - anchor).magnitude) > 0.01f)
        {
            var deltaActive = active.anchorMax - active.anchorMin;
            var deltaPrevious = previous.anchorMax - previous.anchorMin;

            var selectedTabPos = active.anchorMin;
            active.anchorMin = Vector2.Lerp(active.anchorMin, anchor, Time.time * .05f);
            previous.anchorMin = active.anchorMin - (selectedTabPos - previous.anchorMin);

            active.anchorMax = active.anchorMin + deltaActive;
            previous.anchorMax = previous.anchorMin + deltaPrevious;
            yield return null;
        }

        _activeCoroutine = null;
    }
}
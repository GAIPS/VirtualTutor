using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public struct TextEffectData : IBubbleSystemData
    {
        public BalloonsHooks hooks;
        public AnimationCurve curve;
        public float intensity;
        public float duration;
        public bool show;

        public void Clear()
        {
            curve = null;
            intensity = 0.0f;
            duration = 0.0f;
            show = false;
        }

        public bool IsCleared()
        {
            return hooks == null && curve == null && intensity.Equals(0.0f) && duration.Equals(0.0f) && show.Equals(true);
        }
    }
}
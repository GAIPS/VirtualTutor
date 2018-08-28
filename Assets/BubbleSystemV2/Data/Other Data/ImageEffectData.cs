using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public struct ImageEffectData : IBubbleSystemData
    {
        public Renderer renderer;
        public float duration;
        public Color colorToLerpTo;
        public Texture2D textureData;
        public AnimationCurve curve;

        public void Clear()
        {
            curve = null;
            renderer = null;
            duration = 0.0f;
            colorToLerpTo = DefaultData.Instance.noColor;
            textureData = null;

        }

        public bool IsCleared()
        {
            return curve == null && renderer == null && duration.Equals(0.0f) && colorToLerpTo.Equals(DefaultData.Instance.noColor) && textureData == null;
        }
    }
}
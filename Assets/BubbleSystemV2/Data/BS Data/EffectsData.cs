using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class EffectsData<T> : AbstractBubbleSystemData
    {
        public Dictionary<T, AnimationCurve> showEffects = new Dictionary<T, AnimationCurve>();
        public Dictionary<T, AnimationCurve> hideEffects = new Dictionary<T, AnimationCurve>();
        public Dictionary<T, AnimationCurve> colorEffects = new Dictionary<T, AnimationCurve>();

        public override void Clear()
        {
            showEffects.Clear();
            hideEffects.Clear();
            colorEffects.Clear();
        }

        public override bool IsCleared()
        {
            return showEffects.Count == 0 && hideEffects.Count == 0 && colorEffects.Count == 0;
        }
    }
}
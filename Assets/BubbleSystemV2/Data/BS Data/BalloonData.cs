using HookControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class BalloonData : AbstractBubbleSystemData
    {
        public List<string> text = new List<string>();
        public EffectsData<AbstractTextEffect.TextEffectEnum> effects = new EffectsData<AbstractTextEffect.TextEffectEnum>();
        public List<IntFunc> callbacks = new List<IntFunc>();
        public bool options = false;
        public bool show = true;

        public override void Clear()
        {
            text.Clear();
            effects.Clear();
            callbacks.Clear();
            options = false;
            show = true;
        }

        public override bool IsCleared()
        {
            return effects.IsCleared() && text.Count == 0 && callbacks.Count == 0;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class StretchEffect : AbstractStretchEffect
    {
        private static readonly StretchEffect instance = new StretchEffect();

        private StretchEffect()
        {
            x = true;
            y = true;
        }

        public static StretchEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetRectTransform(false, true, true);
        }
    }
}
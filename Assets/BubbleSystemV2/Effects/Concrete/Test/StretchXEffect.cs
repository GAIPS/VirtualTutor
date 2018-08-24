using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class StretchXEffect : AbstractStretchEffect
    {
        private static readonly StretchXEffect instance = new StretchXEffect();

        private StretchXEffect()
        {
            x = true;
            y = false;
        }

        public static StretchXEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetRectTransform(false, true, false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class StretchEffect : AbstractStretchEffect
    {
        public StretchEffect()
        {
            x = true;
            y = true;
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetRectTransform(false, true, true);
        }
    }
}
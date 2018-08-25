using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class StretchXEffect : AbstractStretchEffect
    {
        public StretchXEffect()
        {
            x = true;
            y = false;
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetRectTransform(false, true, false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem
{
    public class StretchYEffect : AbstractStretchEffect
    {
        public StretchYEffect()
        {
            x = false;
            y = true;
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetRectTransform(false, false, true);
        }
    }
}
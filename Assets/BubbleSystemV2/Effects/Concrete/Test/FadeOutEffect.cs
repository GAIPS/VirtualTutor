using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class FadeOutEffect : AbstractFadeEffect
    {
        public FadeOutEffect() {
            wantedAlpha = 0;
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(true, false);
        }
    }
}
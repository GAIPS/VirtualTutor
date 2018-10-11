using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem
{
    public class FadeOutCharactersEffect : AbstractFadeCharactersEffect
    {
        public FadeOutCharactersEffect() {
            wantedAlpha = 0;
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(true, true);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class FadeInCharactersEffect : AbstractFadeCharactersEffect
    {
        public FadeInCharactersEffect() {
            wantedAlpha = 255;
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(false, true);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class FadeOutCharactersEffect : AbstractFadeCharactersEffect
    {
        private static readonly FadeOutCharactersEffect instance = new FadeOutCharactersEffect();

        private FadeOutCharactersEffect() {
            wantedAlpha = 0;
        }

        public static FadeOutCharactersEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(true, true);
        }
    }
}
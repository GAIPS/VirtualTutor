using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class FadeInCharactersEffect : AbstractFadeCharactersEffect
    {
        private static readonly FadeInCharactersEffect instance = new FadeInCharactersEffect();

        private FadeInCharactersEffect() {
            wantedAlpha = 255;
        }

        public static FadeInCharactersEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(false, true);
        }
    }
}
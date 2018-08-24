using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class FadeOutEffect : AbstractFadeEffect
    {
        private static readonly FadeOutEffect instance = new FadeOutEffect();

        protected FadeOutEffect() {
            wantedAlpha = 0;
        }

        public static FadeOutEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(true, false);
        }
    }
}
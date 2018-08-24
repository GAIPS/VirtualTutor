using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class FadeInEffect : AbstractFadeEffect
    {
        private static readonly FadeInEffect instance = new FadeInEffect();

        private FadeInEffect() {
            wantedAlpha = 255;
        }

        public static FadeInEffect Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class WaveEffect : AbstractWaveEffect
    {
        private static readonly WaveEffect instance = new WaveEffect();

        private WaveEffect() {
            characters = false;
        }

        public static WaveEffect Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
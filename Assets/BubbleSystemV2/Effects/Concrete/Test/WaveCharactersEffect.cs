using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class WaveCharactersEffect : AbstractWaveEffect
    {
        private static readonly WaveCharactersEffect instance = new WaveCharactersEffect();

        private WaveCharactersEffect()
        {
            characters = true;
        }

        public static WaveCharactersEffect Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
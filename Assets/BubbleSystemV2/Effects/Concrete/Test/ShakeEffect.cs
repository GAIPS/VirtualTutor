using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class ShakeEffect : AbstractShakeEffect
    {
        private static readonly ShakeEffect instance = new ShakeEffect();

        private ShakeEffect()
        {
            characters = false;
        }

        public static ShakeEffect Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
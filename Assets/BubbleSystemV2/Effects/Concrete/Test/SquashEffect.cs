using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class SquashEffect : AbstractSquashEffect
    {
        private static readonly SquashEffect instance = new SquashEffect();

        private SquashEffect()
        {
            x = true;
            y = true;
        }

        public static SquashEffect Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
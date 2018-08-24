using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class SquashXEffect : AbstractSquashEffect
    {
        private static readonly SquashXEffect instance = new SquashXEffect();

        private SquashXEffect()
        {
            x = true;
            y = false;
        }

        public static SquashXEffect Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
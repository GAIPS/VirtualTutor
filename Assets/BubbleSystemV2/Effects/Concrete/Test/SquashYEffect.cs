using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class SquashYEffect : AbstractSquashEffect
    {
        private static readonly SquashYEffect instance = new SquashYEffect();

        private SquashYEffect()
        {
            x = false;
            y = true;
        }

        public static SquashYEffect Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
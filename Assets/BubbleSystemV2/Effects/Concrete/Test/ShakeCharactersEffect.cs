using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class ShakeCharactersEffect : AbstractShakeEffect
    {
        private static readonly ShakeCharactersEffect instance = new ShakeCharactersEffect();

        private ShakeCharactersEffect()
        {
            characters = true;
        }

        public static ShakeCharactersEffect Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
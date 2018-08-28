using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public abstract class AbstractImageEffect : AbstractEffect
    {
        //Should be alphabetically sorted
        public enum ImageEffectEnum
        {
            FadeTexture,
            FadeColor
        }

        public abstract IEnumerator Run(ImageEffectData data);
    }
}
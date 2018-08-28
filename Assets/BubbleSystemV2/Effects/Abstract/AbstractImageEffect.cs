using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public abstract class AbstractImageEffect : AbstractEffect
    {
        public enum ImageEffectEnum
        {
            FadeTexture,
            FadeColor
        }

        public abstract IEnumerator Run(ImageEffectData data);
    }
}
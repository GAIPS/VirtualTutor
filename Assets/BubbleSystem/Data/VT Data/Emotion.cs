using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class Emotion : AbstractVTData
    {
        public enum EmotionEnum
        {
            Neutral,
            Happiness,
            Sadness,
            Anger,
            Fear,
            Disgust,
            Surprise
        }

        public EmotionEnum Get()
        {
            return base.Get<EmotionEnum>(this.GetType().Name);
        }

        public void Set(string value)
        {
            base.Set<EmotionEnum>(value);
        }

        public void Set(EmotionEnum value)
        {
            Set(value.ToString());
        }

        public override string GetString()
        {
            return Get().ToString();
        }
    }
}
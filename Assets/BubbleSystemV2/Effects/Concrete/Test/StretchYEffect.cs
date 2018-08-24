using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class StretchYEffect : AbstractStretchEffect
    {
        private static readonly StretchYEffect instance = new StretchYEffect();

        private StretchYEffect()
        {
            x = false;
            y = true;
        }

        public static StretchYEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetRectTransform(false, false, true);
        }
    }
}
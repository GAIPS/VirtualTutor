using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class BlushEffect : AbstractTextEffect
    {
        private static readonly BlushEffect instance = new BlushEffect();

        private BlushEffect() { }

        public static BlushEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(false, false);
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            data.hooks.textData.m_TextComponent.ForceMeshUpdate();

            Color32 initialColor = data.hooks.textData.m_TextComponent.color, finalColor;
            int red, green, blue, alpha;

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            Color blushColor = DefaultData.Instance.GetBlushColor();

            while (((Time.time - initialTime) / data.duration) < 1)
            {
                yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));

                red = (int)(initialColor.r + yValue * (((byte)(blushColor.r * 255)) - initialColor.r));
                green = (int)(initialColor.g + yValue * (((byte)(blushColor.g * 255)) - initialColor.g));
                blue = (int)(initialColor.b + yValue * (((byte)(blushColor.b * 255)) - initialColor.b));
                alpha = (int)(initialColor.a + yValue * (((byte)(blushColor.a * 255)) - initialColor.a));

                finalColor.r = (byte)red;
                finalColor.g = (byte)green;
                finalColor.b = (byte)blue;
                finalColor.a = (byte)alpha;

                data.hooks.textData.m_TextComponent.color = finalColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
                data.hooks.textData.m_TextComponent.color = blushColor;
        }
    }
}
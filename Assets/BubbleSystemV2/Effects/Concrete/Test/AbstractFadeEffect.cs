using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public abstract class AbstractFadeEffect : AbstractTextEffect
    {
        protected AbstractFadeEffect()
        {
            wantedAlpha = 0;
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

            int finalAlpha;
            Color32 finalColor = data.hooks.textData.m_TextComponent.color;
            int initialAlpha = finalColor.a;

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            while (((Time.time - initialTime) / data.duration) < 1)
            {
                yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));

                finalAlpha = (int)(initialAlpha + yValue * (wantedAlpha - initialAlpha));
                finalColor.a = (byte)finalAlpha;

                data.hooks.textData.m_TextComponent.color = finalColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
            {
                finalColor.a = (byte)wantedAlpha;
                data.hooks.textData.m_TextComponent.color = finalColor;
            }
        }
    }
}
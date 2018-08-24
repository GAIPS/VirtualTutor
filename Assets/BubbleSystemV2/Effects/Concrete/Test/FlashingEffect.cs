using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class FlashingEffect : AbstractTextEffect
    {
        private static readonly FlashingEffect instance = new FlashingEffect();

        private FlashingEffect() {
            keepAnimating = false;
        }

        public static FlashingEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(true, false);
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            data.hooks.textData.m_TextComponent.ForceMeshUpdate();
            data.curve.preWrapMode = WrapMode.Loop;
            data.curve.postWrapMode = WrapMode.Loop;

            int totalVisibleCharacters = data.hooks.textData.m_TextComponent.textInfo.characterCount; // Get # of Visible Character in text object
            int visibleCount = 0;

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            while (((Time.time - initialTime) / data.duration) < 1 || keepAnimating)
            {
                yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));
                if (yValue >= 1)
                    visibleCount = totalVisibleCharacters;
                else if (yValue <= 0)
                    visibleCount = 0;
                data.hooks.textData.m_TextComponent.maxVisibleCharacters = visibleCount;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
                data.hooks.textData.m_TextComponent.maxVisibleCharacters = totalVisibleCharacters;
        }
    }
}
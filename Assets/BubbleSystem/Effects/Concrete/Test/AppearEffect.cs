using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BubbleSystem
{
    public class AppearEffect : AbstractTextEffect
    {
        public AppearEffect() {
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetCharacterCount();
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);

            data.hooks.textData.m_TextComponent.ForceMeshUpdate();
            int totalVisibleCharacters = data.hooks.textData.m_TextComponent.textInfo.characterCount; // Get # of Visible Character in text object
            int visibleCount = 0;

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            while ((Time.time - initialTime) / data.duration < 1)
            {
                yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));
                visibleCount = (int)(yValue * totalVisibleCharacters);
                data.hooks.textData.m_TextComponent.maxVisibleCharacters = visibleCount;

                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
                data.hooks.textData.m_TextComponent.maxVisibleCharacters = totalVisibleCharacters;
        }
    }
}
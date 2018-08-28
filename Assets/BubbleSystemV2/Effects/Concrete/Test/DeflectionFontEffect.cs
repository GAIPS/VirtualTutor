using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem
{
    public class DeflectionFontEffect : AbstractTextEffect
    {
        public DeflectionFontEffect() {
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.text.enableAutoSizing = false;
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            data.hooks.textData.m_TextComponent.ForceMeshUpdate();

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            float size;
            float initialSize = data.hooks.textData.m_TextComponent.fontSize;
            float finalSize = 0f;

            while (((Time.time - initialTime) / data.duration) < 1)
            {
                yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));

                size = initialSize + yValue * (finalSize - initialSize);

                data.hooks.textData.m_TextComponent.fontSize = size;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
                data.hooks.textData.m_TextComponent.fontSize = finalSize;
        }
    }
}
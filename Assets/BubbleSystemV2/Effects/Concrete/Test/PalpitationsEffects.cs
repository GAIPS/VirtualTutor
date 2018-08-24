using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class PalpitationsEffect : AbstractTextEffect
    {
        private static readonly PalpitationsEffect instance = new PalpitationsEffect();

        private PalpitationsEffect() {
            keepAnimating = false;
        }

        public static PalpitationsEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetRectTransform(true, false, false);
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            data.hooks.textData.m_TextComponent.ForceMeshUpdate();
            data.curve.preWrapMode = WrapMode.Loop;
            data.curve.postWrapMode = WrapMode.Loop;
            
            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            Vector3 localScale = data.hooks.textData.rectTransform.localScale;
            Vector3 finalScale = data.hooks.textData.rectTransform.localScale * 1.5f;
            Vector3 stepScale;

            while (((Time.time - initialTime) / data.duration) < 1 || keepAnimating)
            {
                yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));

                stepScale = localScale + yValue * (finalScale - localScale);

                data.hooks.textData.rectTransform.localScale = stepScale;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
                data.hooks.textData.rectTransform.localScale = localScale;
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public abstract class AbstractStretchEffect : AbstractTextEffect
    {
        protected bool x = false, y = false;
        protected float wantedScaleX = 1.0f, wantedScaleY = 1.0f;

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            data.hooks.textData.m_TextComponent.ForceMeshUpdate();

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            Vector3 finalScale = data.hooks.textData.rectTransform.localScale;

            Vector3 zeroScale = Vector3.zero;
            data.hooks.textData.rectTransform.localScale = zeroScale;
            Vector3 localScale = data.hooks.textData.rectTransform.localScale;

            float initialScaleX = localScale.x;
            float initialScaleY = localScale.y;

            while (((Time.time - initialTime) / data.duration) < 1)
            {
                yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));

                if (x)
                    finalScale.x = initialScaleX + yValue * (wantedScaleX - initialScaleX);
                if (y)
                    finalScale.y = initialScaleY + yValue * (wantedScaleY - initialScaleY);

                data.hooks.textData.rectTransform.localScale = finalScale;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
            {
                if (x)
                    finalScale.x = wantedScaleX;
                if (y)
                    finalScale.y = wantedScaleY;
                data.hooks.textData.rectTransform.localScale = finalScale;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class FadeTextureEffect : AbstractImageEffect
    {
        private static readonly FadeTextureEffect instance = new FadeTextureEffect();

        private FadeTextureEffect() { }

        public static FadeTextureEffect Instance
        {
            get
            {
                return instance;
            }
        }

        public override IEnumerator Run(ImageEffectData data)
        {
            float wantedAlpha = data.colorToLerpTo.a;
            float finalAlpha;
            float initialAlpha = data.renderer.material.color.a;
            Color finalColor = data.renderer.material.color;

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            while (((Time.time - initialTime) / data.duration) < 1)
            {
                yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));

                finalAlpha = initialAlpha + yValue * (wantedAlpha - initialAlpha);
                finalColor.a = finalAlpha;

                for (int i = 0; i < data.renderer.materials.Length; i++)
                    data.renderer.materials[i].color = finalColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            finalColor.a = wantedAlpha;
            for (int i = 0; i < data.renderer.materials.Length; i++)
                data.renderer.materials[i].color = finalColor;
            data.renderer.material.color = finalColor;
        }
    }
}
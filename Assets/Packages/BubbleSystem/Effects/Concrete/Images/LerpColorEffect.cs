using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class LerpColorEffect : AbstractImageEffect
    {
        public LerpColorEffect() { }

        public override IEnumerator Run(ImageEffectData data)
        {
            Color32 initialColor = data.renderer.material.color;
            Color finalColor;
            int red, green, blue, alpha;

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            while (((Time.time - initialTime) / data.duration) < 1)
            {
                yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));

                red = (int)(initialColor.r + yValue * (((byte)(data.colorToLerpTo.r * 255)) - initialColor.r));
                green = (int)(initialColor.g + yValue * (((byte)(data.colorToLerpTo.g * 255)) - initialColor.g));
                blue = (int)(initialColor.b + yValue * (((byte)(data.colorToLerpTo.b * 255)) - initialColor.b));
                alpha = (int)(initialColor.a + yValue * (((byte)(data.colorToLerpTo.a * 255)) - initialColor.a));

                finalColor.r = (byte)red;
                finalColor.g = (byte)green;
                finalColor.b = (byte)blue;
                finalColor.a = (byte)alpha;

                for (int i = 0; i < data.renderer.materials.Length; i++)
                    data.renderer.materials[i].color = finalColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            for (int i = 0; i < data.renderer.materials.Length; i++)
                data.renderer.materials[i].color = data.colorToLerpTo;
        }
    }
}
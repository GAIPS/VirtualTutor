using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem
{
    public class BlushCharactersEffect : AbstractTextEffect
    {
        public BlushCharactersEffect() {
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(false, true);
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            data.hooks.textData.m_TextComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = data.hooks.textData.m_TextComponent.textInfo;
            Color32[] newVertexColors;
            Color32 initialColor;
            Color finalColor;
            int red, green, blue, alpha;

            int characterCount = textInfo.characterCount;

            float initialCharTime;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            Color blushColor = DefaultData.Instance.GetBlushColor();

            for (int i = 0; i < characterCount; i++)
            {
                initialCharTime = Time.time;

                initialColor = data.hooks.textData.m_TextComponent.textInfo.characterInfo[i].color;
                // Skip characters that are not visible
                if (!textInfo.characterInfo[i].isVisible) continue;

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                while (((Time.time - initialCharTime) * characterCount / data.duration) < 1)
                {
                    yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialCharTime) * lastKeyTime * characterCount / data.duration));

                    red = (int)(initialColor.r + yValue * (((byte)(blushColor.r * 255)) - initialColor.r));
                    green = (int)(initialColor.g + yValue * (((byte)(blushColor.g * 255)) - initialColor.g));
                    blue = (int)(initialColor.b + yValue * (((byte)(blushColor.b * 255)) - initialColor.b));
                    alpha = (int)(initialColor.a + yValue * (((byte)(blushColor.a * 255)) - initialColor.a));

                    finalColor.r = (byte)red;
                    finalColor.g = (byte)green;
                    finalColor.b = (byte)blue;
                    finalColor.a = (byte)alpha;

                    newVertexColors[vertexIndex + 0] = finalColor;
                    newVertexColors[vertexIndex + 1] = finalColor;
                    newVertexColors[vertexIndex + 2] = finalColor;
                    newVertexColors[vertexIndex + 3] = finalColor;
                    data.hooks.textData.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                if (DefaultData.Instance.forceTextUpdate)
                {
                    newVertexColors[vertexIndex + 0] = blushColor;
                    newVertexColors[vertexIndex + 1] = blushColor;
                    newVertexColors[vertexIndex + 2] = blushColor;
                    newVertexColors[vertexIndex + 3] = blushColor;
                    data.hooks.textData.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                }
            }
        }
    }
}
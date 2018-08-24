using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class AbstractFadeCharactersEffect : AbstractTextEffect
    {
        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            TMP_TextInfo textInfo = data.hooks.textData.m_TextComponent.textInfo;
            Color32[] newVertexColors;
            Color32 finalColor = data.hooks.textData.m_TextComponent.color;
            float initialAlpha;
            int alpha;

            int characterCount = textInfo.characterCount;

            float initialCharTime;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            for (int i = 0; i < characterCount; i++)
            {
                initialCharTime = Time.time;
                initialAlpha = data.hooks.textData.m_TextComponent.textInfo.characterInfo[i].color.a;

                // Skip characters that are not visible
                if (!textInfo.characterInfo[i].isVisible) continue;

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                while ((((Time.time - initialCharTime) * characterCount / data.duration) < 1))
                {
                    yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialCharTime) * lastKeyTime * characterCount / data.duration));

                    alpha = (int)(initialAlpha + yValue * (wantedAlpha - initialAlpha));

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
                    finalColor.a = (byte)wantedAlpha;
                    newVertexColors[vertexIndex + 0] = finalColor;
                    newVertexColors[vertexIndex + 1] = finalColor;
                    newVertexColors[vertexIndex + 2] = finalColor;
                    newVertexColors[vertexIndex + 3] = finalColor;
                    data.hooks.textData.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                }
            }
        }
    }
}
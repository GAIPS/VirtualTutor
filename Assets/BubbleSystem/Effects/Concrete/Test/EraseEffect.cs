using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem
{
    public class EraseEffect : AbstractTextEffect
    {
        public EraseEffect() {
            wantedAlpha = 0;
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
            data.hooks.textData.ResetColor(true, true);
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            // Need to force the text object to be generated so we have valid data to work with right from the start.
            data.hooks.textData.m_TextComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = data.hooks.textData.m_TextComponent.textInfo;
            Color32[] newVertexColors;

            int characterCount = textInfo.characterCount;

            Color32 finalColor = data.hooks.textData.m_TextComponent.color;
            int initialAlpha;
            byte finalAlpha;

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            while (((Time.time - initialTime) / data.duration) < 1)
            {
                for (int i = 0; i < characterCount; i++)
                {
                    initialAlpha = data.hooks.textData.m_TextComponent.textInfo.characterInfo[i].color.a;
                    // Skip characters that are not visible
                    if (!textInfo.characterInfo[i].isVisible) continue;

                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    // Get the vertex colors of the mesh used by this text element (character or sprite).
                    newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;


                    yValue = Mathf.Clamp01(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration));

                    finalAlpha = (byte)(initialAlpha + yValue * (wantedAlpha - initialAlpha));
                    // Set new alpha values.
                    newVertexColors[vertexIndex + 0].a = finalAlpha;
                    newVertexColors[vertexIndex + 1].a = finalAlpha;
                    newVertexColors[vertexIndex + 2].a = finalAlpha;
                    newVertexColors[vertexIndex + 3].a = finalAlpha;

                    // Upload the changed vertex colors to the Mesh.
                    data.hooks.textData.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }

            if (DefaultData.Instance.forceTextUpdate)
            {
                for (int i = 0; i < characterCount; i++)
                {
                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    // Get the vertex colors of the mesh used by this text element (character or sprite).
                    newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    finalAlpha = (byte)wantedAlpha;
                    // Set new alpha values.
                    newVertexColors[vertexIndex + 0].a = finalAlpha;
                    newVertexColors[vertexIndex + 1].a = finalAlpha;
                    newVertexColors[vertexIndex + 2].a = finalAlpha;
                    newVertexColors[vertexIndex + 3].a = finalAlpha;
                    data.hooks.textData.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                }
            }
        }
    }
}
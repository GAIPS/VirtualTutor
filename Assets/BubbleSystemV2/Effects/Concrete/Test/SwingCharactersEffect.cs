using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class SwingCharactersEffect : AbstractTextEffect
    {
        public SwingCharactersEffect()
        {
            keepAnimating = false;
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            Matrix4x4 matrix;
            Vector3[] vertices;
            int loopCount = 0;

            float initialTime = Time.time;

            float[] vertexAnimAngleRanges = new float[1024];
            float[] vertexAnimSpeeds = new float[1024];
            for (int i = 0; i < 1024; i++)
            {
                vertexAnimAngleRanges[i] = Random.Range(1f, 3f) + Random.Range(3f, 8f) * data.intensity;
                vertexAnimSpeeds[i] = Random.Range(0.2f, 0.5f) + Random.Range(0.5f, 3.0f) * data.intensity;
            }

            int characterCount = data.hooks.textData.m_TextComponent.textInfo.characterCount;

            while (((Time.time - initialTime) / data.duration) < 1 || keepAnimating)
            {
                data.hooks.textData.m_TextComponent.ForceMeshUpdate();
                vertices = data.hooks.textData.m_TextComponent.mesh.vertices;

                for (int i = 0; i < characterCount; i++)
                {
                    var charInfo = data.hooks.textData.m_TextComponent.textInfo.characterInfo[i];

                    if (!charInfo.isVisible) continue;

                    int vertexIndex = charInfo.vertexIndex;

                    Vector3 charMidTopLine = new Vector3((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, charInfo.topRight.y);

                    var vertAnimAngle = Mathf.SmoothStep(-vertexAnimAngleRanges[i], vertexAnimAngleRanges[i], Mathf.PingPong(loopCount / 25f * vertexAnimSpeeds[i], 1f));
                    matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, vertAnimAngle), Vector3.one);
                    ApplyMatrix(vertices, vertices, matrix, charMidTopLine, vertexIndex);
                }

                loopCount++;

                for (int i = 0; i < data.hooks.textData.m_TextComponent.textInfo.meshInfo.Length; i++)
                {
                    data.hooks.textData.m_TextComponent.textInfo.meshInfo[i].mesh.vertices = vertices;
                    data.hooks.textData.m_TextComponent.UpdateGeometry(data.hooks.textData.m_TextComponent.textInfo.meshInfo[i].mesh, i);
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
                data.hooks.textData.m_TextComponent.ForceMeshUpdate();
        }
    }
}
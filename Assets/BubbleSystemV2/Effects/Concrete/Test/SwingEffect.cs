﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class SwingEffect : AbstractTextEffect
    {
        private static readonly SwingEffect instance = new SwingEffect();

        private SwingEffect() {
            keepAnimating = false;
        }

        public static SwingEffect Instance
        {
            get
            {
                return instance;
            }
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            Matrix4x4 matrix;
            Vector3[] vertices;
            int loopCount = 0;

            float initialTime = Time.time;

            float vertexAnimAngleRanges = Random.Range(1f, 3f) + Random.Range(3f, 8f) * data.intensity;
            float vertexAnimSpeeds = Random.Range(0.2f, 0.5f) + Random.Range(0.5f, 3.0f) * data.intensity;

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

                    var vertAnimAngle = Mathf.SmoothStep(-vertexAnimAngleRanges, vertexAnimAngleRanges, Mathf.PingPong(loopCount / 25f * vertexAnimSpeeds, 1f));
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
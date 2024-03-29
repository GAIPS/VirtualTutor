﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem
{
    public abstract class AbstractWaveEffect : AbstractTextEffect
    {
        protected bool characters = false;

        protected AbstractWaveEffect()
        {
            keepAnimating = false;
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            data.curve.preWrapMode = WrapMode.Loop;
            data.curve.postWrapMode = WrapMode.Loop;

            Vector3[] vertices;
            Matrix4x4 matrix;
            float CurveScale = data.intensity * 10;

            float initialTime = Time.time;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;


            while (((Time.time - initialTime) / data.duration) < 1 || keepAnimating)
            {
                data.hooks.textData.m_TextComponent.ForceMeshUpdate();

                TMP_TextInfo textInfo = data.hooks.textData.m_TextComponent.textInfo;
                int characterCount = textInfo.characterCount;

                float boundsMinX = data.hooks.textData.m_TextComponent.bounds.min.x;
                float boundsMaxX = data.hooks.textData.m_TextComponent.bounds.max.x;

                for (int i = 0; i < characterCount; i++)
                {
                    if (!textInfo.characterInfo[i].isVisible)
                        continue;

                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    // Get the index of the mesh used by this character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    vertices = textInfo.meshInfo[materialIndex].vertices;

                    // Compute the baseline mid point for each character
                    Vector3 offsetToMidBaseline = new Vector2((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, textInfo.characterInfo[i].baseLine);

                    // Compute the angle of rotation for each character based on the animation curve
                    float x0 = (offsetToMidBaseline.x - boundsMinX) / (boundsMaxX - boundsMinX); // Character's position relative to the bounds of the mesh.
                    float x1 = x0 + 0.0001f;
                    float y0 = data.curve.Evaluate(x0 + Time.time * lastKeyTime / data.duration) * CurveScale;
                    float y1 = data.curve.Evaluate((characters ? x1 : x0) + Time.time * lastKeyTime / data.duration) * CurveScale;

                    Vector3 horizontal = new Vector3(1, 0, 0);
                    Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetToMidBaseline.x, y0);

                    float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
                    Vector3 cross = Vector3.Cross(horizontal, tangent);
                    float angle = cross.z > 0 ? dot : 360 - dot;

                    matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

                    // Apply offset to adjust our pivot point.
                    ApplyMatrix(vertices, vertices, matrix, offsetToMidBaseline, vertexIndex);
                }

                data.hooks.textData.m_TextComponent.UpdateVertexData();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}
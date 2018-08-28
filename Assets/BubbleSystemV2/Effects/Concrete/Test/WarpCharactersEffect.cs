using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem
{
    public class WarpCharactersEffect : AbstractTextEffect
    {
        public WarpCharactersEffect()
        {
            keepAnimating = false;
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            data.curve.preWrapMode = WrapMode.Clamp;
            data.curve.postWrapMode = WrapMode.Clamp;

            Vector3[] vertices;
            Matrix4x4 matrix;

            data.hooks.textData.m_TextComponent.havePropertiesChanged = true; // Need to force the TextMeshPro Object to be updated.
            float CurveScale = data.intensity * 10;

            float initialTime;
            Keyframe lastframe = data.curve[data.curve.length - 1];
            float lastKeyTime = lastframe.time;

            data.hooks.textData.m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.

            TMP_TextInfo textInfo = data.hooks.textData.m_TextComponent.textInfo;
            int characterCount = textInfo.characterCount;

            float boundsMinX = data.hooks.textData.m_TextComponent.bounds.min.x;
            float boundsMaxX = data.hooks.textData.m_TextComponent.bounds.max.x;

            int nonVisible = 0;
            for (int i = 0; i < characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    nonVisible++;
            }

            for (int i = 0; i < characterCount; i++)
            {
                initialTime = Time.time;

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

                float y0 = data.curve.Evaluate(x0) * CurveScale;
                float y1 = data.curve.Evaluate(x1) * CurveScale;

                Vector3 horizontal = new Vector3(1, 0, 0);
                Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetToMidBaseline.x, y0);

                float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
                Vector3 cross = Vector3.Cross(horizontal, tangent);
                float angle = cross.z > 0 ? dot : 360 - dot;

                matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

                // Apply offset to adjust our pivot point.
                ApplyMatrix(vertices, vertices, matrix, offsetToMidBaseline, vertexIndex);
                data.hooks.textData.m_TextComponent.UpdateVertexData();

                //(duration - 1) -> quick hack due to processing time, which leads to higher duration than wanted
                yield return new WaitForSeconds(Mathf.Max(0, (data.duration - 1)) / (characterCount - nonVisible) - (Time.time - initialTime));
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem2
{
    public class JitterEffect : AbstractTextEffect
    {
        public JitterEffect() {
            keepAnimating = false;
            random = true;
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            data.hooks.textData.m_TextComponent.ForceMeshUpdate();
            Keyframe lastframe;
            float lastKeyTime = 0f;

            if (data.curve != null)
            {
                data.curve.preWrapMode = WrapMode.Loop;
                data.curve.postWrapMode = WrapMode.Loop;
                lastframe = data.curve[data.curve.length - 1];
                lastKeyTime = lastframe.time;
            }

            float initialTime = Time.time;
            float rangeX, rangeY;

            TMP_TextInfo textInfo = data.hooks.textData.m_TextComponent.textInfo;
            TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
            Matrix4x4 matrix;

            data.hooks.textData.hasTextChanged = true;

            while (((Time.time - initialTime) / data.duration) < 1 || keepAnimating)
            {
                if (data.hooks.textData.hasTextChanged)
                {
                    cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
                    data.hooks.textData.hasTextChanged = false;
                }

                int characterCount = textInfo.characterCount;

                // If No Characters then just yield and wait for some text to be added
                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                int lineCount = textInfo.lineCount;

                if (random)
                {
                    rangeX = Random.Range(-0.25f, 0.25f) + Random.Range(-1.25f, 1.25f) * data.intensity;
                    rangeY = Random.Range(-0.25f, 0.25f) + Random.Range(-1.25f, 1.25f) * data.intensity;
                }
                else
                {
                    rangeX = rangeY = Random.Range(-0.25f, 0.25f) + Mathf.Clamp(data.curve.Evaluate((Time.time - initialTime) * lastKeyTime / data.duration), -1.25f, 1.25f) * data.intensity;
                }

                // Iterate through each line of the text.
                for (int i = 0; i < lineCount; i++)
                {
                    int first = textInfo.lineInfo[i].firstCharacterIndex;
                    int last = textInfo.lineInfo[i].lastCharacterIndex;
                    Vector3 centerOfLine = (textInfo.characterInfo[first].bottomLeft + textInfo.characterInfo[last].topRight) / 2;

                    for (int j = first; j <= last; j++)
                    {
                        if (!textInfo.characterInfo[j].isVisible)
                            continue;

                        int materialIndex = textInfo.characterInfo[j].materialReferenceIndex;
                        int vertexIndex = textInfo.characterInfo[j].vertexIndex;
                        Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
                        Vector3[] copyOfVertices = textInfo.meshInfo[materialIndex].vertices;
                        matrix = Matrix4x4.TRS(new Vector3(rangeX, rangeY, 0.0f), Quaternion.identity, Vector3.one);

                        ApplyMatrix(sourceVertices, copyOfVertices, matrix, centerOfLine, vertexIndex);
                    }

                    //Change with intensity
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    data.hooks.textData.m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public abstract class AbstractTextEffect : AbstractEffect
    {
        public enum TextEffectEnum
        {
            None,
            Appear,
            Blush,
            BlushCharacters,
            DeflectionFont,
            Erase,
            FadeIn,
            FadeInCharacters,
            FadeOut,
            FadeOutCharacters,
            Flashing,
            Jitter,
            Palpitations,
            Shake,
            ShakeCharacters,
            Squash,
            SquashX,
            SquashY,
            Stretch,
            StretchX,
            StretchY,
            SwellingFont,
            Swing,
            SwingCharacters,
            Warp,
            WarpCharacters,
            Wave,
            WaveCharacters
        }

        protected int wantedAlpha = 0;
        protected bool keepAnimating = false;
        protected bool random = true;

        public abstract IEnumerator Run(TextEffectData data);

        protected virtual void Clean(TextEffectData data)
        {
            if (!data.hooks.textData.m_TextComponent.IsActive()) return;
            data.hooks.text.enableAutoSizing = true;
            data.hooks.textData.initialColor = data.hooks.textData.m_TextComponent.color;

            if (data.show)
            {
                data.hooks.textData.ResetCharacters(true);
                data.hooks.textData.ResetColor(true, true);
                data.hooks.textData.ResetRectTransform(true, true, true);
                data.hooks.textData.ResetCharacterCount();
            }
        }

        protected void ApplyMatrix(Vector3[] sourceVertices, Vector3[] copyOfVertices, Matrix4x4 matrix, Vector3 center, int vertexIndex)
        {
            // Need to translate all 4 vertices of each quad to aligned with center of character.
            // This is needed so the matrix TRS is applied at the origin for each character.
            copyOfVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - center;
            copyOfVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - center;
            copyOfVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - center;
            copyOfVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - center;

            // Apply the matrix TRS to the individual characters relative to the center of the current line.
            copyOfVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(copyOfVertices[vertexIndex + 0]);
            copyOfVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(copyOfVertices[vertexIndex + 1]);
            copyOfVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(copyOfVertices[vertexIndex + 2]);
            copyOfVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(copyOfVertices[vertexIndex + 3]);

            // Revert the translation change.
            copyOfVertices[vertexIndex + 0] += center;
            copyOfVertices[vertexIndex + 1] += center;
            copyOfVertices[vertexIndex + 2] += center;
            copyOfVertices[vertexIndex + 3] += center;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubbleSystem
{
    public class TextMeshData : AbstractBubbleSystemData
    {
        public TMP_Text m_TextComponent;
        public RectTransform rectTransform;
        public bool hasTextChanged;
        public Color32 initialColor;
        public float initialRectX, initialRectY;

        public void ON_TEXT_CHANGED(UnityEngine.Object obj)
        {
            if (obj == m_TextComponent)
                hasTextChanged = true;
        }

        public void ResetCharacters(bool fadeout)
        {
            m_TextComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = m_TextComponent.textInfo;
            Color32[] newVertexColors;

            int characterCount = textInfo.characterCount;

            for (int i = 0; i < characterCount; i++)
            {
                // Skip characters that are not visible
                if (!textInfo.characterInfo[i].isVisible) continue;

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Get the current character's alpha value.
                byte alpha = fadeout ? (byte)initialColor.a : (byte)0;

                newVertexColors[vertexIndex + 0].a = alpha;
                newVertexColors[vertexIndex + 1].a = alpha;
                newVertexColors[vertexIndex + 2].a = alpha;
                newVertexColors[vertexIndex + 3].a = alpha;

                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }

        public void ResetColor(bool fadeout, bool chars)
        {
            if (chars)
            {
                ResetCharacters(fadeout);
            }
            else
            {
                if (fadeout) m_TextComponent.color = initialColor;
                else m_TextComponent.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0.0f);
            }
        }

        public void ResetRectTransform(bool squash, bool x, bool y)
        {
            if (squash)
                rectTransform.localScale = new Vector3(initialRectX, initialRectY, 1.0f);
            else
            {
                if (x && y)
                    rectTransform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
                else if (x)
                    rectTransform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
                else if (y)
                    rectTransform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
            }
        }

        public void ResetCharacterCount()
        {
            m_TextComponent.maxVisibleCharacters = m_TextComponent.textInfo.characterCount;
        }

        public override void Clear()
        {
            m_TextComponent = null;
            rectTransform = null;
            hasTextChanged = false;
            initialColor = Color.black;
            initialRectX = initialRectY = 0.0f;
        }

        public override bool IsCleared()
        {
            return m_TextComponent == null && rectTransform == null && hasTextChanged.Equals(false) && initialColor.Equals(Color.black) &&
                initialRectX.Equals(0.0f) && initialRectY.Equals(0.0f);
        }
    }
}
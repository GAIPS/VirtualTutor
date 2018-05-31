using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Effect
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

public class Effects : MonoBehaviour
{
    private TMP_Text m_TextComponent;
    private RectTransform rectTransform;
    private bool hasTextChanged;
    
    private Dictionary<Effect, Coroutine> coroutines = new Dictionary<Effect, Coroutine>();
    private Dictionary<Effect, IEnumerator> enumerators = new Dictionary<Effect, IEnumerator>();

    private Color32 initialColor;
    private float initialRectX, initialRectY;

    void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
        m_TextComponent.ForceMeshUpdate();
        initialColor = m_TextComponent.color;
        initialRectX = rectTransform.localScale.x;
        initialRectY = rectTransform.localScale.y;
    }

    void OnEnable()
    {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
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

    private void ResetCharacterCount()
    {
        m_TextComponent.maxVisibleCharacters = m_TextComponent.textInfo.characterCount;
    }

    public void SetEffect(Dictionary<Effect, AnimationCurve> effects, float intensity, float duration, bool show)
    {
        if (m_TextComponent.IsActive())
        {
            initialColor = m_TextComponent.color;

            StopAllCoroutines();

            if (show)
            {
                ResetCharacters(true);
                ResetColor(true, true);
                ResetRectTransform(true, true, true);
                ResetCharacterCount();
            }

            enumerators.Clear();

            foreach (Effect effect in effects.Keys)
            {
                switch (effect)
                {
                    case Effect.None:
                        break;
                    case Effect.Appear:
                        ResetCharacterCount();
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Appear(duration, intensity, effects[effect]));
                        break;
                    case Effect.Blush:
                        ResetColor(false, false);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Blush(duration, intensity, effects[effect]));
                        break;
                    case Effect.BlushCharacters:
                        ResetColor(false, true);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, BlushCharacters(duration, intensity, effects[effect]));
                        break;
                    case Effect.DeflectionFont:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, DeflectionFontSize(duration, intensity, effects[effect]));
                        break;
                    case Effect.Erase:
                        ResetColor(true, true);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Erase(duration, intensity, effects[effect]));
                        break;
                    case Effect.FadeIn:
                        ResetColor(false, false);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, FadeIn(duration, intensity, effects[effect]));
                        break;
                    case Effect.FadeInCharacters:
                        ResetColor(false, true);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, FadeInCharacters(duration, intensity, effects[effect]));
                        break;
                    case Effect.FadeOut:
                        ResetColor(true, false);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, FadeOut(duration, intensity, effects[effect]));
                        break;
                    case Effect.FadeOutCharacters:
                        ResetColor(true, true);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, FadeOutCharacters(duration, intensity, effects[effect]));
                        break;
                    case Effect.Flashing:
                        ResetColor(true, false);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Flash(duration, intensity, effects[effect]));
                        break;
                    case Effect.Jitter:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Jitter(duration, intensity, effects[effect]));
                        break;
                    case Effect.Palpitations:
                        ResetRectTransform(true, false, false);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Palpitations(duration, intensity, effects[effect]));
                        break;
                    case Effect.Shake:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Shake(duration, intensity, effects[effect], false));
                        break;
                    case Effect.ShakeCharacters:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Shake(duration, intensity, effects[effect], true));
                        break;
                    case Effect.Squash:
                        ResetRectTransform(true, false, false);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Squash(duration, intensity, effects[effect], true, true));
                        break;
                    case Effect.SquashX:
                        ResetRectTransform(true, false, false);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Squash(duration, intensity, effects[effect], true, false));
                        break;
                    case Effect.SquashY:
                        ResetRectTransform(true, false, false);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Squash(duration, intensity, effects[effect], false, true));
                        break;
                    case Effect.Stretch:
                        ResetRectTransform(false, true, true);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Stretch(duration, intensity, effects[effect], true, true));
                        break;
                    case Effect.StretchX:
                        ResetRectTransform(false, true, false);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Stretch(duration, intensity, effects[effect], true, false));
                        break;
                    case Effect.StretchY:
                        ResetRectTransform(false, false, true);
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Stretch(duration, intensity, effects[effect], false, true));
                        break;
                    case Effect.SwellingFont:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, SwellingFontSize(duration, intensity, effects[effect]));
                        break;
                    case Effect.Swing:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Swing(duration, intensity));
                        break;
                    case Effect.SwingCharacters:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, SwingCharacters(duration, intensity));
                        break;
                    case Effect.Warp:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, WarpText(duration, intensity, effects[effect], false));
                        break;
                    case Effect.WarpCharacters:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, WarpTextCharacters(duration, intensity, effects[effect]));
                        break;
                    case Effect.Wave:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Wave(duration, intensity, effects[effect], false));
                        break;
                    case Effect.WaveCharacters:
                        BubbleSystem.BubbleSystemUtility.AddToDictionary(ref enumerators, effect, Wave(duration, intensity, effects[effect], true));
                        break;
                }
            }

            foreach (Effect effect in enumerators.Keys)
            {
                BubbleSystem.BubbleSystemUtility.AddToDictionary(ref coroutines, effect, StartCoroutine(enumerators[effect]));
            }
        }
    }

    void ON_TEXT_CHANGED(Object obj)
    {
        if (obj == m_TextComponent)
            hasTextChanged = true;
    }

    private void ApplyMatrix(Vector3[] sourceVertices, Vector3[] copyOfVertices, Matrix4x4 matrix, Vector3 center, int vertexIndex)
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

    IEnumerator Appear(float duration, float intensity, AnimationCurve curve)
    {
        m_TextComponent.ForceMeshUpdate();
        int totalVisibleCharacters = m_TextComponent.textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        while ((Time.time - initialTime) / duration < 1)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));
            visibleCount = (int)(yValue * totalVisibleCharacters);
            m_TextComponent.maxVisibleCharacters = visibleCount;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if(DefaultData.Instance.forceTextUpdate)
            m_TextComponent.maxVisibleCharacters = totalVisibleCharacters;
    }

    IEnumerator Blush(float duration, float intensity, AnimationCurve curve)
    {
        m_TextComponent.ForceMeshUpdate();

        Color32 initialColor = m_TextComponent.color, finalColor;
        int red, green, blue, alpha;

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        Color blushColor = DefaultData.Instance.GetBlushColor();

        while (((Time.time - initialTime) / duration) < 1)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

            red = (int)(initialColor.r + yValue * (((byte)(blushColor.r * 255)) - initialColor.r));
            green = (int)(initialColor.g + yValue * (((byte)(blushColor.g * 255)) - initialColor.g));
            blue = (int)(initialColor.b + yValue * (((byte)(blushColor.b * 255)) - initialColor.b));
            alpha = (int)(initialColor.a + yValue * (((byte)(blushColor.a * 255)) - initialColor.a));

            finalColor.r = (byte)red;
            finalColor.g = (byte)green;
            finalColor.b = (byte)blue;
            finalColor.a = (byte)alpha;

            m_TextComponent.color = finalColor;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
            m_TextComponent.color = blushColor;
    }

    IEnumerator BlushCharacters(float duration, float intensity, AnimationCurve curve)
    {
        m_TextComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        Color32[] newVertexColors;
        Color32 initialColor, finalColor;
        int red, green, blue, alpha;

        int characterCount = textInfo.characterCount;

        float initialCharTime;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        Color blushColor = DefaultData.Instance.GetBlushColor();

        for (int i = 0; i < characterCount; i++)
        {
            initialCharTime = Time.time;

            initialColor = m_TextComponent.textInfo.characterInfo[i].color;
            // Skip characters that are not visible
            if (!textInfo.characterInfo[i].isVisible) continue;

            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            while (((Time.time - initialCharTime) * characterCount / duration) < 1)
            {
                yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialCharTime) * lastKeyTime * characterCount / duration));

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
                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
            {
                newVertexColors[vertexIndex + 0] = blushColor;
                newVertexColors[vertexIndex + 1] = blushColor;
                newVertexColors[vertexIndex + 2] = blushColor;
                newVertexColors[vertexIndex + 3] = blushColor;
                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }
    }

    IEnumerator DeflectionFontSize(float duration, float intensity, AnimationCurve curve)
    {
        m_TextComponent.ForceMeshUpdate();

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        float size;
        float initialSize = m_TextComponent.fontSize;
        float finalSize = 0f;

        while (((Time.time - initialTime) / duration) < 1)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

            size = initialSize + yValue * (finalSize - initialSize);

            m_TextComponent.fontSize = size;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
            m_TextComponent.fontSize = finalSize;
    }

    IEnumerator Erase(float duration, float intensity, AnimationCurve curve, float wantedAlpha = 0)
    {
        // Need to force the text object to be generated so we have valid data to work with right from the start.
        m_TextComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        Color32[] newVertexColors;

        int characterCount = textInfo.characterCount;

        Color32 finalColor = m_TextComponent.color;
        int initialAlpha;
        byte finalAlpha;

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        while (((Time.time - initialTime) / duration) < 1)
        {
            for (int i = 0; i < characterCount; i++)
            {
                initialAlpha = m_TextComponent.textInfo.characterInfo[i].color.a;
                // Skip characters that are not visible
                if (!textInfo.characterInfo[i].isVisible) continue;

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;


                yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

                finalAlpha = (byte)(initialAlpha + yValue * (wantedAlpha - initialAlpha));
                // Set new alpha values.
                newVertexColors[vertexIndex + 0].a = finalAlpha;
                newVertexColors[vertexIndex + 1].a = finalAlpha;
                newVertexColors[vertexIndex + 2].a = finalAlpha;
                newVertexColors[vertexIndex + 3].a = finalAlpha;

                // Upload the changed vertex colors to the Mesh.
                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

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
                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }
    }

    IEnumerator FadeIn(float duration, float intensity, AnimationCurve curve, int wantedAlpha = 255)
    {
        m_TextComponent.ForceMeshUpdate();

        int finalAlpha;
        Color32 finalColor = m_TextComponent.color;
        int initialAlpha = finalColor.a;

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        while (((Time.time - initialTime) / duration) < 1)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

            finalAlpha = (int)(initialAlpha + yValue * (wantedAlpha - initialAlpha));
            finalColor.a = (byte)finalAlpha;

            m_TextComponent.color = finalColor;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
        {
            finalColor.a = (byte)wantedAlpha;
            m_TextComponent.color = finalColor;
        }
    }

    IEnumerator FadeInCharacters(float duration, float intensity, AnimationCurve curve, int wantedAlpha = 255)
    {
        m_TextComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        Color32[] newVertexColors;
        Color32 finalColor = m_TextComponent.color;
        float initialAlpha;
        int alpha;

        int characterCount = textInfo.characterCount;

        float initialCharTime;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        for (int i = 0; i < characterCount; i++)
        {
            initialCharTime = Time.time;
            initialAlpha = m_TextComponent.textInfo.characterInfo[i].color.a;

            // Skip characters that are not visible
            if (!textInfo.characterInfo[i].isVisible) continue;

            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            while ((((Time.time - initialCharTime) * characterCount / duration) < 1))
            {
                yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialCharTime) * lastKeyTime * characterCount / duration));

                alpha = (int)(initialAlpha + yValue * (wantedAlpha - initialAlpha));

                finalColor.a = (byte)alpha;

                newVertexColors[vertexIndex + 0] = finalColor;
                newVertexColors[vertexIndex + 1] = finalColor;
                newVertexColors[vertexIndex + 2] = finalColor;
                newVertexColors[vertexIndex + 3] = finalColor;
                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
            {
                finalColor.a = (byte)wantedAlpha;
                newVertexColors[vertexIndex + 0] = finalColor;
                newVertexColors[vertexIndex + 1] = finalColor;
                newVertexColors[vertexIndex + 2] = finalColor;
                newVertexColors[vertexIndex + 3] = finalColor;
                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }
    }

    IEnumerator FadeOut(float duration, float intensity, AnimationCurve curve, float wantedAlpha = 0)
    {
        m_TextComponent.ForceMeshUpdate();

        int finalAlpha;
        Color32 finalColor = m_TextComponent.color;
        int initialAlpha = finalColor.a;

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        while ((((Time.time - initialTime) / duration) < 1) || finalColor.a == wantedAlpha)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

            finalAlpha = (int)(initialAlpha + yValue * (wantedAlpha - initialAlpha));
            finalColor.a = (byte)finalAlpha;

            m_TextComponent.color = finalColor;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
        {
            finalColor.a = (byte)wantedAlpha;
            m_TextComponent.color = finalColor;
        }
    }

    IEnumerator FadeOutCharacters(float duration, float intensity, AnimationCurve curve, float wantedAlpha = 0)
    {
        m_TextComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        Color32[] newVertexColors;
        Color32 finalColor = m_TextComponent.color;
        float initialAlpha;
        int alpha;

        int characterCount = textInfo.characterCount;

        float initialCharTime;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        for (int i = 0; i < characterCount; i++)
        {
            initialCharTime = Time.time;
            initialAlpha = m_TextComponent.textInfo.characterInfo[i].color.a;

            // Skip characters that are not visible
            if (!textInfo.characterInfo[i].isVisible) continue;

            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            while ((((Time.time - initialCharTime) * characterCount / duration) < 1))
            {
                yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialCharTime) * lastKeyTime * characterCount / duration));

                alpha = (int)(initialAlpha + yValue * (wantedAlpha - initialAlpha));

                finalColor.a = (byte)alpha;

                newVertexColors[vertexIndex + 0] = finalColor;
                newVertexColors[vertexIndex + 1] = finalColor;
                newVertexColors[vertexIndex + 2] = finalColor;
                newVertexColors[vertexIndex + 3] = finalColor;
                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (DefaultData.Instance.forceTextUpdate)
            {
                finalColor.a = (byte)wantedAlpha;
                newVertexColors[vertexIndex + 0] = finalColor;
                newVertexColors[vertexIndex + 1] = finalColor;
                newVertexColors[vertexIndex + 2] = finalColor;
                newVertexColors[vertexIndex + 3] = finalColor;
                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }
    }

    IEnumerator Flash(float duration, float intensity, AnimationCurve curve, bool keepAnimating = false)
    {
        m_TextComponent.ForceMeshUpdate();
        curve.preWrapMode = WrapMode.Loop;
        curve.postWrapMode = WrapMode.Loop;

        int totalVisibleCharacters = m_TextComponent.textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        while (((Time.time - initialTime) / duration) < 1 || keepAnimating)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));
            if (yValue >= 1)
                visibleCount = totalVisibleCharacters;
            else if (yValue <= 0)
                visibleCount = 0;
            m_TextComponent.maxVisibleCharacters = visibleCount;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
            m_TextComponent.maxVisibleCharacters = totalVisibleCharacters;
    }

    IEnumerator Jitter(float duration, float intensity, AnimationCurve curve, bool keepAnimating = false, bool random = true)
    {
        m_TextComponent.ForceMeshUpdate();
        Keyframe lastframe;
        float lastKeyTime = 0f;
        if (curve != null)
        {
            curve.preWrapMode = WrapMode.Loop;
            curve.postWrapMode = WrapMode.Loop;
            lastframe = curve[curve.length - 1];
            lastKeyTime = lastframe.time;
        }

        float initialTime = Time.time;
        float rangeX, rangeY;

        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        Matrix4x4 matrix;

        hasTextChanged = true;

        while (((Time.time - initialTime) / duration) < 1 || keepAnimating)
        {
            if (hasTextChanged)
            {
                cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
                hasTextChanged = false;
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
                rangeX = Random.Range(-0.25f, 0.25f) + Random.Range(-1.25f, 1.25f) * intensity;
                rangeY = Random.Range(-0.25f, 0.25f) + Random.Range(-1.25f, 1.25f) * intensity;
            }
            else
            {
                rangeX = rangeY = Random.Range(-0.25f, 0.25f) + Mathf.Clamp(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration), -1.25f, 1.25f) * intensity;
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
                m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator Palpitations(float duration, float intensity, AnimationCurve curve, bool keepAnimating = false)
    {
        m_TextComponent.ForceMeshUpdate();
        curve.preWrapMode = WrapMode.Loop;
        curve.postWrapMode = WrapMode.Loop;

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        Vector3 localScale = rectTransform.localScale;
        Vector3 finalScale = rectTransform.localScale * 1.5f;
        Vector3 stepScale;

        while (((Time.time - initialTime) / duration) < 1 || keepAnimating)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

            stepScale = localScale + yValue * (finalScale - localScale);

            rectTransform.localScale = stepScale;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
            rectTransform.localScale = localScale;
    }

    IEnumerator Shake(float duration, float intensity, AnimationCurve curve, bool characters, bool keepAnimating = false, bool random = true)
    {
        m_TextComponent.ForceMeshUpdate();
        Keyframe lastframe;
        float lastKeyTime = 0f;
        if (curve != null)
        {
            curve.preWrapMode = WrapMode.Loop;
            curve.postWrapMode = WrapMode.Loop;
            lastframe = curve[curve.length - 1];
            lastKeyTime = lastframe.time;
        }

        float initialTime = Time.time;
        float range;

        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        Matrix4x4 matrix;

        hasTextChanged = true;

        while (((Time.time - initialTime) / duration) < 1 || keepAnimating)
        {
            if (hasTextChanged)
            {
                cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
                hasTextChanged = false;
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
                range = Random.Range(-0.25f, 0.25f) + Random.Range(-1.25f, 1.25f) * intensity;
            }
            else
            {
                range = Random.Range(-0.25f, 0.25f) + Mathf.Clamp(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration), -1.25f, 1.25f) * intensity;
            }

            // Iterate through each line of the text.
            for (int i = 0; i < lineCount; i++)
            {
                int first = textInfo.lineInfo[i].firstCharacterIndex;
                int last = textInfo.lineInfo[i].lastCharacterIndex;

                // Determine the center of each line
                Vector3 centerOfLine = (textInfo.characterInfo[first].bottomLeft + textInfo.characterInfo[last].topRight) / 2;
                Quaternion rotation = Quaternion.Euler(0, 0, range);

                // Iterate through each character of the line.
                for (int j = first; j <= last; j++)
                {
                    // Skip characters that are not visible and thus have no geometry to manipulate.
                    if (!textInfo.characterInfo[j].isVisible)
                        continue;

                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[j].materialReferenceIndex;

                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[j].vertexIndex;

                    // Get the vertices of the mesh used by this text element (character or sprite).
                    Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
                    Vector3[] copyOfVertices = textInfo.meshInfo[materialIndex].vertices;

                    if (!characters)
                    {
                        // Setup the matrix rotation.
                        matrix = Matrix4x4.TRS(Vector3.one, rotation, Vector3.one);
                        ApplyMatrix(sourceVertices, copyOfVertices, matrix, centerOfLine, vertexIndex);
                    }
                    else
                    {
                        Vector3 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;
                        matrix = Matrix4x4.TRS(Vector3.one, rotation, Vector3.one);
                        ApplyMatrix(sourceVertices, copyOfVertices, matrix, charMidBasline, vertexIndex);
                    }
                }

                //Change with intensity
                yield return new WaitForSeconds(Time.deltaTime);
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator Squash(float duration, float intensity, AnimationCurve curve, bool x, bool y, float wantedScaleX = 0f, float wantedScaleY = 0f)
    {
        m_TextComponent.ForceMeshUpdate();

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        Vector3 localScale = rectTransform.localScale;
        Vector3 finalScale = rectTransform.localScale;

        float initialScaleX = localScale.x;
        float initialScaleY = localScale.y;

        while (((Time.time - initialTime) / duration) < 1)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

            if (x)
                finalScale.x = initialScaleX + yValue * (wantedScaleX - initialScaleX);
            if (y)
                finalScale.y = initialScaleY + yValue * (wantedScaleY - initialScaleY);

            rectTransform.localScale = finalScale;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
        {
            if (x)
                finalScale.x = wantedScaleX;
            if (y)
                finalScale.y = wantedScaleY;
            rectTransform.localScale = finalScale;
        }
    }

    IEnumerator Stretch(float duration, float intensity, AnimationCurve curve, bool x, bool y, float wantedScaleX = 1f, float wantedScaleY = 1f)
    {
        m_TextComponent.ForceMeshUpdate();

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        Vector3 finalScale = rectTransform.localScale;

        Vector3 zeroScale = Vector3.zero;
        rectTransform.localScale = zeroScale;
        Vector3 localScale = rectTransform.localScale;

        float initialScaleX = localScale.x;
        float initialScaleY = localScale.y;

        while (((Time.time - initialTime) / duration) < 1)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

            if (x)
                finalScale.x = initialScaleX + yValue * (wantedScaleX - initialScaleX);
            if (y)
                finalScale.y = initialScaleY + yValue * (wantedScaleY - initialScaleY);

            rectTransform.localScale = finalScale;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
        {
            if (x)
                finalScale.x = wantedScaleX;
            if (y)
                finalScale.y = wantedScaleY;
            rectTransform.localScale = finalScale;
        }
    }

    IEnumerator SwellingFontSize(float duration, float intensity, AnimationCurve curve)
    {
        m_TextComponent.ForceMeshUpdate();

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;
        float yValue;

        float size = 0;
        float initialSize = 0f;
        float finalSize = m_TextComponent.fontSize;

        m_TextComponent.fontSize = 0f;

        while (((Time.time - initialTime) / duration) < 1)
        {
            yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

            size = initialSize + yValue * (finalSize - initialSize);

            m_TextComponent.fontSize = size;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
            m_TextComponent.fontSize = finalSize;
    }

    IEnumerator Swing(float duration, float intensity, bool keepAnimating = false)
    {
        Matrix4x4 matrix;
        Vector3[] vertices;
        int loopCount = 0;

        float initialTime = Time.time;

        float vertexAnimAngleRanges = Random.Range(1f, 3f) + Random.Range(3f, 8f) * intensity;
        float vertexAnimSpeeds = Random.Range(0.2f, 0.5f) + Random.Range(0.5f, 3.0f) * intensity;

        int characterCount = m_TextComponent.textInfo.characterCount;

        while (((Time.time - initialTime) / duration) < 1 || keepAnimating)
        {
            m_TextComponent.ForceMeshUpdate();
            vertices = m_TextComponent.mesh.vertices;

            for (int i = 0; i < characterCount; i++)
            {
                var charInfo = m_TextComponent.textInfo.characterInfo[i];

                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;

                Vector3 charMidTopLine = new Vector3((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, charInfo.topRight.y);

                var vertAnimAngle = Mathf.SmoothStep(-vertexAnimAngleRanges, vertexAnimAngleRanges, Mathf.PingPong(loopCount / 25f * vertexAnimSpeeds, 1f));
                matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, vertAnimAngle), Vector3.one);
                ApplyMatrix(vertices, vertices, matrix, charMidTopLine, vertexIndex);
            }

            loopCount++;

            for (int i = 0; i < m_TextComponent.textInfo.meshInfo.Length; i++)
            {
                m_TextComponent.textInfo.meshInfo[i].mesh.vertices = vertices;
                m_TextComponent.UpdateGeometry(m_TextComponent.textInfo.meshInfo[i].mesh, i);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
            m_TextComponent.ForceMeshUpdate();
    }

    IEnumerator SwingCharacters(float duration, float intensity, bool keepAnimating = false)
    {
        Matrix4x4 matrix;
        Vector3[] vertices;
        int loopCount = 0;

        float initialTime = Time.time;

        float[] vertexAnimAngleRanges = new float[1024];
        float[] vertexAnimSpeeds = new float[1024];
        for (int i = 0; i < 1024; i++)
        {
            vertexAnimAngleRanges[i] = Random.Range(1f, 3f) + Random.Range(3f, 8f) * intensity;
            vertexAnimSpeeds[i] = Random.Range(0.2f, 0.5f) + Random.Range(0.5f, 3.0f) * intensity;
        }

        int characterCount = m_TextComponent.textInfo.characterCount;

        while (((Time.time - initialTime) / duration) < 1 || keepAnimating)
        {
            m_TextComponent.ForceMeshUpdate();
            vertices = m_TextComponent.mesh.vertices;

            for (int i = 0; i < characterCount; i++)
            {
                var charInfo = m_TextComponent.textInfo.characterInfo[i];

                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;

                Vector3 charMidTopLine = new Vector3((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, charInfo.topRight.y);

                var vertAnimAngle = Mathf.SmoothStep(-vertexAnimAngleRanges[i], vertexAnimAngleRanges[i], Mathf.PingPong(loopCount / 25f * vertexAnimSpeeds[i], 1f));
                matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, vertAnimAngle), Vector3.one);
                ApplyMatrix(vertices, vertices, matrix, charMidTopLine, vertexIndex);
            }

            loopCount++;

            for (int i = 0; i < m_TextComponent.textInfo.meshInfo.Length; i++)
            {
                m_TextComponent.textInfo.meshInfo[i].mesh.vertices = vertices;
                m_TextComponent.UpdateGeometry(m_TextComponent.textInfo.meshInfo[i].mesh, i);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (DefaultData.Instance.forceTextUpdate)
            m_TextComponent.ForceMeshUpdate();
    }

    IEnumerator WarpText(float duration, float intensity, AnimationCurve curve, bool keepAnimating = false)
    {
        curve.preWrapMode = WrapMode.Clamp;
        curve.postWrapMode = WrapMode.Clamp;

        Vector3[] vertices;
        Matrix4x4 matrix;

        m_TextComponent.havePropertiesChanged = true; // Need to force the TextMeshPro Object to be updated.
        float CurveScale = intensity * 10;

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;

        while (((Time.time - initialTime) / duration) < 1)
        {
            m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.

            TMP_TextInfo textInfo = m_TextComponent.textInfo;
            int characterCount = textInfo.characterCount;

            if (characterCount == 0) continue;

            float boundsMinX = m_TextComponent.bounds.min.x;  //textInfo.meshInfo[0].mesh.bounds.min.x;
            float boundsMaxX = m_TextComponent.bounds.max.x;  //textInfo.meshInfo[0].mesh.bounds.max.x;

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
                float y0 = curve.Evaluate(x0) * CurveScale;
                float y1 = curve.Evaluate(x1) * CurveScale;

                Vector3 horizontal = new Vector3(1, 0, 0);
                Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetToMidBaseline.x, y0);

                float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
                Vector3 cross = Vector3.Cross(horizontal, tangent);
                float angle = cross.z > 0 ? dot : 360 - dot;

                matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

                // Apply offset to adjust our pivot point.
                ApplyMatrix(vertices, vertices, matrix, offsetToMidBaseline, vertexIndex);
            }
            // Upload the mesh with the revised information
            m_TextComponent.UpdateVertexData();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator WarpTextCharacters(float duration, float intensity, AnimationCurve curve)
    {
        curve.preWrapMode = WrapMode.Clamp;
        curve.postWrapMode = WrapMode.Clamp;

        Vector3[] vertices;
        Matrix4x4 matrix;

        m_TextComponent.havePropertiesChanged = true; // Need to force the TextMeshPro Object to be updated.
        float CurveScale = intensity * 10;

        float initialTime;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;

        m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.

        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        int characterCount = textInfo.characterCount;

        float boundsMinX = m_TextComponent.bounds.min.x;
        float boundsMaxX = m_TextComponent.bounds.max.x;

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

            float y0 = curve.Evaluate(x0) * CurveScale;
            float y1 = curve.Evaluate(x1) * CurveScale;

            Vector3 horizontal = new Vector3(1, 0, 0);
            Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetToMidBaseline.x, y0);

            float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
            Vector3 cross = Vector3.Cross(horizontal, tangent);
            float angle = cross.z > 0 ? dot : 360 - dot;

            matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

            // Apply offset to adjust our pivot point.
            ApplyMatrix(vertices, vertices, matrix, offsetToMidBaseline, vertexIndex);
            m_TextComponent.UpdateVertexData();

            //(duration - 1) -> quick hack due to processing time, which leads to higher duration than wanted
            yield return new WaitForSeconds(Mathf.Max(0, (duration - 1)) / (characterCount - nonVisible) - (Time.time - initialTime));
        }
    }

    IEnumerator Wave(float duration, float intensity, AnimationCurve curve, bool characters, bool keepAnimating = false)
    {
        curve.preWrapMode = WrapMode.Loop;
        curve.postWrapMode = WrapMode.Loop;

        Vector3[] vertices;
        Matrix4x4 matrix;
        float CurveScale = intensity * 10;

        float initialTime = Time.time;
        Keyframe lastframe = curve[curve.length - 1];
        float lastKeyTime = lastframe.time;


        while (((Time.time - initialTime) / duration) < 1 || keepAnimating)
        {
            m_TextComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = m_TextComponent.textInfo;
            int characterCount = textInfo.characterCount;

            float boundsMinX = m_TextComponent.bounds.min.x;
            float boundsMaxX = m_TextComponent.bounds.max.x;

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
                float y0 = curve.Evaluate(x0 + Time.time * lastKeyTime / duration) * CurveScale;
                float y1 = curve.Evaluate((characters ? x1 : x0) + Time.time * lastKeyTime / duration) * CurveScale;

                Vector3 horizontal = new Vector3(1, 0, 0);
                Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetToMidBaseline.x, y0);

                float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
                Vector3 cross = Vector3.Cross(horizontal, tangent);
                float angle = cross.z > 0 ? dot : 360 - dot;

                matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

                // Apply offset to adjust our pivot point.
                ApplyMatrix(vertices, vertices, matrix, offsetToMidBaseline, vertexIndex);
            }

            m_TextComponent.UpdateVertexData();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{

    public enum BackgroundEffect
    {
        FadeTexture,
        FadeColor
    }

    public class BackgroundManager : MonoBehaviour
    {
        [Serializable]
        public struct Background
        {
            public string name;
            public GameObject background;
        }

        public Background[] backgrounds;

        private Dictionary<string, Dictionary<BackgroundEffect, IEnumerator>> textureCoroutines = new Dictionary<string, Dictionary<BackgroundEffect, IEnumerator>>();
        private Dictionary<string, Dictionary<BackgroundEffect, IEnumerator>> colorCoroutines = new Dictionary<string, Dictionary<BackgroundEffect, IEnumerator>>();

        public void SetBackground(string bg, BackgroundData data, float duration)
        {
            if (!textureCoroutines.ContainsKey(bg))
                textureCoroutines.Add(bg, new Dictionary<BackgroundEffect, IEnumerator>());
            if (!colorCoroutines.ContainsKey(bg))
                colorCoroutines.Add(bg, new Dictionary<BackgroundEffect, IEnumerator>());

            KeyValuePair<Emotion, float> emotionPair = BubbleSystemUtility.GetHighestEmotion(data.emotions);

            TextureData textureData = DefaultData.Instance.GetDefaultBackgroundDataDictionary(emotionPair.Key, emotionPair.Value, data.reason);
            BackgroundAnimationData backgroundAnimationData = DefaultData.Instance.GetDefaultBackgroundAnimationData(emotionPair.Key, emotionPair.Value);
            StartCoroutine(ChangeImage(bg, textureData, backgroundAnimationData, duration));
        }

        private GameObject GetBackground(string bg)
        {
            foreach (Background b in backgrounds)
            {
                if (b.name.Equals(bg))
                {
                    return b.background;
                }
            }

            throw new KeyNotFoundException("Background with name: " + bg + " not found.");
        }
        
        private IEnumerator ChangeImage(string bg, TextureData textureData, BackgroundAnimationData backgroundAnimationData, float duration)
        {
            Renderer renderer = GetBackground(bg).GetComponent<Renderer>();
            float initialAlpha = renderer.material.color.a;
            float realDuration = duration / 3;

            textureCoroutines[bg].Clear();
            colorCoroutines[bg].Clear();

            if (textureCoroutines.ContainsKey(bg))
                foreach (BackgroundEffect fx in textureCoroutines[bg].Keys)
                    CoroutineStopper.Instance.StopCoroutineWithCheck(textureCoroutines[bg][fx]);
            if (colorCoroutines.ContainsKey(bg))
                foreach (BackgroundEffect fx in colorCoroutines[bg].Keys)
                    CoroutineStopper.Instance.StopCoroutineWithCheck(colorCoroutines[bg][fx]);

            if (!textureData.texture.name.Equals(renderer.material.mainTexture.name))
            {
                foreach (BackgroundEffect fx in backgroundAnimationData.hideBannerEffect.Keys)
                {
                    if (fx == BackgroundEffect.FadeTexture)
                        textureCoroutines[bg].Add(fx, FadeTexture(renderer, backgroundAnimationData.hideBannerEffect[fx], realDuration));
                }

                foreach (BackgroundEffect fx in backgroundAnimationData.hideBannerEffect.Keys)
                    StartCoroutine(textureCoroutines[bg][fx]);

                yield return new WaitForSeconds(realDuration);

                textureCoroutines[bg].Clear();
                renderer.material.mainTexture = textureData.texture;
                renderer.material.mainTexture.wrapMode = TextureWrapMode.Mirror;

                foreach (BackgroundEffect fx in backgroundAnimationData.showBannerEffect.Keys)
                {
                    if (fx == BackgroundEffect.FadeTexture)
                        textureCoroutines[bg].Add(fx, FadeTexture(renderer, backgroundAnimationData.showBannerEffect[fx], realDuration, initialAlpha));
                }

                foreach (BackgroundEffect fx in backgroundAnimationData.showBannerEffect.Keys)
                    StartCoroutine(textureCoroutines[bg][fx]);

                yield return new WaitForSeconds(realDuration);
            }
            else
            {
                realDuration = duration;
            }

            if (!renderer.material.color.Equals(textureData.color))
            {

                foreach (BackgroundEffect fx in backgroundAnimationData.colorEffect.Keys)
                {
                    if (fx == BackgroundEffect.FadeColor)
                        colorCoroutines[bg].Add(fx, LerpColor(renderer, textureData.color, backgroundAnimationData.colorEffect[fx], realDuration));
                }

                foreach (BackgroundEffect fx in backgroundAnimationData.colorEffect.Keys)
                    StartCoroutine(colorCoroutines[bg][fx]);

                yield return new WaitForSeconds(realDuration);
            }
        }

        private IEnumerator FadeTexture(Renderer renderer, AnimationCurve curve, float duration, float wantedAlpha = 0)
        {
            float finalAlpha;
            float initialAlpha = renderer.material.color.a;
            Color finalColor = renderer.material.color;

            float initialTime = Time.time;
            Keyframe lastframe = curve[curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            while (((Time.time - initialTime) / duration) < 1)
            {
                yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

                finalAlpha = initialAlpha + yValue * (wantedAlpha - initialAlpha);
                finalColor.a = finalAlpha;

                renderer.material.color = finalColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            finalColor.a = wantedAlpha;
            renderer.material.color = finalColor;
        }

        private IEnumerator LerpColor(Renderer renderer, Color nextColor, AnimationCurve curve, float duration)
        {
            Color32 initialColor = renderer.material.color, finalColor;
            int red, green, blue, alpha;

            float initialTime = Time.time;
            Keyframe lastframe = curve[curve.length - 1];
            float lastKeyTime = lastframe.time;
            float yValue;

            while (((Time.time - initialTime) / duration) < 1)
            {
                yValue = Mathf.Clamp01(curve.Evaluate((Time.time - initialTime) * lastKeyTime / duration));

                red = (int)(initialColor.r + yValue * (((byte)(nextColor.r * 255)) - initialColor.r));
                green = (int)(initialColor.g + yValue * (((byte)(nextColor.g * 255)) - initialColor.g));
                blue = (int)(initialColor.b + yValue * (((byte)(nextColor.b * 255)) - initialColor.b));
                alpha = (int)(initialColor.a + yValue * (((byte)(nextColor.a * 255)) - initialColor.a));

                finalColor.r = (byte)red;
                finalColor.g = (byte)green;
                finalColor.b = (byte)blue;
                finalColor.a = (byte)alpha;

                renderer.material.color = finalColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            renderer.material.color = nextColor;
        }
    }
}
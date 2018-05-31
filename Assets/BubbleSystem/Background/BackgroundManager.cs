﻿using System;
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
        private Dictionary<string, IEnumerator> changeImageCoroutines = new Dictionary<string, IEnumerator>();

        public void SetBackground(string bg, BackgroundData data)
        {
            if (!textureCoroutines.ContainsKey(bg))
                textureCoroutines.Add(bg, new Dictionary<BackgroundEffect, IEnumerator>());
            if (!colorCoroutines.ContainsKey(bg))
                colorCoroutines.Add(bg, new Dictionary<BackgroundEffect, IEnumerator>());

            KeyValuePair<Emotion, float> emotionPair = BubbleSystemUtility.GetHighestEmotion(data.emotions);

            Texture2D textureData = DefaultData.Instance.GetDefaultBackgroundDataDictionary(emotionPair.Key, emotionPair.Value, data.reason);
            BackgroundAnimationData backgroundAnimationData = DefaultData.Instance.GetDefaultBackgroundAnimationData(emotionPair.Key, emotionPair.Value);

            Color32 colorToLerpTo = BubbleSystemUtility.GetColor(emotionPair, data.emotions);

            if (textureCoroutines.ContainsKey(bg))
                foreach (BackgroundEffect fx in textureCoroutines[bg].Keys)
                    if (BubbleSystemUtility.CheckCoroutine(ref textureCoroutines, bg, fx))
                        StopCoroutine(textureCoroutines[bg][fx]);
            if (colorCoroutines.ContainsKey(bg))
                foreach (BackgroundEffect fx in colorCoroutines[bg].Keys)
                    if (BubbleSystemUtility.CheckCoroutine(ref colorCoroutines, bg, fx))
                        StopCoroutine(colorCoroutines[bg][fx]);

            if (BubbleSystemUtility.CheckCoroutine(ref changeImageCoroutines, bg))
                StopCoroutine(changeImageCoroutines[bg]);
            BubbleSystemUtility.AddToDictionary(ref changeImageCoroutines, bg, ChangeImage(bg, data, textureData, backgroundAnimationData, DefaultData.Instance.GetBackgroundDuration(), colorToLerpTo));
            StartCoroutine(changeImageCoroutines[bg]);
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

        private TextureWrapMode GetWrapMode(BackgroundData data)
        {
            if (data.reason.Equals(Reason.None))
                return TextureWrapMode.Mirror;
            else
                return TextureWrapMode.Repeat;

        }

        private IEnumerator ChangeImage(string bg, BackgroundData data, Texture2D textureData, BackgroundAnimationData backgroundAnimationData, float duration, Color32 colorToLerpTo)
        {
            Renderer renderer = GetBackground(bg).GetComponent<Renderer>();
            float initialAlpha = renderer.material.color.a;
            float realDuration = duration / 2;

            if (!textureData.name.Equals(renderer.materials[1].mainTexture.name))
            {
                foreach (BackgroundEffect fx in backgroundAnimationData.hideBannerEffect.Keys)
                {
                    if (fx == BackgroundEffect.FadeTexture)
                    {
                        BubbleSystemUtility.AddToDictionary(ref textureCoroutines, bg, fx, FadeTexture(renderer, backgroundAnimationData.hideBannerEffect[fx], realDuration));
                        StartCoroutine(textureCoroutines[bg][fx]);
                    }
                }

                yield return new WaitForSeconds(realDuration);

                textureCoroutines[bg].Clear();
                renderer.materials[renderer.materials.Length - 1].mainTexture = textureData;
                renderer.materials[renderer.materials.Length - 1].mainTexture.wrapMode = GetWrapMode(data);
            }
            else
            {
                realDuration = duration;
            }

            if (!renderer.material.color.Equals(colorToLerpTo))
            {

                foreach (BackgroundEffect fx in backgroundAnimationData.colorEffect.Keys)
                {
                    if (fx == BackgroundEffect.FadeColor)
                    {
                        BubbleSystemUtility.AddToDictionary(ref colorCoroutines, bg, fx, LerpColor(renderer, colorToLerpTo, backgroundAnimationData.colorEffect[fx], realDuration));
                        StartCoroutine(colorCoroutines[bg][fx]);
                    }
                }

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

                for(int i = 0; i < renderer.materials.Length; i++)
                    renderer.materials[i].color = finalColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            finalColor.a = wantedAlpha;
            for (int i = 0; i < renderer.materials.Length; i++)
                renderer.materials[i].color = finalColor;
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

                for (int i = 0; i < renderer.materials.Length; i++)
                    renderer.materials[i].color = finalColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            for (int i = 0; i < renderer.materials.Length; i++)
                renderer.materials[i].color = nextColor;
        }
    }
}
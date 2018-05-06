using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BubbleSystem
{
    public static class BubbleSystemUtility
    {
        public static KeyValuePair<BubbleSystem.Emotion, float> GetHighestEmotion(Dictionary<BubbleSystem.Emotion, float> emotions)
        {
            BubbleSystem.Emotion highestEmotion = emotions.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            float intensity = emotions[highestEmotion];

            return new KeyValuePair<BubbleSystem.Emotion, float>(highestEmotion, intensity);
        }

        public static float GetEmotionsSum(Dictionary<BubbleSystem.Emotion, float> emotions)
        {
            return emotions.Sum(x => x.Value);
        }

        public static int RandomExcludingNumbers(int[] index, int length)
        {
            var exclude = new HashSet<int>(index);
            IEnumerable<int> range = Enumerable.Range(0, length).Where(i => !exclude.Contains(i));
            int finalIndex = Random.Range(0, length - exclude.Count - 1);
            return range.ElementAt(finalIndex);
        }

        public static Color32 MixColors(Dictionary<Emotion, float> emotions)
        {
            Color color = Color.white;
            foreach (Emotion emotion in emotions.Keys)
            {
                color = Color.Lerp(color, (Color)DefaultData.Instance.GetColor(emotion), emotions[emotion]);
            }

            if (color.Equals(Color.white))
                color = DefaultData.Instance.GetColor(Emotion.Default);

            return color;
        }

        public static Color GetTextColor(Color color)
        {
            float red = ConvertToLinear(color.r);
            float green = ConvertToLinear(color.g);
            float blue = ConvertToLinear(color.b);

            float luminance = 0.2126f * red + 0.7152f * green + 0.0722f * blue;
            if ((luminance + 0.05f) / 0.05f > 1.05 / (luminance + 0.05))
                return Color.black;
            else
                return Color.white;
        }

        //sRGB to linear RGB
        private static float ConvertToLinear(float value)
        {
            return value <= 0.03928f ? value / 12.92f : Mathf.Pow((value + 0.055f) / 1.055f, 2.4f);
        }
    }

    public class CoroutineStopper : Singleton<CoroutineStopper>
    {
        private CoroutineStopper() { }

        public void StopCoroutineWithCheck(IEnumerator coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        public void StopCoroutineWithCheck(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }
}
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

        public static Color32 GetColor(KeyValuePair<Emotion, float> emotionPair, Dictionary<Emotion, float> emotions)
        {
            Color32 color;
            if (emotionPair.Value.Equals(0.0f) || emotionPair.Key.Equals(BubbleSystem.Emotion.Default) || emotionPair.Key.Equals(BubbleSystem.Emotion.Neutral))
            {
                color = DefaultData.Instance.GetColor(BubbleSystem.Emotion.Neutral);
            }
            else
            {
                color = DefaultData.Instance.mixColors ? BubbleSystemUtility.MixColors(emotions) : DefaultData.Instance.GetColor(emotionPair.Key);
            }

            return color;
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

        public static void AddToDictionary<T1, T2>(ref Dictionary<T1, T2> dict, T1 first, T2 second)
        {
            if (dict.ContainsKey(first))
                dict[first] = second;
            else
                dict.Add(first, second);
        }

        public static void AddToDictionary<T1, T2, T3>(ref Dictionary<T1, Dictionary<T2, T3>> dict, T1 first, T2 second, T3 third)
        {
            if (dict.ContainsKey(first))
            {
                if (dict[first].ContainsKey(second))
                    dict[first][second] = third;
                else
                    dict[first].Add(second, third);
            }
        }

        public static bool CheckCoroutine(ref IEnumerator coroutine)
        {
            return coroutine != null;
        }

        public static bool CheckCoroutine<T1>(ref Dictionary<T1, IEnumerator> dict, T1 first)
        {
            if (dict.ContainsKey(first))
                if (dict[first] != null)
                    return true;
            return false;
        }

        public static bool CheckCoroutine<T1, T2>(ref Dictionary<T1, Dictionary<T2, IEnumerator>> dict, T1 first, T2 second)
        {
            if (dict.ContainsKey(first))
                if (dict[first].ContainsKey(second))
                    if (dict[first][second] != null)
                        return true;
            return false;
        }
    }
}
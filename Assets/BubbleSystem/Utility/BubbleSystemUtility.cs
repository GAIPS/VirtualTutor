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
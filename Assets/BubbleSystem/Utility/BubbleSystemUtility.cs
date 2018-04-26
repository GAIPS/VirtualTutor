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
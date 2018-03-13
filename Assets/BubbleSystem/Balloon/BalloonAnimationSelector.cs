using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BubbleSystem
{
    public class BalloonAnimationSelector : Modifier
    {
        protected Dictionary<Data, BalloonAnimationData> dictionary = new Dictionary<Data, BalloonAnimationData>();

        BalloonAnimationData balloonAnimationData;

        protected void Start()
        {
            SetDictionary("Balloons/", "Jsons/balloon_animation");
        }

        public BalloonAnimationData SelectBalloonAnimation(Data data)
        {
            try
            {
                return dictionary.Where(key => data.intensity <= key.Key.intensity && key.Key.emotion.Equals(data.emotion)).OrderBy(key => key.Key.intensity).First().Value;
            }
            catch
            {
                throw new KeyNotFoundException("Emotion " + data.emotion + " with intensity " + data.intensity + " not defined.");
            }
        }

        protected override void Add(Data data)
        {
            dictionary.Add(data, balloonAnimationData);
        }

        protected override void Set<T>(T data, string attribute, bool defaultData)
        {
            var type = typeof(T);
            if (type.Equals(typeof(AnimatorOverrideController)) && attribute.Equals("animator_path"))
            {
                balloonAnimationData.animator = (AnimatorOverrideController)Convert.ChangeType(data, typeof(AnimatorOverrideController));
            }
            else if (type.Equals(typeof(float)) && attribute.Equals("duration"))
            {
                balloonAnimationData.duration = (float)Convert.ChangeType(data, typeof(float));
            }
        }
    }
}
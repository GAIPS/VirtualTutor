using System;
using System.Collections.Generic;
using System.Linq;

namespace BubbleSystem
{
    public class BackgroundAnimationSelector : Modifier
    {
        protected Dictionary<Data, BackgroundAnimationData> dictionary = new Dictionary<Data, BackgroundAnimationData>();

        private BackgroundAnimationData backgroundAnimationData;

        protected void Start()
        {
            SetDictionary("Backgrounds/", "Jsons/background_animation");
        }

        public BackgroundAnimationData SelectBackgroundAnimation(Data data)
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
            dictionary.Add(data, backgroundAnimationData);
        }

        protected override void Set<T>(T data, string attribute, bool defaultData)
        {
            var type = typeof(T);
            if (type.Equals(typeof(float)) && attribute.Equals("transition_fade"))
            {
                backgroundAnimationData.imageFadePercentage = (float)Convert.ChangeType(data, typeof(float));
            }
            else if (type.Equals(typeof(float)) && attribute.Equals("color_duration"))
            {
                backgroundAnimationData.colorTransitionData.duration = (float)Convert.ChangeType(data, typeof(float));
            }
            else if (type.Equals(typeof(float)) && attribute.Equals("color_smoothness"))
            {
                backgroundAnimationData.colorTransitionData.smoothness = (float)Convert.ChangeType(data, typeof(float));
            }
        }
    }
}
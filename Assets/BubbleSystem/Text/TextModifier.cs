using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{
    public class TextModifier : Modifier
    {
        private Dictionary<Data, TextData> dictionary = new Dictionary<Data, TextData>();

        private TextData textData;

        private void Start()
        {
            SetDictionary("Text/", "Jsons/text");
        }

        public TextData SelectText(Data data)
        {
            try
            {
                return dictionary.Where(key => data.intensity <= key.Key.intensity && key.Key.emotion.Equals(data.emotion)).OrderBy(key => key.Key.intensity).First().Value;
            }
            catch
            {
                throw new KeyNotFoundException("Emotion " + data.emotion + " with intensity " + data.intensity + " and reason " + data.reason + " not defined.");
            }
        }

        protected override void Add(Data data)
        {
            dictionary.Add(data, textData);
        }

        protected override void Set<T>(T data, string attribute, bool defaultData)
        {
            var type = typeof(T);
            if (type.Equals(typeof(Texture2D)) && attribute.Equals("color"))
            {
                textData.colorData.SetColor((List<object>)Convert.ChangeType(data, typeof(List<object>)));
            }
            else if (type.Equals(typeof(Font)) && attribute.Equals("font"))
            {
                textData.font = (Font)Convert.ChangeType(data, typeof(Font));
            }
            else if (type.Equals(typeof(float)) && attribute.Equals("size"))
            {
                textData.size = (float)Convert.ChangeType(data, typeof(float));
            }
        }
    }
}
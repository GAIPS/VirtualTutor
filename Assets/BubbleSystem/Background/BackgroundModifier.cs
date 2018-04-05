using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BubbleSystem
{
    public class BackgroundModifier : Modifier
    {
        protected Dictionary<Data, TextureData> dictionary = new Dictionary<Data, TextureData>();

        private TextureData textureData;

        protected void Start()
        {
            considerReason = true;
            SetDictionary("Backgrounds/", "Jsons/backgrounds");
        }

        protected override void Add(Data data)
        {
            dictionary.Add(data, textureData);
        }

        public TextureData SelectTexture(Data data)
        {
            try
            {
                return dictionary.Where(key => data.intensity <= key.Key.intensity && key.Key.emotion.Equals(data.emotion) && key.Key.reason.Equals(data.reason)).OrderBy(key => key.Key.intensity).First().Value;
            }
            catch
            {
                throw new KeyNotFoundException("Emotion " + data.emotion + " with intensity " + data.intensity + " and reason " + data.reason + " not defined.");
            }
        }

        protected override void Set<T>(T data, string attribute, bool defaultData)
        {
            var type = typeof(T);
            if (type.Equals(typeof(Texture2D)) && attribute.Equals("image_path"))
                textureData.texture = (Texture2D)Convert.ChangeType(data, typeof(Texture2D));
            else if (type.Equals(typeof(List<object>)) && attribute.Equals("color"))
            {
                textureData.colorData.SetColor((List<object>)Convert.ChangeType(data, typeof(List<object>)));
            }
        }
    }
}
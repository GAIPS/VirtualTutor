using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BubbleSystem
{
    public class BalloonModifier : Modifier
    {
        protected Dictionary<Data, SpriteData> dictionary = new Dictionary<Data, SpriteData>();

        private SpriteData spriteData;

        protected void Start()
        {
            SetDictionary("Balloons/", "Jsons/balloon");
        }

        public SpriteData SelectSprite(Data data)
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
            dictionary.Add(data, spriteData);
        }

        protected override void Set<T>(T data, string attribute, bool defaultData)
        {
            var type = typeof(T);
            if (type.Equals(typeof(Texture2D)) && attribute.Equals("image_path"))
            {
                Texture2D tex = (Texture2D)Convert.ChangeType(data, typeof(Texture2D));
                spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }
    }
}
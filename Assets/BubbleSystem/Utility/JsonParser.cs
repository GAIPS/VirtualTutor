using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization;

namespace BubbleSystem
{    
    public class JsonParser
    {
        private static JsonParser instance;
        public static JsonParser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JsonParser();
                }
                return instance;
            }
        }

        private JsonParser() {}

        public Dictionary<string, System.Object> ParseJson(string folder)
        {
            var data = Resources.Load<TextAsset>(folder).text;
            return Json.Deserialize(data) as Dictionary<string, System.Object>;
        }
    }
}
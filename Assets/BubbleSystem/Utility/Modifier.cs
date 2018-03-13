using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BubbleSystem
{
    public abstract class Modifier : MonoBehaviour
    {
        protected bool considerReason = false;

        protected abstract void Add(Data data);
        protected abstract void Set<T>(T data, string attribute, bool defaultData = false);

        protected void Cases(string folder, Texture2D tex, string dictKey, Dictionary<string, System.Object> dictionary)
        {
            switch (dictKey)
            {
                case "image_path":
                    tex = (Texture2D)Resources.Load(folder + dictionary[dictKey]);
                    Set(tex, dictKey);
                    return;
                case "color":
                    var colorList = dictionary[dictKey] as List<System.Object>;
                    Set(colorList, dictKey);
                    return;
                case "font":
                    Font font = (Font)Resources.GetBuiltinResource(typeof(Font), dictionary[dictKey] + ".ttf");
                    Set(font, dictKey);
                    return;
                case "size":
                    Set(Convert.ToSingle(dictionary[dictKey]), dictKey);
                    return;
                case "transition_fade":
                    Set(Convert.ToSingle(dictionary[dictKey]), dictKey);
                    return;
                case "color_duration":
                    Set(Convert.ToSingle(dictionary[dictKey]), dictKey);
                    return;
                case "color_smoothness":
                    Set(Convert.ToSingle(dictionary[dictKey]), dictKey);
                    return;
                case "animator_path":
                    Set((AnimatorOverrideController)Resources.Load(folder + dictionary[dictKey]), dictKey);
                    return;
                case "duration":
                    Set(Convert.ToSingle(dictionary[dictKey]), dictKey);
                    return;
                default:
                    Debug.Log("Key " + dictKey + " not found");
                    return;
            }
        }

        protected void SetDictionary(string folder, string file)
        {
            var json = JsonParser.Instance.ParseJson(folder + file);

            foreach (var emotionKey in json.Keys)
            {
                Data data = new Data();
                Texture2D tex = new Texture2D(2, 2);

                data.emotion = (Emotion)Enum.Parse(typeof(Emotion), emotionKey);

                var emotion = json[emotionKey] as Dictionary<string, System.Object>;

                foreach (var intensityKey in emotion.Keys)
                {
                    data.intensity = Mathf.Clamp01(Convert.ToSingle(intensityKey));
                    var intensity = emotion[intensityKey] as Dictionary<string, System.Object>;
                    foreach (var reasonKey in intensity.Keys)
                    {
                        if (considerReason)
                        {
                            data.reason = (Reason)Enum.Parse(typeof(Reason), reasonKey as string);
                            var reason = intensity[reasonKey] as Dictionary<string, System.Object>;
                            foreach (var dictKey in reason.Keys)
                            {
                                Cases(folder, tex, dictKey, reason);
                            }
                            Add(data);
                        }
                        else
                        {
                            Cases(folder, tex, reasonKey, intensity);
                        }                        
                    }
                    if(!considerReason)
                        Add(data);
                }
            }
        }
    }
}
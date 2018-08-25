using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public static class BubbleSystemCommands
    {
        //<< OverrideTextEffects emotion intensity [showEffects [curve] ...] [hideEffects effect1 [curve] ...] >>
        public static void OverrideTextEffects(string[] info)
        {
            object parsedEnum;
            if (!EnumUtils.TryParse(typeof(string), info[0], out parsedEnum)) return;
            float intensity = Mathf.Clamp01(Convert.ToSingle(info[1]));

            KeyValuePair<Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>> effects = SetTextEffects(info, 2);
            DefaultData.Instance.SetTextEffects((string) parsedEnum, intensity, effects.Key, effects.Value);
        }

        //<< SetMixColors boolInIntFormat >>   0 -> false; 1 -> true
        public static void SetMixColors(string[] info)
        {
            DefaultData.Instance.mixColors = Convert.ToBoolean(Convert.ToInt16(info[0]));
        }

        //<< OverrideBlushColor color >>   Color in #RRGGBBAA format
        public static void OverrideBlushColor(string[] info)
        {
            Color color;
            ColorUtility.TryParseHtmlString(info[0], out color);
            DefaultData.Instance.SetBlushColor(color);
        }

        //Only works for backgrounds
        //<< OverrideEmotionColor emotion color >>   Color in #RRGGBBAA format
        public static void OverrideEmotionColor(string[] info)
        {
            object parsedEnum;
            Color color;
            if (!EnumUtils.TryParse(typeof(Emotion.EmotionEnum), info[0], out parsedEnum) || !ColorUtility.TryParseHtmlString(info[1], out color)) return;
            DefaultData.Instance.SetColor((Emotion.EmotionEnum) parsedEnum, color);
        }

        //Can also override
        //<< AddAnimationCurve name time1 value1 [smooth weight1] time2 value2 [smooth weight2] ... >>
        public static void AddAnimationCurve(string[] info)
        {
            string name = info[0];
            AnimationCurve curve = new AnimationCurve();
            int indexToSmooth = -1;
            List<KeyValuePair<int, float>> smoothTangents = new List<KeyValuePair<int, float>>();

            for (int i = 1; i < info.Length; i = i + 2)
            {
                if (info[i].Equals("smooth"))
                    smoothTangents.Add(new KeyValuePair<int, float>(indexToSmooth, Convert.ToSingle(info[i + 1])));
                else
                {
                    curve.AddKey(new Keyframe(Convert.ToSingle(info[i]), Convert.ToSingle(info[i + 1])));
                    indexToSmooth++;
                }
            }

            foreach (KeyValuePair<int, float> kvp in smoothTangents)
            {
                curve.SmoothTangents(kvp.Key, kvp.Value);
            }

            DefaultData.Instance.AddCurve(name, curve);
        }

        //<< SetForceTextUpdate boolInIntFormat >>   0 -> false; 1 -> true
        public static void SetForceTextUpdate(string[] info)
        {
            DefaultData.Instance.forceTextUpdate = Convert.ToBoolean(Convert.ToInt16(info[0]));
        }

        //<< SetEmotionBlending boolInIntFormat >>   0 -> false; 1 -> true
        public static void SetBalloonAnimationBlending(string[] info)
        {
            DefaultData.Instance.blendBalloonAnimation = Convert.ToBoolean(Convert.ToInt16(info[0]));
        }

        //<< SetBalloonDuration duration >>
        public static void SetBalloonDuration(string[] info)
        {
            DefaultData.Instance.SetBalloonDuration(Convert.ToSingle(info[0]));
        }

        //<< SetBackgroundDuration duration >>
        public static void SetBackgroundDuration(string[] info)
        {
            DefaultData.Instance.SetBackgroundDuration(Convert.ToSingle(info[0]));
        }

        //<<SetOptionsDuration duration>>
        public static void SetOptionsDuration(string[] info)
        {
            DefaultData.Instance.SetOptionsDuration(Convert.ToSingle(info[0]));
        }


        /**********************************************************************************************************
                                                    HELP FUNCTIONS
    **********************************************************************************************************/

        private static KeyValuePair<Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>> SetTextEffects(string[] info, int i)
        {
            Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve> showEffects = null;
            Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve> hideEffects = null;
            KeyValuePair<int, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>> kvp;
            int size = info.Length;
            if (size > i)
            {
                if (info[i].Equals("showEffects"))
                {
                    kvp = GetEffects(info, i + 1);
                    showEffects = kvp.Value;
                    if (size > kvp.Key)
                    {
                        if (info[kvp.Key].Equals("hideEffects"))
                        {
                            kvp = GetEffects(info, kvp.Key + 1);
                            hideEffects = kvp.Value;
                        }
                    }
                }
                else if (info[i].Equals("hideEffects"))
                {
                    kvp = GetEffects(info, i + 1);
                    hideEffects = kvp.Value;
                }
            }

            return new KeyValuePair<Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>>(showEffects, hideEffects);
        }

        private static KeyValuePair<int, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>> GetEffects(string[] info, int i)
        {
            Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve> effects = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
            string stringToLook = "hideEffects";
            while (i < info.Length)
            {
                if (info[i].Equals(stringToLook))
                    break;

                AnimationCurve animationCurve;
                try
                {
                    object parsedEnum;
                    if (!EnumUtils.TryParse(typeof(AbstractTextEffect.TextEffectEnum), info[i], out parsedEnum)) continue;
                    animationCurve = (i + 1 >= info.Length) ? null : (info[i + 1].Equals(stringToLook)) ? null : DefaultData.Instance.GetCurve(info[i + 1]);
                    i += (animationCurve != null) ? 2 : 1;

                    effects.Add((AbstractTextEffect.TextEffectEnum) parsedEnum, animationCurve);
                    continue;
                }
                catch
                {
                    i++;
                    continue;
                }
            }

            return new KeyValuePair<int, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>>(i, effects);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class OverrideTextEffectsCommand : AbstractCommand
    {
        public OverrideTextEffectsCommand()
        {
            _name = "OverrideTextEffects";
        }

        //<< OverrideTextEffects emotion intensity [showEffects [curve] ...] [hideEffects effect1 [curve] ...] >>
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            object parsedEnum;
            float intensity;
            if (!EnumUtils.TryParse(typeof(BubbleSystem.Emotion.EmotionEnum), info[1], out parsedEnum) || !Single.TryParse(info[2], out intensity)) return;
            intensity = Mathf.Clamp01(intensity);
            BubbleSystem.Emotion.EmotionEnum emotion = (BubbleSystem.Emotion.EmotionEnum)parsedEnum;

            KeyValuePair<Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>> effects = GetTextEffects(info, 3);
            DefaultData.Instance.SetTextEffects(emotion.ToString(), intensity, effects.Key, effects.Value);
        }

        /**********************************************************************************************************
                                                HELP FUNCTIONS
        **********************************************************************************************************/

        private static KeyValuePair<Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>> GetTextEffects(string[] info, int i)
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

                    effects.Add((AbstractTextEffect.TextEffectEnum)parsedEnum, animationCurve);
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
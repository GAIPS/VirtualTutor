using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class OverrideEmotionColorCommand : AbstractCommand
    {
        public OverrideEmotionColorCommand()
        {
            _name = "OverrideEmotionColor";
        }

        //Only works for backgrounds
        //<< OverrideEmotionColor emotion color >>   Color in #RRGGBBAA format
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            object parsedEnum;
            Color color;
            if (!EnumUtils.TryParse(typeof(Emotion.EmotionEnum), info[1], out parsedEnum) || !ColorUtility.TryParseHtmlString(info[2], out color)) return;
            DefaultData.Instance.SetColor((Emotion.EmotionEnum)parsedEnum, color);
        }
    }
}
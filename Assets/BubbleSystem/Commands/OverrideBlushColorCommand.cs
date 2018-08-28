using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class OverrideBlushColorCommand : AbstractCommand
    {
        public OverrideBlushColorCommand()
        {
            _name = "OverrideBlushColor";
        }

        //<< OverrideBlushColor color >>    Color in #RRGGBBAA format
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            Color color;
            if(!ColorUtility.TryParseHtmlString(info[1], out color)) return;
            DefaultData.Instance.SetBlushColor(color);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class SetMixColorsCommand : AbstractCommand
    {
        public SetMixColorsCommand()
        {
            _name = "SetMixColors";
        }

        //<< SetMixColors boolInIntFormat >>   0 -> false; 1 -> true
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            Int16 mix;
            if (!Int16.TryParse(info[1], out mix)) return;
            DefaultData.Instance.mixColors = Convert.ToBoolean(mix);
        }
    }
}
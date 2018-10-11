using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class SetBalloonAnimationBlendingCommand : AbstractCommand
    {
        public SetBalloonAnimationBlendingCommand()
        {
            _name = "SetBalloonAnimationBlending";
        }

        //<< SetBalloonAnimationBlending boolInIntFormat >>   0 -> false; 1 -> true
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            Int16 blend;
            if (!Int16.TryParse(info[1], out blend)) return;
            DefaultData.Instance.blendBalloonAnimation = Convert.ToBoolean(blend);
        }
    }
}
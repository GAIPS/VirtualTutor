using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class SetBalloonDurationCommand : AbstractCommand
    {
        public SetBalloonDurationCommand()
        {
            _name = "SetBalloonDuration";
        }

        //<< SetBalloonDuration duration >>
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            float duration;
            if (!Single.TryParse(info[1], out duration)) return;
            DefaultData.Instance.SetBalloonDuration(duration);
        }
    }
}
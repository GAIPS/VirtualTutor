using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class SetBackgroundDurationCommand : AbstractCommand
    {
        public SetBackgroundDurationCommand()
        {
            _name = "SetBackgroundDuration";
        }

        //<< SetBackgroundDuration duration >>
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            float duration;
            if(!Single.TryParse(info[1], out duration)) return;
            DefaultData.Instance.SetBackgroundDuration(duration);
        }
    }
}
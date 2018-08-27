using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class SetOptionsDurationCommand : AbstractCommand
    {
        public SetOptionsDurationCommand()
        {
            _name = "SetOptionsDuration";
        }

        //<<SetOptionsDuration duration>>
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            DefaultData.Instance.SetOptionsDuration(Convert.ToSingle(info[1]));
        }
    }
}
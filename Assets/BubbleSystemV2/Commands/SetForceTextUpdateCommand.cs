using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class SetForceTextUpdateCommand : AbstractCommand
    {
        public SetForceTextUpdateCommand()
        {
            _name = "SetForceTextUpdate";
        }

        //<< SetForceTextUpdate boolInIntFormat >>   0 -> false; 1 -> true
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            Int16 force;
            if (!Int16.TryParse(info[1], out force)) return;
            DefaultData.Instance.forceTextUpdate = Convert.ToBoolean(force);
        }
    }
}
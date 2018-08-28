using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class AddAnimationCurveCommand : AbstractCommand
    {
        public AddAnimationCurveCommand()
        {
            _name = "AddAnimationCurve";
        }

        //Can also override
        //<< AddAnimationCurve name time1 value1 [smooth weight1] time2 value2 [smooth weight2] ... >>
        public override void Run(string[] info)
        {
            if (!CheckName(info[0])) return;
            string name = info[1];
            AnimationCurve curve = new AnimationCurve();
            int indexToSmooth = -1;
            List<KeyValuePair<int, float>> smoothTangents = new List<KeyValuePair<int, float>>();

            for (int i = 2; i < info.Length; i = i + 2)
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
    }
}
using HookControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem2
{
    public class BalloonManager : AbstractBubbleSystemModule
    {
        public Balloon[] balloons;
        private bool firstInteraction = true;

        public override void UpdateScene(BubbleSystemData data)
        {
            if (data.balloonData.IsCleared() && data.balloonData.show == true) return;
            if (firstInteraction)
            {
                ReverseTutorsBalloons(data.tutor.GetString());
                firstInteraction = false;
            }

            foreach (Balloon balloon in balloons)
                balloon.UpdateScene(data);
        }

        public void ReverseTutorsBalloons(string tutor)
        {
            int index = Array.FindIndex(balloons, balloon => balloon.balloonData._name == tutor);
            int options = Array.FindIndex(balloons, balloon => balloon.balloonData.options == true);

            if (balloons[index].balloonData.dontSwitch || balloons[index].balloonData.options) return;

            int element = BubbleSystemUtility.RandomExcludingNumbers(new int[] { index, options }, balloons.Length);

            Control first = balloons[index].control;
            Control second = balloons[element].control;
            balloons[index].control = second;
            balloons[element].control = first;

            foreach (Balloon balloon in balloons)
            {
                balloon.balloonData.isTailLeft = !balloon.balloonData._name.Equals(tutor);
                balloon.SetTails();
            }
        }
    }
}
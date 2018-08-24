using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class BubbleSystemData : AbstractBubbleSystemData
    {
        public Tutor tutor = new Tutor();
        public BackgroundData backgroundData = new BackgroundData();
        public BalloonData balloonData = new BalloonData();
        public Dictionary<Emotion, float> emotions = new Dictionary<Emotion, float>();

        public override bool IsCleared()
        {
            return backgroundData.IsCleared() && balloonData.IsCleared() && emotions.Count == 0 && tutor.IsCleared();
        }

        public override void Clear()
        {
            backgroundData.Clear();
            balloonData.Clear();
            emotions.Clear();
            tutor.Clear();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class BackgroundData : AbstractBubbleSystemData
    {
        public Reason reason = new Reason(Reason.ReasonEnum.None.ToString());
        public EffectsData<AbstractImageEffect.ImageEffectEnum> effects = new EffectsData<AbstractImageEffect.ImageEffectEnum>();


        public override void Clear()
        {
            reason.Clear();
            effects.Clear();
        }

        public override bool IsCleared()
        {
            return reason.IsCleared() && effects.IsCleared();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2{
    public class Reason : AbstractVTData
    {
        public Reason(string value)
        {
            Set<ReasonEnum>(value);
        }

        public enum ReasonEnum
        {
            None,
            Challenge,
            Effort,
            Engagement,
            Enjoyment,
            Importance,
            Performance
        }

        public ReasonEnum Get()
        {
            return base.Get<ReasonEnum>();
        }

        public void Set(string value)
        {
            base.Set<ReasonEnum>(value);
        }

        public override string GetString()
        {
            return Get().ToString();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem{
    public class Reason : AbstractVTData
    {
        public Reason() { }

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
            return base.Get<ReasonEnum>(this.GetType().Name);
        }

        public void Set(string value)
        {
            base.Set<ReasonEnum>(value);
        }

        public void Set(ReasonEnum value)
        {
            Set(value.ToString());
        }

        public override string GetString()
        {
            return Get().ToString();
        }
    }
}
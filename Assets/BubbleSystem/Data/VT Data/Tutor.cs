using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class Tutor : AbstractVTData
    {
        public Tutor() { }

        public Tutor(string value)
        {
            Set<TutorEnum>(value);
        }

        public enum TutorEnum
        {
            User,
            Joao,
            Maria
        }

        public TutorEnum Get()
        {
            return base.Get<TutorEnum>(this.GetType().Name);
        }

        public void Set(string value)
        {
            base.Set<TutorEnum>(value);
        }

        public void Set(TutorEnum value)
        {
            Set(value.ToString());
        }

        public override string GetString()
        {
            return Get().ToString();
        }
    }
}
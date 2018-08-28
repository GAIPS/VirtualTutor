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
            return base.Get<TutorEnum>();
        }

        public void Set(string value)
        {
            base.Set<TutorEnum>(value);
        }

        public override string GetString()
        {
            return Get().ToString();
        }
    }
}
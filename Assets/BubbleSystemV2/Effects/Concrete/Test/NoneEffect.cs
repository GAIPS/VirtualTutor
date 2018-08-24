using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class NoneEffect : AbstractTextEffect
    {
        private static readonly NoneEffect instance = new NoneEffect();

        private NoneEffect() { }

        public static NoneEffect Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            yield return 0;
        }
    }
}
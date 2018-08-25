using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class NoneEffect : AbstractTextEffect
    {
        public NoneEffect() {
        }

        protected override void Clean(TextEffectData data)
        {
            base.Clean(data);
        }

        public override IEnumerator Run(TextEffectData data)
        {
            Clean(data);
            yield break;
        }
    }
}
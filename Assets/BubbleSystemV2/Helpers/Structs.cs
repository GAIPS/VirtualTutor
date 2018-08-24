using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public struct SpriteData
    {
        public Sprite sprite;
        public Sprite tail;
    }

    public struct TextData
    {
        public TMPro.TMP_FontAsset font;
        public float size;
        public Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve> showEffect;
        public Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve> hideEffect;
    }
}
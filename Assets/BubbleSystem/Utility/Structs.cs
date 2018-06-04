using System;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public enum Emotion
    {
        Neutral,
        Happiness,
        Sadness,
        Anger,
        Fear,
        Disgust,
        Surprise
    }

    public enum Reason
    {
        None,
        Challenge,
        Effort,
        Engagement,
        Enjoyment,
        Importance,
        Performance
    }

    public enum Situation
    {
        LesserReward,
        GreaterReward,
        ExpectedReward,
        LesserPunishment,
        GreaterPunishment,
        ExpectedPunishment
    }

    public struct TextData
    {
        public TMPro.TMP_FontAsset font;
        public float size;
        public Dictionary<Effect, AnimationCurve> showEffect;
        public Dictionary<Effect, AnimationCurve> hideEffect;
    }

    public struct BackgroundAnimationData
    {
        public Dictionary<BackgroundEffect, AnimationCurve> showBannerEffect;
        public Dictionary<BackgroundEffect, AnimationCurve> hideBannerEffect;
        public Dictionary<BackgroundEffect, AnimationCurve> colorEffect;
    }

    public struct SpriteData
    {
        public Sprite sprite;
        public Sprite beak;
    }

    public struct TextureData
    {
        public Texture2D texture;
    }

    public struct SpeakData
    {
        public Dictionary<Emotion, float> emotions;
        //Top, Left, Right, Extra
        public string[] text;

        public Dictionary<Effect, AnimationCurve> showEffects;
        public Dictionary<Effect, AnimationCurve> hideEffects;
    }

    public struct BackgroundData
    {
        public Dictionary<Emotion, float> emotions;
        public Reason reason;
        public BackgroundAnimationData animationData;
    }

    public struct NextDialogueData
    {
        public Dictionary<string, float> emotions;
        public Dictionary<Effect, AnimationCurve> showEffects;
        public Dictionary<Effect, AnimationCurve> hideEffects;
        public bool isSet;
    }
}
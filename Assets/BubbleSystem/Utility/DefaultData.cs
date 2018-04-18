using BubbleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultData : Singleton<DefaultData>
{
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, TextData>> defaultTextData = new Dictionary<BubbleSystem.Emotion, Dictionary<float, TextData>>();
    private Dictionary<float, DefaultBalloonAnimationData> neutralBalloonAnimationData = new Dictionary<float, DefaultBalloonAnimationData>();
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, BalloonAnimationData>> balloonAnimationData = new Dictionary<BubbleSystem.Emotion, Dictionary<float, BalloonAnimationData>>();
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, SpriteData>> defaultBalloonData = new Dictionary<BubbleSystem.Emotion, Dictionary<float, SpriteData>>();
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, BackgroundAnimationData>> defaultBackgroundAnimationData = new Dictionary<BubbleSystem.Emotion, Dictionary<float, BackgroundAnimationData>>();
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, Dictionary<Reason, TextureData>>> defaultBackgroundDataDictionary = new Dictionary<BubbleSystem.Emotion, Dictionary<float, Dictionary<Reason, TextureData>>>();
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, Dictionary<string, PositionData>>> defaultPositions = new Dictionary<BubbleSystem.Emotion, Dictionary<float, Dictionary<string, PositionData>>>();

    public Color blushColor = Color.red;
    public AnimationCurve bellCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0));
    public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    public AnimationCurve flashCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f), new Keyframe(2f, 0f), new Keyframe(3f, 0f), new Keyframe(4f, 1f), new Keyframe(5f, 1f), new Keyframe(6f, 0f));
    public AnimationCurve lowerBellCurve = new AnimationCurve(new Keyframe(0f, -0.25f), new Keyframe(1f, 0.25f), new Keyframe(2f, -0.25f));
    public AnimationCurve palpitationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f), new Keyframe(1.5f, 1f), new Keyframe(2f, 0f), new Keyframe(3f, 0f));

    private Color32 defaultColor = Color.white;
    private Color32 happinessColor = new Color32(0xF0, 0xE6, 0x4D, 0xFF);
    private Color32 sadnessColor = new Color32(0x1D, 0x33, 0xCE, 0xFF);
    private Color32 angerColor = new Color32(0xFF, 0x00, 0xFF, 0xFF);
    private Color32 fearColor = new Color32(0xAE, 0x52, 0xEC, 0xFF);
    private Color32 disgustColor = new Color32(0xC5, 0xD1, 0x37, 0xFF);
    private Color32 surpriseColor = new Color32(0xFF, 0xC3, 0x58, 0xFF);

    private float happinessSizeRatio = 1f;
    private float sadnessSizeRatio = 0.7f;
    private float angerSizeRatio = 1.5f;
    private float fearSizeRatio = 0.8f;
    private float disgustSizeRatio = 0.8f;
    private float surpriseSizeRatio = 1.2f;

    public struct PositionData
    {
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Quaternion localRotation;
    }

    private DefaultData() { }

    private void Awake()
    {
        SetTextData();
        SetBalloonAnimation();
        SetBallon();
        SetBackground();
        SetBackgroundAnimation();
        SetBalloonPositions();
    }

    public DefaultBalloonAnimationData GetNeutralBalloonAnimationData(float intensity)
    {
        return neutralBalloonAnimationData.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
    }

    public TextData GetDefaultTextData(BubbleSystem.Emotion emotion, float intensity)
    {
        Dictionary<float, TextData> dict = defaultTextData[emotion];
        return dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
    }

    public BalloonAnimationData GetBalloonAnimationData(BubbleSystem.Emotion emotion, float intensity)
    {
        Dictionary<float, BalloonAnimationData> dict = balloonAnimationData[emotion];
        return dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
    }

    public SpriteData GetDefaultBalloonData(BubbleSystem.Emotion emotion, float intensity)
    {
        Dictionary<float, SpriteData> dict = defaultBalloonData[emotion];
        return dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
    }

    public BackgroundAnimationData GetDefaultBackgroundAnimationData(BubbleSystem.Emotion emotion, float intensity)
    {
        Dictionary<float, BackgroundAnimationData> dict = defaultBackgroundAnimationData[emotion];
        return dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
    }

    public TextureData GetDefaultBackgroundDataDictionary(BubbleSystem.Emotion emotion, float intensity, Reason reason)
    {
        Dictionary<float, Dictionary<Reason, TextureData>> dict = defaultBackgroundDataDictionary[emotion];
        Dictionary<Reason, TextureData> intensityDict = dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
        return intensityDict.Where(key => reason.Equals(key.Key)).FirstOrDefault().Value;
    }

    public PositionData GetDefaultPositions(BubbleSystem.Emotion emotion, float intensity, string beak)
    {
        Dictionary<float, Dictionary<string, PositionData>> dict = defaultPositions[emotion];
        Dictionary<string, PositionData> intensityDict = dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
        return intensityDict.Where(key => beak.Equals(key.Key)).FirstOrDefault().Value;
    }

    public AnimationCurve GetCurve(string name)
    {
        switch (name){
            case "bellCurve":
                return bellCurve;
            case "fadeCurve":
                return fadeCurve;
            case "flashCurve":
                return flashCurve;
            case "jitterCurve":
                return lowerBellCurve;
            case "lowerBellCurve":
                return lowerBellCurve;
            case "palpitationCurve":
                return palpitationCurve;
        }
        throw new KeyNotFoundException("Animation Curve with name " + name + " does not exist.");
    }

    private void SetBalloonPositions()
    {
        Dictionary<string, PositionData> neutralPositions = new Dictionary<string, PositionData>();
        Dictionary<string, PositionData> happinessPositions = new Dictionary<string, PositionData>();
        Dictionary<string, PositionData> sadnessPositions = new Dictionary<string, PositionData>();
        Dictionary<string, PositionData> angerPositions = new Dictionary<string, PositionData>();
        Dictionary<string, PositionData> fearPositions = new Dictionary<string, PositionData>();
        Dictionary<string, PositionData> disgustPositions = new Dictionary<string, PositionData>();
        Dictionary<string, PositionData> surprisePositions = new Dictionary<string, PositionData>();

        Dictionary<float, Dictionary<string, PositionData>> dict = new Dictionary<float, Dictionary<string, PositionData>>();

        PositionData rect = new PositionData();

        //NEUTRAL

        rect.anchorMin = new Vector2(0.6f, 0.91f);
        rect.anchorMax = new Vector2(0.8f, 1.35f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        neutralPositions.Add("Peak_top_right", rect);

        rect.anchorMin = new Vector2(0.2f, 0.91f);
        rect.anchorMax = new Vector2(0.4f, 1.35f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        neutralPositions.Add("Peak_top_left", rect);

        rect.anchorMin = new Vector2(0.6f, -0.36f);
        rect.anchorMax = new Vector2(0.8f, 0.12f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        neutralPositions.Add("Peak_bot_right", rect);

        rect.anchorMin = new Vector2(0.2f, -0.36f);
        rect.anchorMax = new Vector2(0.4f, 0.12f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        neutralPositions.Add("Peak_bot_left", rect);

        dict.Add(1f, neutralPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Neutral, dict);
        defaultPositions.Add(BubbleSystem.Emotion.Default, dict);


        //HAPPINESS
        dict = new Dictionary<float, Dictionary<string, PositionData>>();

        rect.anchorMin = new Vector2(0.6f, 0.81f);
        rect.anchorMax = new Vector2(0.8f, 1.25f);
        rect.localRotation = Quaternion.Euler(0, 0, 190);
        happinessPositions.Add("Peak_top_right", rect);

        rect.anchorMin = new Vector2(0.2f, 0.81f);
        rect.anchorMax = new Vector2(0.4f, 1.25f);
        rect.localRotation = Quaternion.Euler(0, 180, 190);
        happinessPositions.Add("Peak_top_left", rect);

        rect.anchorMin = new Vector2(0.6f, -0.26f);
        rect.anchorMax = new Vector2(0.8f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 180, 10);
        happinessPositions.Add("Peak_bot_right", rect);

        rect.anchorMin = new Vector2(0.2f, -0.26f);
        rect.anchorMax = new Vector2(0.4f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 0, 10);
        happinessPositions.Add("Peak_bot_left", rect);

        dict.Add(1f, happinessPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Happiness, dict);


        //SADNESS
        dict = new Dictionary<float, Dictionary<string, PositionData>>();

        rect.anchorMin = new Vector2(0.6f, 0.83f);
        rect.anchorMax = new Vector2(0.8f, 1.27f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        sadnessPositions.Add("Peak_top_right", rect);

        rect.anchorMin = new Vector2(0.2f, 0.83f);
        rect.anchorMax = new Vector2(0.4f, 1.27f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        sadnessPositions.Add("Peak_top_left", rect);

        rect.anchorMin = new Vector2(0.6f, -0.26f);
        rect.anchorMax = new Vector2(0.8f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        sadnessPositions.Add("Peak_bot_right", rect);

        rect.anchorMin = new Vector2(0.2f, -0.26f);
        rect.anchorMax = new Vector2(0.4f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        sadnessPositions.Add("Peak_bot_left", rect);

        dict.Add(1f, sadnessPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Sadness, dict);


        //ANGER
        dict = new Dictionary<float, Dictionary<string, PositionData>>();

        rect.anchorMin = new Vector2(0.6f, 0.6f);
        rect.anchorMax = new Vector2(0.8f, 1.04f);
        rect.localRotation = Quaternion.Euler(0, 180, 170);
        angerPositions.Add("Peak_top_right", rect);

        rect.anchorMin = new Vector2(0.2f, 0.57f);
        rect.anchorMax = new Vector2(0.4f, 1.03f);
        rect.localRotation = Quaternion.Euler(0, 0, 170);
        angerPositions.Add("Peak_top_left", rect);

        rect.anchorMin = new Vector2(0.6f, -0.04f);
        rect.anchorMax = new Vector2(0.8f, 0.4f);
        rect.localRotation = Quaternion.Euler(0, 0, -10);
        angerPositions.Add("Peak_bot_right", rect);

        rect.anchorMin = new Vector2(0.2f, -0.09f);
        rect.anchorMax = new Vector2(0.4f, 0.35f);
        rect.localRotation = Quaternion.Euler(0, 180, -10);
        angerPositions.Add("Peak_bot_left", rect);

        dict.Add(1f, angerPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Anger, dict);


        //FEAR
        dict = new Dictionary<float, Dictionary<string, PositionData>>();

        rect.anchorMin = new Vector2(0.65f, 0.73f);
        rect.anchorMax = new Vector2(0.8f, 1.17f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        fearPositions.Add("Peak_top_right", rect);

        rect.anchorMin = new Vector2(0.25f, 0.78f);
        rect.anchorMax = new Vector2(0.4f, 1.22f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        fearPositions.Add("Peak_top_left", rect);

        rect.anchorMin = new Vector2(0.65f, -0.24f);
        rect.anchorMax = new Vector2(0.8f, 0.2f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        fearPositions.Add("Peak_bot_right", rect);

        rect.anchorMin = new Vector2(0.25f, -0.2f);
        rect.anchorMax = new Vector2(0.4f, 0.24f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        fearPositions.Add("Peak_bot_left", rect);

        dict.Add(1f, fearPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Fear, dict);


        //DISGUST
        dict = new Dictionary<float, Dictionary<string, PositionData>>();

        rect.anchorMin = new Vector2(0.65f, 0.75f);
        rect.anchorMax = new Vector2(0.85f, 1.19f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        disgustPositions.Add("Peak_top_right", rect);

        rect.anchorMin = new Vector2(0.15f, 0.75f);
        rect.anchorMax = new Vector2(0.35f, 1.19f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        disgustPositions.Add("Peak_top_left", rect);

        rect.anchorMin = new Vector2(0.65f, -0.24f);
        rect.anchorMax = new Vector2(0.85f, 0.24f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        disgustPositions.Add("Peak_bot_right", rect);

        rect.anchorMin = new Vector2(0.15f, -0.24f);
        rect.anchorMax = new Vector2(0.35f, 0.24f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        disgustPositions.Add("Peak_bot_left", rect);

        dict.Add(1f, disgustPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Disgust, dict);


        //SURPRISE
        dict = new Dictionary<float, Dictionary<string, PositionData>>();

        rect.anchorMin = new Vector2(0.6f, 0.66f);
        rect.anchorMax = new Vector2(0.8f, 1.1f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        surprisePositions.Add("Peak_top_right", rect);

        rect.anchorMin = new Vector2(0.2f, 0.66f);
        rect.anchorMax = new Vector2(0.4f, 1.1f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        surprisePositions.Add("Peak_top_left", rect);

        rect.anchorMin = new Vector2(0.65f, -0.11f);
        rect.anchorMax = new Vector2(0.8f, 0.33f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        surprisePositions.Add("Peak_bot_right", rect);

        rect.anchorMin = new Vector2(0.2f, -0.11f);
        rect.anchorMax = new Vector2(0.4f, 0.33f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        surprisePositions.Add("Peak_bot_left", rect);

        dict.Add(1f, surprisePositions);
        defaultPositions.Add(BubbleSystem.Emotion.Surprise, dict);
    }

    private void SetTextData()
    {
        TMPro.TMP_FontAsset neutralFont = (TMPro.TMP_FontAsset)Resources.Load("Text/TextMesh_Fonts/arial");
        //TMPro.TMP_FontAsset happinessFont = (TMPro.TMP_FontAsset)Resources.Load("Text/TextMesh_Fonts/coiny-regular");       //NO
        //TMPro.TMP_FontAsset sadnessFont = (TMPro.TMP_FontAsset)Resources.Load("Text/TextMesh_Fonts/AftaSansThin-Regular");  //NO
        //TMPro.TMP_FontAsset angerFont = (TMPro.TMP_FontAsset)Resources.Load("Text/TextMesh_Fonts/AlfaSlabOne-Regular");     //NO
        //TMPro.TMP_FontAsset fearFont = (TMPro.TMP_FontAsset)Resources.Load("Text/TextMesh_Fonts/Adler");                    //NO
        //TMPro.TMP_FontAsset disgustFont = (TMPro.TMP_FontAsset)Resources.Load("Text/TextMesh_Fonts/1942");                  //NO
        //TMPro.TMP_FontAsset surpriseFont = (TMPro.TMP_FontAsset)Resources.Load("Text/TextMesh_Fonts/Courier-Prime");        //NO
        float initialSize = 40.0f;
        Color color = Color.black;
        
        TextData text = new TextData();
        Dictionary<float, TextData> dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.color = color;
        text.size = initialSize;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Default, dict);
        
        defaultTextData[BubbleSystem.Emotion.Default][1f].showEffect.Add(Effect.None, null);
        defaultTextData[BubbleSystem.Emotion.Default][1f].hideEffect.Add(Effect.None, null);


        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.color = color;
        text.size = initialSize;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Neutral, dict);

        defaultTextData[BubbleSystem.Emotion.Neutral][1f].showEffect.Add(Effect.Appear, fadeCurve);
        defaultTextData[BubbleSystem.Emotion.Neutral][1f].hideEffect.Add(Effect.None, null);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.color = color;
        text.size = initialSize * happinessSizeRatio;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Happiness, dict);

        defaultTextData[BubbleSystem.Emotion.Happiness][1f].showEffect.Add(Effect.Wave, bellCurve);
        defaultTextData[BubbleSystem.Emotion.Happiness][1f].hideEffect.Add(Effect.Wave, bellCurve);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.color = color;
        text.size = initialSize * sadnessSizeRatio;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Sadness, dict);

        defaultTextData[BubbleSystem.Emotion.Sadness][1f].showEffect.Add(Effect.FadeIn, fadeCurve);
        defaultTextData[BubbleSystem.Emotion.Sadness][1f].hideEffect.Add(Effect.FadeOut, fadeCurve);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.color = color;
        text.size = initialSize * angerSizeRatio;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Anger, dict);

        defaultTextData[BubbleSystem.Emotion.Anger][1f].showEffect.Add(Effect.Shake, null);
        defaultTextData[BubbleSystem.Emotion.Anger][1f].hideEffect.Add(Effect.Shake, null);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.color = color;
        text.size = initialSize * fearSizeRatio;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Fear, dict);
        
        defaultTextData[BubbleSystem.Emotion.Fear][1f].showEffect.Add(Effect.Jitter, null);
        defaultTextData[BubbleSystem.Emotion.Fear][1f].hideEffect.Add(Effect.Jitter, null);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.color = color;
        text.size = initialSize * disgustSizeRatio;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Disgust, dict);

        defaultTextData[BubbleSystem.Emotion.Disgust][1f].showEffect.Add(Effect.Warp, bellCurve);
        defaultTextData[BubbleSystem.Emotion.Disgust][1f].hideEffect.Add(Effect.Warp, bellCurve);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.color = color;
        text.size = initialSize * surpriseSizeRatio;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Surprise, dict);

        defaultTextData[BubbleSystem.Emotion.Surprise][1f].showEffect.Add(Effect.SwingCharacters, null);
        defaultTextData[BubbleSystem.Emotion.Surprise][1f].hideEffect.Add(Effect.SwingCharacters, null);
    }

    private void SetBalloonAnimation()
    {
        DefaultBalloonAnimationData defaultBalloon = new DefaultBalloonAnimationData();
        defaultBalloon.animator = (RuntimeAnimatorController)Resources.Load("Balloons/Animators/BallonPopup_v2");
        defaultBalloon.duration = 5;

        neutralBalloonAnimationData.Add(1f, defaultBalloon);

        Dictionary<float, BalloonAnimationData> dict = new Dictionary<float, BalloonAnimationData>();

        BalloonAnimationData balloon = new BalloonAnimationData();
        balloon.animator = (AnimatorOverrideController)Resources.Load("Balloons/Animators/HappyAnimator");
        balloon.duration = 5;
        dict.Add(1f, balloon);
        balloonAnimationData.Add(BubbleSystem.Emotion.Happiness, dict);

        dict = new Dictionary<float, BalloonAnimationData>();
        balloon = new BalloonAnimationData();
        balloon.animator = (AnimatorOverrideController)Resources.Load("Balloons/Animators/SadAnimator");
        balloon.duration = 5;
        dict.Add(1f, balloon);
        balloonAnimationData.Add(BubbleSystem.Emotion.Sadness, dict);

        dict = new Dictionary<float, BalloonAnimationData>();
        balloon = new BalloonAnimationData();
        balloon.animator = (AnimatorOverrideController)Resources.Load("Balloons/Animators/AngerAnimator");
        balloon.duration = 5;
        dict.Add(1f, balloon);
        balloonAnimationData.Add(BubbleSystem.Emotion.Anger, dict);

        dict = new Dictionary<float, BalloonAnimationData>();
        balloon = new BalloonAnimationData();
        balloon.animator = (AnimatorOverrideController)Resources.Load("Balloons/Animators/FearAnimator");
        balloon.duration = 5;
        dict.Add(1f, balloon);
        balloonAnimationData.Add(BubbleSystem.Emotion.Fear, dict);

        dict = new Dictionary<float, BalloonAnimationData>();
        balloon = new BalloonAnimationData();
        balloon.animator = (AnimatorOverrideController)Resources.Load("Balloons/Animators/DisgustAnimator");
        balloon.duration = 5;
        dict.Add(1f, balloon);
        balloonAnimationData.Add(BubbleSystem.Emotion.Disgust, dict);

        dict = new Dictionary<float, BalloonAnimationData>();
        balloon = new BalloonAnimationData();
        balloon.animator = (AnimatorOverrideController)Resources.Load("Balloons/Animators/SurpriseAnimator");
        balloon.duration = 5;
        dict.Add(1f, balloon);
        balloonAnimationData.Add(BubbleSystem.Emotion.Surprise, dict);


    }

    private void SetBallon()
    {
        Dictionary<float, SpriteData> dict = new Dictionary<float, SpriteData>();

        var tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Neutral/neutral_balloon");
        var beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Neutral/neutral_beak");
        SpriteData spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.color = Color.white;
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Neutral, dict);
        defaultBalloonData.Add(BubbleSystem.Emotion.Default, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Happiness/happiness_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Happiness/happiness_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.color = Color.white;
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Happiness, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Sadness/sadness_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Sadness/sadness_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.color = Color.white;
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Sadness, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Anger/anger_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Anger/anger_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.color = Color.white;
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Anger, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Fear/fear_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Fear/fear_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.color = Color.white;
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Fear, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Disgust/disgust_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Disgust/disgust_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.color = Color.white;
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Disgust, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Surprise/surprise_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Surprise/surprise_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.color = Color.white;
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Surprise, dict);
    }

    private void SetBackground()
    {
        BackgroundAnimationData backgroundData = new BackgroundAnimationData();
        Dictionary<float, BackgroundAnimationData> dict = new Dictionary<float, BackgroundAnimationData>();

        backgroundData.showBannerEffect = new Dictionary<BackgroundEffect, AnimationCurve>();
        backgroundData.hideBannerEffect = new Dictionary<BackgroundEffect, AnimationCurve>();
        backgroundData.colorEffect = new Dictionary<BackgroundEffect, AnimationCurve>();

        backgroundData.showBannerEffect.Add(BackgroundEffect.FadeTexture, fadeCurve);
        backgroundData.hideBannerEffect.Add(BackgroundEffect.FadeTexture, fadeCurve);
        backgroundData.colorEffect.Add(BackgroundEffect.FadeColor, fadeCurve);

        dict.Add(1f, backgroundData);
        defaultBackgroundAnimationData.Add(BubbleSystem.Emotion.Neutral, dict);
        defaultBackgroundAnimationData.Add(BubbleSystem.Emotion.Happiness, dict);
        defaultBackgroundAnimationData.Add(BubbleSystem.Emotion.Sadness, dict);
        defaultBackgroundAnimationData.Add(BubbleSystem.Emotion.Anger, dict);
        defaultBackgroundAnimationData.Add(BubbleSystem.Emotion.Fear, dict);
        defaultBackgroundAnimationData.Add(BubbleSystem.Emotion.Disgust, dict);
        defaultBackgroundAnimationData.Add(BubbleSystem.Emotion.Surprise, dict);
    }

    private void SetBackgroundAnimation()
    {
        TextureData defaultBackgroundData;
        Dictionary<float, Dictionary<Reason, TextureData>> dict = new Dictionary<float, Dictionary<Reason, TextureData>>();
        Dictionary<Reason, TextureData> neutralDict = new Dictionary<Reason, TextureData>();
        Dictionary<Reason, TextureData> happinessDict = new Dictionary<Reason, TextureData>();
        Dictionary<Reason, TextureData> sadnessDict = new Dictionary<Reason, TextureData>();
        Dictionary<Reason, TextureData> angerDict = new Dictionary<Reason, TextureData>();
        Dictionary<Reason, TextureData> fearDict = new Dictionary<Reason, TextureData>();
        Dictionary<Reason, TextureData> disgustDict = new Dictionary<Reason, TextureData>();
        Dictionary<Reason, TextureData> surpriseDict = new Dictionary<Reason, TextureData>();

        
        defaultBackgroundData.texture = (Texture2D)Resources.Load("Backgrounds/Images/joaoBackground");
        defaultBackgroundData.color = defaultColor;
        neutralDict.Add(Reason.None, defaultBackgroundData);
        defaultBackgroundData.color = happinessColor;
        happinessDict.Add(Reason.None, defaultBackgroundData);
        defaultBackgroundData.color = sadnessColor;
        sadnessDict.Add(Reason.None, defaultBackgroundData);
        defaultBackgroundData.color = angerColor;
        angerDict.Add(Reason.None, defaultBackgroundData);
        defaultBackgroundData.color = fearColor;
        fearDict.Add(Reason.None, defaultBackgroundData);
        defaultBackgroundData.color = disgustColor;
        disgustDict.Add(Reason.None, defaultBackgroundData);
        defaultBackgroundData.color = surpriseColor;
        surpriseDict.Add(Reason.None, defaultBackgroundData);

        defaultBackgroundData.texture = (Texture2D)Resources.Load("Backgrounds/Images/graph");
        defaultBackgroundData.color = defaultColor;
        neutralDict.Add(Reason.Grades, defaultBackgroundData);
        defaultBackgroundData.color = happinessColor;
        happinessDict.Add(Reason.Grades, defaultBackgroundData);
        defaultBackgroundData.color = sadnessColor;
        sadnessDict.Add(Reason.Grades, defaultBackgroundData);
        defaultBackgroundData.color = angerColor;
        angerDict.Add(Reason.Grades, defaultBackgroundData);
        defaultBackgroundData.color = fearColor;
        fearDict.Add(Reason.Grades, defaultBackgroundData);
        defaultBackgroundData.color = disgustColor;
        disgustDict.Add(Reason.Grades, defaultBackgroundData);
        defaultBackgroundData.color = surpriseColor;
        surpriseDict.Add(Reason.Grades, defaultBackgroundData);

        dict.Add(1f, neutralDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Neutral, dict);
        dict = new Dictionary<float, Dictionary<Reason, TextureData>>();
        dict.Add(1f, happinessDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Happiness, dict);
        dict = new Dictionary<float, Dictionary<Reason, TextureData>>();
        dict.Add(1f, sadnessDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Sadness, dict);
        dict = new Dictionary<float, Dictionary<Reason, TextureData>>();
        dict.Add(1f, angerDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Anger, dict);
        dict = new Dictionary<float, Dictionary<Reason, TextureData>>();
        dict.Add(1f, fearDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Fear, dict);
        dict = new Dictionary<float, Dictionary<Reason, TextureData>>();
        dict.Add(1f, disgustDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Disgust, dict);
        dict = new Dictionary<float, Dictionary<Reason, TextureData>>();
        dict.Add(1f, surpriseDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Surprise, dict);
    }
}

using BubbleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultData : Singleton<DefaultData>
{
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, TextData>> defaultTextData = new Dictionary<BubbleSystem.Emotion, Dictionary<float, TextData>>();
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, SpriteData>> defaultBalloonData = new Dictionary<BubbleSystem.Emotion, Dictionary<float, SpriteData>>();
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, BackgroundAnimationData>> defaultBackgroundAnimationData = new Dictionary<BubbleSystem.Emotion, Dictionary<float, BackgroundAnimationData>>();
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, Dictionary<Reason, Texture2D>>> defaultBackgroundDataDictionary = new Dictionary<BubbleSystem.Emotion, Dictionary<float, Dictionary<Reason, Texture2D>>>();
    private Dictionary<BubbleSystem.Emotion, Dictionary<float, Dictionary<string, List<PositionData>>>> defaultPositions = new Dictionary<BubbleSystem.Emotion, Dictionary<float, Dictionary<string, List<PositionData>>>>();

    private Dictionary<string, AnimationCurve> curves = new Dictionary<string, AnimationCurve>();
    private Dictionary<BubbleSystem.Emotion, Color32> colors = new Dictionary<BubbleSystem.Emotion, Color32>();
    private Dictionary<BubbleSystem.Emotion, float> sizeRatios = new Dictionary<BubbleSystem.Emotion, float>();

    public float duration = 5.0f;
    public bool mixColors = true;
    public bool forceTextUpdate = true;
    public bool blendBalloonAnimation = false;
    private Color32 blushColor = Color.red;

    static System.Random rnd = new System.Random();

    public struct PositionData
    {
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Quaternion localRotation;
    }

    private DefaultData() { }

    private void Awake()
    {
        SetEmotionColors();
        SetAnimationCurves();
        SetSizeRatios();
        SetTextData();
        SetBallon();
        SetBackground();
        SetBackgroundAnimation();
        SetBalloonPositions();
    }

    public TextData GetDefaultTextData(BubbleSystem.Emotion emotion, float intensity)
    {
        Dictionary<float, TextData> dict = defaultTextData[emotion];
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

    public Texture2D GetDefaultBackgroundDataDictionary(BubbleSystem.Emotion emotion, float intensity, Reason reason)
    {
        Dictionary<float, Dictionary<Reason, Texture2D>> dict = defaultBackgroundDataDictionary[emotion];
        Dictionary<Reason, Texture2D> intensityDict = dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
        return intensityDict.Where(key => reason.Equals(key.Key)).FirstOrDefault().Value;
    }

    public PositionData GetDefaultPositions(BubbleSystem.Emotion emotion, float intensity, string beak)
    {
        Dictionary<float, Dictionary<string, List<PositionData>>> dict = defaultPositions[emotion];
        Dictionary<string, List<PositionData>> intensityDict = dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
        List<PositionData> positionList = intensityDict.Where(key => beak.Equals(key.Key)).FirstOrDefault().Value;
        int randomNumber = rnd.Next(positionList.Count);
        return positionList[randomNumber];
    }

    public void SetTextEffects(BubbleSystem.Emotion emotion, float intensity, Dictionary<Effect, AnimationCurve> showEffects, Dictionary<Effect, AnimationCurve> hideEffects)
    {
        Dictionary<float, TextData> dict = defaultTextData[emotion];
        TextData textData = dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
        if (showEffects != null)
            textData.showEffect = showEffects;
        if (hideEffects != null)
            textData.hideEffect = hideEffects;

        BubbleSystemUtility.AddToDictionary(ref defaultTextData, emotion, intensity, textData);
    }

    public AnimationCurve GetCurve(string name)
    {
        return curves[name];
    }

    public Color32 GetColor(BubbleSystem.Emotion emotion)
    {
        return colors[emotion];
    }

    public void SetBlushColor(Color color)
    {
        blushColor = color;
    }

    public Color GetBlushColor()
    {
        return blushColor;
    }

    public void SetColor(BubbleSystem.Emotion emotion, Color color)
    {
        colors[emotion] = color;
    }

    public void AddCurve(string name, AnimationCurve curve)
    {
        BubbleSystemUtility.AddToDictionary(ref curves, name, curve);
    }

    private void SetEmotionColors()
    {
        Color32 defaultColor = Color.white;
        Color32 neutralColor = new Color32(0x8C, 0xDB, 0xA1, 0xFF);
        Color32 happinessColor = new Color32(0xF0, 0xE6, 0x4D, 0xFF);
        Color32 sadnessColor = new Color32(0x1D, 0x33, 0xCE, 0xFF);
        Color32 angerColor = new Color32(0xFF, 0x00, 0x00, 0xFF);
        Color32 fearColor = new Color32(0xAE, 0x52, 0xEC, 0xFF);
        Color32 disgustColor = new Color32(0xC5, 0xD1, 0x37, 0xFF);
        Color32 surpriseColor = new Color32(0xFF, 0xC3, 0x58, 0xFF);

        colors.Add(BubbleSystem.Emotion.Default, defaultColor);
        colors.Add(BubbleSystem.Emotion.Neutral, neutralColor);
        colors.Add(BubbleSystem.Emotion.Happiness, happinessColor);
        colors.Add(BubbleSystem.Emotion.Sadness, sadnessColor);
        colors.Add(BubbleSystem.Emotion.Anger, angerColor);
        colors.Add(BubbleSystem.Emotion.Fear, fearColor);
        colors.Add(BubbleSystem.Emotion.Disgust, disgustColor);
        colors.Add(BubbleSystem.Emotion.Surprise, surpriseColor);
    }

    private void SetAnimationCurves()
    {
        AnimationCurve bellCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0));
        AnimationCurve linearCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        AnimationCurve flashCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f), new Keyframe(2f, 0f), new Keyframe(3f, 0f), new Keyframe(4f, 1f), new Keyframe(5f, 1f), new Keyframe(6f, 0f));
        AnimationCurve lowerBellCurve = new AnimationCurve(new Keyframe(0f, -0.25f), new Keyframe(1f, 0.25f), new Keyframe(2f, -0.25f));
        AnimationCurve palpitationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f), new Keyframe(1.5f, 1f), new Keyframe(2f, 0f), new Keyframe(3f, 0f));

        curves.Add("bellCurve", bellCurve);
        curves.Add("linearCurve", linearCurve);
        curves.Add("flashCurve", flashCurve);
        curves.Add("lowerBellCurve", lowerBellCurve);
        curves.Add("palpitationsCurve", palpitationCurve);
    }

    private void SetSizeRatios()
    {
        float happinessSizeRatio = 1f;
        float sadnessSizeRatio = 0.7f;
        float angerSizeRatio = 1.5f;
        float fearSizeRatio = 0.8f;
        float disgustSizeRatio = 0.8f;
        float surpriseSizeRatio = 1.2f;

        sizeRatios.Add(BubbleSystem.Emotion.Happiness, happinessSizeRatio);
        sizeRatios.Add(BubbleSystem.Emotion.Sadness, sadnessSizeRatio);
        sizeRatios.Add(BubbleSystem.Emotion.Anger, angerSizeRatio);
        sizeRatios.Add(BubbleSystem.Emotion.Fear, fearSizeRatio);
        sizeRatios.Add(BubbleSystem.Emotion.Disgust, disgustSizeRatio);
        sizeRatios.Add(BubbleSystem.Emotion.Surprise, surpriseSizeRatio);
    }


    private void SetBalloonPositions()
    {
        Dictionary<string, List<PositionData>> defPositions = new Dictionary<string, List<PositionData>>();
        Dictionary<string, List<PositionData>> neutralPositions = new Dictionary<string, List<PositionData>>();
        Dictionary<string, List<PositionData>> happinessPositions = new Dictionary<string, List<PositionData>>();
        Dictionary<string, List<PositionData>> sadnessPositions = new Dictionary<string, List<PositionData>>();
        Dictionary<string, List<PositionData>> angerPositions = new Dictionary<string, List<PositionData>>();
        Dictionary<string, List<PositionData>> fearPositions = new Dictionary<string, List<PositionData>>();
        Dictionary<string, List<PositionData>> disgustPositions = new Dictionary<string, List<PositionData>>();
        Dictionary<string, List<PositionData>> surprisePositions = new Dictionary<string, List<PositionData>>();

        Dictionary<float, Dictionary<string, List<PositionData>>> dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();

        List<PositionData> rectList = new List<PositionData>();
        PositionData rect = new PositionData();

        //DEFAULT

        rect.anchorMin = new Vector2(0.6f, 0.91f);
        rect.anchorMax = new Vector2(0.8f, 1.35f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);
        defPositions.Add("Peak_top_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, 0.91f);
        rect.anchorMax = new Vector2(0.4f, 1.35f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);
        defPositions.Add("Peak_top_left", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, -0.36f);
        rect.anchorMax = new Vector2(0.8f, 0.12f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        defPositions.Add("Peak_bot_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, -0.36f);
        rect.anchorMax = new Vector2(0.4f, 0.12f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);
        defPositions.Add("Peak_bot_left", rectList);

        dict.Add(1f, defPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Default, dict);


        //NEUTRAL

        dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(-0.5f, -0.2f);
        rect.anchorMax = new Vector2(1.5f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.4f, -0.2f);
        rect.anchorMax = new Vector2(1.6f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.6f, -0.2f);
        rect.anchorMax = new Vector2(1.4f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        neutralPositions.Add("balloon", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, 0.91f);
        rect.anchorMax = new Vector2(0.8f, 1.35f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.55f, 0.91f);
        rect.anchorMax = new Vector2(0.75f, 1.35f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);

        neutralPositions.Add("Peak_top_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, 0.91f);
        rect.anchorMax = new Vector2(0.4f, 1.35f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.25f, 0.91f);
        rect.anchorMax = new Vector2(0.45f, 1.35f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);

        neutralPositions.Add("Peak_top_left", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, -0.36f);
        rect.anchorMax = new Vector2(0.8f, 0.12f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.75f, -0.36f);
        rect.anchorMax = new Vector2(0.95f, 0.12f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);

        neutralPositions.Add("Peak_bot_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, -0.36f);
        rect.anchorMax = new Vector2(0.4f, 0.12f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.25f, -0.36f);
        rect.anchorMax = new Vector2(0.35f, 0.12f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);

        neutralPositions.Add("Peak_bot_left", rectList);

        dict.Add(1f, neutralPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Neutral, dict);


        //HAPPINESS
        dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(-0.5f, -0.2f);
        rect.anchorMax = new Vector2(1.5f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.4f, -0.2f);
        rect.anchorMax = new Vector2(1.6f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.6f, -0.2f);
        rect.anchorMax = new Vector2(1.4f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        happinessPositions.Add("balloon", rectList);

        rectList = new List<PositionData>();


        rect.anchorMin = new Vector2(0.6f, 0.81f);
        rect.anchorMax = new Vector2(0.8f, 1.25f);
        rect.localRotation = Quaternion.Euler(0, 0, 190);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.55f, 0.81f);
        rect.anchorMax = new Vector2(0.75f, 1.25f);
        rect.localRotation = Quaternion.Euler(0, 0, 190);
        rectList.Add(rect);

        happinessPositions.Add("Peak_top_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, 0.81f);
        rect.anchorMax = new Vector2(0.4f, 1.25f);
        rect.localRotation = Quaternion.Euler(0, 180, 190);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.25f, 0.81f);
        rect.anchorMax = new Vector2(0.45f, 1.25f);
        rect.localRotation = Quaternion.Euler(0, 180, 190);
        rectList.Add(rect);

        happinessPositions.Add("Peak_top_left", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, -0.26f);
        rect.anchorMax = new Vector2(0.8f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 180, 10);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.55f, -0.26f);
        rect.anchorMax = new Vector2(0.75f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 180, 10);
        rectList.Add(rect);

        happinessPositions.Add("Peak_bot_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, -0.26f);
        rect.anchorMax = new Vector2(0.4f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 0, 10);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.25f, -0.26f);
        rect.anchorMax = new Vector2(0.45f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 0, 10);
        rectList.Add(rect);

        happinessPositions.Add("Peak_bot_left", rectList);

        dict.Add(1f, happinessPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Happiness, dict);


        //SADNESS
        dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();

        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(-0.5f, -0.2f);
        rect.anchorMax = new Vector2(1.5f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.4f, -0.2f);
        rect.anchorMax = new Vector2(1.6f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.6f, -0.2f);
        rect.anchorMax = new Vector2(1.4f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        sadnessPositions.Add("balloon", rectList);

        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, 0.83f);
        rect.anchorMax = new Vector2(0.8f, 1.27f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.55f, 0.83f);
        rect.anchorMax = new Vector2(0.75f, 1.27f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);

        sadnessPositions.Add("Peak_top_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, 0.83f);
        rect.anchorMax = new Vector2(0.4f, 1.27f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.25f, 0.83f);
        rect.anchorMax = new Vector2(0.45f, 1.27f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);

        sadnessPositions.Add("Peak_top_left", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, -0.26f);
        rect.anchorMax = new Vector2(0.8f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.55f, -0.26f);
        rect.anchorMax = new Vector2(0.75f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);

        sadnessPositions.Add("Peak_bot_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, -0.26f);
        rect.anchorMax = new Vector2(0.4f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.25f, -0.26f);
        rect.anchorMax = new Vector2(0.45f, 0.18f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        sadnessPositions.Add("Peak_bot_left", rectList);

        dict.Add(1f, sadnessPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Sadness, dict);


        //ANGER
        dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(-0.5f, -0.2f);
        rect.anchorMax = new Vector2(1.5f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.4f, -0.2f);
        rect.anchorMax = new Vector2(1.6f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.6f, -0.2f);
        rect.anchorMax = new Vector2(1.4f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        angerPositions.Add("balloon", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, 0.6f);
        rect.anchorMax = new Vector2(0.8f, 1.04f);
        rect.localRotation = Quaternion.Euler(0, 180, 170);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.55f, 0.6f);
        rect.anchorMax = new Vector2(0.75f, 1.04f);
        rect.localRotation = Quaternion.Euler(0, 180, 170);
        rectList.Add(rect);
        angerPositions.Add("Peak_top_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, 0.57f);
        rect.anchorMax = new Vector2(0.4f, 1.03f);
        rect.localRotation = Quaternion.Euler(0, 0, 170);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.27f, 0.57f);
        rect.anchorMax = new Vector2(0.47f, 1.03f);
        rect.localRotation = Quaternion.Euler(0, 0, 170);
        rectList.Add(rect);
        angerPositions.Add("Peak_top_left", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, -0.04f);
        rect.anchorMax = new Vector2(0.8f, 0.4f);
        rect.localRotation = Quaternion.Euler(0, 0, -10);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.5f, -0.04f);
        rect.anchorMax = new Vector2(0.7f, 0.4f);
        rect.localRotation = Quaternion.Euler(0, 0, -10);
        rectList.Add(rect);
        angerPositions.Add("Peak_bot_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, -0.09f);
        rect.anchorMax = new Vector2(0.4f, 0.35f);
        rect.localRotation = Quaternion.Euler(0, 180, -10);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.3f, -0.09f);
        rect.anchorMax = new Vector2(0.5f, 0.35f);
        rect.localRotation = Quaternion.Euler(0, 180, -10);
        rectList.Add(rect);
        angerPositions.Add("Peak_bot_left", rectList);

        dict.Add(1f, angerPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Anger, dict);


        //FEAR
        dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(-0.5f, -0.2f);
        rect.anchorMax = new Vector2(1.5f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.4f, -0.2f);
        rect.anchorMax = new Vector2(1.6f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.6f, -0.2f);
        rect.anchorMax = new Vector2(1.4f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        fearPositions.Add("balloon", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.65f, 0.73f);
        rect.anchorMax = new Vector2(0.8f, 1.17f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.6f, 0.73f);
        rect.anchorMax = new Vector2(0.75f, 1.17f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);

        fearPositions.Add("Peak_top_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.25f, 0.73f);
        rect.anchorMax = new Vector2(0.4f, 1.17f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.3f, 0.73f);
        rect.anchorMax = new Vector2(0.45f, 1.17f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);

        fearPositions.Add("Peak_top_left", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.65f, -0.24f);
        rect.anchorMax = new Vector2(0.8f, 0.2f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.6f, -0.24f);
        rect.anchorMax = new Vector2(0.75f, 0.2f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);

        fearPositions.Add("Peak_bot_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.25f, -0.2f);
        rect.anchorMax = new Vector2(0.4f, 0.24f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.3f, -0.2f);
        rect.anchorMax = new Vector2(0.45f, 0.24f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        fearPositions.Add("Peak_bot_left", rectList);

        dict.Add(1f, fearPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Fear, dict);


        //DISGUST
        dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(-0.5f, -0.2f);
        rect.anchorMax = new Vector2(1.5f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.4f, -0.2f);
        rect.anchorMax = new Vector2(1.6f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.6f, -0.2f);
        rect.anchorMax = new Vector2(1.4f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        disgustPositions.Add("balloon", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.65f, 0.75f);
        rect.anchorMax = new Vector2(0.85f, 1.19f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.55f, 0.75f);
        rect.anchorMax = new Vector2(0.75f, 1.19f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);
        disgustPositions.Add("Peak_top_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.15f, 0.75f);
        rect.anchorMax = new Vector2(0.35f, 1.19f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.25f, 0.75f);
        rect.anchorMax = new Vector2(0.45f, 1.19f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);
        disgustPositions.Add("Peak_top_left", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.65f, -0.24f);
        rect.anchorMax = new Vector2(0.85f, 0.24f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.55f, -0.24f);
        rect.anchorMax = new Vector2(0.75f, 0.24f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);
        disgustPositions.Add("Peak_bot_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.15f, -0.24f);
        rect.anchorMax = new Vector2(0.35f, 0.24f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.25f, -0.24f);
        rect.anchorMax = new Vector2(0.45f, 0.24f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        disgustPositions.Add("Peak_bot_left", rectList);

        dict.Add(1f, disgustPositions);
        defaultPositions.Add(BubbleSystem.Emotion.Disgust, dict);


        //SURPRISE
        dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(-0.5f, -0.2f);
        rect.anchorMax = new Vector2(1.5f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.4f, -0.2f);
        rect.anchorMax = new Vector2(1.6f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(-0.6f, -0.2f);
        rect.anchorMax = new Vector2(1.4f, 0.5f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        surprisePositions.Add("balloon", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, 0.66f);
        rect.anchorMax = new Vector2(0.8f, 1.1f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.5f, 0.66f);
        rect.anchorMax = new Vector2(0.7f, 1.1f);
        rect.localRotation = Quaternion.Euler(0, 180, 180);
        rectList.Add(rect);
        surprisePositions.Add("Peak_top_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, 0.66f);
        rect.anchorMax = new Vector2(0.4f, 1.1f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.3f, 0.66f);
        rect.anchorMax = new Vector2(0.5f, 1.1f);
        rect.localRotation = Quaternion.Euler(0, 0, 180);
        rectList.Add(rect);
        surprisePositions.Add("Peak_top_left", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.6f, -0.11f);
        rect.anchorMax = new Vector2(0.8f, 0.33f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.5f, -0.11f);
        rect.anchorMax = new Vector2(0.7f, 0.33f);
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        rectList.Add(rect);
        surprisePositions.Add("Peak_bot_right", rectList);


        rectList = new List<PositionData>();

        rect.anchorMin = new Vector2(0.2f, -0.11f);
        rect.anchorMax = new Vector2(0.4f, 0.33f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);

        rect.anchorMin = new Vector2(0.3f, -0.11f);
        rect.anchorMax = new Vector2(0.5f, 0.33f);
        rect.localRotation = Quaternion.Euler(0, 180, 0);
        rectList.Add(rect);
        surprisePositions.Add("Peak_bot_left", rectList);

        dict.Add(1f, surprisePositions);
        defaultPositions.Add(BubbleSystem.Emotion.Surprise, dict);
    }

    private void SetTextData()
    {
        TMPro.TMP_FontAsset neutralFont = (TMPro.TMP_FontAsset)Resources.Load("Text/TextMesh_Fonts/arial");
        float initialSize = 40.0f;

        TextData text = new TextData();
        Dictionary<float, TextData> dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.size = initialSize;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Default, dict);

        defaultTextData[BubbleSystem.Emotion.Default][1f].showEffect.Add(Effect.None, null);
        defaultTextData[BubbleSystem.Emotion.Default][1f].hideEffect.Add(Effect.None, null);


        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.size = initialSize;
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Neutral, dict);

        defaultTextData[BubbleSystem.Emotion.Neutral][1f].showEffect.Add(Effect.Appear, curves["linearCurve"]);
        defaultTextData[BubbleSystem.Emotion.Neutral][1f].hideEffect.Add(Effect.None, null);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.size = initialSize * sizeRatios[BubbleSystem.Emotion.Happiness];
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Happiness, dict);

        defaultTextData[BubbleSystem.Emotion.Happiness][1f].showEffect.Add(Effect.Wave, curves["bellCurve"]);
        defaultTextData[BubbleSystem.Emotion.Happiness][1f].hideEffect.Add(Effect.Wave, curves["bellCurve"]);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.size = initialSize * sizeRatios[BubbleSystem.Emotion.Sadness];
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Sadness, dict);

        defaultTextData[BubbleSystem.Emotion.Sadness][1f].showEffect.Add(Effect.FadeIn, curves["linearCurve"]);
        defaultTextData[BubbleSystem.Emotion.Sadness][1f].hideEffect.Add(Effect.FadeOut, curves["linearCurve"]);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.size = initialSize * sizeRatios[BubbleSystem.Emotion.Anger];
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Anger, dict);

        defaultTextData[BubbleSystem.Emotion.Anger][1f].showEffect.Add(Effect.Shake, null);
        defaultTextData[BubbleSystem.Emotion.Anger][1f].hideEffect.Add(Effect.Shake, null);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.size = initialSize * sizeRatios[BubbleSystem.Emotion.Fear];
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Fear, dict);

        defaultTextData[BubbleSystem.Emotion.Fear][1f].showEffect.Add(Effect.Jitter, null);
        defaultTextData[BubbleSystem.Emotion.Fear][1f].hideEffect.Add(Effect.Jitter, null);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.size = initialSize * sizeRatios[BubbleSystem.Emotion.Disgust];
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Disgust, dict);

        defaultTextData[BubbleSystem.Emotion.Disgust][1f].showEffect.Add(Effect.Warp, curves["bellCurve"]);
        defaultTextData[BubbleSystem.Emotion.Disgust][1f].hideEffect.Add(Effect.Warp, curves["bellCurve"]);


        text = new TextData();
        dict = new Dictionary<float, TextData>();
        text.font = neutralFont;
        text.size = initialSize * sizeRatios[BubbleSystem.Emotion.Surprise];
        text.showEffect = new Dictionary<Effect, AnimationCurve>();
        text.hideEffect = new Dictionary<Effect, AnimationCurve>();
        dict.Add(1f, text);
        defaultTextData.Add(BubbleSystem.Emotion.Surprise, dict);

        defaultTextData[BubbleSystem.Emotion.Surprise][1f].showEffect.Add(Effect.SwingCharacters, null);
        defaultTextData[BubbleSystem.Emotion.Surprise][1f].hideEffect.Add(Effect.SwingCharacters, null);
    }

    private void SetBallon()
    {
        Dictionary<float, SpriteData> dict = new Dictionary<float, SpriteData>();

        var tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Neutral/neutral_balloon");
        var beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Neutral/neutral_beak");
        SpriteData spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Default, dict);


        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Neutral/neutral_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Neutral/neutral_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Neutral, dict);


        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Happiness/happiness_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Happiness/happiness_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Happiness, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Sadness/sadness_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Sadness/sadness_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Sadness, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Anger/anger_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Anger/anger_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Anger, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Fear/fear_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Fear/fear_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Fear, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Disgust/disgust_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Disgust/disgust_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
        dict.Add(1f, spriteData);
        defaultBalloonData.Add(BubbleSystem.Emotion.Disgust, dict);

        dict = new Dictionary<float, SpriteData>();
        tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Surprise/surprise_balloon");
        beak = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Surprise/surprise_beak");
        spriteData = new SpriteData();
        spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteData.beak = Sprite.Create(beak, new Rect(0.0f, 0.0f, beak.width, beak.height), new Vector2(0.5f, 0.5f), 100.0f);
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

        backgroundData.showBannerEffect.Add(BackgroundEffect.FadeTexture, curves["linearCurve"]);
        backgroundData.hideBannerEffect.Add(BackgroundEffect.FadeTexture, curves["linearCurve"]);
        backgroundData.colorEffect.Add(BackgroundEffect.FadeColor, curves["linearCurve"]);

        dict.Add(1f, backgroundData);
        defaultBackgroundAnimationData.Add(BubbleSystem.Emotion.Default, dict);
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
        Texture2D defaultBackgroundData;
        Dictionary<float, Dictionary<Reason, Texture2D>> dict = new Dictionary<float, Dictionary<Reason, Texture2D>>();
        Dictionary<Reason, Texture2D> defaultDict = new Dictionary<Reason, Texture2D>();
        Dictionary<Reason, Texture2D> neutralDict = new Dictionary<Reason, Texture2D>();
        Dictionary<Reason, Texture2D> happinessDict = new Dictionary<Reason, Texture2D>();
        Dictionary<Reason, Texture2D> sadnessDict = new Dictionary<Reason, Texture2D>();
        Dictionary<Reason, Texture2D> angerDict = new Dictionary<Reason, Texture2D>();
        Dictionary<Reason, Texture2D> fearDict = new Dictionary<Reason, Texture2D>();
        Dictionary<Reason, Texture2D> disgustDict = new Dictionary<Reason, Texture2D>();
        Dictionary<Reason, Texture2D> surpriseDict = new Dictionary<Reason, Texture2D>();


        defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/tutorBackground");
        defaultDict.Add(Reason.None, defaultBackgroundData);
        neutralDict.Add(Reason.None, defaultBackgroundData);
        happinessDict.Add(Reason.None, defaultBackgroundData);
        sadnessDict.Add(Reason.None, defaultBackgroundData);
        angerDict.Add(Reason.None, defaultBackgroundData);
        fearDict.Add(Reason.None, defaultBackgroundData);
        disgustDict.Add(Reason.None, defaultBackgroundData);
        surpriseDict.Add(Reason.None, defaultBackgroundData);

        defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/challenge");
        defaultDict.Add(Reason.Challenge, defaultBackgroundData);
        neutralDict.Add(Reason.Challenge, defaultBackgroundData);
        happinessDict.Add(Reason.Challenge, defaultBackgroundData);
        sadnessDict.Add(Reason.Challenge, defaultBackgroundData);
        angerDict.Add(Reason.Challenge, defaultBackgroundData);
        fearDict.Add(Reason.Challenge, defaultBackgroundData);
        disgustDict.Add(Reason.Challenge, defaultBackgroundData);
        surpriseDict.Add(Reason.Challenge, defaultBackgroundData);

        defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/effort");
        defaultDict.Add(Reason.Effort, defaultBackgroundData);
        neutralDict.Add(Reason.Effort, defaultBackgroundData);
        happinessDict.Add(Reason.Effort, defaultBackgroundData);
        sadnessDict.Add(Reason.Effort, defaultBackgroundData);
        angerDict.Add(Reason.Effort, defaultBackgroundData);
        fearDict.Add(Reason.Effort, defaultBackgroundData);
        disgustDict.Add(Reason.Effort, defaultBackgroundData);
        surpriseDict.Add(Reason.Effort, defaultBackgroundData);

        defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/engagement");
        defaultDict.Add(Reason.Engagement, defaultBackgroundData);
        neutralDict.Add(Reason.Engagement, defaultBackgroundData);
        happinessDict.Add(Reason.Engagement, defaultBackgroundData);
        sadnessDict.Add(Reason.Engagement, defaultBackgroundData);
        angerDict.Add(Reason.Engagement, defaultBackgroundData);
        fearDict.Add(Reason.Engagement, defaultBackgroundData);
        disgustDict.Add(Reason.Engagement, defaultBackgroundData);
        surpriseDict.Add(Reason.Engagement, defaultBackgroundData);

        defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/enjoyment");
        defaultDict.Add(Reason.Enjoyment, defaultBackgroundData);
        neutralDict.Add(Reason.Enjoyment, defaultBackgroundData);
        happinessDict.Add(Reason.Enjoyment, defaultBackgroundData);
        sadnessDict.Add(Reason.Enjoyment, defaultBackgroundData);
        angerDict.Add(Reason.Enjoyment, defaultBackgroundData);
        fearDict.Add(Reason.Enjoyment, defaultBackgroundData);
        disgustDict.Add(Reason.Enjoyment, defaultBackgroundData);
        surpriseDict.Add(Reason.Enjoyment, defaultBackgroundData);

        defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/importance");
        defaultDict.Add(Reason.Importance, defaultBackgroundData);
        neutralDict.Add(Reason.Importance, defaultBackgroundData);
        happinessDict.Add(Reason.Importance, defaultBackgroundData);
        sadnessDict.Add(Reason.Importance, defaultBackgroundData);
        angerDict.Add(Reason.Importance, defaultBackgroundData);
        fearDict.Add(Reason.Importance, defaultBackgroundData);
        disgustDict.Add(Reason.Importance, defaultBackgroundData);
        surpriseDict.Add(Reason.Importance, defaultBackgroundData);

        defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/performance");
        defaultDict.Add(Reason.Performance, defaultBackgroundData);
        neutralDict.Add(Reason.Performance, defaultBackgroundData);
        happinessDict.Add(Reason.Performance, defaultBackgroundData);
        sadnessDict.Add(Reason.Performance, defaultBackgroundData);
        angerDict.Add(Reason.Performance, defaultBackgroundData);
        fearDict.Add(Reason.Performance, defaultBackgroundData);
        disgustDict.Add(Reason.Performance, defaultBackgroundData);
        surpriseDict.Add(Reason.Performance, defaultBackgroundData);

        dict.Add(1f, neutralDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Neutral, dict);
        dict = new Dictionary<float, Dictionary<Reason, Texture2D>>();
        dict.Add(1f, defaultDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Default, dict);
        dict = new Dictionary<float, Dictionary<Reason, Texture2D>>();
        dict.Add(1f, happinessDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Happiness, dict);
        dict = new Dictionary<float, Dictionary<Reason, Texture2D>>();
        dict.Add(1f, sadnessDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Sadness, dict);
        dict = new Dictionary<float, Dictionary<Reason, Texture2D>>();
        dict.Add(1f, angerDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Anger, dict);
        dict = new Dictionary<float, Dictionary<Reason, Texture2D>>();
        dict.Add(1f, fearDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Fear, dict);
        dict = new Dictionary<float, Dictionary<Reason, Texture2D>>();
        dict.Add(1f, disgustDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Disgust, dict);
        dict = new Dictionary<float, Dictionary<Reason, Texture2D>>();
        dict.Add(1f, surpriseDict);
        defaultBackgroundDataDictionary.Add(BubbleSystem.Emotion.Surprise, dict);
    }
}
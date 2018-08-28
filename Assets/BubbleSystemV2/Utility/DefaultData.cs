using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BubbleSystem
{
    public class DefaultData : Singleton<DefaultData>
    {
        private Dictionary<string, Dictionary<float, TextData>> defaultTextData = new Dictionary<string, Dictionary<float, TextData>>();
        private Dictionary<Emotion.EmotionEnum, Dictionary<float, SpriteData>> defaultBalloonData = new Dictionary<Emotion.EmotionEnum, Dictionary<float, SpriteData>>();
        private Dictionary<Emotion.EmotionEnum, Dictionary<float, EffectsData<AbstractImageEffect.ImageEffectEnum>>> defaultBackgroundAnimationData = new Dictionary<Emotion.EmotionEnum, Dictionary<float, EffectsData<AbstractImageEffect.ImageEffectEnum>>>();
        private Dictionary<Emotion.EmotionEnum, Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>>> defaultBackgroundDataDictionary = new Dictionary<Emotion.EmotionEnum, Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>>>();
        private Dictionary<Emotion.EmotionEnum, Dictionary<float, Dictionary<string, List<PositionData>>>> defaultPositions = new Dictionary<Emotion.EmotionEnum, Dictionary<float, Dictionary<string, List<PositionData>>>>();
        private Dictionary<Emotion.EmotionEnum, PositionData> textSizes = new Dictionary<Emotion.EmotionEnum, PositionData>();

        private Dictionary<string, AnimationCurve> curves = new Dictionary<string, AnimationCurve>();
        private Dictionary<Emotion.EmotionEnum, Color32> colors = new Dictionary<Emotion.EmotionEnum, Color32>();
        public Color32 defaultColor = Color.white;

        private float balloonDuration = 5.0f;

        private float optionsDuration = -1f;
        private float backgroundDuration = 5.0f;
        public bool mixColors = true;
        public bool forceTextUpdate = true;
        public bool blendBalloonAnimation = false;
        private Color32 blushColor = Color.red;
        public Color noColor = new Color(0, 0, 0, 0);

        //Used by the blend tree
        public string Default = "Default";

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
            SetTextData();
            SetBallon();
            SetBackground();
            SetBackgroundAnimation();
            SetBalloonPositions();
            SetTextSizes();
        }

        public TextData GetDefaultTextData(string emotion, float intensity)
        {
            Dictionary<float, TextData> dict = defaultTextData[emotion];
            return dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
        }

        public SpriteData GetDefaultBalloonData(Emotion.EmotionEnum emotion, float intensity)
        {
            Dictionary<float, SpriteData> dict = defaultBalloonData[emotion];
            return dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
        }

        public EffectsData<AbstractImageEffect.ImageEffectEnum> GetDefaultBackgroundAnimationData(Emotion.EmotionEnum emotion, float intensity)
        {
            Dictionary<float, EffectsData<AbstractImageEffect.ImageEffectEnum>> dict = defaultBackgroundAnimationData[emotion];
            return dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
        }

        public Texture2D GetDefaultBackgroundDataDictionary(Emotion.EmotionEnum emotion, float intensity, Reason reason)
        {
            Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>> dict = defaultBackgroundDataDictionary[emotion];
            Dictionary<Reason.ReasonEnum, Texture2D> intensityDict = dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
            return intensityDict.Where(key => reason.Get().Equals(key.Key)).FirstOrDefault().Value;
        }

        public PositionData GetDefaultPositions(Emotion.EmotionEnum emotion, float intensity, string tail)
        {
            Dictionary<float, Dictionary<string, List<PositionData>>> dict = defaultPositions[emotion];
            Dictionary<string, List<PositionData>> intensityDict = dict.Where(key => intensity <= key.Key).OrderBy(key => key.Key).FirstOrDefault().Value;
            List<PositionData> positionList = intensityDict.Where(key => tail.Equals(key.Key)).FirstOrDefault().Value;
            int randomNumber = rnd.Next(positionList.Count);
            return positionList[randomNumber];
        }

        public void SetTextEffects(string emotion, float intensity, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve> showEffects, Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve> hideEffects)
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

        public Color32 GetColor(Emotion.EmotionEnum emotion)
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

        public void SetColor(Emotion.EmotionEnum emotion, Color color)
        {
            colors[emotion] = color;
        }

        public void AddCurve(string name, AnimationCurve curve)
        {
            BubbleSystemUtility.AddToDictionary(ref curves, name, curve);
        }

        private void SetDuration(ref float duration, float value)
        {
            if (value >= 2f)
                duration = value;
        }

        public float GetBalloonDuration()
        {
            return balloonDuration;
        }

        public void SetBalloonDuration(float duration)
        {
            SetDuration(ref balloonDuration, duration);
        }

        public float GetBackgroundDuration()
        {
            return backgroundDuration;
        }

        public void SetBackgroundDuration(float duration)
        {
            SetDuration(ref backgroundDuration, duration);
        }

        public float GetOptionsDuration()
        {
            return optionsDuration;
        }

        public void SetOptionsDuration(float duration)
        {
            SetDuration(ref optionsDuration, duration);
            if (Math.Abs(duration - (-1)) < .001f)
                optionsDuration = duration;
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

            colors.Add(Emotion.EmotionEnum.Neutral, neutralColor);
            colors.Add(Emotion.EmotionEnum.Happiness, happinessColor);
            colors.Add(Emotion.EmotionEnum.Sadness, sadnessColor);
            colors.Add(Emotion.EmotionEnum.Anger, angerColor);
            colors.Add(Emotion.EmotionEnum.Fear, fearColor);
            colors.Add(Emotion.EmotionEnum.Disgust, disgustColor);
            colors.Add(Emotion.EmotionEnum.Surprise, surpriseColor);
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

        private void SetTextSizes()
        {
            PositionData rect = new PositionData();

            {
                rect.anchorMin = new Vector2(0.1f, 0.1f);
                rect.anchorMax = new Vector2(0.9f, 0.9f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                textSizes.Add(Emotion.EmotionEnum.Neutral, rect);
            }

            {
                rect.anchorMin = new Vector2(0.15f, 0.2f);
                rect.anchorMax = new Vector2(0.85f, 0.75f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                textSizes.Add(Emotion.EmotionEnum.Happiness, rect);
            }

            {
                rect.anchorMin = new Vector2(0.15f, 0.2f);
                rect.anchorMax = new Vector2(0.85f, 0.8f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                textSizes.Add(Emotion.EmotionEnum.Sadness, rect);
            }

            {
                rect.anchorMin = new Vector2(0.2f, 0.3f);
                rect.anchorMax = new Vector2(0.8f, 0.65f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                textSizes.Add(Emotion.EmotionEnum.Anger, rect);
            }

            {
                rect.anchorMin = new Vector2(0.15f, 0.2f);
                rect.anchorMax = new Vector2(0.85f, 0.8f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                textSizes.Add(Emotion.EmotionEnum.Fear, rect);
            }

            {
                rect.anchorMin = new Vector2(0.15f, 0.2f);
                rect.anchorMax = new Vector2(0.85f, 0.75f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                textSizes.Add(Emotion.EmotionEnum.Disgust, rect);
            }

            {
                rect.anchorMin = new Vector2(0.2f, 0.3f);
                rect.anchorMax = new Vector2(0.8f, 0.65f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                textSizes.Add(Emotion.EmotionEnum.Surprise, rect);
            }
        }

        public PositionData GetTextSizes(Emotion.EmotionEnum emotion)
        {
            return textSizes[emotion];
        }

        private void SetBalloonPositions()
        {
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

            //NEUTRAL
            {
                dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.1f, 0.1f);
                rect.anchorMax = new Vector2(0.9f, 0.75f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                neutralPositions.Add("balloon", rectList);


                rectList = new List<PositionData>();


                rect.anchorMin = new Vector2(0.4f, 0.91f);
                rect.anchorMax = new Vector2(0.6f, 1.35f);
                rect.localRotation = Quaternion.Euler(0, 180, 180);
                rectList.Add(rect);

                neutralPositions.Add("Tail_top_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.35f, 0.91f);
                rect.anchorMax = new Vector2(0.55f, 1.35f);
                rect.localRotation = Quaternion.Euler(0, 0, 180);
                rectList.Add(rect);

                neutralPositions.Add("Tail_top_left", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.4f, -0.34f);
                rect.anchorMax = new Vector2(0.6f, 0.1f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);

                neutralPositions.Add("Tail_bot_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.35f, -0.34f);
                rect.anchorMax = new Vector2(0.55f, 0.1f);
                rect.localRotation = Quaternion.Euler(0, 180, 0);
                rectList.Add(rect);

                neutralPositions.Add("Tail_bot_left", rectList);

                dict.Add(1f, neutralPositions);
                defaultPositions.Add(Emotion.EmotionEnum.Neutral, dict);
            }


            //HAPPINESS
            {
                dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.1f, 0.1f);
                rect.anchorMax = new Vector2(0.9f, 0.75f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                happinessPositions.Add("balloon", rectList);

                rectList = new List<PositionData>();


                rect.anchorMin = new Vector2(0.45f, 0.81f);
                rect.anchorMax = new Vector2(0.65f, 1.25f);
                rect.localRotation = Quaternion.Euler(0, 0, 190);
                rectList.Add(rect);
                happinessPositions.Add("Tail_top_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.2f, 0.81f);
                rect.anchorMax = new Vector2(0.4f, 1.25f);
                rect.localRotation = Quaternion.Euler(0, 180, 190);
                rectList.Add(rect);
                happinessPositions.Add("Tail_top_left", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.45f, -0.26f);
                rect.anchorMax = new Vector2(0.65f, 0.18f);
                rect.localRotation = Quaternion.Euler(0, 180, 10);
                rectList.Add(rect);
                happinessPositions.Add("Tail_bot_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.35f, -0.26f);
                rect.anchorMax = new Vector2(0.55f, 0.18f);
                rect.localRotation = Quaternion.Euler(0, 0, 10);
                rectList.Add(rect);
                happinessPositions.Add("Tail_bot_left", rectList);

                dict.Add(1f, happinessPositions);
                defaultPositions.Add(Emotion.EmotionEnum.Happiness, dict);
            }

            //SADNESS
            {
                dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();

                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.1f, 0.1f);
                rect.anchorMax = new Vector2(0.9f, 0.75f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                sadnessPositions.Add("balloon", rectList);

                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.45f, 0.81f);
                rect.anchorMax = new Vector2(0.55f, 1.25f);
                rect.localRotation = Quaternion.Euler(0, 0, 180);
                rectList.Add(rect);

                sadnessPositions.Add("Tail_top_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.35f, 0.81f);
                rect.anchorMax = new Vector2(0.55f, 1.25f);
                rect.localRotation = Quaternion.Euler(0, 180, 180);
                rectList.Add(rect);

                sadnessPositions.Add("Tail_top_left", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.45f, -0.26f);
                rect.anchorMax = new Vector2(0.65f, 0.18f);
                rect.localRotation = Quaternion.Euler(0, 180, 0);
                rectList.Add(rect);
                sadnessPositions.Add("Tail_bot_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.35f, -0.26f);
                rect.anchorMax = new Vector2(0.55f, 0.18f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                sadnessPositions.Add("Tail_bot_left", rectList);

                dict.Add(1f, sadnessPositions);
                defaultPositions.Add(Emotion.EmotionEnum.Sadness, dict);
            }

            //ANGER
            {
                dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.05f, 0.0f);
                rect.anchorMax = new Vector2(0.95f, 0.9f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                angerPositions.Add("balloon", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.47f, 0.7f);
                rect.anchorMax = new Vector2(0.67f, 1.14f);
                rect.localRotation = Quaternion.Euler(0, 180, 170);
                rectList.Add(rect);
                angerPositions.Add("Tail_top_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.33f, 0.7f);
                rect.anchorMax = new Vector2(0.53f, 1.14f);
                rect.localRotation = Quaternion.Euler(0, 0, 170);
                rectList.Add(rect);
                angerPositions.Add("Tail_top_left", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.42f, -0.14f);
                rect.anchorMax = new Vector2(0.62f, 0.3f);
                rect.localRotation = Quaternion.Euler(0, 0, -10);
                rectList.Add(rect);
                angerPositions.Add("Tail_bot_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.36f, -0.12f);
                rect.anchorMax = new Vector2(0.56f, 0.32f);
                rect.localRotation = Quaternion.Euler(0, 180, -10);
                rectList.Add(rect);
                angerPositions.Add("Tail_bot_left", rectList);

                dict.Add(1f, angerPositions);
                defaultPositions.Add(Emotion.EmotionEnum.Anger, dict);
            }

            //FEAR
            {
                dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.1f, 0.1f);
                rect.anchorMax = new Vector2(0.9f, 0.75f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                fearPositions.Add("balloon", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.55f, 0.73f);
                rect.anchorMax = new Vector2(0.75f, 1.17f);
                rect.localRotation = Quaternion.Euler(0, 0, 180);
                rectList.Add(rect);

                fearPositions.Add("Tail_top_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.25f, 0.73f);
                rect.anchorMax = new Vector2(0.45f, 1.17f);
                rect.localRotation = Quaternion.Euler(0, 180, 180);
                rectList.Add(rect);

                fearPositions.Add("Tail_top_left", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.55f, -0.24f);
                rect.anchorMax = new Vector2(0.75f, 0.2f);
                rect.localRotation = Quaternion.Euler(0, 180, 0);
                rectList.Add(rect);

                fearPositions.Add("Tail_bot_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.25f, -0.2f);
                rect.anchorMax = new Vector2(0.45f, 0.24f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);

                fearPositions.Add("Tail_bot_left", rectList);

                dict.Add(1f, fearPositions);
                defaultPositions.Add(Emotion.EmotionEnum.Fear, dict);
            }

            //DISGUST
            {
                dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.1f, 0.1f);
                rect.anchorMax = new Vector2(0.9f, 0.75f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                disgustPositions.Add("balloon", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.5f, 0.75f);
                rect.anchorMax = new Vector2(0.7f, 1.19f);
                rect.localRotation = Quaternion.Euler(0, 0, 180);
                rectList.Add(rect);
                disgustPositions.Add("Tail_top_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.3f, 0.75f);
                rect.anchorMax = new Vector2(0.5f, 1.19f);
                rect.localRotation = Quaternion.Euler(0, 180, 180);
                rectList.Add(rect);
                disgustPositions.Add("Tail_top_left", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.5f, -0.2f);
                rect.anchorMax = new Vector2(0.7f, 0.24f);
                rect.localRotation = Quaternion.Euler(0, 180, 0);
                rectList.Add(rect);
                disgustPositions.Add("Tail_bot_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.3f, -0.22f);
                rect.anchorMax = new Vector2(0.5f, 0.24f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                disgustPositions.Add("Tail_bot_left", rectList);

                dict.Add(1f, disgustPositions);
                defaultPositions.Add(Emotion.EmotionEnum.Disgust, dict);
            }

            //SURPRISE
            {
                dict = new Dictionary<float, Dictionary<string, List<PositionData>>>();
                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.1f, 0.1f);
                rect.anchorMax = new Vector2(0.9f, 0.75f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                surprisePositions.Add("balloon", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.43f, 0.64f);
                rect.anchorMax = new Vector2(0.63f, 1.08f);
                rect.localRotation = Quaternion.Euler(0, 180, 180);
                rectList.Add(rect);
                surprisePositions.Add("Tail_top_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.3f, 0.64f);
                rect.anchorMax = new Vector2(0.5f, 1.08f);
                rect.localRotation = Quaternion.Euler(0, 0, 180);
                rectList.Add(rect);
                surprisePositions.Add("Tail_top_left", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.47f, -0.14f);
                rect.anchorMax = new Vector2(0.67f, 0.3f);
                rect.localRotation = Quaternion.Euler(0, 0, 0);
                rectList.Add(rect);
                surprisePositions.Add("Tail_bot_right", rectList);


                rectList = new List<PositionData>();

                rect.anchorMin = new Vector2(0.35f, -0.14f);
                rect.anchorMax = new Vector2(0.55f, 0.3f);
                rect.localRotation = Quaternion.Euler(0, 180, 0);
                rectList.Add(rect);
                surprisePositions.Add("Tail_bot_left", rectList);

                dict.Add(1f, surprisePositions);
                defaultPositions.Add(Emotion.EmotionEnum.Surprise, dict);
            }
        }

        private void SetTextData()
        {
            TMPro.TMP_FontAsset neutralFont = (TMPro.TMP_FontAsset)Resources.Load("Text/TextMesh_Fonts/arial");
            float initialSize = 40.0f;
            TextData text = new TextData();
            Dictionary<float, TextData> dict = new Dictionary<float, TextData>();

            {
                text.font = neutralFont;
                text.size = initialSize;
                text.showEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                text.hideEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                dict.Add(1f, text);
                defaultTextData.Add("User", dict);

                defaultTextData["User"][1f].showEffect.Add(AbstractTextEffect.TextEffectEnum.None, null);
                defaultTextData["User"][1f].hideEffect.Add(AbstractTextEffect.TextEffectEnum.None, null);
            }

            {
                text = new TextData();
                dict = new Dictionary<float, TextData>();
                text.font = neutralFont;
                text.size = initialSize;
                text.showEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                text.hideEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                dict.Add(1f, text);
                defaultTextData.Add(Emotion.EmotionEnum.Neutral.ToString(), dict);

                defaultTextData[Emotion.EmotionEnum.Neutral.ToString()][1f].showEffect.Add(AbstractTextEffect.TextEffectEnum.Appear, curves["linearCurve"]);
                defaultTextData[Emotion.EmotionEnum.Neutral.ToString()][1f].hideEffect.Add(AbstractTextEffect.TextEffectEnum.None, null);
            }

            {
                text = new TextData();
                dict = new Dictionary<float, TextData>();
                text.font = neutralFont;
                text.size = initialSize;
                text.showEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                text.hideEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                dict.Add(1f, text);
                defaultTextData.Add(Emotion.EmotionEnum.Happiness.ToString(), dict);

                defaultTextData[Emotion.EmotionEnum.Happiness.ToString()][1f].showEffect.Add(AbstractTextEffect.TextEffectEnum.Wave, curves["bellCurve"]);
                defaultTextData[Emotion.EmotionEnum.Happiness.ToString()][1f].hideEffect.Add(AbstractTextEffect.TextEffectEnum.Wave, curves["bellCurve"]);
            }

            {
                text = new TextData();
                dict = new Dictionary<float, TextData>();
                text.font = neutralFont;
                text.size = initialSize;
                text.showEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                text.hideEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                dict.Add(1f, text);
                defaultTextData.Add(Emotion.EmotionEnum.Sadness.ToString(), dict);

                defaultTextData[Emotion.EmotionEnum.Sadness.ToString()][1f].showEffect.Add(AbstractTextEffect.TextEffectEnum.FadeIn, curves["linearCurve"]);
                defaultTextData[Emotion.EmotionEnum.Sadness.ToString()][1f].hideEffect.Add(AbstractTextEffect.TextEffectEnum.FadeOut, curves["linearCurve"]);
            }

            {
                text = new TextData();
                dict = new Dictionary<float, TextData>();
                text.font = neutralFont;
                text.size = initialSize;
                text.showEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                text.hideEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                dict.Add(1f, text);
                defaultTextData.Add(Emotion.EmotionEnum.Anger.ToString(), dict);

                defaultTextData[Emotion.EmotionEnum.Anger.ToString()][1f].showEffect.Add(AbstractTextEffect.TextEffectEnum.Shake, null);
                defaultTextData[Emotion.EmotionEnum.Anger.ToString()][1f].hideEffect.Add(AbstractTextEffect.TextEffectEnum.Shake, null);
            }

            {
                text = new TextData();
                dict = new Dictionary<float, TextData>();
                text.font = neutralFont;
                text.size = initialSize;
                text.showEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                text.hideEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                dict.Add(1f, text);
                defaultTextData.Add(Emotion.EmotionEnum.Fear.ToString(), dict);

                defaultTextData[Emotion.EmotionEnum.Fear.ToString()][1f].showEffect.Add(AbstractTextEffect.TextEffectEnum.Jitter, null);
                defaultTextData[Emotion.EmotionEnum.Fear.ToString()][1f].hideEffect.Add(AbstractTextEffect.TextEffectEnum.Jitter, null);
            }

            {
                text = new TextData();
                dict = new Dictionary<float, TextData>();
                text.font = neutralFont;
                text.size = initialSize;
                text.showEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                text.hideEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                dict.Add(1f, text);
                defaultTextData.Add(Emotion.EmotionEnum.Disgust.ToString(), dict);

                defaultTextData[Emotion.EmotionEnum.Disgust.ToString()][1f].showEffect.Add(AbstractTextEffect.TextEffectEnum.Warp, curves["bellCurve"]);
                defaultTextData[Emotion.EmotionEnum.Disgust.ToString()][1f].hideEffect.Add(AbstractTextEffect.TextEffectEnum.Warp, curves["bellCurve"]);
            }

            {
                text = new TextData();
                dict = new Dictionary<float, TextData>();
                text.font = neutralFont;
                text.size = initialSize;
                text.showEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                text.hideEffect = new Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve>();
                dict.Add(1f, text);
                defaultTextData.Add(Emotion.EmotionEnum.Surprise.ToString(), dict);

                defaultTextData[Emotion.EmotionEnum.Surprise.ToString()][1f].showEffect.Add(AbstractTextEffect.TextEffectEnum.SwingCharacters, null);
                defaultTextData[Emotion.EmotionEnum.Surprise.ToString()][1f].hideEffect.Add(AbstractTextEffect.TextEffectEnum.SwingCharacters, null);
            }
        }

        private void SetBallon()
        {
            Dictionary<float, SpriteData> dict = new Dictionary<float, SpriteData>();
            SpriteData spriteData;
            Texture2D tex, tail;

            {
                dict = new Dictionary<float, SpriteData>();
                tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Neutral/neutral_balloon");
                tail = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Neutral/neutral_tail");
                spriteData = new SpriteData();
                spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                spriteData.tail = Sprite.Create(tail, new Rect(0.0f, 0.0f, tail.width, tail.height), new Vector2(0.5f, 0.5f), 100.0f);
                dict.Add(1f, spriteData);
                defaultBalloonData.Add(Emotion.EmotionEnum.Neutral, dict);
            }

            {
                dict = new Dictionary<float, SpriteData>();
                tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Happiness/happiness_balloon");
                tail = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Happiness/happiness_tail");
                spriteData = new SpriteData();
                spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                spriteData.tail = Sprite.Create(tail, new Rect(0.0f, 0.0f, tail.width, tail.height), new Vector2(0.5f, 0.5f), 100.0f);
                dict.Add(1f, spriteData);
                defaultBalloonData.Add(Emotion.EmotionEnum.Happiness, dict);
            }

            {
                dict = new Dictionary<float, SpriteData>();
                tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Sadness/sadness_balloon");
                tail = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Sadness/sadness_tail");
                spriteData = new SpriteData();
                spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                spriteData.tail = Sprite.Create(tail, new Rect(0.0f, 0.0f, tail.width, tail.height), new Vector2(0.5f, 0.5f), 100.0f);
                dict.Add(1f, spriteData);
                defaultBalloonData.Add(Emotion.EmotionEnum.Sadness, dict);
            }

            {
                dict = new Dictionary<float, SpriteData>();
                tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Anger/anger_balloon");
                tail = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Anger/anger_tail");
                spriteData = new SpriteData();
                spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                spriteData.tail = Sprite.Create(tail, new Rect(0.0f, 0.0f, tail.width, tail.height), new Vector2(0.5f, 0.5f), 100.0f);
                dict.Add(1f, spriteData);
                defaultBalloonData.Add(Emotion.EmotionEnum.Anger, dict);
            }

            {
                dict = new Dictionary<float, SpriteData>();
                tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Fear/fear_balloon");
                tail = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Fear/fear_tail");
                spriteData = new SpriteData();
                spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                spriteData.tail = Sprite.Create(tail, new Rect(0.0f, 0.0f, tail.width, tail.height), new Vector2(0.5f, 0.5f), 100.0f);
                dict.Add(1f, spriteData);
                defaultBalloonData.Add(Emotion.EmotionEnum.Fear, dict);
            }

            {
                dict = new Dictionary<float, SpriteData>();
                tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Disgust/disgust_balloon");
                tail = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Disgust/disgust_tail");
                spriteData = new SpriteData();
                spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                spriteData.tail = Sprite.Create(tail, new Rect(0.0f, 0.0f, tail.width, tail.height), new Vector2(0.5f, 0.5f), 100.0f);
                dict.Add(1f, spriteData);
                defaultBalloonData.Add(Emotion.EmotionEnum.Disgust, dict);
            }

            {
                dict = new Dictionary<float, SpriteData>();
                tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Surprise/surprise_balloon");
                tail = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Scaled/Surprise/surprise_tail");
                spriteData = new SpriteData();
                spriteData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                spriteData.tail = Sprite.Create(tail, new Rect(0.0f, 0.0f, tail.width, tail.height), new Vector2(0.5f, 0.5f), 100.0f);
                dict.Add(1f, spriteData);
                defaultBalloonData.Add(Emotion.EmotionEnum.Surprise, dict);
            }
        }

        private void SetBackground()
        {
            EffectsData<AbstractImageEffect.ImageEffectEnum> effects = new EffectsData<AbstractImageEffect.ImageEffectEnum>();
            Dictionary<float, EffectsData<AbstractImageEffect.ImageEffectEnum>> dict = new Dictionary<float, EffectsData<AbstractImageEffect.ImageEffectEnum>>();

            effects = new EffectsData<AbstractImageEffect.ImageEffectEnum>();
            effects.showEffects = new Dictionary<AbstractImageEffect.ImageEffectEnum, AnimationCurve>();
            effects.hideEffects = new Dictionary<AbstractImageEffect.ImageEffectEnum, AnimationCurve>();
            effects.colorEffects = new Dictionary<AbstractImageEffect.ImageEffectEnum, AnimationCurve>();

            effects.showEffects.Add(AbstractImageEffect.ImageEffectEnum.FadeTexture, curves["linearCurve"]);
            effects.hideEffects.Add(AbstractImageEffect.ImageEffectEnum.FadeTexture, curves["linearCurve"]);
            effects.colorEffects.Add(AbstractImageEffect.ImageEffectEnum.FadeColor, curves["linearCurve"]);

            dict.Add(1f, effects);
            defaultBackgroundAnimationData.Add(Emotion.EmotionEnum.Neutral, dict);
            defaultBackgroundAnimationData.Add(Emotion.EmotionEnum.Happiness, dict);
            defaultBackgroundAnimationData.Add(Emotion.EmotionEnum.Sadness, dict);
            defaultBackgroundAnimationData.Add(Emotion.EmotionEnum.Anger, dict);
            defaultBackgroundAnimationData.Add(Emotion.EmotionEnum.Fear, dict);
            defaultBackgroundAnimationData.Add(Emotion.EmotionEnum.Disgust, dict);
            defaultBackgroundAnimationData.Add(Emotion.EmotionEnum.Surprise, dict);
        }

        private void SetBackgroundAnimation()
        {
            Texture2D defaultBackgroundData;
            Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>> dict = new Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>>();
            Dictionary<Reason.ReasonEnum, Texture2D> neutralDict = new Dictionary<Reason.ReasonEnum, Texture2D>();
            Dictionary<Reason.ReasonEnum, Texture2D> happinessDict = new Dictionary<Reason.ReasonEnum, Texture2D>();
            Dictionary<Reason.ReasonEnum, Texture2D> sadnessDict = new Dictionary<Reason.ReasonEnum, Texture2D>();
            Dictionary<Reason.ReasonEnum, Texture2D> angerDict = new Dictionary<Reason.ReasonEnum, Texture2D>();
            Dictionary<Reason.ReasonEnum, Texture2D> fearDict = new Dictionary<Reason.ReasonEnum, Texture2D>();
            Dictionary<Reason.ReasonEnum, Texture2D> disgustDict = new Dictionary<Reason.ReasonEnum, Texture2D>();
            Dictionary<Reason.ReasonEnum, Texture2D> surpriseDict = new Dictionary<Reason.ReasonEnum, Texture2D>();

            {
                defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/userBackground");
                neutralDict.Add(Reason.ReasonEnum.None, defaultBackgroundData);
                happinessDict.Add(Reason.ReasonEnum.None, defaultBackgroundData);
                sadnessDict.Add(Reason.ReasonEnum.None, defaultBackgroundData);
                angerDict.Add(Reason.ReasonEnum.None, defaultBackgroundData);
                fearDict.Add(Reason.ReasonEnum.None, defaultBackgroundData);
                disgustDict.Add(Reason.ReasonEnum.None, defaultBackgroundData);
                surpriseDict.Add(Reason.ReasonEnum.None, defaultBackgroundData);
            }

            {
                defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/challenge");
                neutralDict.Add(Reason.ReasonEnum.Challenge, defaultBackgroundData);
                happinessDict.Add(Reason.ReasonEnum.Challenge, defaultBackgroundData);
                sadnessDict.Add(Reason.ReasonEnum.Challenge, defaultBackgroundData);
                angerDict.Add(Reason.ReasonEnum.Challenge, defaultBackgroundData);
                fearDict.Add(Reason.ReasonEnum.Challenge, defaultBackgroundData);
                disgustDict.Add(Reason.ReasonEnum.Challenge, defaultBackgroundData);
                surpriseDict.Add(Reason.ReasonEnum.Challenge, defaultBackgroundData);
            }

            {
                defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/effort");
                neutralDict.Add(Reason.ReasonEnum.Effort, defaultBackgroundData);
                happinessDict.Add(Reason.ReasonEnum.Effort, defaultBackgroundData);
                sadnessDict.Add(Reason.ReasonEnum.Effort, defaultBackgroundData);
                angerDict.Add(Reason.ReasonEnum.Effort, defaultBackgroundData);
                fearDict.Add(Reason.ReasonEnum.Effort, defaultBackgroundData);
                disgustDict.Add(Reason.ReasonEnum.Effort, defaultBackgroundData);
                surpriseDict.Add(Reason.ReasonEnum.Effort, defaultBackgroundData);
            }

            {
                defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/engagement");
                neutralDict.Add(Reason.ReasonEnum.Engagement, defaultBackgroundData);
                happinessDict.Add(Reason.ReasonEnum.Engagement, defaultBackgroundData);
                sadnessDict.Add(Reason.ReasonEnum.Engagement, defaultBackgroundData);
                angerDict.Add(Reason.ReasonEnum.Engagement, defaultBackgroundData);
                fearDict.Add(Reason.ReasonEnum.Engagement, defaultBackgroundData);
                disgustDict.Add(Reason.ReasonEnum.Engagement, defaultBackgroundData);
                surpriseDict.Add(Reason.ReasonEnum.Engagement, defaultBackgroundData);
            }

            {
                defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/enjoyment");
                neutralDict.Add(Reason.ReasonEnum.Enjoyment, defaultBackgroundData);
                happinessDict.Add(Reason.ReasonEnum.Enjoyment, defaultBackgroundData);
                sadnessDict.Add(Reason.ReasonEnum.Enjoyment, defaultBackgroundData);
                angerDict.Add(Reason.ReasonEnum.Enjoyment, defaultBackgroundData);
                fearDict.Add(Reason.ReasonEnum.Enjoyment, defaultBackgroundData);
                disgustDict.Add(Reason.ReasonEnum.Enjoyment, defaultBackgroundData);
                surpriseDict.Add(Reason.ReasonEnum.Enjoyment, defaultBackgroundData);
            }

            {
                defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/importance");
                neutralDict.Add(Reason.ReasonEnum.Importance, defaultBackgroundData);
                happinessDict.Add(Reason.ReasonEnum.Importance, defaultBackgroundData);
                sadnessDict.Add(Reason.ReasonEnum.Importance, defaultBackgroundData);
                angerDict.Add(Reason.ReasonEnum.Importance, defaultBackgroundData);
                fearDict.Add(Reason.ReasonEnum.Importance, defaultBackgroundData);
                disgustDict.Add(Reason.ReasonEnum.Importance, defaultBackgroundData);
                surpriseDict.Add(Reason.ReasonEnum.Importance, defaultBackgroundData);
            }

            {
                defaultBackgroundData = (Texture2D)Resources.Load("Backgrounds/Images/performance");
                neutralDict.Add(Reason.ReasonEnum.Performance, defaultBackgroundData);
                happinessDict.Add(Reason.ReasonEnum.Performance, defaultBackgroundData);
                sadnessDict.Add(Reason.ReasonEnum.Performance, defaultBackgroundData);
                angerDict.Add(Reason.ReasonEnum.Performance, defaultBackgroundData);
                fearDict.Add(Reason.ReasonEnum.Performance, defaultBackgroundData);
                disgustDict.Add(Reason.ReasonEnum.Performance, defaultBackgroundData);
                surpriseDict.Add(Reason.ReasonEnum.Performance, defaultBackgroundData);
            }

            dict.Add(1f, neutralDict);
            defaultBackgroundDataDictionary.Add(Emotion.EmotionEnum.Neutral, dict);
            dict = new Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>>();
            dict.Add(1f, happinessDict);
            defaultBackgroundDataDictionary.Add(Emotion.EmotionEnum.Happiness, dict);
            dict = new Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>>();
            dict.Add(1f, sadnessDict);
            defaultBackgroundDataDictionary.Add(Emotion.EmotionEnum.Sadness, dict);
            dict = new Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>>();
            dict.Add(1f, angerDict);
            defaultBackgroundDataDictionary.Add(Emotion.EmotionEnum.Anger, dict);
            dict = new Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>>();
            dict.Add(1f, fearDict);
            defaultBackgroundDataDictionary.Add(Emotion.EmotionEnum.Fear, dict);
            dict = new Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>>();
            dict.Add(1f, disgustDict);
            defaultBackgroundDataDictionary.Add(Emotion.EmotionEnum.Disgust, dict);
            dict = new Dictionary<float, Dictionary<Reason.ReasonEnum, Texture2D>>();
            dict.Add(1f, surpriseDict);
            defaultBackgroundDataDictionary.Add(Emotion.EmotionEnum.Surprise, dict);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem {
    public class TestManager : MonoBehaviour {
        const int DATA_SIZE = 4;

        public BubbleSystemManager manager;
        Queue<Emotion> emotion = new Queue<Emotion>();
        Emotion.EmotionEnum emotionEnum = Emotion.EmotionEnum.Happiness;
        Tutor.TutorEnum _tutor = Tutor.TutorEnum.Maria;
        Reason.ReasonEnum reason = Reason.ReasonEnum.None;
        Queue<BubbleSystemData> data = new Queue<BubbleSystemData>();
        float intensity = 1.0f;
        public VTToModuleBridge bridge;

        private int mix = 0;
        private int forceText = 0;
        private int balloonAnimation = 0;

        private void Start()
        {
            for (int i = 0; i < DATA_SIZE; ++i)
                data.Enqueue(new BubbleSystemData());

            for (int i = 0; i < DATA_SIZE; ++i)
                emotion.Enqueue(new Emotion());
        }

        void UpdateScene()
        {
            BubbleSystemData first = data.Dequeue();
            manager.UpdateScene(first);
            data.Enqueue(first);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                
                data.Peek().Clear();
                Emotion first = emotion.Dequeue();
                first.Set(emotionEnum.ToString());
                data.Peek().emotions.Add(first, intensity);
                data.Peek().tutor.Set(_tutor.ToString());
                data.Peek().backgroundData.reason.Set(reason.ToString());
                UpdateScene();
                emotion.Enqueue(first);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                data.Peek().Clear();
                Emotion first = emotion.Dequeue();
                first.Set(emotionEnum.ToString());
                data.Peek().emotions.Add(first, intensity);
                data.Peek().tutor.Set(_tutor.ToString());
                if (data.Peek().tutor.GetString().Equals("User")) {
                    data.Peek().balloonData.options = true;
                }
                data.Peek().balloonData.text.Add("Hello world!");
                UpdateScene();
                emotion.Enqueue(first);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                data.Peek().Clear();
                Emotion first = emotion.Dequeue();
                first.Set(emotionEnum.ToString());
                data.Peek().emotions.Add(first, intensity);
                data.Peek().tutor.Set(_tutor.ToString());
                if (data.Peek().tutor.GetString().Equals("User"))
                {
                    data.Peek().balloonData.options = true;
                }
                data.Peek().balloonData.show = false;
                UpdateScene();
                emotion.Enqueue(first);
            }

            if (Input.GetKeyDown(KeyCode.Z)) _tutor = Tutor.TutorEnum.Joao;
            if (Input.GetKeyDown(KeyCode.X)) _tutor = Tutor.TutorEnum.Maria;
            if (Input.GetKeyDown(KeyCode.C)) _tutor = Tutor.TutorEnum.User;
            if (Input.GetKeyDown(KeyCode.Q)) emotionEnum = Emotion.EmotionEnum.Happiness;
            if (Input.GetKeyDown(KeyCode.W)) emotionEnum = Emotion.EmotionEnum.Sadness;
            if (Input.GetKeyDown(KeyCode.E)) emotionEnum = Emotion.EmotionEnum.Surprise;
            if (Input.GetKeyDown(KeyCode.R)) emotionEnum = Emotion.EmotionEnum.Fear;
            if (Input.GetKeyDown(KeyCode.T)) emotionEnum = Emotion.EmotionEnum.Anger;
            if (Input.GetKeyDown(KeyCode.Y)) emotionEnum = Emotion.EmotionEnum.Disgust;
            if (Input.GetKeyDown(KeyCode.U)) emotionEnum = Emotion.EmotionEnum.Neutral;
            if (Input.GetKeyDown(KeyCode.Alpha1)) reason = Reason.ReasonEnum.None;
            if (Input.GetKeyDown(KeyCode.Alpha2)) reason = Reason.ReasonEnum.Challenge;
            if (Input.GetKeyDown(KeyCode.Alpha3)) reason = Reason.ReasonEnum.Effort;
            if (Input.GetKeyDown(KeyCode.Alpha4)) reason = Reason.ReasonEnum.Engagement;
            if (Input.GetKeyDown(KeyCode.Alpha5)) reason = Reason.ReasonEnum.Enjoyment;
            if (Input.GetKeyDown(KeyCode.Alpha6)) reason = Reason.ReasonEnum.Importance;
            if (Input.GetKeyDown(KeyCode.Alpha7)) reason = Reason.ReasonEnum.Performance;
            if (Input.GetKeyDown(KeyCode.KeypadPlus)) Mathf.Clamp01(intensity += 0.1f);
            if (Input.GetKeyDown(KeyCode.KeypadMinus)) Mathf.Clamp01(intensity -= 0.1f);

            if (Input.GetKeyDown(KeyCode.D))
            {
                bridge.Handle(new string[] { "OverrideTextEffects", "Happiness", "0.5", "showEffects", "FadeIn", "linearCurve", "hideEffects", "FadeOut", "linearCurve" });
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                bridge.UpdateOptions(new string[] { "hi", "asd", "sadf", "ge3r" });
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                mix = (mix + 1) % 2;
                Debug.Log(Convert.ToBoolean(mix));
                bridge.Handle(new string[] { "SetMixColors", mix.ToString() });
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                bridge.Handle(new string[] { "OverrideBlushColor", "#00FF00FF" });
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                bridge.Handle(new string[] { "OverrideEmotionColor", "Happiness", "#00FF00FF" });
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                bridge.Handle(new string[] { "AddAnimationCurve", "abc", "0", "0", "smooth", "1", "1", "1" });
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                forceText = (forceText + 1) % 2;
                bridge.Handle(new string[] { "SetForceTextUpdate", forceText.ToString() });
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                balloonAnimation = (balloonAnimation + 1) % 2;
                Debug.Log(Convert.ToBoolean(balloonAnimation));
                bridge.Handle(new string[] { "SetBalloonAnimationBlending", balloonAnimation.ToString() });
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                bridge.Handle(new string[] { "SetBalloonDuration", 2f.ToString() });
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                bridge.Handle(new string[] { "SetBackgroundDuration", 2f.ToString() });
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                bridge.Handle(new string[] { "SetOptionsDuration", 2f.ToString() });
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                bridge.HideBalloon("User");
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                bridge.Handle(new string[] { "UpdateBackground", Tutor.TutorEnum.Maria.ToString(), "Fear", "1", "None" });
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                bridge.HideBalloon(BubbleSystem.Tutor.TutorEnum.User.ToString());
            }
        }
    }
}
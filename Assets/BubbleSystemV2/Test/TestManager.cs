using BubbleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2 {
    public class TestManager : MonoBehaviour {
        const int DATA_SIZE = 4;

        public BubbleSystemManager manager;
        Emotion emotion = new Emotion();
        Emotion.EmotionEnum emotionEnum = Emotion.EmotionEnum.Happiness;
        Tutor.TutorEnum _tutor = Tutor.TutorEnum.Maria;
        Reason.ReasonEnum reason = Reason.ReasonEnum.None;
        BubbleSystemData[] data = new BubbleSystemData[DATA_SIZE];
        BubbleSystemData[] newData = new BubbleSystemData[DATA_SIZE];
        float intensity = 1.0f;

        static uint currentData = 0;
        static uint currentNewData = 0;

        private void Start()
        {
            for (int i = 0; i < DATA_SIZE; ++i)
                data[i] = new BubbleSystemData();

            for (int i = 0; i < DATA_SIZE; ++i)
                newData[i] = new BubbleSystemData();
        }

        void UpdateScene()
        {
            manager.UpdateScene(data[currentData]);
            currentData = (currentData + 1) % DATA_SIZE;
        }

        void SetData()
        {
            manager.AddTutorNextData(newData[currentNewData]);
            currentNewData = (currentNewData + 1) % DATA_SIZE;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                
                data[currentData].Clear();
                emotion.Set(emotionEnum.ToString());
                data[currentData].emotions.Add(emotion, 1.0f);
                data[currentData].backgroundData.effects.showEffects.Add(AbstractImageEffect.ImageEffectEnum.FadeTexture, DefaultData.Instance.GetCurve("linearCurve"));
                data[currentData].backgroundData.effects.hideEffects.Add(AbstractImageEffect.ImageEffectEnum.FadeTexture, DefaultData.Instance.GetCurve("linearCurve"));
                data[currentData].backgroundData.effects.colorEffects.Add(AbstractImageEffect.ImageEffectEnum.FadeColor, DefaultData.Instance.GetCurve("linearCurve"));
                data[currentData].tutor.Set(_tutor.ToString());
                data[currentData].backgroundData.reason.Set(reason.ToString());
                UpdateScene();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                data[currentData].Clear();
                emotion.Set(emotionEnum.ToString());
                data[currentData].emotions.Add(emotion, 1.0f);
                data[currentData].tutor.Set(_tutor.ToString());
                if (data[currentData].tutor.GetString().Equals("User")) {
                    data[currentData].balloonData.options = true;
                }
                data[currentData].balloonData.text = new List<string> { "Hello World", "asfd" };
                UpdateScene();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                data[currentData].Clear();
                emotion.Set(emotionEnum.ToString());
                data[currentData].emotions.Add(emotion, 1.0f);
                data[currentData].tutor.Set(_tutor.ToString());
                if (data[currentData].tutor.GetString().Equals("User"))
                {
                    data[currentData].balloonData.options = true;
                }
                data[currentData].balloonData.show = false;
                UpdateScene();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                newData[currentNewData].Clear();
                emotion.Set(emotionEnum.ToString());
                newData[currentNewData].emotions.Add(emotion, 1.0f);
                newData[currentNewData].tutor.Set(_tutor.ToString());
                if (newData[currentNewData].tutor.GetString().Equals("User"))
                {
                    newData[currentNewData].balloonData.options = true;
                }
                newData[currentNewData].balloonData.text = new List<string> { "asdfsad", "123" };

                data[currentData].backgroundData.effects.showEffects.Add(AbstractImageEffect.ImageEffectEnum.FadeTexture, DefaultData.Instance.GetCurve("linearCurve"));
                data[currentData].backgroundData.effects.hideEffects.Add(AbstractImageEffect.ImageEffectEnum.FadeTexture, DefaultData.Instance.GetCurve("linearCurve"));
                data[currentData].backgroundData.effects.colorEffects.Add(AbstractImageEffect.ImageEffectEnum.FadeColor, DefaultData.Instance.GetCurve("linearCurve"));
                data[currentData].backgroundData.reason.Set(reason.ToString());

                SetData();
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

        }
    }
}
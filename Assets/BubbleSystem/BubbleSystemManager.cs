using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{
    public class BubbleSystemManager : MonoBehaviour
    {
        private BackgroundManager backgroundManager;
        private BalloonManager balloonManager;

        private Dictionary<string, SpeakData> tutorSpeakData = new Dictionary<string, SpeakData>();
        private Dictionary<string, BackgroundData> tutorBackgroundData = new Dictionary<string, BackgroundData>();
        private Dictionary<string, NextDialogueData> tutorNextData = new Dictionary<string, NextDialogueData>();

        private string options;
        private bool firstTutor = true;

        private void Start()
        {
            backgroundManager = GetComponent<BackgroundManager>();
            balloonManager = GetComponent<BalloonManager>();
            options = balloonManager.options.name;
        }

        /**********************************************************************************************************
                                                GETTERS AND SETTERS
        **********************************************************************************************************/

        public Dictionary<string, NextDialogueData> GetTutorNextData()
        {
            return tutorNextData;
        }

        public void AddToTutorNextData(string info, NextDialogueData nextData)
        {
            BubbleSystemUtility.AddToDictionary(ref tutorNextData, info, nextData);
        }


        /**********************************************************************************************************
                                                        FUNCTIONS
        **********************************************************************************************************/

        public void UpdateBackground(string tutor, Dictionary<string, float> emotions, Reason reason)
        {
            SetBackgroundData(tutor, emotions, reason);
            backgroundManager.SetBackground(tutor, tutorBackgroundData[tutor]);
        }

        public void Speak(string tutor, Dictionary<string, float> emotions, string[] text, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            if (!tutor.Equals(options))
            {
                if (firstTutor)
                {
                    balloonManager.ReverseTutorsBalloons(tutor);
                    firstTutor = false;
                }

                SetSpeakData(tutor, emotions, text, showEffects, hideEffects);
                balloonManager.Speak(tutor, tutorSpeakData[tutor]);
            }
        }

        public void HideBalloon(string tutor, float duration = 0.0f)
        {
            balloonManager.HideBalloon(tutor, duration, tutorSpeakData[tutor]);
        }

        public void UpdateOptions(string[] text, HookControl.IntFunc[] callbacks = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            Dictionary<string, float> dict = new Dictionary<string, float>();
            dict.Add(Emotion.Neutral.ToString(), 1.0f);

            SetSpeakData(options, dict, text, showEffects, hideEffects);
            balloonManager.ShowOptions(options, tutorSpeakData[options], callbacks);
        }


        /**********************************************************************************************************
                                                    HELP FUNCTIONS
        **********************************************************************************************************/

        private void SetSpeakData(string tutor, Dictionary<string, float> emotions, string[] text = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            SpeakData data = new SpeakData();

            if (CheckNextData(tutor))
            {
                NextDialogueData nextData = tutorNextData[tutor];
                RemoveDefaultAndNeutralEmotions(ref nextData.emotions);
                data.emotions = GetEmotionDictionary(nextData.emotions);
                data.text = text;
                data.showEffects = nextData.showEffects;
                data.hideEffects = nextData.hideEffects;
                nextData.isSet = false;
                tutorNextData[tutor] = nextData;
            }
            else
            {
                RemoveDefaultAndNeutralEmotions(ref emotions);
                data.emotions = GetEmotionDictionary(emotions);
                data.text = text;
                data.showEffects = getEffectsDictionary(showEffects);
                data.hideEffects = getEffectsDictionary(hideEffects);
            }

            BubbleSystemUtility.AddToDictionary(ref tutorSpeakData, tutor, data);
        }

        private void SetBackgroundData(string tutor, Dictionary<string, float> emotions, Reason reason)
        {
            BackgroundData data = new BackgroundData();
            data.emotions = GetEmotionDictionary(emotions);
            data.reason = reason;

            BubbleSystemUtility.AddToDictionary(ref tutorBackgroundData, tutor, data);
        }

        private bool CheckNextData(string tutor)
        {
            if (tutorNextData.ContainsKey(tutor))
            {
                if (tutorNextData[tutor].isSet)
                    return true;
            }
            return false;
        }

        private Dictionary<Emotion, float> GetEmotionDictionary(Dictionary<string, float> emotions)
        {
            Dictionary<Emotion, float> emotionsDict = new Dictionary<Emotion, float>();
            float sum = emotions.Sum(x => x.Value);
            if (sum >= 0 && sum <= 1) sum = 1;
            foreach (string emotionString in emotions.Keys)
            {
                Emotion emotion = (Emotion)Enum.Parse(typeof(Emotion), emotionString);
                emotionsDict.Add(emotion, Mathf.Clamp01(emotions[emotionString]) / sum);
            }

            return emotionsDict;
        }

        private void RemoveDefaultAndNeutralEmotions(ref Dictionary<string, float> emotions)
        {
            if (emotions.Count > 1 && emotions.ContainsKey(BubbleSystem.Emotion.Neutral.ToString()))
            {
                emotions.Remove(BubbleSystem.Emotion.Neutral.ToString());
            }
        }

        private Dictionary<Effect, AnimationCurve> getEffectsDictionary(Dictionary<string, string> effects)
        {
            Dictionary<Effect, AnimationCurve> effectsDictionary = new Dictionary<Effect, AnimationCurve>();
            if (effects != null)
            {
                foreach (string fx in effects.Keys)
                {
                    Effect effect = (Effect)Enum.Parse(typeof(Effect), fx);
                    AnimationCurve curve = DefaultData.Instance.GetCurve(effects[fx]);
                    effectsDictionary.Add(effect, curve);
                }
                return effectsDictionary;
            }
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{
    public class BubbleSystemManager : MonoBehaviour
    {
        public struct NextDialogueData
        {
            public Dictionary<string, float> emotions;
            public float duration;
            public Dictionary<Effect, AnimationCurve> showEffects;
            public Dictionary<Effect, AnimationCurve> hideEffects;
            public bool isSet;
        }

        private BackgroundManager backgroundManager;
        private BalloonManager balloonManager;

        private Dictionary<string, SpeakData> tutorSpeakData = new Dictionary<string, SpeakData>();
        private Dictionary<string, BackgroundData> tutorBackgroundData = new Dictionary<string, BackgroundData>();
        private Dictionary<string, NextDialogueData> tutorNextData = new Dictionary<string, NextDialogueData>();

        private bool firstTutor = true;

        private void Start()
        {
            backgroundManager = GetComponent<BackgroundManager>();
            balloonManager = GetComponent<BalloonManager>();
        }

        public void Handle(string[] info)
        {
            string[] parameters = info.Skip(1).ToArray();

            if (info[0].Equals("SetNextDialogueData"))
            {
                SetNextDialogueData(parameters);
            }

            else if (info[0].Equals("UpdateBackground"))
            {
                UpdateBackground(parameters);
            }

            else if (info[0].Equals("OverrideBackgroundColor"))
            {
                OverrideBackgroundColor(parameters);
            }

            else if (info[0].Equals("OverrideTextEffects"))
            {
                OverrideTextEffects(parameters);
            }
        }

        //<< OverrideBackgroundColor emotion intensity reason color >>  Color in #RRGGBBAA format
        private void OverrideBackgroundColor(string[] info)
        {
            Emotion emotion = (Emotion)Enum.Parse(typeof(Emotion), info[0]);
            float intensity = Mathf.Clamp01(Convert.ToSingle(info[1]));
            Reason reason = (Reason)Enum.Parse(typeof(Reason), info[2]);
            Color color;
            ColorUtility.TryParseHtmlString(info[3], out color);
            DefaultData.Instance.SetBackgroundColor(emotion, intensity, reason, color);
        }

        //<< OverrideTextEffects emotion intensity [showEffects [curve] ...] [hideEffects effect1 [curve] ...] >>
        private void OverrideTextEffects(string[] info)
        {
            Emotion emotion = (Emotion)Enum.Parse(typeof(Emotion), info[0]);
            float intensity = Mathf.Clamp01(Convert.ToSingle(info[1]));
            Dictionary<Effect, AnimationCurve> showEffects = null;
            Dictionary<Effect, AnimationCurve> hideEffects = null;

            int size = info.Length;
            KeyValuePair<int, Dictionary<Effect, AnimationCurve>> kvp;

            if (size > 2) {
                if (info[2].Equals("showEffects"))
                {
                    kvp = GetEffects(info, 3);
                    showEffects = kvp.Value;
                    if (size > kvp.Key)
                    {
                        if (info[kvp.Key].Equals("hideEffects"))
                        {
                            kvp = GetEffects(info, kvp.Key + 1);
                            hideEffects = kvp.Value;
                        }
                    }
                }
                else if (info[2].Equals("hideEffects"))
                {
                    kvp = GetEffects(info, 3);
                    hideEffects = kvp.Value;
                }
            }

            DefaultData.Instance.SetTextEffects(emotion, intensity, showEffects, hideEffects);
        }

        private KeyValuePair<int, Dictionary<Effect, AnimationCurve>> GetEffects(string[] info, int i)
        {
            Dictionary<Effect, AnimationCurve> effects = new Dictionary<Effect, AnimationCurve>();
            string stringToLook = "hideEffects";
            while (i < info.Length)
            {
                if (info[i].Equals(stringToLook))
                    break;

                Effect effect;
                AnimationCurve animationCurve;
                try
                {
                    effect = (Effect)Enum.Parse(typeof(Effect), info[i]);
                    animationCurve = (i + 1 >= info.Length) ? null : (info[i+1].Equals(stringToLook)) ? null : DefaultData.Instance.GetCurve(info[i + 1]);
                    i += (animationCurve != null) ? 2 : 1;

                    effects.Add(effect, animationCurve);
                    continue;
                }
                catch
                {
                    i++;
                    continue;
                }
            }

            return new KeyValuePair<int, Dictionary<Effect, AnimationCurve>>(i, effects);
        }

        private int SetNextDataEffects(string[] info, ref NextDialogueData nextData, int i, bool show)
        {
            KeyValuePair<int, Dictionary<Effect, AnimationCurve>> kvp = GetEffects(info, i);
            if (show)
                nextData.showEffects = kvp.Value;
            else
                nextData.hideEffects = kvp.Value;

            return kvp.Key;
        }

        private int SetNextEmotions(string[] info, ref NextDialogueData nextData, int i)
        {
            string stringToLook = "duration";
            Dictionary<string, float> dict = new Dictionary<string, float>();

            while (!info[i].Equals(stringToLook))
            {
                dict.Add(info[i], Mathf.Clamp01(System.Convert.ToSingle(info[i + 1])));
                i+=2;
            }

            nextData.emotions = dict;
            return ++i;
        }

        //<< UpdateBackground MARIA HAPPINESS 0.5 5 Grades>>
        public void UpdateBackground(string[] info)
        {
            Reason reason = Reason.None;
            try
            {
                reason = (Reason)Enum.Parse(typeof(Reason), info[4]);
            }
            catch { }
            UpdateBackground(info[0], info[1], Convert.ToSingle(info[2]), Convert.ToSingle(info[3]), reason);
        }

        //<< SetNextDialogueData MARIA HAPPINESS 0.5 duration 5 [showEffects effect1 [curve] ...] [hideEffects effect1 [curve] ...] >>
        public void SetNextDialogueData(string[] info)
        {
            NextDialogueData nextData = new NextDialogueData();            
            int i = SetNextEmotions(info, ref nextData, 1);

            int size = info.Length;

            nextData.duration = Convert.ToSingle(info[i++]);

            if (size > i)
            {
                if (info[i].Equals("showEffects"))
                {
                    nextData.showEffects = new Dictionary<Effect, AnimationCurve>();
                    i = SetNextDataEffects(info, ref nextData, i, true);

                    if (size > i)
                    {
                        if (info[i].Equals("hideEffects"))
                        {
                            nextData.hideEffects = new Dictionary<Effect, AnimationCurve>();
                            i = SetNextDataEffects(info, ref nextData, ++i, false);
                        }
                    }
                }

                else if (info[i].Equals("hideEffects"))
                {
                    nextData.hideEffects = new Dictionary<Effect, AnimationCurve>();
                    i = SetNextDataEffects(info, ref nextData, i, false);
                }
            }

            nextData.isSet = true;

            if (tutorNextData.ContainsKey(info[0]))
                tutorNextData[info[0]] = nextData;
            else
                tutorNextData.Add(info[0], nextData);
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

        private void SetSpeakData(string tutor, Dictionary<string, float> emotions, string[] text = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            SpeakData data = new SpeakData();
            Dictionary<Emotion, float> emotionsDict = new Dictionary<Emotion, float>();
            float sum;

            if (CheckNextData(tutor))
            {
                NextDialogueData nextData = tutorNextData[tutor];

                if (nextData.emotions.Count > 1 && nextData.emotions.ContainsKey(Emotion.Default.ToString()))
                {
                    nextData.emotions.Remove(Emotion.Default.ToString());
                }
                if (nextData.emotions.Count > 1 && nextData.emotions.ContainsKey(Emotion.Neutral.ToString()))
                {
                    nextData.emotions.Remove(Emotion.Neutral.ToString());
                }

                sum = nextData.emotions.Sum(x => x.Value);
                if (sum == 0) sum = 1;

                foreach (string emotionString in nextData.emotions.Keys)
                {
                    Emotion emotion = (Emotion)Enum.Parse(typeof(Emotion), emotionString);
                    emotionsDict.Add(emotion, nextData.emotions[emotionString] / sum);
                }

                data.emotions = emotionsDict;

                data.text = text;
                data.showEffects = nextData.showEffects;
                data.hideEffects = nextData.hideEffects;
                nextData.isSet = false;
                tutorNextData[tutor] = nextData;
            }
            else
            {
                if (emotions.Count > 1 && emotions.ContainsKey(Emotion.Default.ToString()))
                {
                    emotions.Remove(Emotion.Default.ToString());
                }
                if (emotions.Count > 1 && emotions.ContainsKey(Emotion.Neutral.ToString()))
                {
                    emotions.Remove(Emotion.Neutral.ToString());
                }

                sum = emotions.Sum(x => x.Value);
                if (sum == 0) sum = 1;

                foreach (string emotionString in emotions.Keys)
                {
                    Emotion emotion = (Emotion)Enum.Parse(typeof(Emotion), emotionString);
                    emotionsDict.Add(emotion, Mathf.Clamp01(emotions[emotionString]) / sum);
                }

                data.emotions = emotionsDict;

                data.text = text;
                data.showEffects = getEffectsDictionary(showEffects);
                data.hideEffects = getEffectsDictionary(hideEffects);
            }


            if (tutorSpeakData.ContainsKey(tutor))
                tutorSpeakData[tutor] = data;
            else
                tutorSpeakData.Add(tutor, data);
        }

        private void SetBackgroundData(string tutor, string emotion = "Neutral", float intensity = 0.0f, Reason reason = Reason.None)
        {
            BackgroundData data = new BackgroundData();
            try
            {
                data.emotion = (Emotion)Enum.Parse(typeof(Emotion), emotion);
            }
            catch
            {
                throw new MissingFieldException("Emotion enum does not contain " + emotion + ".");
            }
            data.intensity = Mathf.Clamp01(intensity);
            data.reason = reason;

            if (tutorBackgroundData.ContainsKey(tutor))
                tutorBackgroundData[tutor] = data;
            else
                tutorBackgroundData.Add(tutor, data);
        }

        public void UpdateBackground(string tutor, string emotion, float intensity, float duration, Reason reason)
        {
            SetBackgroundData(tutor, emotion, intensity, reason);
            backgroundManager.SetBackground(tutor, tutorBackgroundData[tutor], duration);
        }

        public void UpdateBackground(Tutor tutor, float duration, Reason reason)
        {
            UpdateBackground(tutor.Name, tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity, duration, reason);
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

        public void Speak(string tutor, Dictionary<string, float> emotions, string[] text, float duration = 0.0f, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            if (firstTutor)
            {
                balloonManager.ReverseTutorsBalloons(tutor);
                firstTutor = false;
            }

            SetSpeakData(tutor, emotions, text, showEffects, hideEffects);
            balloonManager.ShowBalloon(tutor, tutorSpeakData[tutor], duration);
        }

        public void Speak(Tutor tutor, string[] text, float duration = 0.0f, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            Dictionary<string, float> dict = new Dictionary<string, float>();
            dict.Add(tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity);
            Speak(tutor.Name, dict, text, duration, showEffects, hideEffects);
        }

        public void HideBalloon(string tutor, float duration = 0.0f)
        {
            if (tutorSpeakData.ContainsKey(tutor))
            {
                balloonManager.HideBalloon(tutor, duration, tutorSpeakData[tutor]);
            }
            else
            {
                throw new KeyNotFoundException("Key " + tutor + " does not exist yet");
            }
        }

        public void HideBalloon(Tutor tutor, float duration = 0.0f)
        {
            HideBalloon(tutor.Name, duration);
        }

        public void UpdateOptions(string[] text, float duration = 5.0f, HookControl.IntFunc[] callbacks = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            Dictionary<string, float> dict = new Dictionary<string, float>();
            dict.Add("Default", 0.0f);
            SetSpeakData("Options", dict, text, showEffects, hideEffects);
            balloonManager.ShowBalloon("Options", tutorSpeakData["Options"], duration, callbacks);
        }

    }
}
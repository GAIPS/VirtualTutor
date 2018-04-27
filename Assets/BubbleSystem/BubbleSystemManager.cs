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

        private string options;
        private bool firstTutor = true;

        private void Start()
        {
            backgroundManager = GetComponent<BackgroundManager>();
            balloonManager = GetComponent<BalloonManager>();
            options = balloonManager.options.name;
        }

        public void Handle(string[] info)
        {
            string[] parameters = info.Skip(1).ToArray();

            switch (info[0])
            {
                case "SetNextDialogueData":
                    SetNextDialogueData(parameters);
                    break;
                case "UpdateBackground":
                    UpdateBackground(parameters);
                    break;
                case "OverrideBackgroundColor":
                    OverrideBackgroundColor(parameters);
                    break;
                case "OverrideTextEffects":
                    OverrideTextEffects(parameters);
                    break;
                default:
                    break;
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

        private KeyValuePair<Dictionary<Effect, AnimationCurve>, Dictionary<Effect, AnimationCurve>> SetTextEffects(string[] info, int i)
        {
            Dictionary<Effect, AnimationCurve> showEffects = null;
            Dictionary<Effect, AnimationCurve> hideEffects = null;
            KeyValuePair<int, Dictionary<Effect, AnimationCurve>> kvp;
            int size = info.Length;
            if (size > i)
            {
                if (info[i].Equals("showEffects"))
                {
                    kvp = GetEffects(info, i + 1);
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
                else if (info[i].Equals("hideEffects"))
                {
                    kvp = GetEffects(info, i + 1);
                    hideEffects = kvp.Value;
                }
            }

            return new KeyValuePair<Dictionary<Effect, AnimationCurve>, Dictionary<Effect, AnimationCurve>>(showEffects, hideEffects);
        }

        //<< OverrideTextEffects emotion intensity [showEffects [curve] ...] [hideEffects effect1 [curve] ...] >>
        private void OverrideTextEffects(string[] info)
        {
            Emotion emotion = (Emotion)Enum.Parse(typeof(Emotion), info[0]);
            float intensity = Mathf.Clamp01(Convert.ToSingle(info[1]));

            KeyValuePair<Dictionary<Effect, AnimationCurve>, Dictionary<Effect, AnimationCurve>> effects = SetTextEffects(info, 2);
            DefaultData.Instance.SetTextEffects(emotion, intensity, effects.Key, effects.Value);
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

        private KeyValuePair<int, Dictionary<string, float>> GetEmotions(string[] info, int i)
        {
            string stringToLook = "duration";
            Dictionary<string, float> dict = new Dictionary<string, float>();
            while (!info[i].Equals(stringToLook))
            {
                dict.Add(info[i], Mathf.Clamp01(System.Convert.ToSingle(info[i + 1])));
                i += 2;
            }

            return new KeyValuePair<int, Dictionary<string, float>>(i, dict);
        }

        //<< UpdateBackground MARIA HAPPINESS 0.5 duration 5 Grades>>
        public void UpdateBackground(string[] info)
        {
            Reason reason = (Reason)Enum.Parse(typeof(Reason), info[info.Length - 1]);
            KeyValuePair<int, Dictionary<string, float>> kvp = GetEmotions(info, 1);
            UpdateBackground(info[0], kvp.Value, Convert.ToSingle(info[kvp.Key + 1]), reason);
        }

        //<< SetNextDialogueData MARIA HAPPINESS 0.5 duration 5 [showEffects effect1 [curve] ...] [hideEffects effect1 [curve] ...] >>
        public void SetNextDialogueData(string[] info)
        {
            NextDialogueData nextData = new NextDialogueData();
            KeyValuePair<int, Dictionary<string, float>> kvp = GetEmotions(info, 1);

            nextData.emotions = kvp.Value;
            int i = kvp.Key + 1;

            nextData.duration = Convert.ToSingle(info[i++]);

            KeyValuePair<Dictionary<Effect, AnimationCurve>, Dictionary<Effect, AnimationCurve>> effects = SetTextEffects(info, i);
            nextData.showEffects = effects.Key;
            nextData.hideEffects = effects.Value;

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
            if (emotions.Count > 1 && emotions.ContainsKey(Emotion.Default.ToString()))
            {
                emotions.Remove(Emotion.Default.ToString());
            }
            if (emotions.Count > 1 && emotions.ContainsKey(Emotion.Neutral.ToString()))
            {
                emotions.Remove(Emotion.Neutral.ToString());
            }
        }

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

            if (tutorSpeakData.ContainsKey(tutor))
                tutorSpeakData[tutor] = data;
            else
                tutorSpeakData.Add(tutor, data);
        }

        private void SetBackgroundData(string tutor, Dictionary<string, float> emotions, Reason reason)
        {
            BackgroundData data = new BackgroundData();
            data.emotions = GetEmotionDictionary(emotions);
            data.reason = reason;

            if (tutorBackgroundData.ContainsKey(tutor))
                tutorBackgroundData[tutor] = data;
            else
                tutorBackgroundData.Add(tutor, data);
        }

        public void UpdateBackground(string tutor, Dictionary<string, float> emotions, float duration, Reason reason)
        {
            SetBackgroundData(tutor, emotions, reason);
            backgroundManager.SetBackground(tutor, tutorBackgroundData[tutor], duration);
        }

        public void UpdateBackground(Tutor tutor, float duration, Reason reason)
        {
            Dictionary<string, float> dict = new Dictionary<string, float>();
            dict.Add(tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity);
            UpdateBackground(tutor.Name, dict, duration, reason);
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
            if (!tutor.Equals(options)) {
                if (firstTutor)
                {
                    balloonManager.ReverseTutorsBalloons(tutor);
                    firstTutor = false;
                }

                SetSpeakData(tutor, emotions, text, showEffects, hideEffects);
                balloonManager.Speak(tutor, tutorSpeakData[tutor], duration);
            }
        }

        public void Speak(Tutor tutor, string[] text, float duration = 0.0f, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            Dictionary<string, float> dict = new Dictionary<string, float>();
            dict.Add(tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity);
            Speak(tutor.Name, dict, text, duration, showEffects, hideEffects);
        }

        public void HideBalloon(string tutor, float duration = 0.0f)
        {
            balloonManager.HideBalloon(tutor, duration, tutorSpeakData[tutor]);
        }

        public void HideBalloon(Tutor tutor, float duration = 0.0f)
        {
            HideBalloon(tutor.Name, duration);
        }
        
        public void UpdateOptions(string[] text, float duration = 5.0f, HookControl.IntFunc[] callbacks = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            Dictionary<string, float> dict = new Dictionary<string, float>();
            dict.Add(Emotion.Default.ToString(), 0.0f);

            SetSpeakData(options, dict, text, showEffects, hideEffects);
            balloonManager.ShowOptions(options, tutorSpeakData[options], duration, callbacks);
        }

    }
}
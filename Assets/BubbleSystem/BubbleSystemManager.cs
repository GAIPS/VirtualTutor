using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{
    public class BubbleSystemManager : MonoBehaviour
    {
        public struct NextDialogueData
        {
            public string emotion;
            public float intensity;
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

        private int SetNextDataEffects(string[] info, ref NextDialogueData nextData, int i, bool show)
        {
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
                    animationCurve = i + 1 >= info.Length ? null : DefaultData.Instance.GetCurve(info[i + 1]);// >= info.Length ? null : info[i+1]);
                    i += (animationCurve != null) ? 2 : 1;

                    if (show)
                        nextData.showEffects.Add(effect, animationCurve);
                    else
                        nextData.hideEffects.Add(effect, animationCurve);
                    continue;
                }
                catch
                {
                    i++;
                    continue;
                }
            }
            return i;
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

        //<< SETNEXTDIALOGUEDATA MARIA HAPPINESS 0.5 5 [showEffects effect1 [curve] ...] [hideEffects effect1 [curve] ...] >>
        public void SetNextDialogueData(string[] info)
        {
            NextDialogueData nextData = new NextDialogueData();
            int size = info.Length;

            nextData.emotion = info[1];
            nextData.intensity = Mathf.Clamp01(Convert.ToSingle(info[2]));
            nextData.duration = Convert.ToSingle(info[3]);

            if (size > 4)
            {
                int i = 5;
                if (info[4].Equals("showEffects"))
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

                else if (info[4].Equals("hideEffects"))
                {
                    nextData.hideEffects = new Dictionary<Effect, AnimationCurve>();
                    i = SetNextDataEffects(info, ref nextData, i, false);
                }
            }

            nextData.isSet = true;

            if (tutorNextData.ContainsKey(info[1]))
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

        private void SetSpeakData(string tutor, string emotion = "Neutral", float intensity = 0.0f, string[] text = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            SpeakData data = new SpeakData();
            if (CheckNextData(tutor))
            {
                NextDialogueData nextData = tutorNextData[tutor];
                data.emotion = (Emotion)Enum.Parse(typeof(Emotion), nextData.emotion);
                data.intensity = nextData.intensity;
                data.text = text;
                data.showEffects = nextData.showEffects;
                data.hideEffects = nextData.hideEffects;
                nextData.isSet = false;
                tutorNextData[tutor] = nextData;
            }
            else
            {
                try
                {
                    data.emotion = (Emotion)Enum.Parse(typeof(Emotion), emotion);
                }
                catch
                {
                    throw new MissingFieldException("Emotion enum does not contain " + emotion + ".");
                }

                intensity = Mathf.Clamp01(intensity);
                data.intensity = intensity;
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

        public void Speak(string tutor, string emotion, float intensity, string[] text, float duration = 0.0f, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            if (firstTutor)
            {
                balloonManager.ReverseTutorsBalloons(tutor);
                firstTutor = false;
            }

            SetSpeakData(tutor, emotion, intensity, text, showEffects, hideEffects);
            balloonManager.ShowBalloon(tutor, tutorSpeakData[tutor], duration);
        }

        public void Speak(Tutor tutor, string[] text, float duration = 0.0f, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            Speak(tutor.Name, tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity, text, duration, showEffects, hideEffects);
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

        public void UpdateOptions(string[] text, float intensity = 0.0f, float duration = 5.0f, HookControl.IntFunc[] callbacks = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
        {
            SetSpeakData("Options", "Default", intensity, text, showEffects, hideEffects);
            balloonManager.ShowBalloon("Options", tutorSpeakData["Options"], duration, callbacks);
        }

    }
}
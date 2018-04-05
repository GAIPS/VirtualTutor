using System;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{
    public class BubbleSystemManager : MonoBehaviour
    {
        [SerializeField]
        private BackgroundManager backgroundManager;
        [SerializeField]
        private BalloonManager balloonManager;
        [SerializeField]
        private TextManager textManager;

        private Data data = new Data();

        private void SetData(string emotion = "Neutral", float intensity = 0.0f, Reason reason = Reason.Grades, string[] text = null )
        {
            try
            {
                data.emotion = (Emotion)Enum.Parse(typeof(Emotion), emotion);
            }
            catch
            {
                throw new MissingFieldException("Emotion enum does not contain " + emotion + ".");
            }
            data.intensity = intensity;
            data.reason = reason;
            data.text = text;
        }

        public void UpdateBackground(string tutor, string emotion, float intensity, Reason reason)
        {
            SetData(emotion, intensity, reason);
            backgroundManager.SetBackground(tutor, data);
        }

        public void Speak(string tutor, string emotion, float intensity, string[] text, float duration = 0.0f)
        {
            SetData(emotion, intensity, Reason.None, text);
            balloonManager.ShowBalloon(tutor, data, duration);
        }

        public void HideBalloon(string tutor, float duration = 0.0f)
        {
            balloonManager.HideBalloon(tutor, duration);
        }

        public void UpdateOptions(string[] text, float duration = 0.0f)
        {
            SetData("Neutral", 0.0f, Reason.None, text);
            balloonManager.ShowBalloon("Options", data, duration);
        }
    }
}

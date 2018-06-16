using HookControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{
    public class BalloonManager : MonoBehaviour
    {
        [Serializable]
        public struct Balloon
        {
            public string name;
            public GameObject balloon;
            public bool dontSwitch;
            public bool isPeakTop, isPeakLeft;
        }

        public Balloon[] balloons;
        public Balloon options;

        private Dictionary<string, Control> controllers = new Dictionary<string, Control>();
        private Dictionary<BalloonsHooks, IEnumerator> hideCoroutines = new Dictionary<BalloonsHooks, IEnumerator>();

        private float durationThreshold = 0.3f;

        private void Start()
        {
            foreach (Balloon balloon in balloons)
            {
                controllers.Add(balloon.name, new Control(balloon.balloon));
                balloon.balloon.GetComponentInChildren<BalloonsHooks>().SetPeak(balloon.isPeakTop, balloon.isPeakLeft);
            }

            controllers.Add(options.name, new Control(options.balloon));
        }
        
        public void ReverseTutorsBalloons(string tutor)
        {
            int index = Array.FindIndex(balloons, balloon => balloon.name == tutor);

            if (balloons[index].dontSwitch)
                return;
            
            int element = BubbleSystemUtility.RandomExcludingNumbers(new int[] { index }, balloons.Length);

            Control first = controllers[tutor];
            Control second = controllers[balloons[element].name];
            controllers[tutor] = second;
            controllers[balloons[element].name] = first;

            foreach (Balloon balloon in balloons)
            {
                balloon.balloon.GetComponentInChildren<BalloonsHooks>().SetPeak(balloon.isPeakTop, !balloon.name.Equals(tutor));
            }
        }

        public void HideBalloon(string tutor, float duration, SpeakData data)
        {
            var controller = controllers[tutor];
            var balloonHooks = controller.instance.GetComponentsInChildren<BalloonsHooks>();

            foreach (BalloonsHooks hooks in balloonHooks)
            {
                if (BubbleSystemUtility.CheckCoroutine(ref hideCoroutines, hooks))
                    StopCoroutine(hideCoroutines[hooks]);
                AddCoroutine(hooks, duration, data);
            }
        }

        private void SetBeaks(Emotion emotion, GameObject beakObject, Sprite beak, float intensity, Color color)
        {
            Image image = beakObject.GetComponent<Image>();
            image.sprite = beak;
            image.color = color;

            DefaultData.PositionData positionData = DefaultData.Instance.GetDefaultPositions(emotion, intensity, beakObject.name);
            beakObject.GetComponent<RectTransform>().anchorMin = positionData.anchorMin;
            beakObject.GetComponent<RectTransform>().anchorMax = positionData.anchorMax;
            beakObject.GetComponent<RectTransform>().localRotation = positionData.localRotation;
        }

        private void SetSprites(Emotion emotion, BalloonsHooks hooks, SpriteData spriteData, float intensity, Color color)
        {
            if (hooks)
            {
                Image image = hooks.balloon.GetComponent<Image>();
                image.sprite = spriteData.sprite;
                image.color = color;

                if (!emotion.Equals(Emotion.Default))
                {
                    DefaultData.PositionData positionData = DefaultData.Instance.GetDefaultPositions(emotion, intensity, "balloon");
                    hooks.balloon.GetComponent<RectTransform>().anchorMin = positionData.anchorMin;
                    hooks.balloon.GetComponent<RectTransform>().anchorMax = positionData.anchorMax;
                }

                SetBeaks(emotion, hooks.peakTopLeft, spriteData.beak, intensity, color);
                SetBeaks(emotion, hooks.peakBotLeft, spriteData.beak, intensity, color);
                SetBeaks(emotion, hooks.peakTopRight, spriteData.beak, intensity, color);
                SetBeaks(emotion, hooks.peakBotRight, spriteData.beak, intensity, color);
            }
        }

        private void ResetAllFloats(Animator animator)
        {
            foreach(AnimatorControllerParameter parameter in animator.parameters)
            {
                if(!parameter.name.Equals("Showing"))
                {
                    animator.SetFloat(parameter.name, 0.0f);
                }
            }
        }

        private void SetAnimators(BalloonsHooks hooks, Emotion emotion)
        {
            Animator animator = hooks.GetComponent<Animator>();
            ResetAllFloats(animator);
            animator.SetFloat(emotion.ToString(), 1.0f);
            animator.SetFloat(Emotion.Default.ToString(), 1.0f);
        }

        private void SetAnimators(BalloonsHooks hooks, SpeakData data)
        {
            Animator animator = hooks.GetComponent<Animator>();
            ResetAllFloats(animator);

            float sum = 0;

            foreach (Emotion emotion in data.emotions.Keys)
            {
                if (emotion.Equals(Emotion.Neutral) || emotion.Equals(Emotion.Default))
                    continue;

                sum += data.emotions[emotion];
                animator.SetFloat(emotion.ToString(), data.emotions[emotion]);
            }
            animator.SetFloat(Emotion.Neutral.ToString(), 1.0f - sum);
            animator.SetFloat(Emotion.Default.ToString(), 1.0f);
        }

        private void SetEffects(BalloonsHooks hooks, Dictionary<Effect, AnimationCurve> effects, float intensity, float duration, bool show = true)
        {
            if (hooks.text)
            {
                hooks.text.GetComponent<Effects>().SetEffect(effects, intensity, duration, show);
            }
        }

        private void SetTexts(BalloonsHooks hooks, TextData textData, Color color)
        {
            if (hooks.text)
            {
                hooks.text.font = textData.font;
                hooks.text.fontSize = textData.size;
                hooks.text.color = color;
            }
        }

        private void SetContent(BalloonsHooks hooks, string text)
        {
            hooks.Content = text;
        }

        private void SetCallback(BalloonsHooks hooks, IntFunc callback, int index)
        {
            hooks.onClick = () => {
                callback(index);
            };
        }

        public void Speak(string balloon, SpeakData data)
        {
            ShowBalloon(balloon, data, null, false);
        }

        public void ShowOptions(string balloon, SpeakData data, IntFunc[] callbacks)
        {
            ShowBalloon(balloon, data, callbacks, true);
        }

        private void ShowBalloon(string balloon, SpeakData data, IntFunc[] callbacks, bool options)
        {
            var controller = controllers[balloon];

            if (controller.Show() != ShowResult.FAIL)
            {
                var balloonHooks = controller.instance.GetComponentsInChildren<BalloonsHooks>();
                int i = 0;

                foreach (BalloonsHooks hooks in balloonHooks)
                {
                    if (hooks != null)
                    {
                        float realDuration = DefaultData.Instance.GetBalloonDuration();

                        if (BubbleSystemUtility.CheckCoroutine(ref hideCoroutines, hooks))
                            StopCoroutine(hideCoroutines[hooks]);

                        SetContent(hooks, data.text.Length > i ? data.text[i] : null);
                        
                        KeyValuePair<Emotion, float> emotionPair = BubbleSystemUtility.GetHighestEmotion(data.emotions);
                        float sum = BubbleSystemUtility.GetEmotionsSum(data.emotions);

                        if (options)
                        {
                            realDuration = DefaultData.Instance.GetBlankDuration();
                            if (callbacks != null && i < callbacks.Length)
                                SetCallback(hooks, callbacks[i], i);
                        }
                        else
                        {
                            if (DefaultData.Instance.blendBalloonAnimation)
                                SetAnimators(hooks, data);
                            else
                                SetAnimators(hooks, emotionPair.Key);
                        }

                        SpriteData spriteData = DefaultData.Instance.GetDefaultBalloonData(emotionPair.Key, emotionPair.Value);

                        Color color;
                        ColorUtility.TryParseHtmlString(balloon.Equals("Maria") ? "#E1B24EFF" : "#BD8E5FFF", out color);

                        //if (emotionPair.Value.Equals(0.0f) || emotionPair.Key.Equals(BubbleSystem.Emotion.Default) || emotionPair.Key.Equals(BubbleSystem.Emotion.Neutral))
                        //{
                        //    color = DefaultData.Instance.GetColor(BubbleSystem.Emotion.Neutral);
                        //}
                        //else
                        //{
                        //    color = DefaultData.Instance.mixColors? BubbleSystemUtility.MixColors(data.emotions) : DefaultData.Instance.GetColor(emotionPair.Key);
                        //}

                        SetSprites(emotionPair.Key, hooks, spriteData, emotionPair.Value, color);

                        Color textColor = BubbleSystemUtility.GetTextColor(color);
                        TextData textData = DefaultData.Instance.GetDefaultTextData(emotionPair.Key, emotionPair.Value);
                        SetTexts(hooks, textData, textColor);

                        float textDuration = realDuration - durationThreshold; // so it finishes before hide

                        if (data.showEffects != null)
                            SetEffects(hooks, data.showEffects, sum, textDuration);
                        else
                        {
                            SetEffects(hooks, textData.showEffect, sum, textDuration);
                        }

                        hooks.Show();
                        AddCoroutine(hooks, realDuration, data);
                    }
                    i++;
                }
            }
        }

        // ANIMATIONS ARE NORMALIZED WITH A LENGTH OF 1 (WE CAN GET THE FIRST ONE)
        private float GetClipsDuration(BalloonsHooks hooks)
        {
            return hooks.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
        }

        public void AddCoroutine(BalloonsHooks hooks, float duration, SpeakData data)
        {
            BubbleSystemUtility.AddToDictionary(ref hideCoroutines, hooks, Clean(hooks, duration, data));
            StartCoroutine(hideCoroutines[hooks]);
        }

        IEnumerator Clean(BalloonsHooks hooks, float duration, SpeakData data)
        {
            yield return new WaitForSeconds(duration);

            if (hooks)
            {
                if (hooks.Content != null)
                {
                    hooks.Hide();

                    var animationClips = hooks.GetComponent<Animator>().runtimeAnimatorController.animationClips;
                    float length = GetClipsDuration(hooks) * 2; //times 2, to give it a little more time
                    
                    KeyValuePair<Emotion, float> emotionPair = BubbleSystemUtility.GetHighestEmotion(data.emotions);
                    float sum = BubbleSystemUtility.GetEmotionsSum(data.emotions);

                    if (data.hideEffects != null)
                    {
                        SetEffects(hooks, data.hideEffects, sum, length, false);
                    }
                    else
                    {
                        TextData textData = DefaultData.Instance.GetDefaultTextData(emotionPair.Key, emotionPair.Value);
                        SetEffects(hooks, textData.hideEffect, sum, length, false);
                    }
                }
            }
        }
    }
}
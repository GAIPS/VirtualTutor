using HookControl;
using System;
using System.Collections;
using System.Collections.Generic;
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
            public bool isPeakTop, isPeakLeft;
        }

        public Balloon[] balloons;

        private Dictionary<string, Control> controllers = new Dictionary<string, Control>();
        private Dictionary<string, IEnumerator> hideCoroutines = new Dictionary<string, IEnumerator>();

        private void Start()
        {
            foreach (Balloon balloon in balloons)
            {
                controllers.Add(balloon.name, new Control(balloon.balloon));
                if (!balloon.name.Equals("Options"))
                    balloon.balloon.GetComponentInChildren<BalloonsHooks>().SetPeak(balloon.isPeakTop, balloon.isPeakLeft);
            }
        }

        //QUICK HACK: SHOULD NOT USE TUTOR NAMES
        public void ReverseTutorsBalloons(string tutor)
        {
            if (tutor.Equals("Joao"))
                return;
            else
            {
                Control maria = controllers["Maria"];
                Control joao = controllers["Joao"];
                controllers["Maria"] = joao;
                controllers["Joao"] = maria;

                foreach (Balloon balloon in balloons)
                {
                    if (!balloon.name.Equals("Options"))
                        balloon.balloon.GetComponentInChildren<BalloonsHooks>().SetPeak(balloon.isPeakTop, !balloon.name.Equals(tutor));
                }
            }
        }

        public void HideBalloon(string tutor, float duration, SpeakData data)
        {
            var controller = controllers[tutor];
            try
            {
                var balloonHooks = controller.instance.GetComponentsInChildren<BalloonsHooks>();
                foreach (BalloonsHooks hooks in balloonHooks)
                {
                    CoroutineStopper.Instance.StopCoroutineWithCheck(hideCoroutines[tutor]);
                    //StartCoroutine(Clean(hooks, duration, data));
                    AddCoroutine(tutor, hooks, duration, data);
                }
            }
            catch
            {
                throw new NotSupportedException("Balloon can not disappear, since it does not exist yet.");
            }
        }

        private void SetBeaks(Emotion emotion, GameObject beakObject, Sprite beak, float intensity)
        {
            beakObject.GetComponent<Image>().sprite = beak;
            DefaultData.PositionData positionData = DefaultData.Instance.GetDefaultPositions(emotion, intensity, beakObject.name);
            beakObject.GetComponent<RectTransform>().anchorMin = positionData.anchorMin;
            beakObject.GetComponent<RectTransform>().anchorMax = positionData.anchorMax;
            beakObject.GetComponent<RectTransform>().localRotation = positionData.localRotation;
        }

        private void SetSprites(Emotion emotion, BalloonsHooks hooks, SpriteData spriteData, float intensity)
        {
            if (hooks)
            {
                hooks.balloon.GetComponent<Image>().sprite = spriteData.sprite;

                if (!emotion.Equals(BubbleSystem.Emotion.Default))
                {
                    DefaultData.PositionData positionData = DefaultData.Instance.GetDefaultPositions(emotion, intensity, "balloon");
                    hooks.balloon.GetComponent<RectTransform>().anchorMin = positionData.anchorMin;
                    hooks.balloon.GetComponent<RectTransform>().anchorMax = positionData.anchorMax;
                }

                SetBeaks(emotion, hooks.peakTopLeft, spriteData.beak, intensity);
                SetBeaks(emotion, hooks.peakBotLeft, spriteData.beak, intensity);
                SetBeaks(emotion, hooks.peakTopRight, spriteData.beak, intensity);
                SetBeaks(emotion, hooks.peakBotRight, spriteData.beak, intensity);
            }
        }

        private void SetAnimators(BalloonsHooks hooks, BalloonAnimationData animatorData, float intensity)
        {
            if (hooks)
            {
                Animator anim = hooks.GetComponent<Animator>();
                anim.runtimeAnimatorController = animatorData.animator;
                anim.speed = intensity + 1f;
            }
        }

        private void SetAnimators(BalloonsHooks hooks, DefaultBalloonAnimationData animatorData, float intensity)
        {
            if (hooks)
            {
                Animator anim = hooks.GetComponent<Animator>();
                anim.runtimeAnimatorController = animatorData.animator;
                anim.speed = intensity + 1f;
            }
        }

        private void SetEffects(BalloonsHooks hooks, Dictionary<Effect, AnimationCurve> effects, float intensity, float duration, bool show = true)
        {
            if (hooks.text)
            {
                hooks.text.GetComponent<Effects>().SetEffect(effects, intensity, duration, show);
            }
        }

        private void SetTexts(BalloonsHooks hooks, TextData textData)
        {
            if (hooks.text)
            {
                hooks.text.font = textData.font;
                hooks.text.fontSize = textData.size;
                hooks.text.color = textData.color;
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

        public void ShowBalloon(string balloon, SpeakData data, float duration, IntFunc[] callbacks = null)
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
                        DefaultBalloonAnimationData defaultBalloonAnimationData = DefaultData.Instance.GetNeutralBalloonAnimationData(data.intensity);
                        float realDuration = defaultBalloonAnimationData.duration;
                        SetContent(hooks, data.text.Length > i ? data.text[i] : null);

                        SpriteData spriteData = DefaultData.Instance.GetDefaultBalloonData(data.emotion, data.intensity);
                        TextData textData = DefaultData.Instance.GetDefaultTextData(data.emotion, data.intensity);
                        if (data.emotion.Equals(Emotion.Neutral) || data.emotion.Equals(Emotion.Default))
                        {
                            SetAnimators(hooks, defaultBalloonAnimationData, data.intensity);
                        }
                        else
                        {
                            BalloonAnimationData balloonAnimationData = DefaultData.Instance.GetBalloonAnimationData(data.emotion, data.intensity);
                            realDuration = balloonAnimationData.duration;
                            SetAnimators(hooks, balloonAnimationData, data.intensity);
                        }
                        SetSprites(data.emotion, hooks, spriteData, data.intensity);
                        SetTexts(hooks, textData);
                        if (callbacks != null && i < callbacks.Length)
                            SetCallback(hooks, callbacks[i], i);

                        realDuration = duration > 0 ? duration : realDuration;

                        if (data.showEffects != null)
                            SetEffects(hooks, data.showEffects, data.intensity, realDuration);
                        else
                        {
                            SetEffects(hooks, textData.showEffect, data.intensity, realDuration);
                        }

                        hooks.Show();
                        AddCoroutine(balloon, hooks, realDuration, data);
                    }
                    i++;
                }
            }
        }

        public void AddCoroutine(string balloon, BalloonsHooks hooks, float duration, SpeakData data)
        {
            IEnumerator clean = Clean(hooks, duration, data);
            if (hideCoroutines.ContainsKey(balloon))
                hideCoroutines[balloon] = clean;
            else
                hideCoroutines.Add(balloon, clean);
            StartCoroutine(clean);
        }

        IEnumerator Clean(BalloonsHooks hooks, float duration, SpeakData data)
        {
            yield return new WaitForSeconds(duration);

            if (hooks)
            {
                if (hooks.Content != null)
                {
                    hooks.Hide();

                    var animationClips = (data.emotion.Equals(BubbleSystem.Emotion.Neutral) || data.emotion.Equals(BubbleSystem.Emotion.Default)) ? DefaultData.Instance.GetNeutralBalloonAnimationData(data.intensity).animator.animationClips : DefaultData.Instance.GetBalloonAnimationData(data.emotion, data.intensity).animator.animationClips;
                    float length = 1f;
                    foreach (AnimationClip clip in animationClips)
                    {
                        if (clip.name.Contains("hide"))
                            length = clip.length;
                    }


                    if (data.hideEffects != null)
                    {
                        SetEffects(hooks, data.hideEffects, data.intensity, length, false);
                    }
                    else
                    {
                        TextData textData = DefaultData.Instance.GetDefaultTextData(data.emotion, data.intensity);
                        SetEffects(hooks, textData.hideEffect, data.intensity, length, false);
                    }
                }
            }
        }
    }
}
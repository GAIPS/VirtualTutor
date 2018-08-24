using HookControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace BubbleSystem2
{
    public class Balloon : AbstractBubbleSystemModule, IStopper
    {
        public BalloonSystemData balloonData;
        private List<IEnumerator> coroutines = new List<IEnumerator>();
        private float durationThreshold = 0.3f;
        [HideInInspector]
        public Control control;
        private Dictionary<Emotion, float> emotions = new Dictionary<Emotion, float>();
        private TextEffectData textEffectsData = new TextEffectData();

        private void Start()
        {
            control = new Control(balloonData.prefab);
            SetTails();
        }

        public void SetTails()
        {
            if (balloonData.options) return;
            BalloonsHooks[] balloonHooks = balloonData.prefab.GetComponentsInChildren<BalloonsHooks>();
            foreach (BalloonsHooks hooks in balloonHooks)
                hooks.SetTail(balloonData.isTailTop, balloonData.isTailLeft);
        }

        public void Play(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void Stop(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }

        public void AddCoroutine(IEnumerator coroutine)
        {
            coroutines.Add(coroutine);
            StartCoroutine(coroutine);
        }

        public override void UpdateScene(BubbleSystemData data)
        {
            if (!data.tutor.GetString().Equals(balloonData._name)) return;
            foreach (IEnumerator coroutine in coroutines)
                Stop(coroutine);

            if (data.balloonData.show)
                ShowBalloon(data);
            else
                HideBalloon(data);
        }

        private void SetTextEffects(BalloonsHooks hooks, AnimationCurve curve, float intensity, float duration, bool show)
        {
            textEffectsData.Clear();
            textEffectsData.hooks = hooks;
            textEffectsData.curve = curve;
            textEffectsData.intensity = intensity;
            textEffectsData.duration = duration;
            textEffectsData.show = show;
        }

        public void HideBalloon(BubbleSystemData data)
        {
            if (control.Show() == ShowResult.FAIL) return;
            BalloonsHooks[] balloonHooks = control.instance.GetComponentsInChildren<BalloonsHooks>();

            foreach (BalloonsHooks hooks in balloonHooks)
            {
                AddCoroutine(Clean(hooks, 0.0f, data));
            }
        }

        private void ShowBalloon(BubbleSystemData data)
        {
            if (control.Show() == ShowResult.FAIL) return;
            int i = 0;
            BalloonsHooks[] balloonHooks = control.instance.GetComponentsInChildren<BalloonsHooks>();

            foreach (BalloonsHooks hooks in balloonHooks)
            {
                if (hooks == null) return;

                KeyValuePair<Emotion, float> emotionPair = BubbleSystemUtility.GetHighestEmotion(data.emotions);
                float sum = BubbleSystemUtility.GetEmotionsSum(data.emotions);
                SpriteData spriteData = DefaultData.Instance.GetDefaultBalloonData(emotionPair.Key.Get(), emotionPair.Value);
                Color color = BubbleSystemUtility.GetColor(emotionPair, data.emotions);
                float realDuration = balloonData.options ? DefaultData.Instance.GetOptionsDuration() : DefaultData.Instance.GetBalloonDuration();
                TextData textData;
                Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve> showEffects;
                Color textColor = BubbleSystemUtility.GetTextColor(color);
                float textDuration = realDuration - durationThreshold; // so it finishes before hide
                emotions.Clear();

                balloonData.SetContent(hooks, data.balloonData.text.Count > i ? data.balloonData.text[i] : null);
                balloonData.SetSprites(emotionPair.Key, hooks, spriteData, emotionPair.Value, color, balloonData.options);

                if (i < data.balloonData.callbacks.Count)
                    balloonData.SetCallback(hooks, data.balloonData.callbacks[i], i);
                
                // Do not animate options
                if (!balloonData.options)
                {
                    emotions.Add(emotionPair.Key, emotionPair.Value);
                    if (DefaultData.Instance.blendBalloonAnimation)
                        balloonData.SetAnimators(hooks, data.emotions);
                    else
                        balloonData.SetAnimators(hooks, emotions);

                    textData = DefaultData.Instance.GetDefaultTextData(emotionPair.Key.GetString(), emotionPair.Value);
                    balloonData.SetTexts(hooks, textData, textColor, emotionPair.Key);
                }
                else
                    textData = DefaultData.Instance.GetDefaultTextData(data.tutor.GetString(), emotionPair.Value);
                
                showEffects = data.balloonData.effects.showEffects.Count != 0 ? data.balloonData.effects.showEffects : textData.showEffect;

                foreach (AbstractTextEffect.TextEffectEnum effect in showEffects.Keys)
                {
                    SetTextEffects(hooks, showEffects[effect], sum, textDuration, true);
                    coroutines.Add(balloonData.SetEffects(effect, textEffectsData));
                }
                
                hooks.Show();
                if (realDuration != -1)
                    AddCoroutine(Clean(hooks, realDuration, data));
                i++;
            }
        }

        IEnumerator Clean(BalloonsHooks hooks, float duration, BubbleSystemData data)
        {
            yield return new WaitForSeconds(duration);

            if (hooks)
            {
                if (hooks.Content != null)
                {
                    hooks.Hide();

                    var animationClips = hooks.GetComponent<Animator>().runtimeAnimatorController.animationClips;
                    float length = balloonData.GetClipsDuration(hooks) * 2; //times 2, to give it a little more time
                    KeyValuePair<Emotion, float> emotionPair = BubbleSystemUtility.GetHighestEmotion(data.emotions);
                    float sum = BubbleSystemUtility.GetEmotionsSum(data.emotions);
                    TextData textData;
                    if (balloonData.options)
                        textData = DefaultData.Instance.GetDefaultTextData(data.tutor.GetString(), emotionPair.Value);
                    else
                        textData = DefaultData.Instance.GetDefaultTextData(emotionPair.Key.GetString(), emotionPair.Value);
                    Dictionary<AbstractTextEffect.TextEffectEnum, AnimationCurve> hideEffects = data.balloonData.effects.hideEffects.Count != 0 ? data.balloonData.effects.hideEffects : textData.hideEffect;

                    foreach (AbstractTextEffect.TextEffectEnum effect in hideEffects.Keys)
                    {
                        SetTextEffects(hooks, hideEffects[effect], sum, length, false);
                        coroutines.Add(balloonData.SetEffects(effect, textEffectsData));
                    }
                }
            }
        }
    }
}
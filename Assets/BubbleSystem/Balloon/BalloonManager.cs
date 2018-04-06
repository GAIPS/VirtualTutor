using HookControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{
    public class BalloonManager : ImageManager
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

        [SerializeField]
        private BalloonModifier balloonModifier;
        [SerializeField]
        private TextManager textManager;
        [SerializeField]
        private BalloonAnimationSelector balloonAnimationSelector;

        private void Start()
        {
            foreach(Balloon balloon in balloons)
            {
                controllers.Add(balloon.name, new Control(balloon.balloon));
                balloon.balloon.GetComponent<BalloonsHooks>().SetPeak(balloon.isPeakTop, balloon.isPeakLeft);
            }
        }

        private SpriteData SelectBalloon(Data data)
        {
            return balloonModifier.SelectSprite(data);
        }

        private BalloonAnimationData SelectAnimator(Data data)
        {
            return balloonAnimationSelector.SelectBalloonAnimation(data);
        }

        private TextData SelectText(Data data)
        {
            return textManager.SelectText(data);
        }

        public void HideBalloon(string tutor, float duration)
        {
            var controller = controllers[tutor];
            try
            {
                var hooks = controller.instance.GetComponent<BalloonsHooks>();
                StopCoroutineWithCheck(hideCoroutines[tutor]);
                StartCoroutine(Clean(hooks, duration));
            }
            catch
            {
                throw new NotSupportedException("Balloon can not disappear, since it does not exist yet.");
            }
        }

        private void SetSprite(GameObject hooksTopic, Sprite sprite)
        {
            if(hooksTopic)
                hooksTopic.GetComponentInChildren<Image>().sprite = sprite;
        }

        private void SetSprites(BalloonsHooks hooks, SpriteData spriteData)
        {
            SetSprite(hooks.topicTop, spriteData.sprite);
            SetSprite(hooks.topicLeft, spriteData.sprite);
            SetSprite(hooks.topicRight, spriteData.sprite);
            SetSprite(hooks.topicExtra, spriteData.sprite);
        }

        private void SetAnimator(GameObject hooksTopic, AnimatorOverrideController animator)
        {
            if (hooksTopic)
                hooksTopic.GetComponent<Animator>().runtimeAnimatorController = animator;
        }

        private void SetAnimator(GameObject hooksTopic, RuntimeAnimatorController animator)
        {
            if (hooksTopic)
                hooksTopic.GetComponent<Animator>().runtimeAnimatorController = animator;
        }

        private void SetAnimators(BalloonsHooks hooks, BalloonAnimationData animatorData)
        {
            SetAnimator(hooks.topicTop, animatorData.animator);
            SetAnimator(hooks.topicLeft, animatorData.animator);
            SetAnimator(hooks.topicRight, animatorData.animator);
            SetAnimator(hooks.topicExtra, animatorData.animator);
        }

        private void SetAnimators(BalloonsHooks hooks, DefaultBalloonAnimationData animatorData)
        {
            SetAnimator(hooks.topicTop, animatorData.animator);
            SetAnimator(hooks.topicLeft, animatorData.animator);
            SetAnimator(hooks.topicRight, animatorData.animator);
            SetAnimator(hooks.topicExtra, animatorData.animator);
        }

        private void SetText(Text hooksTopicText, TextData textData)
        {
            if (hooksTopicText)
            {
                hooksTopicText.font = textData.font;
                hooksTopicText.fontSize = (int)textData.size;
                hooksTopicText.color = textData.colorData.color;
            }
        }

        private void SetTexts(BalloonsHooks hooks, TextData textData)
        {
            SetText(hooks.topicTextTop, textData);
            SetText(hooks.topicTextLeft, textData);
            SetText(hooks.topicTextRight, textData);
            SetText(hooks.topicTextExtra, textData);
        }

        private void SetContent(BalloonsHooks hooks, string[] texts)
        {
            int size = texts.Length, i = 0;
            hooks.ContentTop = size > i ? texts[i++] : null;
            hooks.ContentLeft = size > i ? texts[i++] : null;
            hooks.ContentRight = size > i ? texts[i++] : null;
            hooks.ContentExtra = size > i ? texts[i++] : null;
        }
        
        private void SetCallbacks(BalloonsHooks hooks, IntFunc[] callbacks)
        {
            List<VoidFunc> funcs = new List<VoidFunc>();
            for (int i = 0; i < callbacks.Length; i++)
            {
                int index = i;
                funcs.Add(() => {
                    callbacks[index](index);
                });
            }
            int size = callbacks.Length, f = 0;
            hooks.onTop = size > f ? funcs[f++] : (VoidFunc) null;
            hooks.onLeft = size > f ? funcs[f++] : (VoidFunc)null;
            hooks.onRight = size > f ? funcs[f++] : (VoidFunc)null;
            hooks.onExtra = size > f ? funcs[f++] : (VoidFunc)null;
        }

        public void ShowBalloon(string balloon, Data data, float duration, IntFunc[] callbacks = null)
        {
            var controller = controllers[balloon];

            if (controller.Show() != ShowResult.FAIL)
            {
                var hooks = controller.instance.GetComponent<BalloonsHooks>();

                if (hooks != null)
                {
                    float realDuration = DefaultData.Instance.defaultBalloonAnimationData.duration;
                    SetContent(hooks, data.text);

                    try
                    {
                        SpriteData spriteData;
                        TextData textData;
                        if (data.emotion.Equals(Emotion.Neutral))
                        {
                            spriteData = DefaultData.Instance.defaultBalloonData;
                            textData = DefaultData.Instance.defaultTextData;
                            SetAnimators(hooks, DefaultData.Instance.defaultBalloonAnimationData);
                        }
                        else
                        {
                            try
                            {
                                spriteData = SelectBalloon(data);
                            }
                            catch
                            {
                                spriteData = DefaultData.Instance.defaultBalloonData;
                            }
                            try
                            {
                                textData = SelectText(data);
                            }
                            catch
                            {
                                textData = DefaultData.Instance.defaultTextData;
                            }
                            try
                            {
                                var balloonAnimationData = SelectAnimator(data);
                                SetAnimators(hooks, balloonAnimationData);
                                realDuration = balloonAnimationData.duration;
                            }
                            catch
                            {
                                realDuration = DefaultData.Instance.defaultBalloonAnimationData.duration;
                                SetAnimators(hooks, DefaultData.Instance.defaultBalloonAnimationData);
                            }
                        }
                        SetSprites(hooks, spriteData);
                        SetTexts(hooks, textData);
                        SetCallbacks(hooks, callbacks);
                    }
                    catch { }

                    realDuration = duration > 0 ? duration : realDuration;

                    hooks.Show();
                    AddCoroutine(balloon, hooks, realDuration);
                }
            }
        }

        public void AddCoroutine(string balloon, BalloonsHooks hooks, float duration)
        {
            IEnumerator clean = Clean(hooks, duration);
            if (hideCoroutines.ContainsKey(balloon))
                hideCoroutines[balloon] = clean;
            else
                hideCoroutines.Add(balloon, clean);
            StartCoroutine(clean);
        }

        IEnumerator Clean(BalloonsHooks hooks, float duration)
        {
            yield return new WaitForSeconds(duration);
            if (hooks)
            {
                hooks.Hide();
            }
        }
    }
}
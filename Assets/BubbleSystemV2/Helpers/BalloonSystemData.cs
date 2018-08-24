using HookControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace BubbleSystem2
{
    [System.Serializable]
    public class BalloonSystemData
    {
        public string _name = "";
        public GameObject prefab;
        public bool dontSwitch = false;
        public bool isTailTop = false, isTailLeft = false;
        public bool options = false;

        // ANIMATIONS SHOULD BE NORMALIZED (LENGTH OF 1)
        public float GetClipsDuration(BalloonsHooks hooks)
        {
            return 1;
        }

        public void SetTails(Emotion emotion, GameObject tailObject, Sprite tail, float intensity, Color color)
        {
            Image image = tailObject.GetComponent<Image>();
            image.sprite = tail;
            image.color = color;

            DefaultData.PositionData positionData = DefaultData.Instance.GetDefaultPositions(emotion.Get(), intensity, tailObject.name);
            tailObject.GetComponent<RectTransform>().anchorMin = positionData.anchorMin;
            tailObject.GetComponent<RectTransform>().anchorMax = positionData.anchorMax;
            tailObject.GetComponent<RectTransform>().localRotation = positionData.localRotation;
        }

        public void SetSprites(Emotion emotion, BalloonsHooks hooks, SpriteData spriteData, float intensity, Color color, bool options)
        {
            if (!hooks || options) return;
            Image image = hooks.balloon.GetComponent<Image>();
            DefaultData.PositionData positionData = DefaultData.Instance.GetDefaultPositions(emotion.Get(), intensity, "balloon");
            RectTransform rect = hooks.balloon.GetComponent<RectTransform>();

            image.sprite = spriteData.sprite;
            image.color = color;
            rect.anchorMin = positionData.anchorMin;
            rect.anchorMax = positionData.anchorMax;

            SetTails(emotion, hooks.tailTopLeft, spriteData.tail, intensity, color);
            SetTails(emotion, hooks.tailBotLeft, spriteData.tail, intensity, color);
            SetTails(emotion, hooks.tailTopRight, spriteData.tail, intensity, color);
            SetTails(emotion, hooks.tailBotRight, spriteData.tail, intensity, color);
            
        }

        public void ResetAllFloats(Animator animator)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (!parameter.name.Equals("Showing"))
                {
                    animator.SetFloat(parameter.name, 0.0f);
                }
            }
        }

        public void SetAnimators(BalloonsHooks hooks, Dictionary<Emotion, float> emotions)
        {
            Animator animator = hooks.GetComponent<Animator>();
            if (!(animator && animator.isActiveAndEnabled)) return;
            float sum = 0;
            ResetAllFloats(animator);

            foreach (Emotion emotion in emotions.Keys)
            {
                if (emotion.Get().Equals(Emotion.EmotionEnum.Neutral))
                    continue;

                sum += emotions[emotion];
                animator.SetFloat(emotion.GetString(), emotions[emotion]);
            }
            animator.SetFloat(Emotion.EmotionEnum.Neutral.ToString(), 1.0f - sum);
            animator.SetFloat(DefaultData.Instance.Default, 1.0f);
        }

        public IEnumerator SetEffects(AbstractTextEffect.TextEffectEnum effect, TextEffectData data)
        {
            if (!data.hooks.text) throw new MissingComponentException("Hooks text attribute not found.");
            return EffectsManager.Instance.Play(effect, data);
        }

        public void SetTexts(BalloonsHooks hooks, TextData textData, Color color, Emotion emotion)
        {
            if (!hooks.text) return;
            DefaultData.PositionData positionData = DefaultData.Instance.GetTextSizes(emotion.Get());
            RectTransform rect = hooks.text.GetComponent<RectTransform>();

            hooks.text.font = textData.font;
            hooks.text.color = color;
            rect.anchorMin = positionData.anchorMin;
            rect.anchorMax = positionData.anchorMax;
        }

        public void SetContent(BalloonsHooks hooks, string text)
        {
            hooks.Content = text;
        }

        public void SetCallback(BalloonsHooks hooks, IntFunc callback, int index)
        {
            hooks.onClick = () => {
                callback(index);
            };
        }
    }
}
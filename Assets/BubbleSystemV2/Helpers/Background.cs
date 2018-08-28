using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class Background : MonoBehaviour
    {
        public string _name;
        private ImageEffectData bgData = new ImageEffectData();
        private List<IEnumerator> coroutines = new List<IEnumerator>();

        public void UpdateScene(BubbleSystemData data)
        {
            if (!data.tutor.GetString().Equals(_name)) return;
            bgData.Clear();
            foreach (IEnumerator coroutine in coroutines)
                EffectsManager.Instance.Stop(coroutine);
            StopAllCoroutines();
            StartCoroutine(ChangeImage(data));
        }

        private TextureWrapMode GetWrapMode(Reason reason)
        {
            if (reason.Get().Equals(Reason.ReasonEnum.None))
                return TextureWrapMode.Mirror;
            else
                return TextureWrapMode.Repeat;
        }

        private IEnumerator ChangeImage(BubbleSystemData data)
        {
            KeyValuePair<Emotion, float> emotionPair = BubbleSystemUtility.GetHighestEmotion(data.emotions);
            bgData.textureData = DefaultData.Instance.GetDefaultBackgroundDataDictionary(emotionPair.Key.Get(), emotionPair.Value, data.backgroundData.reason);
            float duration = DefaultData.Instance.GetBackgroundDuration();
            bgData.renderer = GetComponent<Renderer>();
            EffectsData<AbstractImageEffect.ImageEffectEnum> effects = data.backgroundData.effects.IsCleared() ? DefaultData.Instance.GetDefaultBackgroundAnimationData(emotionPair.Key.Get(), emotionPair.Value) : data.backgroundData.effects;

            bgData.duration = duration / 2;

            if (!bgData.textureData.name.Equals(bgData.renderer.materials[1].mainTexture.name))
            {
                foreach (AbstractImageEffect.ImageEffectEnum fx in effects.hideEffects.Keys)
                {
                    bgData.curve = effects.hideEffects[fx];
                    coroutines.Add(EffectsManager.Instance.Play(fx, bgData));
                }

                yield return new WaitForSeconds(bgData.duration);

                bgData.renderer.materials[bgData.renderer.materials.Length - 1].mainTexture = bgData.textureData;
                bgData.renderer.materials[bgData.renderer.materials.Length - 1].mainTexture.wrapMode = GetWrapMode(data.backgroundData.reason);
            }
            else
            {
                bgData.duration = duration;
            }

            bgData.colorToLerpTo = BubbleSystemUtility.GetColor(emotionPair, data.emotions);

            if (!bgData.renderer.material.color.Equals(bgData.colorToLerpTo))
            {

                foreach (AbstractImageEffect.ImageEffectEnum fx in effects.colorEffects.Keys)
                {
                    bgData.curve = effects.colorEffects[fx];
                    coroutines.Add(EffectsManager.Instance.Play(fx, bgData));
                }

                yield return new WaitForSeconds(bgData.duration);
            }
        }
    }
}
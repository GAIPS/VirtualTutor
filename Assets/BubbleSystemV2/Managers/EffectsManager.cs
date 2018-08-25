using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BubbleSystem2
{
    public class EffectsManager : Singleton<EffectsManager>, IStopper
    {
        private Dictionary<AbstractImageEffect.ImageEffectEnum, AbstractImageEffect> images = new Dictionary<AbstractImageEffect.ImageEffectEnum, AbstractImageEffect>();
        private Dictionary<AbstractTextEffect.TextEffectEnum, AbstractTextEffect> text = new Dictionary<AbstractTextEffect.TextEffectEnum, AbstractTextEffect>();

        private void Awake()
        {
            List<AbstractImageEffect> image_effects = ReflectiveEnumerator.GetEnumerableOfType<AbstractImageEffect>().ToList();
            for (int i = 0; i < Enum.GetNames(typeof(AbstractImageEffect.ImageEffectEnum)).Length; i++)
                images.Add((AbstractImageEffect.ImageEffectEnum)i, image_effects[i]);

            List<AbstractTextEffect> text_effects = ReflectiveEnumerator.GetEnumerableOfType<AbstractTextEffect>().ToList();
            for (int i = 0; i < Enum.GetNames(typeof(AbstractTextEffect.TextEffectEnum)).Length; i++)
                text.Add((AbstractTextEffect.TextEffectEnum)i, text_effects[i]);
        }
        
        public IEnumerator Play(AbstractImageEffect.ImageEffectEnum effect, ImageEffectData bgData)
        {
            if(!images.ContainsKey(effect)) throw new KeyNotFoundException(effect.ToString() + " not found.");

            IEnumerator coroutine = images[effect].Run(bgData);
            Play(coroutine);
            return coroutine;
        }

        public IEnumerator Play(AbstractTextEffect.TextEffectEnum effect, TextEffectData data)
        {
            if (!text.ContainsKey(effect)) throw new KeyNotFoundException(effect.ToString() + " not found.");

            IEnumerator coroutine = text[effect].Run(data);
            Play(coroutine);
            return coroutine;
        }

        public void Play(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void Stop(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class EffectsManager : Singleton<EffectsManager>, IStopper
    {
        private Dictionary<AbstractImageEffect.ImageEffectEnum, AbstractImageEffect> images = new Dictionary<AbstractImageEffect.ImageEffectEnum, AbstractImageEffect>();
        private Dictionary<AbstractTextEffect.TextEffectEnum, AbstractTextEffect> text = new Dictionary<AbstractTextEffect.TextEffectEnum, AbstractTextEffect>();

        private void Awake()
        {
            images.Add(AbstractImageEffect.ImageEffectEnum.FadeTexture, FadeTextureEffect.Instance);
            images.Add(AbstractImageEffect.ImageEffectEnum.FadeColor, LerpColorEffect.Instance);

            text.Add(AbstractTextEffect.TextEffectEnum.None, NoneEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Appear, AppearEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Blush, BlushEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.BlushCharacters, BlushCharactersEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.DeflectionFont, DeflectionFontEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Erase, EraseEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.FadeIn, FadeInEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.FadeInCharacters, FadeInCharactersEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.FadeOut, FadeOutEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.FadeOutCharacters, FadeOutCharactersEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Flashing, FlashingEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Jitter, JitterEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Palpitations, PalpitationsEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Shake, ShakeEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.ShakeCharacters, ShakeCharactersEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Squash, SquashEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.SquashX, SquashXEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.SquashY, SquashYEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Stretch, StretchEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.StretchX, StretchXEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.StretchY, StretchYEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.SwellingFont, SwellingFontEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Swing, SwingEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.SwingCharacters, SwingCharactersEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Warp, WarpEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.WarpCharacters, WarpCharactersEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.Wave, WaveEffect.Instance);
            text.Add(AbstractTextEffect.TextEffectEnum.WaveCharacters, WaveCharactersEffect.Instance);
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
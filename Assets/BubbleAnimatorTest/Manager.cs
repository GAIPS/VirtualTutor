using BubbleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public VTToModuleBridge manager;
    private float intensity = 0.0f;
    private EmotionEnum emotion = EmotionEnum.Neutral;
    private Tutor[] tutors;
    private Reason reason = Reason.None;
    public float duration = 5.0f;
    private int currentTutor = 0;
    private int mix = 0;

    private void Start()
    {
        tutors = new Tutor[2];
        tutors[0] = new Tutor("Maria");
        tutors[0].Emotion = new Emotion();
        tutors[1] = new Tutor("Joao");
        tutors[1].Emotion = new Emotion();
    }

    void Update () {
        if(Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            intensity += 0.1f;
            intensity = Mathf.Clamp01(intensity);
        }
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            intensity -= 0.1f;
            intensity = Mathf.Clamp01(intensity);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            emotion = EmotionEnum.Happiness;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            emotion = EmotionEnum.Sadness;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            emotion = EmotionEnum.Anger;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            emotion = EmotionEnum.Fear;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            emotion = EmotionEnum.Disgust;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            emotion = EmotionEnum.Surprise;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            emotion = EmotionEnum.Neutral;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            reason = Reason.Grades;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            reason = Reason.None;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            currentTutor = 0;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            currentTutor = 1;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            tutors[currentTutor].Emotion.Name = emotion;
            tutors[currentTutor].Emotion.Intensity = intensity;

            manager.Speak(tutors[currentTutor], new string[] { "hi" }, duration);
            Debug.Log(tutors[currentTutor] + " " + emotion.ToString() + " " + intensity);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            tutors[currentTutor].Emotion.Name = emotion;
            tutors[currentTutor].Emotion.Intensity = intensity;
            manager.UpdateBackground(tutors[currentTutor], duration, reason);
            Debug.Log(tutors[currentTutor] + " " + emotion.ToString() + " " + intensity + " " + reason);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            manager.Handle(new string[] { "SetNextDialogueData", "Maria", "Happiness", "1.0", "duration", "5", "showEffects", "Blush", "fadeCurve", "hideEffects", "FadeOut", "fadeCurve" });
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            manager.Handle(new string[] { "OverrideTextEffects", "Happiness", "0.5", "showEffects", "Shake", "hideEffects", "FadeOut", "fadeCurve" });
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            manager.UpdateOptions(new string[] { "hi", "asd" });
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            mix = (mix + 1) % 2;
            Debug.Log(Convert.ToBoolean(mix));
            manager.Handle(new string[] { "SetMixColors", mix.ToString() });
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            manager.Handle(new string[] { "OverrideBlushColor", "#00FF00FF" });
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            manager.Handle(new string[] { "OverrideEmotionColor", "Happiness", "#00FF00FF" });
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            manager.Handle(new string[] { "AddAnimationCurve", "abc", "0", "0", "1", "1" });
        }
    }
    
}

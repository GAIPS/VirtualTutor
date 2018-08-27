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
    private int currentTutor = 0;
    private int mix = 0;
    private int forceText = 0;
    private int balloonAnimation = 0;

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
            reason = Reason.Importance;
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
            var emotion1 = tutors[currentTutor].Emotion;
            emotion1.Name = emotion;
            emotion1.Intensity = intensity;
            tutors[currentTutor].Emotion = emotion1;

            manager.Speak(tutors[currentTutor], new string[] { "Say nothing, grab a nearby pipe and hit the scientist in the head to knock him out. For all you knew, he was planning to blow up the vault." });
            Debug.Log(tutors[currentTutor] + " " + emotion.ToString() + " " + intensity);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            var emotion1 = tutors[currentTutor].Emotion;
            emotion1.Name = emotion;
            emotion1.Intensity = intensity;
            tutors[currentTutor].Emotion = emotion1;
            //manager.UpdateBackground(tutors[currentTutor], reason);
            Debug.Log(tutors[currentTutor] + " " + emotion.ToString() + " " + intensity + " " + reason);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            manager.Handle(new string[] { "SetNextDialogueData", "Maria", "Happiness", "1.0", "showEffects", "Blush", "linearCurve", "hideEffects", "FadeOut", "linearCurve" });
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            manager.Handle(new string[] { "OverrideTextEffects", "Happiness", "0.5", "showEffects", "FadeIn", "linearCurve", "hideEffects", "FadeOut", "linearCurve" });
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            manager.UpdateOptions(new string[] { "hi", "asd", "sadf", "ge3r" });
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
            manager.Handle(new string[] { "AddAnimationCurve", "abc", "0", "0", "smooth", "1", "1", "1" });
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            forceText = (forceText + 1) % 2;
            Debug.Log(Convert.ToBoolean(forceText));
            manager.Handle(new string[] { "SetForceTextUpdate", forceText.ToString() });
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            balloonAnimation = (balloonAnimation + 1) % 2;
            Debug.Log(Convert.ToBoolean(balloonAnimation));
            manager.Handle(new string[] { "SetBalloonAnimationBlending", balloonAnimation.ToString() });
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            manager.Handle(new string[] { "SetBalloonDuration", 2f.ToString() });
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            manager.Handle(new string[] { "SetBackgroundDuration", 2f.ToString() });
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            manager.Handle(new string[] { "SetOptionsDuration", 2f.ToString() });
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //manager.HideBalloon("Options");
        }
    }
    
}

using BubbleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public BubbleSystemManager manager;
    private float intensity = 0.0f;
    private BubbleSystem.Emotion emotion = BubbleSystem.Emotion.Neutral;
    private string tutor = "Maria";
    private Reason reason = Reason.None;
    public float duration = 5.0f;

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
            emotion = BubbleSystem.Emotion.Happiness;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            emotion = BubbleSystem.Emotion.Sadness;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            emotion = BubbleSystem.Emotion.Anger;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            emotion = BubbleSystem.Emotion.Fear;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            emotion = BubbleSystem.Emotion.Disgust;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            emotion = BubbleSystem.Emotion.Surprise;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            emotion = BubbleSystem.Emotion.Neutral;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            emotion = BubbleSystem.Emotion.Default;
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
            tutor = "Maria";
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            tutor = "Joao";
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            manager.Speak(tutor, emotion.ToString(), intensity, new string[] { "hi" }, duration);
            Debug.Log(tutor + " " + emotion.ToString() + " " +intensity);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            manager.UpdateBackground(tutor, emotion.ToString(), intensity, duration, reason);
            Debug.Log(tutor + " " + emotion.ToString() + " " + intensity + " " + reason);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            manager.Handle(new string[] { "OverrideBackgroundColor", "Happiness", "0.1", "None", "#FF0000FF" });
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            manager.Handle(new string[] { "SetNextDialogueData", "Maria", "Happiness", "1.0", "5", "showEffects", "Shake", "hideEffects", "FadeOut", "fadeCurve" });
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            manager.Handle(new string[] { "OverrideTextEffects", "Happiness", "0.5", "showEffects", "Shake", "hideEffects", "FadeOut", "fadeCurve" });
        }
    }
    
}

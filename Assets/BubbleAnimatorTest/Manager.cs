using BubbleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public BubbleSystemManager manager;
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            manager.Speak("Maria", BubbleSystem.Emotion.Happiness.ToString(), 1.0f, new string[] { "hi" }, 5f);
        }
	}
}

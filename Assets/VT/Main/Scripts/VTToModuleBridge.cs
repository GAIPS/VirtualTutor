using BubbleSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class VTToModuleBridge : MonoBehaviour
{
    public BubbleSystemManager bubbleSystem;
    public AvatarManager avatarManager;

    public void Handle(string[] info)
    {
        string[] parameters = info.Skip(1).ToArray();

        switch (info[0])
        {
            case "SetNextDialogueData":
                bubbleSystem.SetNextDialogueData(parameters);
                break;
            case "UpdateBackground":
                bubbleSystem.UpdateBackground(parameters);
                break;
            case "OverrideBackgroundColor":
                bubbleSystem.OverrideBackgroundColor(parameters);
                break;
            case "OverrideTextEffects":
                bubbleSystem.OverrideTextEffects(parameters);
                break;
            case "SetMixColors":
                bubbleSystem.SetMixColors(parameters);
                break;
            default:
                break;
        }
    }

    /**********************************************************************************************************
                                                 HEAD SYSTEM
    **********************************************************************************************************/

    public void Feel(Tutor tutor, Emotion emotion)
    {

    }

    public void Act(Tutor speaker, MovementWithState movement)
    {
        
    }
    
    /**********************************************************************************************************
                                                 BUBBLE SYSTEM
    **********************************************************************************************************/

    public void UpdateBackground(Tutor tutor, float duration, Reason reason)
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add(tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity);
        bubbleSystem.UpdateBackground(tutor.Name, dict, duration, reason);
    }

    public void Speak(Tutor tutor, string[] text, float duration = 0.0f, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add(tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity);
        bubbleSystem.Speak(tutor.Name, dict, text, duration, showEffects, hideEffects);
    }

    public void HideBalloon(Tutor tutor, float duration = 0.0f)
    {
        bubbleSystem.HideBalloon(tutor.Name, duration);
    }

    public void HideBalloon(string tutor, float duration = 0.0f)
    {
        bubbleSystem.HideBalloon(tutor, duration);
    }

    public void UpdateOptions(string[] text, float duration = 5.0f, HookControl.IntFunc[] callbacks = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
    {
        bubbleSystem.UpdateOptions(text, duration, callbacks, showEffects, hideEffects);
    }
}

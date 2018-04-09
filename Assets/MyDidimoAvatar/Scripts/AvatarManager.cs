using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    [SerializeField]
    private List<AvatarController> Controllers;

    public void Feel(Tutor tutor, Emotion emotion)
    {
        string moodString = getStateString(emotion);
        MoodState moodState = getStateValue<MoodState>(moodString);
        AvatarController controller = getController(tutor);
        if (controller == null)
            return;

        controller.SetMood(moodState);
    }
    public void Express(Tutor tutor, Expression expression)
    {
        string expressionString = getStateString(expression);
        ExpressionState expressionState = getStateValue<ExpressionState>(expressionString);
        AvatarController controller = getController(tutor);
        if (controller == null)
            return;

        controller.ExpressEmotion(expressionState);
    }
    public void Act(Tutor tutor, HeadAction action)
    {
        AvatarController controller = getController(tutor);
        if (controller == null)
            return;

        string actionString = getStateString(action);

        try
        {
            NodState actionState = getStateValue<NodState>(actionString);
            controller.DoNodding(actionState);
        }
        catch (ArgumentException)
        {
            try
            {
                TalkState actionState = getStateValue<TalkState>(actionString);
                controller.DoTalking(actionState);
                StartCoroutine(React(tutor, actionState));
            }
            catch (ArgumentException)
            {
                try
                {
                    GazeState actionState = getStateValue<GazeState>(actionString);
                    controller.DoGazing(actionState);
                }
                catch (ArgumentException ae)
                {
                    Debug.Log(ae.Message);
                }
            }
        }  
    }
    IEnumerator React(Tutor tutor, TalkState actionState)
    {
        float delay = 0.5f;
        yield return new WaitForSeconds(delay);
        if (actionState.Equals(TalkState.TALK))
        {
            AvatarController partnerController = getPartnerController(tutor);
            partnerController.isListening(true);
            partnerController.DoGazing(GazeState.GAZE_ATPARTNER);
        }
        if (actionState.Equals(TalkState.TALK_END))
        {
            AvatarController partnerController = getPartnerController(tutor);
            partnerController.isListening(false);
            partnerController.DoGazing(GazeState.GAZE_BACKFROMPARTNER);
        }
    }

    // public method for receiving tags from other classes
    // Accepts tags in the form of "TutorName_Command_Argument" 
    public void sendRequest(string input)
    {
        //TODO: PROPERLY PROCESS TAGS SO INVALID TAGS, SUCH AS
        //      "maria_gazebat_1.5", ARE NOT ACCEPTED 

        string[] sArray = input.Split('_');

        if (sArray.Length < 3)
        {
            Debug.Log(String.Format("{0} could not be parsed, because it is not a valid tag.", input));
            return;
        }

        CultureInfo culture = CultureInfo.InvariantCulture;

        for (int i = 0; i < sArray.Length; i++)
            sArray[i] = culture.TextInfo.ToTitleCase(sArray[i].ToLower());

        // tag contains a frequency\speed command
        string[] matchStrings = { "frequency", "speed" };
        if (matchStrings.Any(sArray[1].ToLowerInvariant().Contains))
            ChangeAnimationParameters(new Tutor(sArray[0]), sArray[1], float.Parse(sArray[2], culture.NumberFormat));
        
        //TODO: PROCESS THE REMAINING TAGS
    }

    private void ChangeAnimationParameters(Tutor tutor, string parameter, float value)
    {
        AvatarController controller = getController(tutor);
        if (controller == null)
            return;

        //TODO: PROPERLY PROCESS TAGS SO INVALID TAGS, SUCH AS
        //      "maria_gazebat_1.5", ARE NOT ACCEPTED 

        CultureInfo culture = CultureInfo.InvariantCulture;
        if (culture.CompareInfo.IndexOf(parameter, "speed", CompareOptions.IgnoreCase) >= 0)
            controller.setAnimationSpeed(parameter, value);
        if (culture.CompareInfo.IndexOf(parameter, "frequency", CompareOptions.IgnoreCase) >= 0)
            controller.setAnimationFrequency(parameter, value);
    }

    private AvatarController getController(Tutor tutor)
    {
        foreach (var controller in Controllers)
        {
            if (tutor.Name.Equals(controller.name))
                return controller;
        }
        return null;
    }
    private AvatarController getPartnerController(Tutor tutor)
    {
        foreach (var controller in Controllers)
        {
            if (!tutor.Name.Equals(controller.name))
                return controller;
        }
        return null;
    }

    private string getStateString(IState state) {
        string stateString;

        if (String.IsNullOrEmpty(state.Param1))
            stateString = state.Name.ToUpperInvariant();
        else
            stateString = string.Concat(state.Name.ToUpperInvariant(), "_", state.Param1.Replace(" ", "").ToUpperInvariant());

        return stateString;
    }
    private string getStateString(Emotion emotion)
    {
        string emotionString, intensityString;

        if (emotion.Intensity < 0.0f)
            intensityString = "";
        else
            intensityString = emotion.Intensity < 0.5f ? "_LOW" : "_HIGH";

        switch (emotion.Name)
        {
            case EmotionEnum.Happiness:
                emotionString = "Happy";
                break;
            case EmotionEnum.Sadness:
                emotionString = "Sad";
                break;
            default:
                emotionString = emotion.Name.ToString();
                break;      
        }

        return string.Concat(emotionString.ToUpperInvariant(), intensityString); ;
    }

    private T getStateValue<T>(string stateString)
    {
        try
        {
            T value = (T)Enum.Parse(typeof(T), stateString);
            if (!Enum.IsDefined(typeof(T), value))
            {
                Debug.Log(String.Format("{0} is not an underlying value of the {1} enumeration.", stateString, typeof(T)));
                return default(T);
            }
            else
            {
                //    Debug.Log(String.Format("Converted '{0}' to {1}.", stateString, value.ToString()));
                return value;
            }

        }
        catch (ArgumentException)
        {
            throw new ArgumentException(String.Format("'{0}' is not a member of the {1} enumeration.", stateString, typeof(T)));
        }  
    }
}
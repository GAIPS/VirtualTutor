using System;
using System.Collections.Generic;
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
        string actionString = getStateString(action);
        ActionState actionState = getStateValue<ActionState>(actionString);
        AvatarController controller = getController(tutor);
        if (controller == null)
            return;

        controller.PerformAction(actionState);
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
    private string getStateString(IState state) {
        string stateString;

        if (String.IsNullOrEmpty(state.Param1))
            stateString = state.Name.ToUpperInvariant();
        else
            stateString = string.Concat(state.Name.ToUpperInvariant(), "_", state.Param1.Replace(" ", "").ToUpperInvariant());

        return stateString;
    }
    private T getStateValue<T>(string stateString)
    {
        try
        {
            T value = (T)Enum.Parse(typeof(T), stateString);
            if (!Enum.IsDefined(typeof(T), value))
                Debug.Log(String.Format("{0} is not an underlying value of the {1} enumeration.", stateString, typeof(T))); 
            else
                //    Debug.Log(String.Format("Converted '{0}' to {1}.", stateString, value.ToString()));
                return value;
        }
        catch (ArgumentException)
        {
            Debug.Log(String.Format("'{0}' is not a member of the {1} enumeration.", stateString, typeof(T)));
        }
        return default(T);

    }
}
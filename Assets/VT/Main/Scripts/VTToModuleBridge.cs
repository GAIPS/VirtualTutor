using BubbleSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class VTToModuleBridge : MonoBehaviour
{
    public BubbleSystemManager bubbleSystem;
    public AvatarManager avatarManager;

    public void Handle(string[] info)
    {
        string[] parameters = info.Skip(1).ToArray();

        //Parse the "[0]" field of the command
        object parsedEnum;
        if (EnumUtils.TryParse(typeof(ActionGroup), info[0], out parsedEnum))
        {
            switch ((ActionGroup)parsedEnum)
            {
                case ActionGroup.FEEL:
                    Feel(parameters);
                    break;

                case ActionGroup.EXPRESS:
                    Express(parameters);
                    break;

                case ActionGroup.GAZEAT:
                    GazeAt(parameters);
                    break;

                case ActionGroup.GAZEBACK:
                    GazeBack(parameters);
                    break;

                case ActionGroup.MOVEEYES:
                    MoveEyes(parameters);
                    break;

                case ActionGroup.NOD:
                    Nod(parameters);
                    break;

                case ActionGroup.TALK:
                    Talk(parameters);
                    break;

                default:
                    break;
            }
        }
        else
        {
            switch (info[0])
            {
                //BubbleSystem Commands
                case "SetNextDialogueData":
                    bubbleSystem.SetNextDialogueData(parameters);
                    break;

                case "UpdateBackground":
                    bubbleSystem.UpdateBackground(parameters);
                    break;

                case "OverrideTextEffects":
                    bubbleSystem.OverrideTextEffects(parameters);
                    break;

                case "SetMixColors":
                    bubbleSystem.SetMixColors(parameters);
                    break;

                case "OverrideBlushColor":
                    bubbleSystem.OverrideBlushColor(parameters);
                    break;

                case "OverrideEmotionColor":
                    bubbleSystem.OverrideEmotionColor(parameters);
                    break;

                case "AddAnimationCurve":
                    bubbleSystem.AddAnimationCurve(parameters);
                    break;

                default:
                    break;
            }
        }
    }

    /**********************************************************************************************************
                                                 HEAD SYSTEM
    **********************************************************************************************************/

    // Main Parsers and Invokers
    private void Feel(string[] arguments)
    {
        Tutor tutor;
        Emotion emotion;

        if(parseTutorName(arguments[0], out tutor) && parseEmotion(arguments[1], arguments[2], out emotion))
        {
            tutor.Emotion = emotion;
            Feel(tutor);
        }
    }
    private void Express(string[] arguments)
    {
        Tutor tutor;
        Emotion emotion;

        if (parseTutorName(arguments[0], out tutor) && parseEmotion(arguments[1], arguments[2], out emotion))
        {
            tutor.Emotion = emotion;
            Express(tutor);
        }
    }
    private void Talk(string[] arguments)
    {
        Tutor tutor;
        MovementWithProperty movementWithProperty;
        MovementWithState movementWithState;

        if (parseTutorName(arguments[0], out tutor))
        {
            if (arguments.Length == 3 && parseProperty(arguments[1], arguments[2], out movementWithProperty))
            {
                movementWithProperty.Name = MovementEnum.Talk;
                setParameter(tutor, movementWithProperty);
            }
            if (arguments.Length == 2 && parseState(arguments[1], out movementWithState))
            {
                movementWithState.Name = MovementEnum.Talk;
                Act(tutor, movementWithState);
            }
        }       
    }
    private void Nod(string[] arguments)
    {
        Tutor tutor = new Tutor(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(arguments[0].ToLower()));

        //Parse the action type field of the command
        object action;

        if (arguments.Length == 3 && EnumUtils.TryParse(typeof(ArgumentType3), arguments[1], out action))
        { // this is a parameter set command
            object property;
            EnumUtils.TryParse(typeof(PropertyEnum), arguments[1], out property);
            float parsedFloat = 0.0f;
            if (float.TryParse(arguments[2], out parsedFloat))
                setParameter(tutor, new MovementWithProperty(MovementEnum.Nod, (PropertyEnum)property, parsedFloat));
            else
            {
                Debug.Log(String.Format("{0} could not be parsed as a float.", arguments[2]));
                return;
            }
        }
        else if (arguments.Length == 2 && EnumUtils.TryParse(typeof(ArgumentType2), arguments[1], out action))
        { // this is an animation command
            if ((ArgumentType2)action == ArgumentType2.START)
                Act(tutor, new MovementWithState(MovementEnum.Nod, StateEnum.Start));
            else
                Act(tutor, new MovementWithState(MovementEnum.Nod, StateEnum.End));
        }
        else
            Debug.Log(String.Format("[{0}] are not valid arguments for this command", string.Join(", ", arguments)));
    }
    private void GazeAt(string[] arguments)
    {
        Tutor tutor = new Tutor(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(arguments[0].ToLower()));

        //Parse the action type field of the command
        object action;

        if (arguments.Length == 3 && EnumUtils.TryParse(typeof(ArgumentType3), arguments[1], out action))
        { // this is a parameter set command
            object property;
            EnumUtils.TryParse(typeof(PropertyEnum), arguments[1], out property);
            float parsedFloat = 0.0f;
            if (float.TryParse(arguments[2], out parsedFloat))
                setParameter(tutor, new MovementWithProperty(MovementEnum.Gazeat, (PropertyEnum)property, parsedFloat));
            else
            {
                Debug.Log(String.Format("{0} could not be parsed as a float.", arguments[2]));
                return;
            }
        }
        else if (arguments.Length == 2 && EnumUtils.TryParse(typeof(ArgumentType1), arguments[1], out action))
        { // this is an animation command
            if ((ArgumentType1)action == ArgumentType1.USER)
                Act(tutor, new MovementWithTarget(MovementEnum.Gazeat, TargetEnum.User));
            else
                Act(tutor, new MovementWithTarget(MovementEnum.Gazeat, TargetEnum.Partner));
        }
        else
            Debug.Log(String.Format("[{0}] are not valid arguments for this command", string.Join(", ", arguments)));
    }
    private void GazeBack(string[] arguments)
    {
        Tutor tutor = new Tutor(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(arguments[0].ToLower()));

        //Parse the action type field of the command
        object action;

        if (arguments.Length == 3 && EnumUtils.TryParse(typeof(ArgumentType3), arguments[1], out action))
        { // this is a parameter set command
            object property;
            EnumUtils.TryParse(typeof(PropertyEnum), arguments[1], out property);
            float parsedFloat = 0.0f;
            if (float.TryParse(arguments[2], out parsedFloat))
                setParameter(tutor, new MovementWithProperty(MovementEnum.Gazeback, (PropertyEnum)property, parsedFloat));
            else
            {
                Debug.Log(String.Format("{0} could not be parsed as a float.", arguments[2]));
                return;
            }
        }
        else if (arguments.Length == 2 && EnumUtils.TryParse(typeof(ArgumentType1), arguments[1], out action))
        { // this is an animation command
            if ((ArgumentType1)action == ArgumentType1.USER)
                Act(tutor, new MovementWithTarget(MovementEnum.Gazeback, TargetEnum.User));
            else
                Act(tutor, new MovementWithTarget(MovementEnum.Gazeback, TargetEnum.Partner));
        }
        else
            Debug.Log(String.Format("[{0}] are not valid arguments for this command", string.Join(", ", arguments)));
    }
    private void MoveEyes(string[] arguments)
    {
        throw new NotImplementedException();
    } //TODO

    public void Feel(Tutor tutor)
    {
        string moodString = getEmotionName(tutor.Emotion);
        EmotionalState emotionalState = getStateType<EmotionalState>(moodString);
        avatarManager.Feel(tutor.Name, emotionalState, tutor.Emotion.Intensity);
    }
    public void Express(Tutor tutor)
    {
        string expressionString = getEmotionName(tutor.Emotion);
        EmotionalState expressionState = getStateType<EmotionalState>(expressionString);
        avatarManager.Express(tutor.Name, expressionState, tutor.Emotion.Intensity);
    }
    public void Act(Tutor tutor, IMovement movement)
    {
        string s1 = getFirstMovementString(movement), s2;

        if (movement is MovementWithState)
        {
            s2 = getSecondMovementString((MovementWithState)movement);
            s1 = string.Concat(s1, "_", s2);
        }
        else if (movement is MovementWithTarget)
        {
            s2 = getSecondMovementString((MovementWithTarget)movement);
            s1 = string.Concat(s1, "_", s2);
        }
        else
        {
            Debug.Log(String.Format("\"{0}\" is not a valid action.", s1));
            return;
        }

        try // NOD
        {
            NodState actionState = getStateType<NodState>(s1);
            avatarManager.Nod(tutor.Name, actionState);
        }
        catch (ArgumentException)
        {
            try // GAZE
            {
                GazeState actionState = getStateType<GazeState>(s1);
                avatarManager.Gaze(tutor.Name, actionState);
            }
            catch (ArgumentException)
            {
                try // TALK
                {
                    TalkState actionState = getStateType<TalkState>(s1);
                    avatarManager.Talk(tutor.Name, actionState);
                }
                catch (ArgumentException ae)
                {
                    Debug.Log(ae.Message);
                }
            }
        }
    }
    public void setParameter(Tutor tutor, MovementWithProperty movement)
    {
        object paramEnum;
        string parameterString = string.Concat(getFirstMovementString(movement), "_", getSecondMovementString(movement));
        if (EnumUtils.TryParse(typeof(AnimatorParams), parameterString, out paramEnum))
            avatarManager.setParameter(tutor.Name, (AnimatorParams)paramEnum, movement.Value);
        if (EnumUtils.TryParse(typeof(ControllerParams), parameterString, out paramEnum))
            avatarManager.setParameter(tutor.Name, (ControllerParams)paramEnum, movement.Value);
    }

    // Auxiliary Methods
    private string getSecondMovementString(MovementWithProperty movement)
    {
        return movement.Property.ToString().ToUpperInvariant();
    }
    private string getSecondMovementString(MovementWithState movement)
    {
        return movement.State.ToString().ToUpperInvariant();
    }
    private string getSecondMovementString(MovementWithTarget movement)
    {
        return movement.Target.ToString().ToUpperInvariant();
    }
    private string getFirstMovementString(IMovement movement)
    {
        return movement.Name.ToString().ToUpperInvariant();
    }
    private string getEmotionName(Emotion emotion)
    {
        return emotion.Name.ToString().ToUpperInvariant();
    }
    // Outdated, similar to EnumUtils.TryParse()
    private static T getStateType<T>(string stateString)
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

    // Aux Methods
    private bool parseTutorName(string input, out Tutor tutor)
    {
        tutor = new Tutor(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower()));
        return true;
    }
    private bool parseEmotion(string emotion, string intensity, out Emotion parsedEmotion)
    {
        object parsedEnum;
        float parsedFloat;

        bool successfulEnumParse = EnumUtils.TryParse(typeof(EmotionEnum), emotion, out parsedEnum);
        bool successfulFloatParse = float.TryParse(intensity, out parsedFloat);

        if (successfulEnumParse && successfulFloatParse)
        {
            parsedEmotion = new Emotion((EmotionEnum)parsedEnum, parsedFloat);
            return true;
        }
        else
        {
            if (!successfulEnumParse)
                Debug.Log(String.Format("{0} is not a reconizable emotion.", emotion));
            else
                Debug.Log(String.Format("{0} could not be parsed as a float.", intensity));
            parsedEmotion = null;
            return false;
        }
    }
    private bool parseProperty(string type, string intensity, out MovementWithProperty parsedProperty)
    {
        object parsedEnum;
        float parsedFloat;

        bool successfulEnumParse = EnumUtils.TryParse(typeof(PropertyEnum), type, out parsedEnum);
        bool successfulFloatParse = float.TryParse(intensity, out parsedFloat);

        if (successfulEnumParse && successfulFloatParse)
        {
            parsedProperty = new MovementWithProperty((PropertyEnum)parsedEnum, parsedFloat);
            return true;
        }
        else
        {
            if (!successfulEnumParse)
                Debug.Log(String.Format("{0} is not a reconizable movement property.", type));
            else
                Debug.Log(String.Format("{0} could not be parsed as a float.", intensity));
            parsedProperty = null;
            return false;
        }
    }
    private bool parseState(string state, out MovementWithState parsedState)
    {
        object parsedEnum;

        if (EnumUtils.TryParse(typeof(StateEnum), state, out parsedEnum))
        {
            parsedState = new MovementWithState((StateEnum)parsedEnum);
            return true;
        }
        else
        {
            Debug.Log(String.Format("{0} is not a reconizable movement state.", state));
            parsedState = null;
            return false;
        }
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
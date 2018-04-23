using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

// Main interface class between the VT application and the animation module. 
// Handles the propagation of any commands sent by relevant VT modules to the avatar controllers 
public class AvatarManager : MonoBehaviour
{
    [SerializeField]
    private List<AvatarController> Controllers;

    // Methods to directly invoke controller actions via VT base classes 
    public void Feel(Tutor tutor)
    {
        string moodString = getStateString(tutor.Emotion);
        EmotionalState emotionalState = getStateType<EmotionalState>(moodString);
        AvatarController controller = getController(tutor);
        if (controller == null)
            return;

        controller.SetMood(emotionalState, tutor.Emotion.Intensity);
    }
    public void Express(Tutor tutor)
    {
        string expressionString = getStateString(tutor.Emotion);
        EmotionalState expressionState = getStateType<EmotionalState>(expressionString);
        AvatarController controller = getController(tutor);
        if (controller == null)
            return;

        controller.ExpressEmotion(expressionState, tutor.Emotion.Intensity);
    }
    public void Act(Tutor tutor, Movement action)
    {
        AvatarController controller = getController(tutor);
        if (controller == null)
            return;

        string actionString = string.Concat(getStateString(action),"_",getStateString(action.State));

        try
        {
            NodState actionState = getStateType<NodState>(actionString);
            controller.DoNodding(actionState);
        }
        catch (ArgumentException)
        {
            try
            {
                TalkState actionState = getStateType<TalkState>(actionString);
                controller.DoTalking(actionState);
                StartCoroutine(React(tutor, actionState));
            }
            catch (ArgumentException ae)
            {
                Debug.Log(ae.Message);
            }
        }  
    }
    public void Gaze(Tutor tutor1, Movement movement, Tutor tutor2)
    {
        AvatarController controller = getController(tutor1);
        if (controller == null)
            return;
   
        string actionString = string.Concat(getStateString(movement), "_PARTNER");

        try
        {
            GazeState actionState = getStateType<GazeState>(actionString);
            controller.DoGazing(actionState);
        }
        catch (ArgumentException ae)
        {
            Debug.Log(ae.Message);
        }
    }
    public void Gaze(Tutor tutor, Movement movement, User user)
    {
        AvatarController controller = getController(tutor);
        if (controller == null)
            return;

        string actionString = string.Concat(getStateString(movement), "_USER");

        try
        {
            GazeState actionState = getStateType<GazeState>(actionString);
            controller.DoGazing(actionState);
        }
        catch (ArgumentException ae)
        {
            Debug.Log(ae.Message);
        }
    }
    public int setParameter(Tutor tutor, Movement movement)
    {
        AvatarController controller = getController(tutor);
        if (controller == null)
            return -1;
        object paramEnum;
        string parameterString = string.Concat(getStateString(movement), "_", getStateString(movement.Property));
        if (EnumUtils.TryParse(typeof(AnimatorParams), parameterString, out paramEnum))
            controller.setParameter((AnimatorParams)paramEnum, movement.Property.Value);
        else if (EnumUtils.TryParse(typeof(ControllerParams), parameterString, out paramEnum))
            controller.setParameter((ControllerParams)paramEnum, movement.Property.Value);
        else
            return -1;
        return 0;
    }

    // Method to receive tag commands from the VT dialog module. 
    public void sendCommand(string[] input)
    {
        //Parse the "[0]" field of the command
        object parsedEnum;

        if (EnumUtils.TryParse(typeof(ActionGroup), input[0], out parsedEnum))
        {
            //Debug.Log(String.Format("{0} was parsed as a {1} command", input[0], (ActionGroup)parsedEnum));
            switch ((ActionGroup)parsedEnum)
            {
                case ActionGroup.EXPRESS:
                    Express(input.Skip(1).ToArray());
                    break;
                case ActionGroup.FEEL:
                    Feel(input.Skip(1).ToArray());
                    break;
                case ActionGroup.GAZEAT:
                    GazeAt(input.Skip(1).ToArray());
                    break;
                case ActionGroup.GAZEBACK:
                    GazeBack(input.Skip(1).ToArray());
                    break;
                case ActionGroup.MOVEEYES:
                    MoveEyes(input.Skip(1).ToArray());
                    break;
                case ActionGroup.NOD:
                    Nod(input.Skip(1).ToArray());
                    break;
                case ActionGroup.TALK:
                    Talk(input.Skip(1).ToArray());
                    break;
                default:
                    break;
            }
        }
        else
            Debug.Log(String.Format("{0} could not be parsed.", input[0]));
    }

    // Tag command auxiliary parsers
    private void Feel(string[] parameters)
    {
        Tutor tutor = new Tutor();
        tutor.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parameters[0].ToLower());

        //Parse the emotion field of the command
        object emotionEnum;
        if (EnumUtils.TryParse(typeof(EmotionEnum), parameters[1], out emotionEnum))
        {
            //Debug.Log(String.Format("{0} was parsed as a {1} emotion", parameters[1], (EmotionEnum)emotionEnum)); 
            float parsedFloat;
            tutor.Emotion = new Emotion((EmotionEnum)emotionEnum);
            if (float.TryParse(parameters[2], out parsedFloat))
                tutor.Emotion.Intensity = parsedFloat;
            else
            {
                Debug.Log(String.Format("{0} could not be parsed as a float.", parameters[2]));
                return;
            }
            Feel(tutor);
        }
        else
            Debug.Log(String.Format("{0} is not a reconizable emotion.", parameters[1]));
    }
    private void Express(string[] parameters)
    {
        Tutor tutor = new Tutor();
        tutor.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parameters[0].ToLower());

        //Parse the emotion field of the command
        object emotionEnum;
        if (EnumUtils.TryParse(typeof(EmotionEnum), parameters[1], out emotionEnum))
        {
            //Debug.Log(String.Format("{0} was parsed as a {1} emotion", parameters[1], (EmotionEnum)emotionEnum)); 
            float parsedFloat;
            tutor.Emotion = new Emotion((EmotionEnum)emotionEnum);
            if (float.TryParse(parameters[2], out parsedFloat))
                tutor.Emotion.Intensity = parsedFloat;
            else
            {
                Debug.Log(String.Format("{0} could not be parsed as a float.", parameters[2]));
                return;
            }
            Express(tutor);
        }
        else
            Debug.Log(String.Format("{0} is not a reconizable emotion.", parameters[1]));
    }
    private void Talk(string[] arguments)
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
                setParameter(tutor, new Movement(MovementEnum.Talk, new Property((PropertyEnum)property, parsedFloat)));
            else
            {
                Debug.Log(String.Format("{0} could not be parsed as a float.", arguments[2]));
                return;
            }
        }
        else if (arguments.Length == 2 && EnumUtils.TryParse(typeof(ArgumentType2), arguments[1], out action))
        { // this is an animation command
            if ((ArgumentType2)action == ArgumentType2.START)
                Act(tutor, new Movement(MovementEnum.Talk, new State(StateEnum.Start)));
            else
                Act(tutor, new Movement(MovementEnum.Talk, new State(StateEnum.End)));
        }
        else
            Debug.Log(String.Format("[{0}] are not valid arguments for this command", string.Join(", ", arguments)));
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
                setParameter(tutor, new Movement(MovementEnum.Nod, new Property((PropertyEnum)property, parsedFloat)));
            else
            {
                Debug.Log(String.Format("{0} could not be parsed as a float.", arguments[2]));
                return;
            }
        }
        else if (arguments.Length == 2 && EnumUtils.TryParse(typeof(ArgumentType2), arguments[1], out action))
        { // this is an animation command
            if ((ArgumentType2)action == ArgumentType2.START)
                Act(tutor, new Movement(MovementEnum.Nod, new State(StateEnum.Start)));
            else
                Act(tutor, new Movement(MovementEnum.Nod, new State(StateEnum.End)));
        }
        else
            Debug.Log(String.Format("[{0}] are not valid arguments for this command", string.Join(", ", arguments)));
    }
    private void MoveEyes(string[] v)
    {
        throw new NotImplementedException();
    } //TODO
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
                setParameter(tutor, new Movement(MovementEnum.Gazeback, new Property((PropertyEnum)property, parsedFloat)));
            else
            {
                Debug.Log(String.Format("{0} could not be parsed as a float.", arguments[2]));
                return;
            }
        }
        else if (arguments.Length == 2 && EnumUtils.TryParse(typeof(ArgumentType1), arguments[1], out action))
        { // this is an animation command
            if ((ArgumentType1)action == ArgumentType1.USER)
                Gaze(tutor, new Movement(MovementEnum.Gazeback), new User());
            else
            {
                Tutor partner = new Tutor(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(arguments[1].ToLower()));
                Gaze(tutor, new Movement(MovementEnum.Gazeback), partner);
            }
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
                setParameter(tutor, new Movement(MovementEnum.Gazeat, new Property((PropertyEnum)property, parsedFloat)));
            else
            {
                Debug.Log(String.Format("{0} could not be parsed as a float.", arguments[2]));
                return;
            }
        }
        else if (arguments.Length == 2 && EnumUtils.TryParse(typeof(ArgumentType1), arguments[1], out action))
        { // this is an animation command
            if ((ArgumentType1)action == ArgumentType1.USER)
                Gaze(tutor, new Movement(MovementEnum.Gazeat), new User());
            else
            {
                Tutor partner = new Tutor(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(arguments[1].ToLower()));
                Gaze(tutor, new Movement(MovementEnum.Gazeat), partner);
            }
        }
        else
            Debug.Log(String.Format("[{0}] are not valid arguments for this command", string.Join(", ", arguments)));
    }

    //Reaction method for talk actions
    IEnumerator React(Tutor tutor, TalkState actionState)
    {
        float delay = 0.5f;
        yield return new WaitForSeconds(delay);
        if (actionState.Equals(TalkState.TALK_START))
        {
            AvatarController partnerController = getPartnerController(tutor);
            partnerController.isListening(true);
            partnerController.DoGazing(GazeState.GAZEAT_PARTNER);
        }
        if (actionState.Equals(TalkState.TALK_END))
        {
            AvatarController partnerController = getPartnerController(tutor);
            partnerController.isListening(false);
            partnerController.DoGazing(GazeState.GAZEBACK_PARTNER);
        }
    }

    public AvatarController getController(Tutor tutor)
    {
        foreach (var controller in Controllers)
        {
            if (tutor.Name.Equals(controller.name))
                return controller;
        }
        return null;
    }
    public AvatarController getPartnerController(Tutor tutor)
    {
        foreach (var controller in Controllers)
        {
            if (!tutor.Name.Equals(controller.name))
                return controller;
        }
        return null;
    }

    private string getStateString(State state)
    {
        return state.Name.ToString().ToUpperInvariant();
    }
    private string getStateString(Movement movement)
    {
        return movement.Name.ToString().ToUpperInvariant();
    }
    private string getStateString(Emotion emotion)
    {
        return emotion.Name.ToString().ToUpperInvariant();
    }
    private string getStateString(Property property)
    {
        return property.Name.ToString().ToUpperInvariant();
    }

    //Outdated, similar to EnumUtils.TryParse()
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
}
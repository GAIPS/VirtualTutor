using BubbleSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class VTToModuleBridge : MonoBehaviour
{
    public BubbleSystem.BubbleSystemManager bubbleSystem;
    public AvatarManager avatarManager;
    private Dictionary<string, BubbleSystemData> bsData = new Dictionary<string, BubbleSystemData>(); 

    public List<Tutor> Tutors;

    private void Start()
    {
        foreach (BubbleSystem.Tutor.TutorEnum t in (BubbleSystem.Tutor.TutorEnum[])Enum.GetValues(typeof(BubbleSystem.Tutor.TutorEnum)))
        {
            bsData.Add(t.ToString(), new BubbleSystemData());
        }
    }

    public void Handle(string[] info)
    {
        string[] parameters = info.Skip(1).ToArray();

        //Parse the "[0]" field of the command
        object parsedEnum;
        if (EnumUtils.TryParse(typeof(ActionGroup), info[0], out parsedEnum))
        {
            //HeadSystem Commands
            switch ((ActionGroup)parsedEnum)
            {
                //case ActionGroup.AVATARFEEL:
                //    AvatarFeel(parameters);
                //    break;

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

                case "UpdateBackground":
                    UpdateBackground(parameters);
                    break;


                //  SHARED COMMANDS
                case "Feel":
                    Feel(parameters);
                    break;

                default:
                    BubbleSystemCommands.Instance.Run(info);
                    break;
            }
        }
    }

    /**********************************************************************************************************
                                                SHARED COMMANDS
    **********************************************************************************************************/

    // <<Feel Tutor Emotion Intensity Reason>>
    public void Feel(string[] parameters)
    {
        Tutor tutor;
        Emotion emotion;
        Reason.ReasonEnum reason = Reason.ReasonEnum.None;
        object parsedReason;

        if (parseTutorName(parameters[0], out tutor) && parseEmotion(parameters[1], parameters[2], out emotion))
        {
            tutor.Emotion = emotion;
        }

        if (EnumUtils.TryParse(typeof(Reason.ReasonEnum), parameters[3], out parsedReason))
            reason = (Reason.ReasonEnum)parsedReason;

        Feel(tutor, reason);
    }

    /**********************************************************************************************************
                                                SHARED CALLS
    **********************************************************************************************************/

    public void Feel(Tutor who, Reason.ReasonEnum why)
    {
        Feel(who);
        UpdateBackground(who, why);
    }

    public void StartSpeaking(Tutor who, string what)
    {
        Speak(who, new string[] { what });
        Act(who, new MovementWithState(MovementEnum.Talk, StateEnum.Start));
    }

    public void StopSpeaking(Tutor who)
    {
        Act(who, new MovementWithState(MovementEnum.Talk, StateEnum.End));
    }

    /**********************************************************************************************************
                                                 HEAD SYSTEM
    **********************************************************************************************************/

    // Main Parsers and Invokers
    private void AvatarFeel(string[] arguments)
    {
        Tutor tutor;
        Emotion emotion;

        if (parseTutorName(arguments[0], out tutor) && parseEmotion(arguments[1], arguments[2], out emotion))
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
        tutor = null;
        if (Tutors != null && Tutors.Count > 0)
        {
            tutor = Tutors.Find(x => x.Name.ToString().ToLower() == input.ToLower());
        }

        if (tutor == null)
        {
            tutor = new Tutor(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower()));    
        }
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
            parsedEmotion = new Emotion();
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

    public void UpdateBackground(Tutor tutor, Reason.ReasonEnum reason)
    {
        BubbleSystemData data = new BubbleSystemData();
        data.Clear();
        BubbleSystem.Emotion emotion = new BubbleSystem.Emotion();
        emotion.Set(tutor.Emotion.Name.ToString());
        data.tutor.Set(tutor.Name);
        data.emotions.Add(emotion, tutor.Emotion.Intensity);
        data.backgroundData.reason.Set(reason);
        bubbleSystem.UpdateScene(data);
    }

    public void Speak(Tutor tutor, string[] text, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
    {
        BubbleSystemData data = bsData[tutor.Name];
        data.Clear();
        BubbleSystem.Emotion emotion = new BubbleSystem.Emotion();
        emotion.Set(tutor.Emotion.Name.ToString());
        data.tutor.Set(tutor.Name);
        data.emotions.Add(emotion, tutor.Emotion.Intensity);
        data.balloonData.text = text.ToList();
        if (showEffects != null)
            data.balloonData.effects.showEffects = getEffectsDictionary(showEffects);
        if (hideEffects != null)
            data.balloonData.effects.hideEffects = getEffectsDictionary(hideEffects);
        bubbleSystem.UpdateScene(data);
    }

    public void HideBalloon(Tutor tutor)
    {
        BubbleSystemData data = bsData[tutor.Name];
        data.balloonData.show = false;
        bubbleSystem.UpdateScene(data);
    }

    public void HideBalloon(string tutor)
    {
        object parsedTutor;
        if (!EnumUtils.TryParse(typeof(BubbleSystem.Tutor.TutorEnum), tutor, out parsedTutor)) return;
        BubbleSystem.Tutor.TutorEnum tutorEnum = (BubbleSystem.Tutor.TutorEnum)parsedTutor;
        BubbleSystemData data = bsData[tutorEnum.ToString()];
        data.balloonData.show = false;
        bubbleSystem.UpdateScene(data);
    }

    public void UpdateOptions(string[] text, List<HookControl.IntFunc> callbacks = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
    {
        BubbleSystemData data = bsData[BubbleSystem.Tutor.TutorEnum.User.ToString()];
        data.Clear();
        data.balloonData.text = text.ToList();
        BubbleSystem.Emotion emotion = new BubbleSystem.Emotion();
        emotion.Set(BubbleSystem.Emotion.EmotionEnum.Neutral);
        data.tutor.Set(BubbleSystem.Tutor.TutorEnum.User);
        data.emotions.Add(emotion, 1.0f);
        data.balloonData.options = true;
        if(callbacks != null)
            data.balloonData.callbacks = callbacks;
        if (showEffects != null)
            data.balloonData.effects.showEffects = getEffectsDictionary(showEffects);
        if (hideEffects != null)
            data.balloonData.effects.hideEffects = getEffectsDictionary(hideEffects);
        bubbleSystem.UpdateScene(data);
    }


    /**********************************************************************************************************
                                                    COMMANDS
    **********************************************************************************************************/

    //<< UpdateBackground tutor emotion intensity reason>>
    private void UpdateBackground(string[] info)
    {
        object parsedTutor, parsedReason;
        if (!EnumUtils.TryParse(typeof(BubbleSystem.Tutor.TutorEnum), info[0], out parsedTutor) ||
            !EnumUtils.TryParse(typeof(Reason.ReasonEnum), info[info.Length - 1], out parsedReason)) return;
        BubbleSystem.Tutor.TutorEnum tutor = (BubbleSystem.Tutor.TutorEnum)parsedTutor;
        BubbleSystemData data = new BubbleSystemData();
        data.Clear();
        KeyValuePair<int, Dictionary<BubbleSystem.Emotion, float>> emotions = GetEmotions(info, 1);
        data.tutor.Set(tutor);
        data.emotions = emotions.Value;
        data.backgroundData.reason.Set((Reason.ReasonEnum)parsedReason);
        bubbleSystem.UpdateScene(data);
    }

    /**********************************************************************************************************
                                                    HELP FUNCTIONS
    **********************************************************************************************************/

    private Dictionary<BubbleSystem.AbstractTextEffect.TextEffectEnum, AnimationCurve> getEffectsDictionary(Dictionary<string, string> effects)
    {
        Dictionary<BubbleSystem.AbstractTextEffect.TextEffectEnum, AnimationCurve> effectsDictionary = new Dictionary<BubbleSystem.AbstractTextEffect.TextEffectEnum, AnimationCurve>();
        if (effects != null)
        {
            foreach (string fx in effects.Keys)
            {
                object parsedEnum;
                if (!EnumUtils.TryParse(typeof(BubbleSystem.AbstractTextEffect.TextEffectEnum), fx, out parsedEnum)) continue;
                AnimationCurve curve = DefaultData.Instance.GetCurve(effects[fx]);
                effectsDictionary.Add((BubbleSystem.AbstractTextEffect.TextEffectEnum) parsedEnum, curve);
            }
            return effectsDictionary;
        }
        return null;
    }

    private KeyValuePair<int, Dictionary<BubbleSystem.Emotion, float>> GetEmotions(string[] info, int i)
    {
        Dictionary<BubbleSystem.Emotion, float> dict = new Dictionary<BubbleSystem.Emotion, float>();
        while (i < info.Length)
        {
            BubbleSystem.Emotion emotion = new BubbleSystem.Emotion();
            object parsedEmotion;
            float intensity;

            if (info[i].Equals("showEffects") || info[i].Equals("hideEffects"))
                break;
            if (!EnumUtils.TryParse(typeof(BubbleSystem.Emotion.EmotionEnum), info[i], out parsedEmotion) || !Single.TryParse(info[i + 1], out intensity)) {
                i += 2;
                continue;
            }
            emotion.Set((BubbleSystem.Emotion.EmotionEnum)parsedEmotion);    

            dict.Add(emotion, Mathf.Clamp01(intensity));
            i += 2;
        }

        return new KeyValuePair<int, Dictionary<BubbleSystem.Emotion, float>>(i, dict);
    }
}
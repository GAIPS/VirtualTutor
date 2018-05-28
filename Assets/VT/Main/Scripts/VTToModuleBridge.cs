using BubbleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class VTToModuleBridge : MonoBehaviour
{
    public BubbleSystemManager bubbleSystem;
    public AvatarManager avatarManager;

    private Dictionary<string, float> skills = new Dictionary<string, float>();
    private Dictionary<string, string> translations = new Dictionary<string, string>();

    public List<Tutor> Tutors;

    private void Start()
    {
        SetSkills();

        translations.Add("Barter", "They say the G.O.A.T never lies. According to this, you're slated to be the next vault ... Chaplain. God help us all.");
        translations.Add("Big Guns", "Well according to this, you're in line to be trained as a laundry cannon operator. First time for everything indeed.");
        translations.Add("Energy Weapons", "It's nice to know I can still be surprised. Pedicurist! I might have guessed Manicurist, or even Masseuse. But apparently you're a foot person.");
        translations.Add("Explosives", "It says here you're perfectly suited for a career as a Waste Management Specialist. A specialist, mind you, not just a dabbler. Congratulations!");
        translations.Add("Lockpick", "Huh. \"Vault Loyalty Inspector\"... I thought that had been phased out decades ago. Well, sounds like a job right up your alley, hmm?");
        translations.Add("Medicine", "Interesting. \"Clinical Test Subject\"... sounds like something you should excel at. I guess you and your dad will be working together.");
        translations.Add("Melee Weapons", "Looks like the diner's going to get a new Fry Cook. I'll just say this once: hold the mustard, extra pickles. Ha ha ha.");
        translations.Add("Repair", "Thank goodness. We're finally getting a new Jukebox Technician. That thing hasn't worked right since old Joe Palmer passed.");
        translations.Add("Science", "Well, well. Pip-Boy Programmer, eh? Stanley will finally have someone to talk shop with.");
        translations.Add("Small Guns", "Huh. I wonder who will be brave enough to be your first customer as the vault's new Tattoo Artist? I promise it won't be me.");
        translations.Add("Sneak", "Apparently you're management material. You're going to be trained as a Shift Supervisor. Could I be talking to the next Overseer? Stranger things have happened.");
        translations.Add("Speech", "Wow. Wow. Says here you're going to be the vault's Marriage Counselor. Almost makes me want to get married, just to be able to avail myself of your services.");
        translations.Add("Unarmed", "I always thought you'd have a career in professional sports. You're the new vault Little League coach! Congratulations.");
        translations.Add("Unarmed?", "Looks like you'll be putting your ... physical talents to good use as the vault's new Masseuse.");
    }

    private void SetSkills()
    {
        skills.Clear();
        skills.Add("Barter", 0);
        skills.Add("Big Guns", 0);
        skills.Add("Energy Weapons", 0);
        skills.Add("Explosives", 0);
        skills.Add("Lockpick", 0);
        skills.Add("Medicine", 0);
        skills.Add("Melee Weapons", 0);
        skills.Add("Repair", 0);
        skills.Add("Science", 0);
        skills.Add("Small Guns", 0);
        skills.Add("Sneak", 0);
        skills.Add("Speech", 0);
        skills.Add("Unarmed", 0);
        skills.Add("Unarmed?", 0);
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
            //BubbleSystem Commands
            switch (info[0])
            {     
                case "SetNextDialogueData":
                    SetNextDialogueData(parameters);
                    break;

                //case "UpdateBackground":
                //    UpdateBackground(parameters);
                //    break;

                case "OverrideTextEffects":
                    OverrideTextEffects(parameters);
                    break;

                case "SetMixColors":
                    SetMixColors(parameters);
                    break;

                case "OverrideBlushColor":
                    OverrideBlushColor(parameters);
                    break;

                case "OverrideEmotionColor":
                    OverrideEmotionColor(parameters);
                    break;

                case "AddAnimationCurve":
                    AddAnimationCurve(parameters);
                    break;

                case "SetForceTextUpdate":
                    SetForceTextUpdate(parameters);
                    break;

                case "SetBalloonAnimationBlending":
                    SetBalloonAnimationBlending(parameters);
                    break;

                case "SetBalloonDuration":
                    SetBalloonDuration(parameters);
                    break;

                case "SetBackgroundDuration":
                    SetBackgroundDuration(parameters);
                    break;

                //  SHARED COMMANDS
                case "Feel":
                    Feel(parameters);
                    break;

                case "FeelRandomBoth":
                    FeelRandom(true);
                    break;

                case "FeelRandomEach":
                    FeelRandom(false);
                    break;

                case "SetBlankDuration":
                    SetBlankDuration(parameters);
                    break;

                case "Inc":
                    Inc(parameters);
                    break;

                case "ShowResults":
                    ShowResults();
                    break;

                case "Reset":
                    Reset();
                    break;

                default:
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

        if (parseTutorName(parameters[0], out tutor) && parseEmotion(parameters[1], parameters[2], out emotion))
        {
            tutor.Emotion = emotion;
        }

        Reason reason = (Reason)Enum.Parse(typeof(Reason), parameters[3]);

        Feel(tutor, reason);
    }

    /**********************************************************************************************************
                                               MOJO
    **********************************************************************************************************/

    private KeyValuePair<Emotion, Reason> GenerateEmotionAndReason()
    {
        Emotion emotion;
        int emotionInt = UnityEngine.Random.Range(0, Enum.GetNames(typeof(EmotionEnum)).Length);
        int reasonInt = UnityEngine.Random.Range(0, Enum.GetNames(typeof(Reason)).Length);
        float intensity = UnityEngine.Random.Range(0.0f, 1.0f);
        emotion.Name = (EmotionEnum)emotionInt;
        emotion.Intensity = intensity;
        Reason reason = (Reason)reasonInt;

        return new KeyValuePair<Emotion, Reason>(emotion, reason);
    }

    // <<FeelRandom>>
    public void FeelRandom(bool both)
    {
        KeyValuePair<Emotion, Reason> random = GenerateEmotionAndReason();

        foreach (Tutor tutor in Tutors)
        {
            tutor.Emotion = random.Key;
            Feel(tutor, random.Value);

            if (!both)
            {
                random = GenerateEmotionAndReason();
            }
        }
    }

    // <<SetBlankDuration [Duration]>>
    public void SetBlankDuration(string[] parameters) {
        DefaultData.Instance.SetBlankDuration(Convert.ToSingle(parameters[0]));
    }

    // <<Inc string>>
    public void Inc(string[] parameters)
    {
        string key = "";
        for(int i = 0; i < parameters.Length; i++)
        {
            key += parameters[i];
            if(i < parameters.Length - 1)
                key += " ";
        }

        skills[key] += 1;
    }

    public void ShowResults()
    {
        string result = skills.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        StartSpeaking(Tutors[1], translations[result]);
        StartCoroutine(Wait(DefaultData.Instance.GetBalloonDuration()));
    }

    public void Reset()
    {
        SetSkills();
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        StopSpeaking(Tutors[1]);
    }

    /**********************************************************************************************************
                                                SHARED CALLS
    **********************************************************************************************************/

    public void Feel(Tutor who, Reason why)
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

    public void UpdateBackground(Tutor tutor, Reason reason)
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add(tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity);
        bubbleSystem.UpdateBackground(tutor.Name, dict, reason);
    }

    public void Speak(Tutor tutor, string[] text, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add(tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity);
        bubbleSystem.Speak(tutor.Name, dict, text, showEffects, hideEffects);
    }

    public void HideBalloon(Tutor tutor, float duration = 0.0f)
    {
        bubbleSystem.HideBalloon(tutor.Name, duration);
    }

    public void HideBalloon(string tutor, float duration = 0.0f)
    {
        bubbleSystem.HideBalloon(tutor, duration);
    }

    public void UpdateOptions(string[] text, HookControl.IntFunc[] callbacks = null, Dictionary<string, string> showEffects = null, Dictionary<string, string> hideEffects = null)
    {
        bubbleSystem.UpdateOptions(text, callbacks, showEffects, hideEffects);
    }


    /**********************************************************************************************************
                                                    COMMANDS
    **********************************************************************************************************/

    //<< OverrideTextEffects emotion intensity [showEffects [curve] ...] [hideEffects effect1 [curve] ...] >>
    public void OverrideTextEffects(string[] info)
    {
        BubbleSystem.Emotion emotion = (BubbleSystem.Emotion)Enum.Parse(typeof(BubbleSystem.Emotion), info[0]);
        float intensity = Mathf.Clamp01(Convert.ToSingle(info[1]));

        KeyValuePair<Dictionary<Effect, AnimationCurve>, Dictionary<Effect, AnimationCurve>> effects = SetTextEffects(info, 2);
        DefaultData.Instance.SetTextEffects(emotion, intensity, effects.Key, effects.Value);
    }

    //<< UpdateBackground tutor emotion intensity reason>>
    private void UpdateBackground(string[] info)
    {
        Reason reason = (Reason)Enum.Parse(typeof(Reason), info[info.Length - 1]);
        KeyValuePair<int, Dictionary<string, float>> kvp = GetEmotions(info, 1);
        bubbleSystem.UpdateBackground(info[0], kvp.Value, reason);
    }

    //<< SetNextDialogueData tutor emotion intensity [showEffects effect1 [curve] ...] [hideEffects effect1 [curve] ...] >>
    public void SetNextDialogueData(string[] info)
    {
        NextDialogueData nextData = new NextDialogueData();
        KeyValuePair<int, Dictionary<string, float>> kvp = GetEmotions(info, 1);

        nextData.emotions = kvp.Value;
        int i = kvp.Key;

        KeyValuePair<Dictionary<Effect, AnimationCurve>, Dictionary<Effect, AnimationCurve>> effects = SetTextEffects(info, i);
        nextData.showEffects = effects.Key;
        nextData.hideEffects = effects.Value;

        nextData.isSet = true;

        bubbleSystem.AddToTutorNextData(info[0], nextData);
    }

    //<< SetMixColors boolInIntFormat >>   0 -> false; 1 -> true
    public void SetMixColors(string[] info)
    {
        DefaultData.Instance.mixColors = Convert.ToBoolean(Convert.ToInt16(info[0]));
    }

    //<< OverrideBlushColor color >>   Color in #RRGGBBAA format
    public void OverrideBlushColor(string[] info)
    {
        Color color;
        ColorUtility.TryParseHtmlString(info[0], out color);
        DefaultData.Instance.SetBlushColor(color);
    }

    //Only works for backgrounds
    //<< OverrideEmotionColor emotion color >>   Color in #RRGGBBAA format
    public void OverrideEmotionColor(string[] info)
    {
        BubbleSystem.Emotion emotion = (BubbleSystem.Emotion)Enum.Parse(typeof(BubbleSystem.Emotion), info[0]);
        Color color;
        ColorUtility.TryParseHtmlString(info[1], out color);
        DefaultData.Instance.SetColor(emotion, color);
    }

    //Can also override
    //<< AddAnimationCurve name time1 value1 [smooth weight1] time2 value2 [smooth weight2] ... >>
    public void AddAnimationCurve(string[] info)
    {
        string name = info[0];
        AnimationCurve curve = new AnimationCurve();
        int indexToSmooth = -1;
        List<KeyValuePair<int, float>> smoothTangents = new List<KeyValuePair<int, float>>();

        for (int i = 1; i < info.Length; i = i + 2)
        {
            if (info[i].Equals("smooth"))
                smoothTangents.Add(new KeyValuePair<int, float>(indexToSmooth, Convert.ToSingle(info[i + 1])));
            else
            {
                curve.AddKey(new Keyframe(Convert.ToSingle(info[i]), Convert.ToSingle(info[i + 1])));
                indexToSmooth++;
            }
        }

        foreach(KeyValuePair<int, float> kvp in smoothTangents)
        {
            curve.SmoothTangents(kvp.Key, kvp.Value);
        }

        DefaultData.Instance.AddCurve(name, curve);
    }

    //<< SetForceTextUpdate boolInIntFormat >>   0 -> false; 1 -> true
    public void SetForceTextUpdate(string[] info)
    {
        DefaultData.Instance.forceTextUpdate = Convert.ToBoolean(Convert.ToInt16(info[0]));
    }

    //<< SetEmotionBlending boolInIntFormat >>   0 -> false; 1 -> true
    public void SetBalloonAnimationBlending(string[] info)
    {
        DefaultData.Instance.blendBalloonAnimation = Convert.ToBoolean(Convert.ToInt16(info[0]));
    }

    //<< SetBalloonDuration duration >>
    public void SetBalloonDuration(string[] info)
    {
        DefaultData.Instance.SetBalloonDuration(Convert.ToSingle(info[0]));
    }

    //<< SetBackgroundDuration duration >>
    public void SetBackgroundDuration(string[] info)
    {
        DefaultData.Instance.SetBackgroundDuration(Convert.ToSingle(info[0]));
    }

    /**********************************************************************************************************
                                                    HELP FUNCTIONS
    **********************************************************************************************************/

    private KeyValuePair<Dictionary<Effect, AnimationCurve>, Dictionary<Effect, AnimationCurve>> SetTextEffects(string[] info, int i)
    {
        Dictionary<Effect, AnimationCurve> showEffects = null;
        Dictionary<Effect, AnimationCurve> hideEffects = null;
        KeyValuePair<int, Dictionary<Effect, AnimationCurve>> kvp;
        int size = info.Length;
        if (size > i)
        {
            if (info[i].Equals("showEffects"))
            {
                kvp = GetEffects(info, i + 1);
                showEffects = kvp.Value;
                if (size > kvp.Key)
                {
                    if (info[kvp.Key].Equals("hideEffects"))
                    {
                        kvp = GetEffects(info, kvp.Key + 1);
                        hideEffects = kvp.Value;
                    }
                }
            }
            else if (info[i].Equals("hideEffects"))
            {
                kvp = GetEffects(info, i + 1);
                hideEffects = kvp.Value;
            }
        }

        return new KeyValuePair<Dictionary<Effect, AnimationCurve>, Dictionary<Effect, AnimationCurve>>(showEffects, hideEffects);
    }

    private KeyValuePair<int, Dictionary<Effect, AnimationCurve>> GetEffects(string[] info, int i)
    {
        Dictionary<Effect, AnimationCurve> effects = new Dictionary<Effect, AnimationCurve>();
        string stringToLook = "hideEffects";
        while (i < info.Length)
        {
            if (info[i].Equals(stringToLook))
                break;

            Effect effect;
            AnimationCurve animationCurve;
            try
            {
                effect = (Effect)Enum.Parse(typeof(Effect), info[i]);
                animationCurve = (i + 1 >= info.Length) ? null : (info[i + 1].Equals(stringToLook)) ? null : DefaultData.Instance.GetCurve(info[i + 1]);
                i += (animationCurve != null) ? 2 : 1;

                effects.Add(effect, animationCurve);
                continue;
            }
            catch
            {
                i++;
                continue;
            }
        }

        return new KeyValuePair<int, Dictionary<Effect, AnimationCurve>>(i, effects);
    }

    private KeyValuePair<int, Dictionary<string, float>> GetEmotions(string[] info, int i)
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        while (i < info.Length)
        {
            if (info[i].Equals("showEffects") || info[i].Equals("hideEffects"))
                break;
            dict.Add(info[i], Mathf.Clamp01(System.Convert.ToSingle(info[i + 1])));
            i += 2;
        }

        return new KeyValuePair<int, Dictionary<string, float>>(i, dict);
    }
}
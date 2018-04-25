using System;
using System.Collections;
using UnityEngine;

public class AvatarTestMain : MonoBehaviour
{
    //This will be the main animation manager for the synthetic characters
    [Header("Object Hooks")]
    [SerializeField]
    private AvatarManager manager;
    private AvatarParameters parameters;
    //Placeholder, system should be aware of the available tutors (by name)
    [SerializeField]
    private GameObject tutor;   
    private string tutorName;
    [Space(2.0f)]
    [Header("Yarn Commands")]
    [SerializeField]
    private bool testCommands = false;
    [Space(2.0f)]
    [Header("Keybinds")]
    [SerializeField]
    private MoodVariables moodCommands;
    [SerializeField]
    private ExpressionVariables expressionCommands;
    [SerializeField]
    private MovementVariables movementCommands;
    [SerializeField]
    private ParameterVariables changeParameterCommands;
    [Space(2.0f)]
    [Header("Debug")]
    [SerializeField]
    private bool displayDebugInfo = false;
    [SerializeField]
    private float displayInterval = 5.0f;

    void Awake()
    {
        if (manager == null || tutor == null)
            Debug.Log("[WARNING]: One or more editor references (required for testing) are currently unassigned.");
        else
            OnValidate();
    }
    private void Start()
    {
        StartCoroutine("controllerParameterDebugRoutine");
    }
    private void OnValidate()
    {
        moodCommands = new MoodVariables();
        expressionCommands = new ExpressionVariables();
        movementCommands = new MovementVariables();
        changeParameterCommands = new ParameterVariables();
        tutorName = tutor.name;
        parameters = manager.getController(new Tutor(tutorName)).getParameters();
    }

    // Input driven commands
    void FixedUpdate()
    {
        if (manager == null || tutor == null)
            return;

        // Emotion
        if (Input.GetKey(moodCommands.neutral))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Feel", tutorName, "Neutral", "0.0" });
            else
                manager.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Neutral, 0.0f)));
        }   
        if (Input.GetKey(moodCommands.happy))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Feel", tutorName, "Happiness", "0.5" });
            else
                manager.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Happiness, 0.5f)));
        }
        if (Input.GetKey(moodCommands.veryHappy))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Feel", tutorName, "Happiness", "1.0" });
            else
                manager.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Happiness, 1.0f)));
        }
        if (Input.GetKey(moodCommands.sad))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Feel", tutorName, "Sadness", "0.5" });
            else
                manager.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Sadness, 0.5f)));
        }
        if (Input.GetKey(moodCommands.verySad))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Feel", tutorName, "Sadness", "1.0" });
            else
                manager.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Sadness, 1.0f)));
        }
        if (Input.GetKey(moodCommands.afraid))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Feel", tutorName, "Fear", "0.0" });
            else
                manager.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Fear, 0.0f)));
        }
        if (Input.GetKey(moodCommands.surprised))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Feel", tutorName, "Surprise", "0.0" });
            else
                manager.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Surprise, 0.0f)));
        }

        // Expression
        if (Input.GetKey(expressionCommands.neutral))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Neutral", "0.0" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Neutral, 1.0f)));
        }
        if (Input.GetKey(expressionCommands.happinessLow))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Happiness", "0.5" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Happiness, 0.5f)));
        }
        if (Input.GetKey(expressionCommands.happinessHigh))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Happiness", "1.0" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Happiness, 1.0f)));
        }
        if (Input.GetKey(expressionCommands.sadnessLow))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Sadness", "0.5" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Sadness, 0.5f)));
        }
        if (Input.GetKey(expressionCommands.sadnessHigh))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Sadness", "1.0" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Sadness, 1.0f)));
        }
        if (Input.GetKey(expressionCommands.fearLow))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Fear", "0.5" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Fear, 0.5f)));
        }
        if (Input.GetKey(expressionCommands.fearHigh))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Fear", "1.0" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Fear, 1.0f)));
        }
        if (Input.GetKey(expressionCommands.surpriseLow))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Surprise", "0.5" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Surprise, 0.5f)));
        }
        if (Input.GetKey(expressionCommands.surpriseHigh))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Surprise", "1.0" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Surprise, 1.0f)));
        }
        if (Input.GetKey(expressionCommands.angerLow))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Anger", "0.5" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Anger, 0.5f)));
        }
        if (Input.GetKey(expressionCommands.angerHigh))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Anger", "1.0" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Anger, 1.0f)));
        }
        if (Input.GetKey(expressionCommands.disgustLow))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Disgust", "0.5" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Disgust, 0.5f)));
        }
        if (Input.GetKey(expressionCommands.disgustHigh))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Express", tutorName, "Disgust", "1.0" });
            else
                manager.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Disgust, 1.0f)));
        }

        // Actions
        if (Input.GetKey(movementCommands.nodStart))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Nod", tutorName, "Start" });
            else
                manager.Act(new Tutor(tutorName), new MovementWithState(MovementEnum.Nod, StateEnum.Start));
        }
        if (Input.GetKey(movementCommands.nodStop))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Nod", tutorName, "End" });
            else
                manager.Act(new Tutor(tutorName), new MovementWithState(MovementEnum.Nod, StateEnum.End));
        }
        if (Input.GetKey(movementCommands.talkStart))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Talk", tutorName, "Start" });
            else
                manager.Act(new Tutor(tutorName), new MovementWithState(MovementEnum.Talk, StateEnum.Start));
        }
        if (Input.GetKey(movementCommands.talkStop))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Talk", tutorName, "End" });
            else
                manager.Act(new Tutor(tutorName), new MovementWithState(MovementEnum.Talk, StateEnum.End));
        }
        if (Input.GetKey(movementCommands.gazeAtPartner))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Gazeat", tutorName, tutorName=="Maria" ? "Joao" : "Maria" });
            else
                manager.Act(new Tutor(tutorName), new MovementWithTarget(MovementEnum.Gazeat, TargetEnum.Partner));
        }
        if (Input.GetKey(movementCommands.gazeBackFromPartner))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Gazeback", tutorName, tutorName == "Maria" ? "Joao" : "Maria" });
            else
                manager.Act(new Tutor(tutorName), new MovementWithTarget(MovementEnum.Gazeback, TargetEnum.Partner));
        }
        if (Input.GetKey(movementCommands.gazeAtUser))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Gazeat", tutorName, "User" });
            else
                manager.Act(new Tutor(tutorName), new MovementWithTarget(MovementEnum.Gazeat, TargetEnum.User));
        }
        if (Input.GetKey(movementCommands.gazeBackFromUser))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Gazeback", tutorName, "User" });
            else
                manager.Act(new Tutor(tutorName), new MovementWithTarget(MovementEnum.Gazeback, TargetEnum.User));
        }

        // Action Speed\Frequency
        if (Input.GetKey(changeParameterCommands.nodFrequency))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Nod", tutorName, "Frequency", "0.5" });
            else
                manager.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Nod, PropertyEnum.Frequency, 0.5f));
        }
        if (Input.GetKey(changeParameterCommands.nodSpeed))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Nod", tutorName, "Speed", "2.0" });
            else
                manager.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Nod, PropertyEnum.Speed, 2.0f));
        }
        if (Input.GetKey(changeParameterCommands.gazeFrequency))
        {
            if (testCommands)
                manager.sendCommand(new string[] { "Gazeat", tutorName, "Frequency", "0.5" });
            else
                manager.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Gazeat, PropertyEnum.Frequency, 0.5f));
        }
        if (Input.GetKey(changeParameterCommands.gazeSpeed))
        {
            if (testCommands)
            {
                manager.sendCommand(new string[] { "Gazeat", tutorName, "Speed", "1.5" });
                manager.sendCommand(new string[] { "Gazeback", tutorName, "Speed", "2.0" });
            }
            else
            {
                manager.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Gazeat, PropertyEnum.Speed, 1.5f));
                manager.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Gazeback, PropertyEnum.Speed, 2.0f));
            }
        }
    }
    // UI driven commands
    public void talk(string who)
    {
        manager.Act(new Tutor(who), new MovementWithState(MovementEnum.Talk, StateEnum.Start));
    }
    public void stopTalking(string who)
    {
        manager.Act(new Tutor(who), new MovementWithState(MovementEnum.Talk, StateEnum.End));
    }

    IEnumerator controllerParameterDebugRoutine()
    {
        float nodSpeed, nodFrequency, nodDuration, nodInterval;
        while (true)
        {
            if (displayDebugInfo)
            {
                nodSpeed = parameters.getParameter(AnimatorParams.NOD_SPEED);
                nodFrequency = parameters.getParameter(ControllerParams.NOD_FREQUENCY);
                nodDuration = Mathf.Abs(parameters.nodLength) / (nodSpeed < 0.001f ? 0.001f : nodSpeed);
                nodInterval = parameters.nodInterval * (1 - nodFrequency) + 0.001f;

                Debug.Log(String.Format("animParams.nodFrequency: {0}", parameters.getParameter(ControllerParams.NOD_FREQUENCY)));
                Debug.Log(String.Format("animParams.gazeAtFrequency: {0}", parameters.getParameter(ControllerParams.GAZEAT_FREQUENCY)));
                Debug.Log(String.Format("animParams.gazeBackFrequency: {0}", parameters.getParameter(ControllerParams.GAZEBACK_FREQUENCY)));

                Debug.Log(String.Format("NODDURATION: {0}", nodDuration));
                Debug.Log(String.Format("NODINTERVAL: {0}", nodInterval));
            }
            yield return new WaitForSeconds(displayInterval);
        }
    }
}

[Serializable]
class MoodVariables {
    public string neutral = "q"; 
    public string happy = "w";
    public string veryHappy = "e";
    public string sad = "r";
    public string verySad = "t";
    public string afraid = "1";
    public string surprised = "2";
}
[Serializable]
class ExpressionVariables
{
    public string neutral = "a";
    public string happinessLow = "s";
    public string happinessHigh = "d";
    public string sadnessLow = "f";
    public string sadnessHigh = "g";
    public string fearLow = "h";
    public string fearHigh = "j";
    public string surpriseLow = "k";
    public string surpriseHigh = "l";
    public string angerLow = "z";
    public string angerHigh = "x";
    public string disgustLow = "c";
    public string disgustHigh = "v";
}
[Serializable]
class MovementVariables
{
    public string nodStart = "n";
    public string nodStop = "m";
    public string talkStart = "u";
    public string talkStop = "i";
    public string gazeAtPartner = "o";
    public string gazeBackFromPartner = "p";
    public string gazeAtUser = "9";
    public string gazeBackFromUser = "0";
}
[Serializable]
class ParameterVariables
{
    public string nodFrequency = "[";
    public string nodSpeed = "]";
    public string gazeFrequency = ",";
    public string gazeSpeed = ".";
}
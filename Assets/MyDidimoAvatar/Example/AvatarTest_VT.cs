using System;
using System.Collections;
using UnityEngine;

public class AvatarTest_VT : MonoBehaviour
{
    //This will be the main animation manager for the synthetic characters
    [Header("Object Hooks")]
    [SerializeField]
    private VTToModuleBridge bridge;
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
        if (bridge == null || tutor == null)
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
        parameters = bridge.avatarManager.getControllerParameters(tutorName);
    }

    // Input driven commands
    void FixedUpdate()
    {
        if (bridge == null || tutor == null)
            return;

        // Emotion
        if (Input.GetKeyDown(moodCommands.neutral))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Feel", tutorName, "Neutral", "0.0" });
            else
                bridge.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Neutral, 0.0f)));
        }   
        if (Input.GetKeyDown(moodCommands.happy))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Feel", tutorName, "Happiness", "0.5" });
            else
                bridge.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Happiness, 0.5f)));
        }
        if (Input.GetKeyDown(moodCommands.veryHappy))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Feel", tutorName, "Happiness", "1.0" });
            else
                bridge.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Happiness, 1.0f)));
        }
        if (Input.GetKeyDown(moodCommands.sad))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Feel", tutorName, "Sadness", "0.5" });
            else
                bridge.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Sadness, 0.5f)));
        }
        if (Input.GetKeyDown(moodCommands.verySad))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Feel", tutorName, "Sadness", "1.0" });
            else
                bridge.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Sadness, 1.0f)));
        }
        if (Input.GetKeyDown(moodCommands.afraid))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Feel", tutorName, "Fear", "0.6" });
            else
                bridge.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Fear, 0.6f)));
        }
        if (Input.GetKeyDown(moodCommands.surprised))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Feel", tutorName, "Surprise", "0.6" });
            else
                bridge.Feel(new Tutor(tutorName, new Emotion(EmotionEnum.Surprise, 0.6f)));
        }

        // Expression
        if (Input.GetKeyDown(expressionCommands.neutral))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Neutral", "0.0" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Neutral, 1.0f)));
        }
        if (Input.GetKeyDown(expressionCommands.happinessLow))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Happiness", "0.5" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Happiness, 0.5f)));
        }
        if (Input.GetKeyDown(expressionCommands.happinessHigh))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Happiness", "1.0" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Happiness, 1.0f)));
        }
        if (Input.GetKeyDown(expressionCommands.sadnessLow))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Sadness", "0.5" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Sadness, 0.5f)));
        }
        if (Input.GetKeyDown(expressionCommands.sadnessHigh))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Sadness", "1.0" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Sadness, 1.0f)));
        }
        if (Input.GetKeyDown(expressionCommands.fearLow))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Fear", "0.5" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Fear, 0.5f)));
        }
        if (Input.GetKeyDown(expressionCommands.fearHigh))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Fear", "1.0" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Fear, 1.0f)));
        }
        if (Input.GetKeyDown(expressionCommands.surpriseLow))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Surprise", "0.5" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Surprise, 0.5f)));
        }
        if (Input.GetKeyDown(expressionCommands.surpriseHigh))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Surprise", "1.0" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Surprise, 1.0f)));
        }
        if (Input.GetKeyDown(expressionCommands.angerLow))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Anger", "0.5" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Anger, 0.5f)));
        }
        if (Input.GetKeyDown(expressionCommands.angerHigh))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Anger", "1.0" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Anger, 1.0f)));
        }
        if (Input.GetKeyDown(expressionCommands.disgustLow))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Disgust", "0.5" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Disgust, 0.5f)));
        }
        if (Input.GetKeyDown(expressionCommands.disgustHigh))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Express", tutorName, "Disgust", "1.0" });
            else
                bridge.Express(new Tutor(tutorName, new Emotion(EmotionEnum.Disgust, 1.0f)));
        }

        // Actions
        if (Input.GetKeyDown(movementCommands.nodStart))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Nod", tutorName, "Start" });
            else
                bridge.Act(new Tutor(tutorName), new MovementWithState(MovementEnum.Nod, StateEnum.Start));
        }
        if (Input.GetKeyDown(movementCommands.nodStop))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Nod", tutorName, "End" });
            else
                bridge.Act(new Tutor(tutorName), new MovementWithState(MovementEnum.Nod, StateEnum.End));
        }
        if (Input.GetKeyDown(movementCommands.talkStart))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Talk", tutorName, "Start" });
            else
                bridge.Act(new Tutor(tutorName), new MovementWithState(MovementEnum.Talk, StateEnum.Start));
        }
        if (Input.GetKeyDown(movementCommands.talkStop))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Talk", tutorName, "End" });
            else
                bridge.Act(new Tutor(tutorName), new MovementWithState(MovementEnum.Talk, StateEnum.End));
        }
        if (Input.GetKeyDown(movementCommands.gazeAtPartner))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Gazeat", tutorName, tutorName=="Maria" ? "Joao" : "Maria" });
            else
                bridge.Act(new Tutor(tutorName), new MovementWithTarget(MovementEnum.Gazeat, TargetEnum.Partner));
        }
        if (Input.GetKeyDown(movementCommands.gazeBackFromPartner))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Gazeback", tutorName, tutorName == "Maria" ? "Joao" : "Maria" });
            else
                bridge.Act(new Tutor(tutorName), new MovementWithTarget(MovementEnum.Gazeback, TargetEnum.Partner));
        }
        if (Input.GetKeyDown(movementCommands.gazeAtUser))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Gazeat", tutorName, "User" });
            else
                bridge.Act(new Tutor(tutorName), new MovementWithTarget(MovementEnum.Gazeat, TargetEnum.User));
        }
        if (Input.GetKeyDown(movementCommands.gazeBackFromUser))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Gazeback", tutorName, "User" });
            else
                bridge.Act(new Tutor(tutorName), new MovementWithTarget(MovementEnum.Gazeback, TargetEnum.User));
        }

        // Action Speed\Frequency
        if (Input.GetKeyDown(changeParameterCommands.nodFrequency))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Nod", tutorName, "Frequency", "0.5" });
            else
                bridge.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Nod, PropertyEnum.Frequency, 0.5f));
        }
        if (Input.GetKeyDown(changeParameterCommands.nodSpeed))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Nod", tutorName, "Speed", "2.0" });
            else
                bridge.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Nod, PropertyEnum.Speed, 2.0f));
        }
        if (Input.GetKeyDown(changeParameterCommands.gazeFrequency))
        {
            if (testCommands)
                bridge.Handle(new string[] { "Gazeat", tutorName, "Frequency", "0.5" });
            else
                bridge.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Gazeat, PropertyEnum.Frequency, 0.5f));
        }
        if (Input.GetKeyDown(changeParameterCommands.gazeSpeed))
        {
            if (testCommands)
            {
                bridge.Handle(new string[] { "Gazeat", tutorName, "Speed", "1.5" });
                bridge.Handle(new string[] { "Gazeback", tutorName, "Speed", "2.0" });
            }
            else
            {
                bridge.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Gazeat, PropertyEnum.Speed, 1.5f));
                bridge.setParameter(new Tutor(tutorName), new MovementWithProperty(MovementEnum.Gazeback, PropertyEnum.Speed, 2.0f));
            }
        }
    }
    // UI driven commands
    public void talk(string who)
    {
        if (isActiveAndEnabled) {
            if (testCommands)
                bridge.Handle(new string[] { "Talk", tutorName, "Start" });
            else
                bridge.Act(new Tutor(who), new MovementWithState(MovementEnum.Talk, StateEnum.Start));
        }

    }
    public void stopTalking(string who)
    {
        if (isActiveAndEnabled)
        {
            if (testCommands)
                bridge.Handle(new string[] { "Talk", tutorName, "Start" });
            else
                bridge.Act(new Tutor(who), new MovementWithState(MovementEnum.Talk, StateEnum.End));
        }
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
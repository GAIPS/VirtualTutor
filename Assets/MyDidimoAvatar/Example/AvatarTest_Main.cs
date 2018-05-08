using System;
using System.Collections;
using UnityEngine;

public class AvatarTest_Main : MonoBehaviour
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

    private void Awake()
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
        parameters = manager.getControllerParameters(tutorName);
    }

    // Input driven commands
    private void FixedUpdate()
    {
        if (manager == null || tutor == null)
            return;

        // Emotion
        if (Input.GetKey(moodCommands.neutral))
            manager.Feel(tutorName, EmotionalState.NEUTRAL, 0.0f);

        if (Input.GetKey(moodCommands.happy))
            manager.Feel(tutorName, EmotionalState.HAPPINESS, 0.5f);

        if (Input.GetKey(moodCommands.veryHappy))
            manager.Feel(tutorName, EmotionalState.HAPPINESS, 1.0f);

        if (Input.GetKey(moodCommands.sad))
            manager.Feel(tutorName, EmotionalState.SADNESS, 0.5f);

        if (Input.GetKey(moodCommands.verySad))
            manager.Feel(tutorName, EmotionalState.SADNESS, 1.0f);

        if (Input.GetKey(moodCommands.afraid))
            manager.Feel(tutorName, EmotionalState.FEAR, 0.6f);

        if (Input.GetKey(moodCommands.surprised))
            manager.Feel(tutorName, EmotionalState.SURPRISE, 0.6f);

        // Expression
        if (Input.GetKey(expressionCommands.neutral))
            manager.Express(tutorName, EmotionalState.NEUTRAL, 1.0f);

        if (Input.GetKey(expressionCommands.happinessLow))
            manager.Express(tutorName, EmotionalState.HAPPINESS, 0.5f);

        if (Input.GetKey(expressionCommands.happinessHigh))
            manager.Express(tutorName, EmotionalState.HAPPINESS, 1.0f);

        if (Input.GetKey(expressionCommands.sadnessLow))
            manager.Express(tutorName, EmotionalState.SADNESS, 0.5f);

        if (Input.GetKey(expressionCommands.sadnessHigh))
            manager.Express(tutorName, EmotionalState.SADNESS, 1.0f);

        if (Input.GetKey(expressionCommands.fearLow))
            manager.Express(tutorName, EmotionalState.FEAR, 0.5f);

        if (Input.GetKey(expressionCommands.fearHigh))
            manager.Express(tutorName, EmotionalState.FEAR, 1.0f);

        if (Input.GetKey(expressionCommands.surpriseLow))
            manager.Express(tutorName, EmotionalState.SURPRISE, 0.5f);

        if (Input.GetKey(expressionCommands.surpriseHigh))
            manager.Express(tutorName, EmotionalState.SURPRISE, 1.0f);

        if (Input.GetKey(expressionCommands.angerLow))
            manager.Express(tutorName, EmotionalState.ANGER, 0.5f);

        if (Input.GetKey(expressionCommands.angerHigh))
            manager.Express(tutorName, EmotionalState.ANGER, 1.0f);

        if (Input.GetKey(expressionCommands.disgustLow))
            manager.Express(tutorName, EmotionalState.DISGUST, 0.5f);

        if (Input.GetKey(expressionCommands.disgustHigh))
            manager.Express(tutorName, EmotionalState.DISGUST, 1.0f);

        // Actions
        if (Input.GetKey(movementCommands.nodStart))
            manager.Nod(tutorName, NodState.NOD_START);

        if (Input.GetKey(movementCommands.nodStop))
            manager.Nod(tutorName, NodState.NOD_END);

        if (Input.GetKey(movementCommands.talkStart))
            manager.Talk(tutorName, TalkState.TALK_START);

        if (Input.GetKey(movementCommands.talkStop))
            manager.Talk(tutorName, TalkState.TALK_END);

        if (Input.GetKey(movementCommands.gazeAtPartner))
            manager.Gaze(tutorName, GazeState.GAZEAT_PARTNER);

        if (Input.GetKey(movementCommands.gazeBackFromPartner))
            manager.Gaze(tutorName, GazeState.GAZEBACK_PARTNER);

        if (Input.GetKey(movementCommands.gazeAtUser))
            manager.Gaze(tutorName, GazeState.GAZEAT_USER);

        if (Input.GetKey(movementCommands.gazeBackFromUser))
            manager.Gaze(tutorName, GazeState.GAZEBACK_USER);

        // Action Speed\Frequency
        if (Input.GetKey(changeParameterCommands.nodFrequency))
            manager.setParameter(tutorName, ControllerParams.NOD_FREQUENCY, 0.5f);

        if (Input.GetKey(changeParameterCommands.nodSpeed))
            manager.setParameter(tutorName, AnimatorParams.NOD_SPEED, 2.0f);

        if (Input.GetKey(changeParameterCommands.gazeFrequency))
            manager.setParameter(tutorName, ControllerParams.GAZEAT_FREQUENCY, 0.5f);

        if (Input.GetKey(changeParameterCommands.gazeSpeed))
        {
            manager.setParameter(tutorName, AnimatorParams.GAZEAT_SPEED, 1.5f);
            manager.setParameter(tutorName, AnimatorParams.GAZEBACK_SPEED, 2.0f);
        }
    }

    // UI driven commands
    public void talk(string who)
    {
        manager.Talk(who, TalkState.TALK_START);
    }

    public void stopTalking(string who)
    {
        manager.Talk(who, TalkState.TALK_END);
    }

    private IEnumerator controllerParameterDebugRoutine()
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
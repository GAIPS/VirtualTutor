using System;
using SimpleJSON;
using UnityEngine;
using Utilities;
using Yarn;
using YarnDialog;

public class EmotivectorDemo : MonoBehaviour
{
    private SystemManager _manager;

    public TextAsset[] YarnDialogDatabase;

    public VTToModuleBridge ModuleManager;

    [SerializeField] private MenuCommandHandler _commandHandler;

    public bool Playing;

    // Use this for initialization
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        DebugLog.Clean();
        DebugLog.Add(new UnityDebugLogger());

        PersistentDataStorage.Instance.ResetState();

        _manager = new SystemManager();

        Tutor joao = new Tutor("Joao");
        Tutor maria = new Tutor("Maria");
        SetPersonality(joao, maria);
        _manager.Tutors.Add(joao);
        _manager.Tutors.Add(maria);


        {
            EmotivectorAppraisal appraisal = new EmotivectorAppraisal();
            IPredictor predictor = new AdditiveSecondDerivativePredictor(new WeightedMovingAveragePredictor(),
                new WeightedMovingAveragePredictor(), new WeightedMovingAveragePredictor());
            {
                AffectiveUpdater updater = new NamedArrayAffectiveUpdater("Grades", 0, 20)
                    {Emotivector = new Emotivector("Grades", predictor)};
                appraisal.AddUpdater(updater);
            }
            {
                AffectiveUpdater updater = new NamedArrayAffectiveUpdater("Hours", 0, 16)
                    {Emotivector = new Emotivector("StudyHours", predictor)};
                appraisal.AddUpdater(updater);
            }
            _manager.AffectiveAppraisal = appraisal;
        }

        {
            // Setup Empathic Strategy
            _manager.EmpathicStrategySelector = new SS_SelectFirst();
            BasicStrategy strategy = new BasicStrategy();
            strategy.Intentions.Add(new Intention("demo"));
            _manager.Strategies.Add(strategy);
        }

        // Setup Dialog Selector
        if (YarnDialogDatabase != null)
        {
            string[] yarnFilesContent = new string[YarnDialogDatabase.Length];
            for (int i = 0; i < YarnDialogDatabase.Length; i++)
            {
                yarnFilesContent[i] = YarnDialogDatabase[i].text;
            }

            var dialogSelector = new BasicYarnDialogSelector(yarnFilesContent);

            _manager.DialogSelector = dialogSelector;
        }

        {
            // Setup Dialog Manager
            var dialogManager = new YarnDialogManager(false);
            _manager.DialogManager = dialogManager;
            dialogManager.Tutors.Add(joao);
            dialogManager.Tutors.Add(maria);
            dialogManager.ModuleManager = ModuleManager;


            // Handlers Order matters

            // Tag Handlers (should always be first)
            dialogManager.Handlers.Add(new EmotionTagNodeHandler());

            // Line Handlers
            dialogManager.Handlers.Add(new ParallelLineHandler());

            // Options Handlers
            dialogManager.Handlers.Add(new SequenceOptionsHandler());

            // Node Handlers
            dialogManager.Handlers.Add(new LogCompleteNodeHandler());

            // Command Handlers
            dialogManager.Handlers.Add(new WaitCommandHandler());
            dialogManager.Handlers.Add(new ModuleCommandHandler());
            dialogManager.Handlers.Add(new ExitCommandHandler());
            dialogManager.Handlers.Add(new LogCommandHandler());

            if (_commandHandler)
            {
                dialogManager.Handlers.Add(_commandHandler);
            }
        }

        Playing = true;
    }

    private static void SetPersonality(Tutor joao, Tutor maria)
    {
        joao.Personality = new ExpectancyPersonality(new Emotion[3, 6]
        {
            {
                /* Negative */
                new Emotion(EmotionEnum.Anger, .5f), // Punishment Worse Than Expected
                new Emotion(EmotionEnum.Neutral, 1f), // Punishment As Expected
                new Emotion(EmotionEnum.Surprise, .2f), // Punishment Better Than Expected
                new Emotion(EmotionEnum.Anger, .2f), // Punishment Worse Than Expected
                new Emotion(EmotionEnum.Neutral, 1f), // Punishment As Expected
                new Emotion(EmotionEnum.Surprise, .8f) // Punishment Better Than Expected
            },
            {
                /* Neutral */
                new Emotion(EmotionEnum.Sadness, .3f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .6f),
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .8f)
            },
            {
                /* Positive */
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .2f),
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, 1f)
            }
        });
        maria.Personality = new ExpectancyPersonality(new Emotion[3, 6]
        {
            {
                /* Negative */
                new Emotion(EmotionEnum.Anger, .5f), // Punishment Worse Than Expected
                new Emotion(EmotionEnum.Neutral, 1f), // Punishment As Expected
                new Emotion(EmotionEnum.Surprise, .2f), // Punishment Better Than Expected
                new Emotion(EmotionEnum.Anger, .2f), // Punishment Worse Than Expected
                new Emotion(EmotionEnum.Neutral, 1f), // Punishment As Expected
                new Emotion(EmotionEnum.Surprise, .8f) // Punishment Better Than Expected
            },
            {
                /* Neutral */
                new Emotion(EmotionEnum.Sadness, .3f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .6f),
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .8f)
            },
            {
                /* Positive */
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .2f),
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, 1f)
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Playing)
        {
            _manager.Update();
        }
    }

    void OnApplicationQuit()
    {
        PersistentDataStorage.Instance.SaveState();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        PersistentDataStorage.Instance.SaveState();
    }
}
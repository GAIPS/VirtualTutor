using System;
using SimpleJSON;
using UnityEngine;
using Utilities;
using Yarn;
using YarnDialog;

public class VT_Main : MonoBehaviour
{
    private SystemManager _manager;

    public TextAsset[] YarnDialogDatabase;

    public VTToModuleBridge ModuleManager;

    public string[] Intentions;

    [SerializeField] private MenuCommandHandler _commandHandler;

    public bool Playing;

    // Use this for initialization
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        DebugLog.Clean();
        DebugLog.Add(new UnityDebugLogger());
        
//        PersistentDataStorage.Instance.ResetState();

        _manager = new SystemManager();

        Tutor joao = new Tutor("Joao");
        Tutor maria = new Tutor("Maria");
        SetPersonality(joao, maria);
        _manager.Tutors.Add(joao);
        _manager.Tutors.Add(maria);

        BasicYarnDialogSelector dialogSelector = null;
        // Setup Dialog Selector
        if (YarnDialogDatabase != null)
        {
            string[] yarnFilesContent = new string[YarnDialogDatabase.Length];
            for (int i = 0; i < YarnDialogDatabase.Length; i++)
            {
                yarnFilesContent[i] = YarnDialogDatabase[i].text;
            }

            dialogSelector = new BasicYarnDialogSelector(yarnFilesContent);

            _manager.DialogSelector = dialogSelector;
        }
        
        {
            EmotivectorAppraisal appraisal = new EmotivectorAppraisal();
            IPredictor predictor = new AdditiveSecondDerivativePredictor(new WeightedMovingAveragePredictor(),
                new WeightedMovingAveragePredictor(), new WeightedMovingAveragePredictor());
            {
                AffectiveUpdater updater = new GradesAffectiveUpdater {Emotivector = new Emotivector(predictor)};
                appraisal.AddUpdater(updater);
            }
            {
                AffectiveUpdater updater = new StudyHoursAffectiveUpdater {Emotivector = new Emotivector(predictor)};
                appraisal.AddUpdater(updater);
            }
            _manager.AffectiveAppraisal = appraisal;
        }

        {
            // Setup Empathic Strategy
            _manager.EmpathicStrategySelector = new BaseStrategySelector();
            VariableStorage storage = null;
            if (dialogSelector != null)
            {
                storage = dialogSelector.VariableStorage;
            }

            var welcome = new TaskStrategy
            {
                VariableStorage = storage,
                Name = "Welcome",
                NodeName = "welcome",
                BeginDate = new DateTime(2018, 6, 18, 0, 0, 0)
            };
            _manager.Strategies.Add(welcome);
            var userID = new TaskStrategy
            {
                VariableStorage = storage,
                Name = "UserID",
                NodeName = "UserID",
                BeginDate = new DateTime(2018, 6, 18, 0, 0, 0)
            };
            _manager.Strategies.Add(userID);
            var af1Studyhours = new TaskStrategy
            {
                VariableStorage = storage,
                Name = "AF1StudyHours",
                NodeName = "af1studyhours",
                BeginDate = new DateTime(2018, 6, 19, 0, 0, 0)
            };
            _manager.Strategies.Add(af1Studyhours);
            var af1Grade = new TaskStrategy
            {
                VariableStorage = storage,
                Name = "AF1Grades",
                NodeName = "af1grades",
//                BeginDate = new DateTime(2018, 6, 19, 0, 0, 0)
                BeginDate = new DateTime(2018, 6, 21, 0, 0, 0)
            };
            af1Grade.DependsOn.Add(af1Studyhours);
            _manager.Strategies.Add(af1Grade);
            var af2Studyhours = new TaskStrategy
            {
                VariableStorage = storage,
                Name = "AF2StudyHours",
                NodeName = "af2studyhours",
//                BeginDate = new DateTime(2018, 6, 19, 0, 0, 0)
                BeginDate = new DateTime(2018, 6, 22, 0, 0, 0)
            };
            _manager.Strategies.Add(af2Studyhours);
            var af2Grade = new TaskStrategy
            {
                VariableStorage = storage,
                Name = "AF2Grades",
                NodeName = "af2grades",
//                BeginDate = new DateTime(2018, 6, 19, 0, 0, 0)
                BeginDate = new DateTime(2018, 6, 23, 0, 0, 0)
            };
            af2Grade.DependsOn.Add(af2Studyhours);
            _manager.Strategies.Add(af2Grade);
            var af3Studyhours = new TaskStrategy
            {
                VariableStorage = storage,
                Name = "AF3StudyHours",
                NodeName = "af3studyhours",
//                BeginDate = new DateTime(2018, 6, 19, 0, 0, 0)
                BeginDate = new DateTime(2018, 6, 23, 0, 0, 0)
            };
            _manager.Strategies.Add(af3Studyhours);
            var af3Grade = new TaskStrategy
            {
                VariableStorage = storage,
                Name = "AF3Grades",
                NodeName = "af3grades",
//                BeginDate = new DateTime(2018, 6, 19, 0, 0, 0)
                BeginDate = new DateTime(2018, 6, 25, 0, 0, 0)
            };
            af3Grade.DependsOn.Add(af3Studyhours);
            _manager.Strategies.Add(af3Grade);
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
            dialogManager.Handlers.Add(new SequenceLineHandler());

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
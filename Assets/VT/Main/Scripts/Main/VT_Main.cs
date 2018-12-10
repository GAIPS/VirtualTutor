using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Utilities;
using YarnDialog;

public class VT_Main : MonoBehaviour
{
    private SystemManager _manager;

    public bool StoreDataOnline;

    public TextAsset[] TutorsPersonality;

    public TextAsset Tasks;

    public TextAsset[] YarnDialogDatabase;

    public VTToModuleBridge ModuleManager;

    [SerializeField] private MenuCommandHandler _commandHandler;

    public bool Playing;

    [SerializeField] private GameObject _activityMenuPrefab;
    [SerializeField] private GameObject _loginMenuPrefab;

    [SerializeField] private WebManager _webManager;

    // Use this for initialization
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        DebugLog.Clean();
        DebugLog.Add(new UnityDebugLogger());

        PersistentDataStorage.Instance.StoreDataOnline = StoreDataOnline;

        IDataStorage dataStorage = PersistentDataStorage.Instance;

        _manager = new SystemManager();

        Tutor joao = new Tutor("Joao")
        {
            Personality = new ExpectancyPersonality(TutorsPersonality[0].text)
        };
        Tutor maria = new Tutor("Maria")
        {
            Personality = new ExpectancyPersonality(TutorsPersonality[1].text)
        };
        _manager.Tutors.Add(joao);
        _manager.Tutors.Add(maria);

        {
            EmotivectorAppraisal appraisal = new EmotivectorAppraisal();
            IPredictor predictor = new AdditiveSecondDerivativePredictor(new WeightedMovingAveragePredictor(),
                new WeightedMovingAveragePredictor(), new WeightedMovingAveragePredictor());
            {
                AffectiveUpdater updater = new GradesAffectiveUpdater
                    {Emotivector = new Emotivector("objective-performance", predictor)};
                appraisal.AddUpdater(updater);
            }
            {
                AffectiveUpdater updater = new StudyHoursAffectiveUpdater
                    {Emotivector = new Emotivector("objective-effort", predictor)};
                appraisal.AddUpdater(updater);
            }
            {
                AffectiveUpdater updater = new VisitsAffectiveUpdater
                    {Emotivector = new Emotivector("objective-engagement", predictor)};
                appraisal.AddUpdater(updater);
            }
            // TODO Add updaters to all the metrics
            _manager.AffectiveAppraisal = appraisal;
        }

//        {
//            
//            // Setup Empathic Strategy
//            _manager.EmpathicStrategySelector = new BaseStrategySelector();
//
//            var strategies = TaskFactory.FromJson(Tasks.text);
//            foreach (var taskStrategy in strategies)
//            {
//                taskStrategy.DataStorage = dataStorage;
//                _manager.Strategies.Add(taskStrategy);
//            }
//        }

        {
            // Setup Empathic Strategy
            _manager.EmpathicStrategySelector = new ExpectancyStrategySelector();
            
            // Welcoming and Goodbye
            {
                var strategy = new BasicStrategy();
                strategy.Name = "welcome";
                strategy.Intentions.Add(new Intention("welcome"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "exit";
                strategy.Intentions.Add(new Intention("exit"));
                _manager.Strategies.Add(strategy);
            }
            
            // Theme Introduction
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-challenging-intro";
                strategy.Intentions.Add(new Intention("subjective-challenging-intro"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-enjoyment-intro";
                strategy.Intentions.Add(new Intention("subjective-enjoyment-intro"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-importance-intro";
                strategy.Intentions.Add(new Intention("subjective-importance-intro"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-performance-intro";
                strategy.Intentions.Add(new Intention("objective-performance-intro"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-effort-intro";
                strategy.Intentions.Add(new Intention("objective-effort-intro"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-engagement-intro";
                strategy.Intentions.Add(new Intention("objective-engagement-intro"));
                _manager.Strategies.Add(strategy);
            }

            // Subjective - Challenging
            // Reward
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-challenging-reward-as-expected";
                strategy.Intentions.Add(new Intention("summary"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-challenging-reward-greater";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("rest"));
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-challenging-reward-lesser";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("help"));
                _manager.Strategies.Add(strategy);
            }
            // Punishment
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-challenging-punishment-as-expected";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("change-view"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-challenging-punishment-lesser";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("rest"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-challenging-punishment-greater";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }

            // Subjective - Enjoyment
            // Reward
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-enjoyment-reward-as-expected";
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-enjoyment-reward-greater";
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-enjoyment-reward-lesser";
                strategy.Intentions.Add(new Intention("change-view"));
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            // Punishment
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-enjoyment-punishment-as-expected";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-enjoyment-punishment-lesser";
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-enjoyment-punishment-greater";
                strategy.Intentions.Add(new Intention("help"));
                _manager.Strategies.Add(strategy);
            }

            // Subjective - Importance
            // Reward
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-importance-reward-as-expected";
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-importance-reward-greater";
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-importance-reward-lesser";
                strategy.Intentions.Add(new Intention("help"));
                _manager.Strategies.Add(strategy);
            }
            // Punishment
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-importance-punishment-as-expected";
                strategy.Intentions.Add(new Intention("change-view"));
                strategy.Intentions.Add(new Intention("help"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-importance-punishment-lesser";
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "subjective-importance-punishment-greater";
                strategy.Intentions.Add(new Intention("help"));
                _manager.Strategies.Add(strategy);
            }

            // Objective - Performance
            // Reward
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-performance-reward-as-expected";
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-performance-reward-greater";
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-performance-reward-lesser";
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            // Punishment
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-performance-punishment-as-expected";
                strategy.Intentions.Add(new Intention("change-view"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-performance-punishment-lesser";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-performance-punishment-greater";
                strategy.Intentions.Add(new Intention("change-view"));
                _manager.Strategies.Add(strategy);
            }

            // Objective - Effort
            // Reward
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-effort-reward-as-expected";
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-effort-reward-greater";
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-effort-reward-lesser";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            // Punishment
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-effort-punishment-as-expected";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("help"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-effort-punishment-lesser";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-effort-punishment-greater";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("change-view"));
                _manager.Strategies.Add(strategy);
            }

            // Objective - Engagement
            // Reward
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-engagement-reward-as-expected";
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-engagement-reward-greater";
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-engagement-reward-lesser";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("believe-you"));
                _manager.Strategies.Add(strategy);
            }
            // Punishment
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-engagement-punishment-as-expected";
                strategy.Intentions.Add(new Intention("summary"));
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-engagement-punishment-lesser";
                strategy.Intentions.Add(new Intention("capabilities"));
                _manager.Strategies.Add(strategy);
            }
            {
                var strategy = new BasicStrategy();
                strategy.Name = "objective-engagement-punishment-greater";
                strategy.Intentions.Add(new Intention("change-view"));
                _manager.Strategies.Add(strategy);
            }
        }

        // Setup Dialog Selector
        if (YarnDialogDatabase != null)
        {
            string[] yarnFilesContent = new string[YarnDialogDatabase.Length];
            for (int i = 0; i < YarnDialogDatabase.Length; i++)
            {
                yarnFilesContent[i] = YarnDialogDatabase[i].text;
            }

            var dialogSelector =
                new TagYarnDialogSelector(new PersistentVariableStorage(dataStorage), yarnFilesContent);

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

//        var state = dataStorage.GetState();
//        state["Current"].AsObject["Activity"] = "Test";
//        var activity = state["Activities"].AsObject["Test"].AsObject;
//        activity["Name"] = "Test Activity";
//        var checkpoints = activity["Checkpoints"].AsArray;
//        {
//            var checkpoint = new JSONObject();
//            checkpoint["Type"] = "Checkbox";
//            checkpoint["Name"] = "Test Checkpoint";
//            checkpoint["Date"] = "23/07/2018";
//            checkpoint["Effort"] = .5f;
//            checkpoint["Importance"] = .7f;
//            checkpoint["CheckboxDone"] = true;
//            checkpoints[0] = checkpoint;
//        }
//        {
//            var checkpoint = new JSONObject();
//            checkpoint["Type"] = "Evaluation";
//            checkpoint["Name"] = "Test Evaluation";
//            checkpoint["Date"] = "23/07/2018";
//            checkpoint["Effort"] = .7f;
//            checkpoint["Importance"] = .5f;
//            checkpoint["EvaluationScore"] = null;
//            checkpoints[1] = checkpoint;
//        }
//        {
//            var checkpoint = new JSONObject();
//            checkpoint["Type"] = "Evaluation";
//            checkpoint["Name"] = "Test Evaluation 2";
//            checkpoint["Date"] = "23/07/2018";
//            checkpoint["Effort"] = .7f;
//            checkpoint["Importance"] = .5f;
//            checkpoint["EvaluationScore"] = 16;
//            checkpoints[2] = checkpoint;
//        }

        ActivityMenuController activityMenuController = new ActivityMenuController();
        activityMenuController.MenuPrefab = _activityMenuPrefab;

        MoodleLoginController loginController = new MoodleLoginController(_webManager);
        loginController.MenuPrefab = _loginMenuPrefab;

        if (_commandHandler)
        {
            _commandHandler.controllers.Add(activityMenuController);
            _commandHandler.controllers.Add(loginController);
        }

        Playing = true;
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
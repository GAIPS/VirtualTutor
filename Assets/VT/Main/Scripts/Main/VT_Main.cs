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
                AffectiveUpdater updater = new GradesAffectiveUpdater {Emotivector = new Emotivector(predictor)};
                appraisal.AddUpdater(updater);
            }
            {
                AffectiveUpdater updater = new StudyHoursAffectiveUpdater {Emotivector = new Emotivector(predictor)};
                appraisal.AddUpdater(updater);
            }
            // TODO Add updaters to all the metrics
            _manager.AffectiveAppraisal = appraisal;
        }

        {
            // Setup Empathic Strategy
            _manager.EmpathicStrategySelector = new BaseStrategySelector();

            var strategies = TaskFactory.FromJson(Tasks.text);
            foreach (var taskStrategy in strategies)
            {
                taskStrategy.DataStorage = dataStorage;
                _manager.Strategies.Add(taskStrategy);
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
                new BasicYarnDialogSelector(new PersistentVariableStorage(dataStorage), yarnFilesContent);

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
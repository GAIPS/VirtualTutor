using System.Collections.Generic;
using System.IO;
using System.Linq;
using HookControl;
using UnityEngine;
using Utilities;
using YarnDialog;

public class Preview : MonoBehaviour
{
    private SystemManager _manager;

    public TextAsset[] YarnDialogDatabase;

    public VTToModuleBridge moduleManager;

    public string IntentionName;

    public bool Playing = true;

    [SerializeField] private GameObject _stringInput;

    // Use this for initialization
    void Start()
    {
        DebugLog.logger = new UnityDebugLogger();

        _manager = new SystemManager();

        Tutor joao = new Tutor("Joao");
        Tutor maria = new Tutor("Maria");
        _manager.Tutors.Add(joao);
        _manager.Tutors.Add(maria);

        {
            // Setup Affective Appraisal
            ModularAffectiveAppraisal appraisal = new ModularAffectiveAppraisal(
                new UserAA_OneEmotion(new Emotion(EmotionEnum.Happiness,
                    0.2f)),
                new TutorAA_CopyUser()
            );
            _manager.AffectiveAppraisal = appraisal;
        }

        {
            // Setup Empathic Strategy
            _manager.EmpathicStrategySelector = new SS_SelectFirst();
//            BasicStrategy strategy = new BasicStrategy();
//            strategy.Intentions.Add(new Intention(IntentionName));
//
//            _manager.Strategies.Add(strategy);
        }

        {
            // Setup Dialog Selector
            if (YarnDialogDatabase != null)
            {
//                string[] yarnFilesContent = new string[YarnDialogDatabase.Length];
                IList<string> yarnFilesContent = new List<string>();
                for (int i = 0; i < YarnDialogDatabase.Length; i++)
                {
                    yarnFilesContent.Add(YarnDialogDatabase[i].text);
                }

                DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
                DirectoryInfo parent = directory.Parent;

                yarnFilesContent = yarnFilesContent.Concat(ReadFiles(directory))
                    .Concat(ReadFiles(parent)).ToList();

                var dialogSelector = new YarnPreviewDialogSelector(yarnFilesContent.ToArray());

                _manager.DialogSelector = dialogSelector;
            }
        }

        {
            // Setup Dialog Manager
            var dialogManager = new YarnDialogManager(false);
            _manager.DialogManager = dialogManager;
            dialogManager.Tutors.Add(joao);
            dialogManager.Tutors.Add(maria);
            dialogManager.ModuleManager = this.moduleManager;


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
        }

        if (_stringInput)
        {
            Playing = false;
            Control control = new Control(_stringInput);
            if (control.Show() == ShowResult.FIRST)
            {
                StringInputHook hook = control.instance.GetComponent<StringInputHook>();

                if (PlayerPrefs.HasKey("PreviousNode"))
                {
                    hook.SetName(PlayerPrefs.GetString("PreviousNode"));
                }

                hook.OnSubmit = node =>
                {
                    control.Destroy();
                    Playing = true;

                    PlayerPrefs.SetString("PreviousNode", node);
                    
                    BasicStrategy strategy = new BasicStrategy();
                    strategy.Intentions.Add(new Intention(node));
                    _manager.Strategies.Add(strategy);
                };
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Playing)
        {
            _manager.Update();
        }
    }

    private IList<string> ReadFiles(DirectoryInfo directory)
    {
        FileInfo[] dirFiles = directory.GetFiles("*.yarn.txt");
        IList<string> filesContent = new List<string>();
        foreach (FileInfo file in dirFiles)
        {
            filesContent.Add(File.ReadAllText(file.FullName));
        }

        return filesContent;
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HookControl;
using SFB;
using UnityEngine;
using Utilities;
using YarnDialog;

public class Preview : MonoBehaviour
{
    private SystemManager _manager;

    public TextAsset[] YarnDialogDatabase;

    public VTToModuleBridge ModuleManager;

    public bool Playing = true;

    [SerializeField] private GameObject _fileInput;

    [SerializeField] private GameObject _stringInput;

    [SerializeField] private PreviewDebugLogger _previewDebugLogger;

    // Use this for initialization
    void Start()
    {
        DebugLog.Clean();
        DebugLog.Add(new UnityDebugLogger());
        if (_previewDebugLogger != null)
        {
            _previewDebugLogger.Preview = this;
            DebugLog.Add(_previewDebugLogger);
        }

        Playing = false;
        FileSelectInput();
    }

    private void FileSelectInput()
    {
        if (!_fileInput)
        {
            DebugLog.Warn("No File Selector Menu not defined.");
            return;
        }

        Control fileControl = new Control(_fileInput);
        if (fileControl.Show() != ShowResult.FIRST)
        {
            DebugLog.Warn("Failed to show File Chooser menu.");
            return;
        }

        FileInputHook fileHook = fileControl.Instance.GetComponent<FileInputHook>();

        fileHook.SetTitle("Select Yarn File");
        string extension = "yarn.txt";
#if UNITY_STANDALONE_OSX
        extension = "txt";
#endif
        fileHook.DialogExtensions = extension;
        if (PlayerPrefs.HasKey("PreviousFile"))
        {
            string file = PlayerPrefs.GetString("PreviousFile");
            fileHook.SetName(file);
            fileHook.DialogDirectory = Path.GetDirectoryName(file);
        }
        else
        {
            string directory = Directory.GetCurrentDirectory();
#if UNITY_STANDALONE_OSX
            directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
#endif
            fileHook.DialogDirectory = directory;
        }

        fileHook.OnSubmit = file =>
        {
            if (string.IsNullOrEmpty(file))
            {
                DebugLog.Warn("File path is empty.");
                return;
            }

            fileControl.Destroy();
            PlayerPrefs.SetString("PreviousFile", file);
            NodeNameInput(file);
        };
    }

    private void NodeNameInput(string file)
    {
        if (!_stringInput)
        {
            DebugLog.Warn("No String Input Menu not defined.");
            return;
        }

        Control control = new Control(_stringInput);
        if (control.Show() != ShowResult.FIRST)
        {
            DebugLog.Warn("Failed to show Node Selector menu.");
            return;
        }

        StringInputHook hook = control.Instance.GetComponent<StringInputHook>();
        hook.SetTitle("Start Node Name");

        if (PlayerPrefs.HasKey("PreviousNode"))
        {
            hook.SetName(PlayerPrefs.GetString("PreviousNode"));
        }

        hook.OnSubmit = node =>
        {
            if (string.IsNullOrEmpty(node))
            {
                DebugLog.Warn("Node Name is empty.");
                return;
            }
            
            control.Destroy();
            Playing = true;

            PlayerPrefs.SetString("PreviousNode", node);

            InitTutors(file, node);
        };
    }

    private void InitTutors(string file, string node)
    {
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
            BasicStrategy strategy = new BasicStrategy();
            strategy.Intentions.Add(new Intention(node));
            _manager.Strategies.Add(strategy);
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

                yarnFilesContent = yarnFilesContent.Concat(ReadFiles(new[] {file})).ToList();

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
            dialogManager.ModuleManager = this.ModuleManager;


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

    private IList<string> ReadFiles(string[] files)
    {
        IList<string> filesContent = new List<string>();
        foreach (string file in files)
        {
            string filepath = file;
#if UNITY_STANDALONE_OSX
			filepath = filepath.Replace("file://", "");
			filepath = filepath.Replace("%20", " ");
#endif
			try {
				var content = File.ReadAllText (filepath);
				filesContent.Add(content);
			} catch (System.Exception ex) {
				DebugLog.Err ("Unable to read file.\n" + ex.Message);
			}
        }

        return filesContent;
    }

    private bool CheckFiles(string[] files)
    {
        if (files == null || files.Length == 0)
        {
            return false;
        }

        bool isValid = false;
        foreach (var file in files)
        {
            if (!string.IsNullOrEmpty(file))
            {
                isValid = true;
            }
        }

        return isValid;
    }
}
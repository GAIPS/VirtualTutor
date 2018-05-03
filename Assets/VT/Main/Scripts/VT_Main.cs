using UnityEngine;
using Utilities;
using YarnDialog;

public class VT_Main : MonoBehaviour
{
    private SystemManager manager;

    public TextAsset[] yarnDialogDatabase;

    public VTToModuleBridge moduleManager;

    public string[] intentions;
    public bool forceIntention;
    public string forceIntentionName;

    public bool playing = false;

    // Use this for initialization
    void Start()
    {
        DebugLog.logger = new UnityDebugLogger();

        manager = new SystemManager();

        Tutor joao = new Tutor("Joao");
        Tutor maria = new Tutor("Maria");
        manager.Tutors.Add(joao);
        manager.Tutors.Add(maria);

        {
            // Setup Affective Appraisal
            ModularAffectiveAppraisal appraisal = new ModularAffectiveAppraisal(
                new UserAA_OneEmotion(new Emotion(EmotionEnum.Happiness,
                    0.2f)),
                new TutorAA_CopyUser()
            );
            manager.AffectiveAppraisal = appraisal;
        }

        {
            // Setup Empathic Strategy
            manager.EmpathicStrategySelector = new SS_SelectFirst();
            BasicStrategy strategy = new BasicStrategy();
            if (forceIntention)
            {
                strategy.Intentions.Add(new Intention(forceIntentionName));
            }
            else
            {
                foreach (string intention in intentions)
                {
                    strategy.Intentions.Add(new Intention(intention));
                }
            }

            manager.Strategies.Add(strategy);
        }

        {
            // Setup Dialog Selector
            if (yarnDialogDatabase != null)
            {
                string[] yarnFilesContent = new string[yarnDialogDatabase.Length];
                for (int i = 0; i < yarnDialogDatabase.Length; i++)
                {
                    yarnFilesContent[i] = yarnDialogDatabase[i].text;
                }

                var dialogSelector = new YarnDialogSelector(yarnFilesContent);

                manager.DialogSelector = dialogSelector;
            }
        }

        {
            // Setup Dialog Manager
            var dialogManager = new YarnDialogManager(false);
            manager.DialogManager = dialogManager;
            dialogManager.Tutors.Add(joao);
            dialogManager.Tutors.Add(maria);
            dialogManager.ModuleManager = this.moduleManager;


            // Order matters
            //dialogManager.Handlers.Add(new ParallelLineHandler());
            dialogManager.Handlers.Add(new EmotionTagNodeHandler());
            dialogManager.Handlers.Add(new LogCommandHandler());
            dialogManager.Handlers.Add(new LogCompleteNodeHandler());
            dialogManager.Handlers.Add(new SequenceLineHandler());
            dialogManager.Handlers.Add(new SequenceOptionsHandler());
            dialogManager.Handlers.Add(new ExitCommandHandler());
        }

        playing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            manager.Update();
        }
    }
}
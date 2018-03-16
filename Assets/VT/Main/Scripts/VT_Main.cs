using UnityEngine;
using Utilities;

public class VT_Main : MonoBehaviour
{

    private SystemManager manager;

    public TextAsset yarnDialogDatabase;

    // Use this for initialization
    void Start()
    {
        DebugLog.logger = new UnityDebugLogger();

        manager = new SystemManager();

        // Setup Affective Appraisal
        ModularAffectiveAppraisal appraisal = new ModularAffectiveAppraisal(
                new UserAA_OneEmotion(new Emotion("Fear")),
                new TutorAA_CopyUser()
            );
        manager.AffectiveAppraisal = appraisal;

        // Setup Empathic Strategy
        manager.EmpathicStrategySelector = new SS_SelectFirst();
        Intention intention = new Intention("DiscussGrades");
        BasicStrategy strategy = new BasicStrategy();
        strategy.Intentions.Add(intention);
        manager.Strategies.Add(strategy);

        // Setup Dialog Selector
        if (yarnDialogDatabase != null)
        {
            var dialogSelector = new YarnDialogSelector(yarnDialogDatabase.text);

            manager.DialogSelector = dialogSelector;
        }


        // Setup Dialog Manager
        var dialogManager = new YarnDialogManager();
        manager.DialogManager = dialogManager;
    }
    
    // Update is called once per frame
    void Update()
    {
        // TODO When should I update the manager?
        //manager.Update();
    }
}

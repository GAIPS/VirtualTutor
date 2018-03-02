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
        Intention intention = new Intention("Review Grades");
        BasicStrategy strategy = new BasicStrategy();
        strategy.Intentions.Add(intention);
        manager.Strategies.Add(strategy);

        // Setup Dialog Selector


        // Setup Dialog Manager
        if (yarnDialogDatabase != null)
        {
            var dialogSelector = new YarnDialogSelector(yarnDialogDatabase.text);
            dialogSelector.LoadString();

            manager.DialogSelector = dialogSelector;

            // TESTING
            dialogSelector.SelectDialog(null, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO When should I update the manager?
        //manager.Update();
    }
}

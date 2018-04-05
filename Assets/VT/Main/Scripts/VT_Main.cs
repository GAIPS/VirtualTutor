using UnityEngine;
using Utilities;

public class VT_Main : MonoBehaviour {

    private SystemManager manager;

    public TextAsset yarnDialogDatabase;

    public AvatarManager headAnimationManager;

    // Use this for initialization
    void Start() {
        DebugLog.logger = new UnityDebugLogger();

        manager = new SystemManager();

        Tutor joao = new Tutor("Joao");
        Tutor maria = new Tutor("Maria");
        manager.Tutors.Add(joao);
        manager.Tutors.Add(maria);

        {
            // Setup Affective Appraisal
            ModularAffectiveAppraisal appraisal = new ModularAffectiveAppraisal(
                                                      new UserAA_OneEmotion(new Emotion(EmotionEnum.Sadness,
                                                          0.2f)),
                                                      new TutorAA_CopyUser()
                                                  );
            manager.AffectiveAppraisal = appraisal;
        }

        {
            // Setup Empathic Strategy
            manager.EmpathicStrategySelector = new SS_SelectFirst();
            Intention intention = new Intention("DiscussGrades");
            BasicStrategy strategy = new BasicStrategy();
            strategy.Intentions.Add(intention);
            manager.Strategies.Add(strategy);
        }

        {
            // Setup Dialog Selector
            if (yarnDialogDatabase != null) {
                var dialogSelector = new YarnDialogSelector(yarnDialogDatabase.text);

                manager.DialogSelector = dialogSelector;
            }
        }

        {
            // Setup Dialog Manager
            var dialogManager = new YarnDialogManager();
            dialogManager.HeadAnimationManager = this.headAnimationManager;
            manager.DialogManager = dialogManager;
        }

        // Testing
        manager.Update();
    }
    
    // Update is called once per frame
    void Update() {
        // TODO When should I update the manager?
        //manager.Update();
        manager.DialogManager.Update();
    }
}

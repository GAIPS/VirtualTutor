using System.Collections.Generic;

public class SystemManager
{
    public IAffectiveAppraisal affectiveAppraisal { get; set; }
    public IEmpathicStrategySelector empathicStrategySelector { get; set; }
    public IDialogSelector dialogSelector { get; set; }
    public IDialogManager dialogManager { get; set; }

    private ICollection<History> histories;
    private ICollection<IEmpathicStrategy> strategies;
    private ICollection<IDialogTree> dialogTreeDatabase;

    public void Setup()
    {
        // Load Interaction History
        histories = new List<History>();

        // Load Strategies
        strategies = new List<IEmpathicStrategy>();

        // Load Dialog Tree Database
        dialogTreeDatabase = new List<IDialogTree>();
    }

    public void Update()
    {
        // Placeholder content

        // Affective Appraisal
        Emotion userEmotion = affectiveAppraisal.ComputeUserEmotion(histories);
        Tutor joao = new Tutor(),
            maria = new Tutor();
        Emotion joaoEmotion = affectiveAppraisal.ComputeTutorEmotion(histories, userEmotion, joao);
        Emotion mariaEmotion = affectiveAppraisal.ComputeTutorEmotion(histories, userEmotion, maria);

        // Empathic Strategy
        Intention intention = empathicStrategySelector.SelectIntention(histories, strategies, userEmotion);

        // Dialog Selector
        IDialogTree dialogTree = dialogSelector.SelectDialog(histories, intention, dialogTreeDatabase);

        dialogManager.SetDialogTree(dialogTree);
        dialogManager.SetTutorEmotion(joao, joaoEmotion);
        dialogManager.SetTutorEmotion(maria, mariaEmotion);

        dialogManager.Update();
    }
}

using System.Collections.Generic;

public class SystemManager
{
    public IAffectiveAppraisal AffectiveAppraisal { get; set; }
    public IEmpathicStrategySelector EmpathicStrategySelector { get; set; }
    public IDialogSelector DialogSelector { get; set; }
    public IDialogManager DialogManager { get; set; }

    private User user;
    private Tutor joao, maria;

    private History history;
    private ICollection<IEmpathicStrategy> strategies;
    private ICollection<IDialogTree> dialogTreeDatabase;

    // Placeholder content

    public void Setup()
    {
        // Initialize User
        user = new User();

        // Initialize Tutors
        joao = new Tutor();
        maria = new Tutor();

        // Load Interaction History
        history = new History();

        // Load Strategies
        strategies = new List<IEmpathicStrategy>();

        // Load Dialog Tree Database
        dialogTreeDatabase = new List<IDialogTree>();
    }

    public void Update()
    {
        // Affective Appraisal
        AffectiveAppraisal.ComputeUserEmotion(history, user);
        AffectiveAppraisal.ComputeTutorEmotion(history, user, joao);
        AffectiveAppraisal.ComputeTutorEmotion(history, user, maria);

        // Empathic Strategy
        Intention intention = EmpathicStrategySelector.SelectIntention(history, strategies, user);

        // Dialog Selector
        IDialogTree dialogTree = DialogSelector.SelectDialog(history, intention, dialogTreeDatabase);

        DialogManager.SetDialogTree(dialogTree);
        DialogManager.SetTutorEmotion(joao);
        DialogManager.SetTutorEmotion(maria);

        DialogManager.Update();
    }
}

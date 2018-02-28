using System.Collections.Generic;

public class SystemManager
{
    public IAffectiveAppraisal AffectiveAppraisal { get; set; }
    public IEmpathicStrategySelector EmpathicStrategySelector { get; set; }
    public IDialogSelector DialogSelector { get; set; }
    public IDialogManager DialogManager { get; set; }

    public User User { get; set; }
    public ICollection<Tutor> Tutors { get; set; }

    public History History { get; set; }
    public ICollection<IEmpathicStrategy> Strategies { get; set; }

    // Placeholder content

    public SystemManager()
    {
        // Initialize User
        User = new User();

        // Initialize Tutors
        Tutors = new List<Tutor>();

        // Load Interaction History
        History = new History();

        // Load Strategies
        Strategies = new List<IEmpathicStrategy>();
    }

    public void Update()
    {
        // Affective Appraisal
        AffectiveAppraisal.ComputeUserEmotion(History, User);
        foreach (Tutor tutor in Tutors)
        {
            AffectiveAppraisal.ComputeTutorEmotion(History, User, tutor);
        }

        // Empathic Strategy
        Intention intention = EmpathicStrategySelector.SelectIntention(History, Strategies, User);

        // Dialog Selector
        IDialogTree dialogTree = DialogSelector.SelectDialog(History, intention);

        DialogManager.SetDialogTree(dialogTree);
        foreach (Tutor tutor in Tutors)
        {
            DialogManager.SetTutorEmotion(tutor);
        }

        DialogManager.Update();
    }
}

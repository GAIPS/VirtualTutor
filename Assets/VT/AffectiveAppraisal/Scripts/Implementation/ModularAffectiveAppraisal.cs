
public class ModularAffectiveAppraisal : IAffectiveAppraisal
{

    public IUserAffectiveAppraisal UserAffectiveAppraisal { get; set; }
    public ITutorAffectiveAppraisal TutorAffectiveAppraisal { get; set; }

    public ModularAffectiveAppraisal() { }

    public ModularAffectiveAppraisal(IUserAffectiveAppraisal userAffectiveAppraisal, ITutorAffectiveAppraisal tutorAffectiveAppraisal)
    {
        UserAffectiveAppraisal = userAffectiveAppraisal;
        TutorAffectiveAppraisal = tutorAffectiveAppraisal;
    }

    public void ComputeTutorEmotion(History history, User user, Tutor tutor)
    {
        if (TutorAffectiveAppraisal == null) return;

        TutorAffectiveAppraisal.ComputeTutorEmotion(history, user, tutor);
    }

    public void ComputeUserEmotion(History history, User user)
    {
        if (UserAffectiveAppraisal == null) return;

        UserAffectiveAppraisal.ComputeUserEmotion(history, user);
    }
}

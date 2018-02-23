public class TutorAA_CopyUser : ITutorAffectiveAppraisal
{
    public void ComputeTutorEmotion(History history, User user, Tutor tutor)
    {
        if (user == null || tutor == null)
        {
            return;
        }

        tutor.Emotion = user.Emotion;
    }
}


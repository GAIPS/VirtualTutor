public class UserAA_OneEmotion : IUserAffectiveAppraisal
{
    public Emotion Emotion { get; set; }

    public UserAA_OneEmotion() { }

    public UserAA_OneEmotion(Emotion emotion)
    {
        Emotion = emotion;
    }

    public void ComputeUserEmotion(History history, User user)
    {
        user.Emotion = Emotion;
    }
}


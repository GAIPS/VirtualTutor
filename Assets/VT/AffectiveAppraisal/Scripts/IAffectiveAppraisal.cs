using System.Collections.Generic;

public interface IAffectiveAppraisal {

    Emotion ComputeUserEmotion(ICollection<History> history);

    Emotion ComputeTutorEmotion(ICollection<History> history, Emotion userEmotion, Tutor tutor);
}

using System.Collections.Generic;

public interface IAffectiveAppraisal {

    Emotion ComputeUserEmotion(List<History> history);

    Emotion ComputeTutorEmotion(List<History> history, Tutor tutor);
}

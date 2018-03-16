
using System.Collections;

public interface IDialogManager {
    void SetDialogTree(IDialogTree dialogTree);
    void SetTutorEmotion(Tutor tutor);
    void Update();
    void Reset();
}

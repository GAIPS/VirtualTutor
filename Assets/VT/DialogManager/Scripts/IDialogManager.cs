
public interface IDialogManager {
    void SetDialogTree(IDialogTree dialogTree);
    void SetTutorEmotion(Tutor tutor);
    /// <summary>
    /// Updates the dialog to further interact with the user.
    /// </summary>
    /// <returns>True if a new Appraisal is needed, False otherwise.</returns>
    bool Update();
    void Reset();
}

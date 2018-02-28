public interface IDialogSelector
{
    /// <summary>
    /// Returns the best Dialog Tree for the given Intention and previous interactions
    /// </summary>
    /// <param name="history">Previous interactions with the user</param>
    /// <param name="intention">Empathic Strategy intention to contact the user</param>
    /// <returns>Dialog Tree with the correct intention</returns>
    IDialogTree SelectDialog(History history, Intention intention);
}

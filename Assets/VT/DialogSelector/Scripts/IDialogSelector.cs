using System.Collections.Generic;

public interface IDialogSelector {
    IDialogTree SelectDialog(ICollection<History> history, Intention intention, ICollection<IDialogTree> DialogDatabase);
}

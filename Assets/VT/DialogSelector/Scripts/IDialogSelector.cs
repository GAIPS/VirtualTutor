using System.Collections.Generic;

public interface IDialogSelector {
    IDialogTree SelectDialog(History history, Intention intention, ICollection<IDialogTree> DialogDatabase);
}

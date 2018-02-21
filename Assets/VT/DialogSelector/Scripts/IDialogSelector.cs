using System.Collections.Generic;

public interface IDialogSelector {
    IDialogTree SelectDialog(List<History> history, Intention intention, List<IDialogTree> DialogDatabase);
}

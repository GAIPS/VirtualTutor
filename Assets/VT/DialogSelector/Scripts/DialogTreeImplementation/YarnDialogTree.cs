
public class YarnDialogTree : IDialogTree
{
    private Yarn.Dialogue dialogue;

    private string startNode = Yarn.Dialogue.DEFAULT_START;

    public YarnDialogTree(Yarn.Dialogue dialogue, string startNode)
    {
        this.dialogue = dialogue;
        this.startNode = startNode;
    }
}

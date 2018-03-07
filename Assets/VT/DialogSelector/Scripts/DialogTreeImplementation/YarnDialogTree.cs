
using System;
using System.Collections.Generic;
using Yarn;

public class YarnDialogTree : IDialogTree
{
    private Yarn.Dialogue dialogue;

    private string startNode = Yarn.Dialogue.DEFAULT_START;

    public YarnDialogTree(Yarn.Dialogue dialogue, string startNode)
    {
        this.dialogue = dialogue;
        this.startNode = startNode;
    }

    public Dialogue Dialogue {
        get {
            return dialogue;
        }

        set {
            dialogue = value;
        }
    }

    public override bool Equals(object obj)
    {
        var tree = obj as YarnDialogTree;
        return tree != null &&
               EqualityComparer<Dialogue>.Default.Equals(dialogue, tree.dialogue) &&
               startNode == tree.startNode;
    }

    public override int GetHashCode()
    {
        var hashCode = -795588566;
        hashCode = hashCode * -1521134295 + EqualityComparer<Dialogue>.Default.GetHashCode(dialogue);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(startNode);
        return hashCode;
    }

    public override string ToString()
    {
        return String.Format("Start Node {0} of Dialogue {1}", this.startNode, this.dialogue);
    }
}


using System;
using System.Collections.Generic;
using Utilities;

public class YarnDialogSelector : IDialogSelector
{
    protected Yarn.VariableStorage variableStorage;

    protected Yarn.Dialogue dialogue;

    public string YarnFileContent { get; set; }

    public YarnDialogSelector()
    {
        variableStorage = new Yarn.MemoryVariableStore();
        dialogue = new Yarn.Dialogue(variableStorage)
        {
            LogDebugMessage = delegate (string message)
            {
                DebugLog.Log(message);
            },
            LogErrorMessage = delegate (string message)
            {
                DebugLog.Err(message);
            }
        };
    }

    public YarnDialogSelector(string fileContent) : this()
    {
        YarnFileContent = fileContent;
        dialogue.LoadString(YarnFileContent);
    }
    
    public IDialogTree SelectDialog(History history, Intention intention)
    {
        if (intention == null)
        {
            DebugLog.Warn("Intention is null. Returning null.");
            return null;
        }
        // Get all nodes
        IEnumerable<string> allNodes = dialogue.allNodes;
        // Filter to only start nodes
        List<string> startNodes = GetNodesStartWith(allNodes, "Start.");
        // Select a dialog tree
        YarnDialogTree dialogTree = SelectDialogYarn(history, intention, startNodes);
        if (dialogTree == null)
        {
            DebugLog.Warn("No Dialog Tree was created. Returning null."); 
        }
        return dialogTree;
    }

    private static List<string> GetNodesStartWith(IEnumerable<string> allNodes, string startWith)
    {
        List<string> startNodes = new List<string>();
        foreach (string nodeName in allNodes)
        {
            if (nodeName.StartsWith("Start."))
            {
                startNodes.Add(nodeName);
            }
        }

        return startNodes;
    }

    /// <summary>
    /// Select a dialog tree based on the start nodes of the dialog database.
    /// 
    /// You can also search the tags of the node by using `dialog.GetTagsFromNode(nodeName)`
    /// </summary>
    /// <param name="history">History of previous and current interactions</param>
    /// <param name="intention">Strategy intention of the interaction</param>
    /// <param name="startNodes">List of nodes to select from.</param>
    /// <returns>Dialog tree that contains information on which start node to begin from.</returns>
    protected virtual YarnDialogTree SelectDialogYarn(History history, Intention intention, List<string> startNodes)
    {
        // Filter by name
        foreach (string node in startNodes)
        {
            string filterNode = node.Replace("Start.", "");
            // Check if the intention matches with node name.
            if (filterNode.Contains(intention.Name))
            {
                return new YarnDialogTree(dialogue, node);
            }
        }
        return null;
    }


    // UTILITY
    protected static void DebugNodes(Yarn.Dialogue dialogue)
    {
        IEnumerable<string> allNodes = dialogue.allNodes;
        List<string> startNodes = GetNodesStartWith(allNodes, "Start.");

        string debugLog = "Start Nodes\n";
        foreach (string node in startNodes)
        {
            debugLog += '\t' + node + '\n';
            IEnumerable<string> tags = dialogue.GetTagsForNode(node);
            foreach (var tag in tags)
            {
                debugLog += "\t\t" + tag + '\n';
            }
        }
        DebugLog.Log(debugLog);
    }
}

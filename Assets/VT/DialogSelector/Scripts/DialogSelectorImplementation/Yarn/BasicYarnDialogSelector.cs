using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace YarnDialog
{
    public class BasicYarnDialogSelector: IDialogSelector
    {
        public Yarn.VariableStorage VariableStorage;

        public Yarn.Dialogue Dialogue;

        public BasicYarnDialogSelector()
        {
            VariableStorage = new PersistentVariableStorage();
            Dialogue = new Yarn.Dialogue(VariableStorage)
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

        public BasicYarnDialogSelector(string fileContent) : this()
        {
            Dialogue.LoadString(fileContent);
        }

        public BasicYarnDialogSelector(string[] fileContent) : this()
        {
            foreach (var content in fileContent)
            {
                Dialogue.LoadString(content);
            }
        }

        public IDialogTree SelectDialog(History history, Intention intention)
        {
            if (intention == null)
            {
                DebugLog.Warn("Intention is null. Returning null.");
                return null;
            }
            // Get all nodes
            IEnumerable<string> allNodes = Dialogue.allNodes;
            // Select a dialog tree
            YarnDialogTree dialogTree = SelectDialogYarn(history, intention, allNodes.ToList());
            if (dialogTree == null)
            {
                DebugLog.Warn("No Dialog Tree was created. Returning null. Given intention is " + intention.Name);
            }
            return dialogTree;
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
                // Check if the intention matches with node name.
                if (node.Equals(intention.Name))
                {
                    return new YarnDialogTree(Dialogue, node);
                }
            }
            return null;
        }
    }
}
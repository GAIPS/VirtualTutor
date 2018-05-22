using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace YarnDialog
{
    public class YarnPreviewDialogSelector : IDialogSelector
    {
        protected Yarn.VariableStorage variableStorage;

        protected Yarn.Dialogue dialogue;

        public YarnPreviewDialogSelector()
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

        public YarnPreviewDialogSelector(string fileContent) : this()
        {
            dialogue.LoadString(fileContent);
        }

        public YarnPreviewDialogSelector(string[] fileContent) : this()
        {
            foreach (var content in fileContent)
            {
                dialogue.LoadString(content);
            }
        }

        public IDialogTree SelectDialog(History history, Intention intention)
        {
            if (intention == null)
            {
                DebugLog.Warn("Intention is null. Returning null.");
                return null;
            }
            // Select a dialog tree
            YarnDialogTree dialogTree = SelectDialogYarn(history, intention, dialogue.allNodes.ToList());
            if (dialogTree == null)
            {
                DebugLog.Warn("No Dialog Tree was created. Returning null.");
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
                if (node.Contains(intention.Name))
                {
                    return new YarnDialogTree(dialogue, node);
                }
            }
            return null;
        }


        // UTILITY
        protected static void DebugNodes(Yarn.Dialogue dialogue)
        {
            List<string> startNodes = dialogue.allNodes.ToList();

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
}
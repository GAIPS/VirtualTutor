using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace YarnDialog
{
    public class TagYarnDialogSelector : IDialogSelector
    {
        public Yarn.VariableStorage VariableStorage;

        public Yarn.Dialogue Dialogue;

        public TagYarnDialogSelector(Yarn.VariableStorage variableStorage)
        {
            VariableStorage = variableStorage;
            Dialogue = new Yarn.Dialogue(VariableStorage)
            {
                LogDebugMessage = delegate(string message) { DebugLog.Log(message); },
                LogErrorMessage = delegate(string message) { DebugLog.Err(message); }
            };
        }

        public TagYarnDialogSelector(string[] fileContent) : this(new Yarn.MemoryVariableStore())
        {
            foreach (var content in fileContent)
            {
                Dialogue.LoadString(content);
            }
        }

        public TagYarnDialogSelector(Yarn.VariableStorage variableStorage, string[] fileContent) : this(variableStorage)
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
            var potencialNodes = new List<string>();
            // Filter by name
            foreach (string node in startNodes)
            {
                var tags = Dialogue.GetTagsForNode(node);
                var tagsList = tags.ToList();
                // TODO Remove to use Moodle
                if (tagsList.Contains("online")) continue;
                foreach (var tag in tagsList)
                {
                    if (intention.Name.ToLower().Contains(tag.ToLower()))
                    {
                        // a node can be inserted several times, the more tags that match with the name the more it is inserted.
                        potencialNodes.Add(node);
                    }
                }
            }

            if (potencialNodes.Count == 0) return null;

            // Shuffle list
            potencialNodes = potencialNodes.OrderBy(a => Guid.NewGuid()).ToList();
            // pick first
            string n = potencialNodes[0];

            return new YarnDialogTree(Dialogue, n);
        }
    }
}
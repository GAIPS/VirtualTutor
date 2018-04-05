using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;


public class YarnDialogManager : IDialogManager {
    YarnDialogTree dialogTree;
    IEnumerator<Yarn.Dialogue.RunnerResult> enumerator;

    public AvatarManager HeadAnimationManager { get; set; }

    public void SetDialogTree(IDialogTree dialogTree) {
        if (dialogTree == null) {
            DebugLog.Warn("Dialog Tree is null");
            return;
        }

        if (dialogTree.Equals(this.dialogTree))
            return;

        if (dialogTree is YarnDialogTree) {
            this.dialogTree = dialogTree as YarnDialogTree;
            this.dialogTree.Dialogue.Stop();
        } else {
            DebugLog.Warn("Dialog Tree is not a YarnDialogTree. It is " + dialogTree);
        }
    }

    public void SetTutorEmotion(Tutor tutor) {
        if (HeadAnimationManager != null)
        {
            HeadAnimationManager.Feel(tutor, tutor.Emotion);
        }
    }

    public void Reset() {
        if (dialogTree != null) {
            dialogTree.Dialogue.Stop();
        }
        if (enumerator != null) {
            enumerator = null;
        }
    }

    private IEnumerator step;

    public void Update() {
        if (dialogTree == null) {
            DebugLog.Warn("Dialog Tree is null");
            return;
        }

        if (step == null) {
            step = DialogStep();
        }

        if (!step.MoveNext()) {
            step = DialogStep();
        }
    }

    private IEnumerator DialogStep() {
        if (enumerator == null) {
            enumerator = dialogTree.Dialogue.Run(dialogTree.StartNode).GetEnumerator();
        }

        if (enumerator.MoveNext()) {
            Yarn.Dialogue.RunnerResult step = enumerator.Current;

            if (step is Yarn.Dialogue.LineResult) {
                // Wait for line to finish displaying
                var lineResult = step as Yarn.Dialogue.LineResult;
                DebugLog.Log("Dialogue: " + lineResult.line.text);
            } else if (step is Yarn.Dialogue.OptionSetResult) {

                // Wait for user to finish picking an option
                var optionSetResult = step as Yarn.Dialogue.OptionSetResult;
                //optionSetResult.options,
                //optionSetResult.setSelectedOptionDelegate
                foreach (var option in optionSetResult.options.options) {
                    DebugLog.Log("Option: " + option);
                }
                optionSetResult.setSelectedOptionDelegate(0);
            } else if (step is Yarn.Dialogue.CommandResult) {

                // Wait for command to finish running

                var commandResult = step as Yarn.Dialogue.CommandResult;

                DebugLog.Log("Command: " + commandResult.command.text);

                //if (DispatchCommand(commandResult.command.text) == true)
                //{
                //    // command was dispatched
                //}
                //else
                //{
                //    Yarn.Command command = commandResult.command;
                //}

                // See DialogRunner. 
                // It uses reflection to select the function and runs it.
            } else if (step is Yarn.Dialogue.NodeCompleteResult) {
                // Wait for post-node action
                var nodeResult = step as Yarn.Dialogue.NodeCompleteResult;
                string nextNode = nodeResult.nextNode;

                DebugLog.Log("Next Node: " + nextNode);
            }

            yield return null;
        } 
//        else {
//            DebugLog.Log("Finished Dialog Execution");
//        }
    }
}


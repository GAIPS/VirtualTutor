using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Utilities;
using UnityEngine;

public class YarnDialogManager : IDialogManager {
    YarnDialogTree dialogTree;
    IEnumerator<Yarn.Dialogue.RunnerResult> enumerator;

    public ICollection<Tutor> Tutors { get; set; }

    public AvatarManager HeadAnimationManager { get; set; }
    public BubbleSystem.BubbleSystemManager BubbleManager { get; set; }

    public YarnDialogManager()
    {
        Tutors = new List<Tutor>();
    }

    public void SetDialogTree(IDialogTree dialogTree) {
        if (dialogTree == null) {
            DebugLog.Warn("Dialog Tree is null");
            return;
        }

        if (dialogTree.Equals(this.dialogTree))
        {
            Reset();
            return;
        }

        if (dialogTree is YarnDialogTree) {
            this.dialogTree = dialogTree as YarnDialogTree;
            Reset();
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
    private bool newAppraisal = false;

    public bool Update() {
        if (dialogTree == null) {
            DebugLog.Warn("Dialog Tree is null");
            return true;
        }
        newAppraisal = false;

        if (step == null) {
            step = DialogStep();
        }

        if (!step.MoveNext()) {
            step = DialogStep();
        }
        return newAppraisal;
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
                if (BubbleManager != null)
                {
                    float duration = 5;

                    string[] splited = lineResult.line.text.Split(':');
                    string tutorName = splited[0].Trim();
                    string line = splited[1].Trim();

                    Tutor tutor = Tutors.First();
                    foreach (Tutor tut in Tutors)
                    {
                        if (tutorName.Contains(tut.Name))
                        {
                            tutor = tut;
                        }
                    }

                    BubbleManager.Speak(tutor.Name, tutor.Emotion.Name.ToString(), tutor.Emotion.Intensity, new string[] { line }, duration);
                    float count = 0;
                    while (count <= duration)
                    {
                        yield return null;
                        count += Time.deltaTime;
                    }
                }
            } else if (step is Yarn.Dialogue.OptionSetResult) {
                bool continueLoop = false;
                // Wait for user to finish picking an option
                var optionSetResult = step as Yarn.Dialogue.OptionSetResult;
                //optionSetResult.options,
                //optionSetResult.setSelectedOptionDelegate
                List<HookControl.IntFunc> callbacks = new List<HookControl.IntFunc>();
                foreach (var option in optionSetResult.options.options) {
                    DebugLog.Log("Option: " + option);
                    callbacks.Add((int i) =>
                    {
                        optionSetResult.setSelectedOptionDelegate(i);
                        continueLoop = true;
                    });
                }

                if (BubbleManager != null)
                {
                    float duration = 50;
                    
                    BubbleManager.UpdateOptions(optionSetResult.options.options.ToArray(), duration, callbacks.ToArray());
                    float count = 0;
                    while (count <= duration && !continueLoop)
                    {
                        // Active wait
                        yield return null;
                        count += Time.deltaTime;
                    }
                    // Hide Options
                    // HACK
                    BubbleManager.UpdateOptions(new string[] {"", "", "", ""});
                }
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
        else
        {
            DebugLog.Log("Finished Dialog Execution");
            newAppraisal = true;
        }
    }
}


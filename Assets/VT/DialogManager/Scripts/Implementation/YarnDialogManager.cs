using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;


public class YarnDialogManager : IDialogManager
{
    YarnDialogTree dialogTree;
    IEnumerable<Yarn.Dialogue.RunnerResult> enumerable;

    public void SetDialogTree(IDialogTree dialogTree)
    {
        if (dialogTree == null)
        {
            DebugLog.Warn("Dialog Tree is null");
            return;
        }

        if (dialogTree.Equals(this.dialogTree))
            return;

        if (dialogTree is YarnDialogTree)
        {
            this.dialogTree = dialogTree as YarnDialogTree;
            this.dialogTree.Dialogue.Stop();
        }
        else
        {
            DebugLog.Warn("Dialog Tree is not a YarnDialogTree. It is " + dialogTree);
        }
    }

    public void SetTutorEmotion(Tutor tutor)
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        if (dialogTree != null)
        {
            dialogTree.Dialogue.Stop();
        }
        if (enumerable != null)
        {
            enumerable = null;
        }
    }

    public void Update()
    {
        if (dialogTree == null)
        {
            DebugLog.Warn("Dialog Tree is null");
            return;
        }

        if (enumerable == null)
        {
            enumerable = dialogTree.Dialogue.Run();
        }

        IEnumerator<Yarn.Dialogue.RunnerResult> enumerator = enumerable.GetEnumerator();
        if (enumerator.MoveNext())
        {
            Yarn.Dialogue.RunnerResult step = enumerator.Current;

            if (step is Yarn.Dialogue.LineResult)
            {

                // Wait for line to finish displaying
                var lineResult = step as Yarn.Dialogue.LineResult;

            }
            else if (step is Yarn.Dialogue.OptionSetResult)
            {

                // Wait for user to finish picking an option
                var optionSetResult = step as Yarn.Dialogue.OptionSetResult;
                //optionSetResult.options,
                //optionSetResult.setSelectedOptionDelegate

            }
            else if (step is Yarn.Dialogue.CommandResult)
            {

                // Wait for command to finish running

                var commandResult = step as Yarn.Dialogue.CommandResult;

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


            }
            else if (step is Yarn.Dialogue.NodeCompleteResult)
            {
                // Wait for post-node action
                var nodeResult = step as Yarn.Dialogue.NodeCompleteResult;
                string nextNode = nodeResult.nextNode;
            }
        }

        // TODO How should I implement this dialog runner?
        //foreach (Yarn.Dialogue.RunnerResult step in dialogTree.Dialogue.Run())
        //{

        //    if (step is Yarn.Dialogue.LineResult)
        //    {

        //        // Wait for line to finish displaying
        //        var lineResult = step as Yarn.Dialogue.LineResult;

        //    }
        //    else if (step is Yarn.Dialogue.OptionSetResult)
        //    {

        //        // Wait for user to finish picking an option
        //        var optionSetResult = step as Yarn.Dialogue.OptionSetResult;
        //        //optionSetResult.options,
        //        //optionSetResult.setSelectedOptionDelegate

        //    }
        //    else if (step is Yarn.Dialogue.CommandResult)
        //    {

        //        // Wait for command to finish running

        //        var commandResult = step as Yarn.Dialogue.CommandResult;

        //        //if (DispatchCommand(commandResult.command.text) == true)
        //        //{
        //        //    // command was dispatched
        //        //}
        //        //else
        //        //{
        //        //    Yarn.Command command = commandResult.command;
        //        //}

        //        // See DialogRunner. 
        //        // It uses reflection to select the function and runs it.


        //    }
        //    else if (step is Yarn.Dialogue.NodeCompleteResult)
        //    {
        //        // Wait for post-node action
        //        var nodeResult = step as Yarn.Dialogue.NodeCompleteResult;
        //        string nextNode = nodeResult.nextNode;
        //    }
        //}
    }
}


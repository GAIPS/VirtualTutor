
using System;
using System.Collections.Generic;

public class YarnDialogSelector : IDialogSelector
{
    private Yarn.VariableStorage variableStorage;

    private Yarn.Dialogue dialogue;

    public string YarnFileContent { get; set; }

    public YarnDialogSelector()
    {
        variableStorage = new Yarn.MemoryVariableStore();
        dialogue = new Yarn.Dialogue(variableStorage);
    }

    public void LoadString()
    {
        LoadString(YarnFileContent);
    }

    public void LoadString(string fileContent)
    {
        dialogue.LoadString(fileContent);
    }

    public IDialogTree SelectDialog(History history, Intention intention)
    {
        //dialogue.Stop();
        //foreach (Yarn.Dialogue.RunnerResult step in dialogue.Run())
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
        //            //optionSetResult.options,
        //            //optionSetResult.setSelectedOptionDelegate

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

        IEnumerable<string> allNodes = dialogue.allNodes;
        List<string> startNodes = new List<string>();
        foreach (string nodeName in allNodes)
        {
            if (nodeName.StartsWith("Start_"))
            {
                startNodes.Add(nodeName);
            }
        }

        return null;
    }
}

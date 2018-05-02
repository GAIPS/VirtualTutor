using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Yarn;

namespace YarnDialog
{
    public abstract class LineHandler : YarnDialogManager.IDialogHandler
    {
        protected class LineInfo
        {
            public string message;
            public Tutor speaker;
        }

        public abstract IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager);
        public abstract void Reset(YarnDialogManager manager);
        public abstract void Update(YarnDialogManager manager);

        protected LineInfo InterpretLine(string line, YarnDialogManager manager)
        {
            string[] splited = line.Split(':');
            string tutorName = splited[0].Trim();
            string message = splited[1].Trim();

            Tutor tutor = manager.Tutors.First();
            foreach (Tutor tut in manager.Tutors)
            {
                if (tutorName.Contains(tut.Name))
                {
                    tutor = tut;
                }
            }
            return new LineInfo() { speaker = tutor, message = message };
        }

        protected void ShowLine(LineInfo line, float duration, YarnDialogManager manager)
        {
            if (manager.BubbleManager != null && manager.HeadAnimationManager != null)
            {
                manager.BubbleManager.Speak(line.speaker, new string[] { line.message }, duration);
                manager.HeadAnimationManager.Act(line.speaker, new MovementWithState(MovementEnum.Talk, StateEnum.Start));
                //manager.HeadAnimationManager.Act(line.speaker, new Movement(MovementEnum.Talk, new State(StateEnum.Start)));
            }
        }

        protected void HideLine(LineInfo line, YarnDialogManager manager)
        {
            if (manager.HeadAnimationManager != null)
            {
                manager.HeadAnimationManager.Act(line.speaker, new MovementWithState(MovementEnum.Talk, StateEnum.End));
                //manager.HeadAnimationManager.Act(line.speaker, new Movement(MovementEnum.Talk, new State(StateEnum.End)));
            }
        }
    }

    public class SequenceLineHandler : LineHandler
    {
        private float duration = 5; // Bubble last 5 seconds

        public override IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var lineResult = result as Dialogue.LineResult;
            if (lineResult == null)
            {
                yield break;
            }

            DebugLog.Log("Dialogue: " + lineResult.line.text);

            LineInfo lineInfo = InterpretLine(lineResult.line.text, manager);

            ShowLine(lineInfo, duration, manager);
            float count = 0;
            while (count <= duration)
            {
                yield return null;
                count += Time.deltaTime;
            }
            HideLine(lineInfo, manager);
        }

        public override void Reset(YarnDialogManager manager) { }

        public override void Update(YarnDialogManager manager) { }
    }

    public class ParallelLineHandler : LineHandler
    {
        private IList<LineInfo> lines = new List<LineInfo>();
        private LineInfo playingLine;

        private float count = 0;
        private float duration = 5;


        public override IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var lineResult = result as Dialogue.LineResult;
            if (lineResult == null)
            {
                yield break;
            }

            lines.Add(InterpretLine(lineResult.line.text, manager));
            
        }

        public override void Update(YarnDialogManager manager)
        {
            if (playingLine == null)
            {
                if (lines.Count > 0) // if not playing
                {
                    // Pop line
                    playingLine = lines[0];
                    lines.RemoveAt(0);
                    // Show Line
                    ShowLine(playingLine, duration, manager);
                    // Reset counter
                    count = 0;
                }
            }
            else
            {
                if (count > duration)
                {
                    HideLine(playingLine, manager);
                    playingLine = null;
                }
                count += Time.deltaTime;
            }
        }

        public override void Reset(YarnDialogManager manager)
        {
            lines.Clear();
            if (playingLine != null)
            {
                HideLine(playingLine, manager);
                playingLine = null;
            }
        }
    }

    public class SequenceOptionsHandler : YarnDialogManager.IDialogHandler
    {
        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var optionSetResult = result as Yarn.Dialogue.OptionSetResult;
            if (optionSetResult == null)
            {
                yield break;
            }

            // Wait for user to finish picking an option
            bool continueLoop = false;
            List<HookControl.IntFunc> callbacks = new List<HookControl.IntFunc>();
            foreach (var option in optionSetResult.options.options)
            {
                DebugLog.Log("Option: " + option);
                callbacks.Add((int i) =>
                {
                    optionSetResult.setSelectedOptionDelegate(i);
                    continueLoop = true;
                });
            }

            if (manager.BubbleManager != null)
            {
                float duration = 60; // One minute wait.
                IList<string> options = new List<string>(optionSetResult.options.options);
                options.Remove("BLANK");
                manager.BubbleManager.UpdateOptions(options.ToArray(), 0.0f, duration, callbacks.ToArray());
                float count = 0;
                while (count <= duration && !continueLoop)
                {
                    // Active wait
                    yield return null;
                    count += Time.deltaTime;
                }
                if (count > duration)
                {
                    bool foundBlank = false;
                    for (int i = 0; i < optionSetResult.options.options.Count; i++)
                    {
                        if ("BLANK".Equals(optionSetResult.options.options[i]))
                        {
                            foundBlank = true;
                            optionSetResult.setSelectedOptionDelegate(i);
                        }
                    }
                    // If BLANK is not found do something else...
                    if (!foundBlank)
                    {
                        manager.RunNode("ChangePlan");
                    }
                }
                // Hide Options
                // HACK
                manager.BubbleManager.HideBalloon("Options");
            }
        }

        public void Reset(YarnDialogManager manager) { }

        public void Update(YarnDialogManager manager) { }
    }

    public class ExitCommandHandler : YarnDialogManager.IDialogHandler
    {
        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var commandResult = result as Yarn.Dialogue.CommandResult;
            if (commandResult == null)
            {
                yield break;
            }

            if (commandResult.command.text.Contains("exit"))
            {
                DebugLog.Log("Exiting...");
                Application.Quit();
            }
        }

        public void Reset(YarnDialogManager manager) { }

        public void Update(YarnDialogManager manager) { }
    }

    public class BubbleSystemCommandHandler : YarnDialogManager.IDialogHandler
    {
        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var commandResult = result as Yarn.Dialogue.CommandResult;
            if (commandResult == null)
            {
                yield break;
            }

            //manager.BubbleManager.Handle(commandResult.command.text);
        }

        public void Reset(YarnDialogManager manager) { }

        public void Update(YarnDialogManager manager) { }
    }

    public class HeadSystemCommandHandler : YarnDialogManager.IDialogHandler
    {
        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var commandResult = result as Yarn.Dialogue.CommandResult;
            if (commandResult == null)
            {
                yield break;
            }
            //manager.HeadAnimationManager.Handle(commandResult.command.text);
        }

        public void Reset(YarnDialogManager manager) { }

        public void Update(YarnDialogManager manager) { }
    }

    public class LogCommandHandler : YarnDialogManager.IDialogHandler
    {
        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var commandResult = result as Yarn.Dialogue.CommandResult;
            if (commandResult == null)
            {
                yield break;
            }
            DebugLog.Log("Command: " + commandResult.command.text);
        }

        public void Reset(YarnDialogManager manager) { }

        public void Update(YarnDialogManager manager) { }
    }

    public class LogCompleteNodeHandler : YarnDialogManager.IDialogHandler
    {
        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var nodeResult = result as Yarn.Dialogue.NodeCompleteResult;
            if (nodeResult == null)
            {
                yield break;
            }
            string nextNode = nodeResult.nextNode;

            DebugLog.Log("Next Node: " + nextNode);
        }

        public void Reset(YarnDialogManager manager) { }

        public void Update(YarnDialogManager manager) { }
    }

    public class EmotionTagNodeHandler : YarnDialogManager.IDialogHandler
    {
        private string _currentNode = string.Empty;

        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var dialogue = manager.GetDialogue();
            if (dialogue == null || string.IsNullOrEmpty(dialogue.currentNode)) return null;

            if (_currentNode.Equals(dialogue.currentNode)) return null;
            _currentNode = dialogue.currentNode;

            var tags = dialogue.GetTagsForNode(_currentNode);
            foreach (var tag in tags)
            {
                var tagSplit = tag.Split('.');

                if (tagSplit.Length < 3)
                {
                    continue;
                }

                var action = tagSplit[1].ToLower();
                if (!action.Equals("emotion"))
                {
                    continue;
                }
                
                var target = tagSplit[0].ToLower();

                if (target.Equals("user"))
                {
                    
                } else if (target.Equals("joao"))
                {
                    
                } else if (target.Equals("maria"))
                {
                    
                }
            }

            return null;
        }

        public void Reset(YarnDialogManager manager)
        {
        }

        public void Update(YarnDialogManager manager)
        {
        }
    }
}
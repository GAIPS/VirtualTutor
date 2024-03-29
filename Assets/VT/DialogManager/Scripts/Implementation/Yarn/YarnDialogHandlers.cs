﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Utilities;
using Yarn;
using BubbleSystem;

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
        public abstract void HandlerReset(YarnDialogManager manager);
        public abstract void HandlerUpdate(YarnDialogManager manager);

        protected LineInfo InterpretLine(string line, YarnDialogManager manager)
        {
            string[] splited = line.Split(':');
            string tutorName = splited[0].Trim();
            string message = splited[1].Trim().Replace("|", "#");

            Tutor tutor = manager.Tutors.First();
            foreach (Tutor tut in manager.Tutors)
            {
                if (tutorName.Contains(tut.Name))
                {
                    tutor = tut;
                }
            }

            return new LineInfo() {speaker = tutor, message = message};
        }

        protected void ShowLine(LineInfo line, YarnDialogManager manager)
        {
            if (manager.ModuleManager != null)
            {
                manager.ModuleManager.StartSpeaking(line.speaker, line.message);
            }
            else
            {
                DebugLog.Warn("No Module Manager defined.");
            }
        }

        protected void HideLine(LineInfo line, YarnDialogManager manager)
        {
            if (manager.ModuleManager != null)
                manager.ModuleManager.StopSpeaking(line.speaker);
        }
    }

    public class SequenceLineHandler : LineHandler
    {
        private float duration = DefaultData.Instance.GetBalloonDuration(); // Bubble last 5 seconds

        public override IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var lineResult = result as Dialogue.LineResult;
            if (lineResult == null)
            {
                yield break;
            }

            DebugLog.Log("Dialogue: " + lineResult.line.text);

            LineInfo lineInfo = InterpretLine(lineResult.line.text, manager);

            ShowLine(lineInfo, manager);
            float count = 0;
            while (count <= duration)
            {
                yield return null;
                count += Time.deltaTime;
            }

            HideLine(lineInfo, manager);
        }

        public override void HandlerReset(YarnDialogManager manager)
        {
        }

        public override void HandlerUpdate(YarnDialogManager manager)
        {
        }
    }

    public class ParallelLineHandler : LineHandler
    {
        private IList<LineInfo> lines = new List<LineInfo>();
        private LineInfo playingLine;

        private float count = 0;
        private float duration = DefaultData.Instance.GetBalloonDuration();


        public override IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var lineResult = result as Dialogue.LineResult;
            if (lineResult == null)
            {
                yield break;
            }

            lines.Add(InterpretLine(lineResult.line.text, manager));
        }

        public override void HandlerUpdate(YarnDialogManager manager)
        {
            if (playingLine == null)
            {
                if (lines.Count > 0) // if not playing
                {
                    // Pop line
                    playingLine = lines[0];
                    lines.RemoveAt(0);
                    // Show Line
                    ShowLine(playingLine, manager);
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

        public override void HandlerReset(YarnDialogManager manager)
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

            if (manager.ModuleManager != null)
            {
                IList<string> options = new List<string>(optionSetResult.options.options);
                options.Remove("BLANK");
                manager.ModuleManager.UpdateOptions(options.ToArray(), callbacks);

                float duration = DefaultData.Instance.GetOptionsDuration();
                if (duration < 0)
                {
                    while (!continueLoop)
                    {
                        // Active wait
                        yield return null;
                    }
                }
                else
                {
                    float count = 0;
                    while (count <= duration && !continueLoop)
                    {
                        // Active wait until duration
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
                }


                // Hide Options
                manager.ModuleManager.HideBalloon("User");
            }
        }

        public void HandlerReset(YarnDialogManager manager)
        {
        }

        public void HandlerUpdate(YarnDialogManager manager)
        {
        }
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

        public void HandlerReset(YarnDialogManager manager)
        {
        }

        public void HandlerUpdate(YarnDialogManager manager)
        {
        }
    }

    public class ModuleCommandHandler : YarnDialogManager.IDialogHandler
    {
        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var commandResult = result as Yarn.Dialogue.CommandResult;
            if (commandResult == null)
            {
                yield break;
            }

            manager.ModuleManager.Tutors = manager.Tutors.ToList();
            manager.ModuleManager.Handle(commandResult.command.text.Split(' '));
        }

        public void HandlerReset(YarnDialogManager manager)
        {
        }

        public void HandlerUpdate(YarnDialogManager manager)
        {
        }
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

        public void HandlerReset(YarnDialogManager manager)
        {
        }

        public void HandlerUpdate(YarnDialogManager manager)
        {
        }
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

        public void HandlerReset(YarnDialogManager manager)
        {
        }

        public void HandlerUpdate(YarnDialogManager manager)
        {
        }
    }

    public class EmotionTagNodeHandler : YarnDialogManager.IDialogHandler
    {
        private string _currentNode = string.Empty;

        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var dialogue = manager.GetDialogue();
            if (dialogue == null || string.IsNullOrEmpty(dialogue.currentNode)) yield break;

            if (_currentNode.Equals(dialogue.currentNode)) yield break;
            _currentNode = dialogue.currentNode;

            var tags = dialogue.GetTagsForNode(_currentNode);
            foreach (var tag in tags)
            {
                // The split to work should be formated as [target].emotion.[emotion].[intensity]

                var tagSplit = tag.Split('.');

                // Does it have 4 elements
                if (tagSplit.Length != 4)
                {
                    continue;
                }

                // Is the second element `emotion`
                var action = tagSplit[1].ToLower();
                if (!action.Equals("emotion"))
                {
                    continue;
                }

                // What is the emotion?
                var emotion = tagSplit[2];
                EmotionEnum emotionEnum;
                try
                {
                    emotionEnum = (EmotionEnum) Enum.Parse(typeof(EmotionEnum), emotion, true);
                }
                catch (OverflowException)
                {
                    // If unable to convert, skip
                    continue;
                }

                // What is the emotion's intention?
                var intentityStr = tagSplit[3];
                float intensity;
                if (!float.TryParse(intentityStr, NumberStyles.Any, CultureInfo.CreateSpecificCulture("pt-PT"),
                    out intensity))
                {
                    continue;
                }

                var target = tagSplit[0];
                if (target.ToLower().Equals("user"))
                {
                    // Update background
                }
                else
                {
                    var tutor = manager.GetTutor(target);
                    if (tutor == null) continue;
                    tutor.Emotion = new Emotion(emotionEnum, intensity);
                    manager.ModuleManager.Feel(tutor, BubbleSystem.Reason.ReasonEnum.None);
                }
            }
        }

        public void HandlerReset(YarnDialogManager manager)
        {
        }

        public void HandlerUpdate(YarnDialogManager manager)
        {
        }
    }

    public class WaitCommandHandler : YarnDialogManager.IDialogHandler
    {
        public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
        {
            var commandResult = result as Yarn.Dialogue.CommandResult;
            if (commandResult == null)
            {
                yield break;
            }

            string[] param = commandResult.command.text.Split(' ');

            if (param.Length != 2)
            {
                yield break;
            }

            if (!param[0].Contains("wait"))
            {
                yield break;
            }

            float time;
            if (!float.TryParse(param[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("pt-PT"),
                out time))
            {
                yield break;
            }

            float counter = 0;
            while (counter < time)
            {
                yield return null;
                counter += Time.deltaTime;
            }
        }

        public void HandlerReset(YarnDialogManager manager)
        {
        }

        public void HandlerUpdate(YarnDialogManager manager)
        {
        }
    }
}
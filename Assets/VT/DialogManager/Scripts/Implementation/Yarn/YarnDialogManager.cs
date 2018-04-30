using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Utilities;
using UnityEngine;

namespace YarnDialog
{
    public class YarnDialogManager : IDialogManager
    {
        public interface IDialogHandler
        {
            IEnumerator Handle(Yarn.Dialogue.RunnerResult result, YarnDialogManager manager);
            void Update(YarnDialogManager manager);
            void Reset(YarnDialogManager manager);
        }

        YarnDialogTree dialogTree;
        IEnumerator<Yarn.Dialogue.RunnerResult> enumerator;

        public ICollection<Tutor> Tutors { get; set; }

        public VTToModuleBridge ModuleManager { get; set; }

        public IList<IDialogHandler> Handlers { get; set; }

        public YarnDialogManager(bool useDefaultHandlers = true)
        {
            Tutors = new List<Tutor>();
            Handlers = new List<IDialogHandler>();

            if (useDefaultHandlers)
            {
                // Order matters
                this.Handlers.Add(new SequenceLineHandler());
                this.Handlers.Add(new SequenceOptionsHandler());
                this.Handlers.Add(new LogCommandHandler());
                this.Handlers.Add(new ExitCommandHandler());
                this.Handlers.Add(new LogCompleteNodeHandler());
            }
        }

        public void SetDialogTree(IDialogTree dialogTree)
        {
            if (dialogTree == null)
            {
                DebugLog.Warn("Dialog Tree is null");
                return;
            }

            if (dialogTree.Equals(this.dialogTree))
            {
                Reset();
                return;
            }

            if (dialogTree is YarnDialogTree)
            {
                this.dialogTree = (YarnDialogTree)dialogTree;
                Reset();
            }
            else
            {
                DebugLog.Warn("Dialog Tree is not a YarnDialogTree. It is " + dialogTree);
            }
        }

        public void SetTutorEmotion(Tutor tutor)
        {
            if (ModuleManager != null)
            {
                ModuleManager.Feel(tutor);
                ModuleManager.UpdateBackground(tutor, 5f, BubbleSystem.Reason.None);
            }
        }

        public void Reset()
        {
            if (dialogTree != null)
            {
                dialogTree.Dialogue.Stop();
            }
            enumerator = null;
            step = null;
            foreach (IDialogHandler handler in Handlers)
            {
                handler.Reset(this);
            }
        }

        private IEnumerator step;
        private bool newAppraisal = false;

        public bool Update()
        {
            if (dialogTree == null)
            {
                DebugLog.Warn("Dialog Tree is null");
                return true;
            }
            newAppraisal = false;

            foreach (IDialogHandler handler in Handlers)
            {
                handler.Update(this);
            }

            if (step == null)
            {
                step = DialogStep();
            }

            if (!step.MoveNext())
            {
                step = DialogStep();
            }
            return newAppraisal;
        }

        private IEnumerator DialogStep()
        {
            if (enumerator == null)
            {
                RunNode(dialogTree.StartNode);
            }

            if (enumerator.MoveNext())
            {
                Yarn.Dialogue.RunnerResult step = enumerator.Current;

                foreach (IDialogHandler handler in Handlers)
                {
                    var handle = handler.Handle(step, this);
                    while (handle.MoveNext())
                    {
                        yield return null;
                    }
                }

                yield return null;
            }
            else
            {
                DebugLog.Log("Finished Dialog Execution");
                newAppraisal = true;
            }
        }

        public void RunNode(string node)
        {
            foreach (IDialogHandler handler in Handlers)
            {
                handler.Reset(this);
            }
            enumerator = dialogTree.Dialogue.Run(node).GetEnumerator();
        }
    }

}
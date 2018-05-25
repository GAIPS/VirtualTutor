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

        private YarnDialogTree _dialogTree;
        private IEnumerator<Yarn.Dialogue.RunnerResult> _enumerator;
        private VTToModuleBridge _moduleManager;

        public ICollection<Tutor> Tutors { get; set; }

        public VTToModuleBridge ModuleManager
        {
            get
            {
                if (!_moduleManager)
                {
                    DebugLog.Warn("No Module Manager defined.");
                }

                return _moduleManager;
            }
            set { _moduleManager = value; }
        }

        public IList<IDialogHandler> Handlers { get; set; }

        public YarnDialogManager(bool useDefaultHandlers = true)
        {
            Tutors = new List<Tutor>();
            Handlers = new List<IDialogHandler>();

            if (useDefaultHandlers)
            {
                // Order matters
                this.Handlers.Add(new ModuleCommandHandler());
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

            if (dialogTree.Equals(this._dialogTree))
            {
                Reset();
                return;
            }

            if (dialogTree is YarnDialogTree)
            {
                this._dialogTree = (YarnDialogTree) dialogTree;
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
                ModuleManager.Feel(tutor, BubbleSystem.Reason.None);
        }

        public void Reset()
        {
            if (_dialogTree != null)
            {
                _dialogTree.Dialogue.Stop();
            }

            _enumerator = null;
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
            if (_dialogTree == null)
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
            if (_enumerator == null)
            {
                RunNode(_dialogTree.StartNode);
            }

            if (_enumerator.MoveNext())
            {
                Yarn.Dialogue.RunnerResult step = _enumerator.Current;

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

            _enumerator = _dialogTree.Dialogue.Run(node).GetEnumerator();
        }

        public Yarn.Dialogue GetDialogue()
        {
            if (_dialogTree == null) return null;
            return _dialogTree.Dialogue;
        }

        public Tutor GetTutor(string name)
        {
            foreach (var tutor in Tutors)
            {
                if (tutor.Name.ToLower().Contains(name.ToLower()))
                {
                    return tutor;
                }
            }

            return null;
        }
    }
}
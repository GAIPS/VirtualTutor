﻿using HookControl;
using System.Collections.Generic;
using UnityEngine;

namespace VT {
    public class FacesControl : IControl {
        private Control control;
        private Topic currentTopic;
        private float start = 0.0f;
        private bool started;
        private FacesHooks hooks;

        

        public FacesControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(Topic currentTopic) {
            this.currentTopic = currentTopic;
            start = 0.0f;
            activeTopLine = null;
            activeBottomLine = null;
            if (hooks) {
                hooks.LeftContent = null;
                hooks.RightContent = null;
            }

        }

        public ShowResult Show() {	
            started = true;
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hooks = control.instance.GetComponent<FacesHooks>();
                if (hooks) {
                    hooks.onClick = skipLine;
                }
            }
            return ret;
            
        }


        private Line activeTopLine;
        private Line activeBottomLine;

        public void update(float delta) {
            start += delta;
            //lineOffset += 2;
            if (hooks && started) {
                if (activeTopLine != null && activeTopLine.End <= start) {
                    hooks.LeftContent = null;
                }
                if (activeBottomLine != null && activeBottomLine.End <= start) {
                    hooks.RightContent = null;
                }
                foreach (Line l in currentTopic.Lines) {
                    if (activeTopLine == l || activeBottomLine == l) {
                        continue;
                    }
                    if (l.Start <= start && l.End > start) {
                        if (l.Speaker.IsLeft) {
//							hooks.LeftLine.SetActive (true);
                            hooks.LeftContent = l.Content;
                            hooks.changeExpression((int)l.Speaker.CurrentEmotion, true);
//                                else {
//                                    hooks.AudioSource.clip = hooks.AudiosFemale[(int)l.Speaker.CurrentEmotion];
//                                    hooks.AudioSource.Play();
//                                }
                            activeTopLine = l;
                        } else {
//							hooks.RightLine.SetActive (true);
                            hooks.RightContent = l.Content;
                            hooks.changeExpression((int)l.Speaker.CurrentEmotion, false);
//                                else {
//                                    hooks.AudioSource.clip = hooks.Audios[(int)l.Speaker.CurrentEmotion];
//                                    hooks.AudioSource.Play();
//                                }
                            activeBottomLine = l;
                        }
                    }
                }
            }
            Show();
        }

        public ShowResult SetAndShow(Topic currentTopic) {
            this.Set(currentTopic);
            return Show();
        }

        public void Destroy() {
            control.Destroy();
        }

        public void Disable() {
            control.Disable();
        }

        public bool IsVisible() {
            return control.IsVisible();
        }

        public void Enable() {
            control.Enable();
        }

        public float Start {
            get {
                return this.start;
            }
            set {
                start = value;
            }
        }

        private void skipLine() {
            for (int i = 0; i < currentTopic.Lines.Count; i++) {
                Line l = currentTopic.Lines[i];
                if (l.Start > start) {
                    start = l.Start;
                    break;
                } else if (i == currentTopic.Lines.Count - 1) {
                    start = l.End;
                }
            }
        }
    
    }

}
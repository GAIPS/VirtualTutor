﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace VT {
    [Serializable]
    public class ExpressionsControl : IControl {
        private Control control;
        private Topic currentTopic;
        private float start = 0.0f;
        private bool started;
        private ExpressionsHooks hooks;

		

        public ExpressionsControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(Topic currentTopic) {
            this.currentTopic = currentTopic;
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
                hooks = control.instance.GetComponent<ExpressionsHooks>();
                if (hooks) {
                    if (hooks.LeftSprites == null)
                        hooks.LeftSprites = new List<Sprite>();
                    if (hooks.RightSprites == null)
                        hooks.RightSprites = new List<Sprite>();
                    if (hooks.Audios == null)
                        hooks.Audios = new List<AudioClip>();
                    if (hooks.AudiosFemale == null)
                        hooks.AudiosFemale = new List<AudioClip>();

				
				
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
                            hooks.LeftSprite = hooks.LeftSprites[(int)l.Speaker.CurrentEmotion];
                            if (!hooks.AudioSource.isPlaying) {
                                if (hooks.useTTS) {
                                    hooks.ttsSpeaker.Play(l.Content,
                                                          Sex.Female);
                                } else {
                                    hooks.AudioSource.clip = hooks.AudiosFemale[(int)l.Speaker.CurrentEmotion];
                                    hooks.AudioSource.Play();
                                }
                            }
                            activeTopLine = l;
                        } else {
//							hooks.RightLine.SetActive (true);
                            hooks.RightContent = l.Content;
                            hooks.RightSprite = hooks.RightSprites[(int)l.Speaker.CurrentEmotion];
                            if (!hooks.AudioSource.isPlaying) {
                                if (hooks.useTTS) {
                                    hooks.ttsSpeaker.Play(l.Content,
                                                          Sex.Male);
                                } else {
                                    hooks.AudioSource.clip = hooks.Audios[(int)l.Speaker.CurrentEmotion];
                                    hooks.AudioSource.Play();
                                }
                            }
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
	
    }

}

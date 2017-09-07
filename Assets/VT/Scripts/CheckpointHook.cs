using HookControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace VT {
    public class CheckpointHook : Hook {
        [SerializeField]
        private Text pointName;
        [SerializeField]
        private Text date;
        [SerializeField]
        private Text score;
        [SerializeField]
        private Text daysMissing;
        [SerializeField]
        private bool check;
        [SerializeField]
        private Toggle checkObj;

        public string Name {
            get {
                if (pointName) {
                    return pointName.text;
                } else {
                    Debug.LogWarning("No Checkpoint Name found.");
                    return string.Empty;
                }
            }

            set {
                if (pointName) {
                    pointName.text = value;
                } else {
                    Debug.LogWarning("No Checkpoint Name found.");
                }
            }
        }

        public string Date {
            get {
                if (date) {
                    return date.text;
                } else {
                    Debug.LogWarning("No Checkpoint Date found.");
                    return string.Empty;
                }
            }

            set {
                if (date) {
                    date.text = value;
                } else {
                    Debug.LogWarning("No Checkpoint Date found.");
                }
            }
        }

        public string Score {
            get {
                if (score) {
                    return score.text;
                } else {
                    Debug.LogWarning("No Checkpoint Score found.");
                    return string.Empty;
                }
            }

            set {
                if (score) {
                    score.text = value;
                } else {
                    Debug.LogWarning("No Checkpoint Score found.");
                }
            }
        }

        public bool ScoreActive {
            get {
                if (score) {
                    return score.gameObject.activeInHierarchy;
                } else {
                    Debug.LogWarning("No Checkpoint Score found.");
                    return false;
                }
            }

            set {
                if (score) {
                    score.gameObject.SetActive(value);
                } else {
                    Debug.LogWarning("No Checkpoint Score found.");
                }
            }
        }

        public string DaysMissing {
            get {
                if (daysMissing) {
                    return daysMissing.text;
                } else {
                    Debug.LogWarning("No Checkpoint DaysMissing found.");
                    return string.Empty;
                }
            }

            set {
                if (daysMissing) {
                    daysMissing.text = value;
                } else {
                    Debug.LogWarning("No Checkpoint DaysMissing found.");
                }
            }
        }

        public bool DaysMissingActive {
            get {
                if (daysMissing) {
                    return daysMissing.gameObject.activeInHierarchy;
                } else {
                    Debug.LogWarning("No Checkpoint DaysMissing found.");
                    return false;
                }
            }

            set {
                if (daysMissing) {
                    daysMissing.gameObject.SetActive(value);
                } else {
                    Debug.LogWarning("No Checkpoint DaysMissing found.");
                }
            }
        }

        public bool Check {
            get {
                return check;
            }

            set {
                check = value;
            }
        }

        public bool CheckActive {
            get {
                if (checkObj) {
                    return checkObj.gameObject.activeInHierarchy;
                } else {
                    Debug.LogWarning("No Checkpoint Check found.");
                    return false;
                }
            }

            set {
                if (checkObj) {
                    checkObj.gameObject.SetActive(value);
                } else {
                    Debug.LogWarning("No Checkpoint Check found.");
                }
            }
        }

        public void Set(Checkpoint checkpoint) {

            Name = checkpoint.Name;
            Date = checkpoint.Date;

            // check respective conversions to Evaluation or checkbox
            DaysMissingActive = false;
            ScoreActive = false;
            CheckActive = false;

            Evaluation evaluation = checkpoint as Evaluation;
            if (evaluation != null) {
                if (string.IsNullOrEmpty(evaluation.Score)) {
                    DaysMissingActive = true;
                } else {
                    Score = evaluation.Score;
                    ScoreActive = true;
                }
            } else {
                CheckBoxPoint checkbox = checkpoint as CheckBoxPoint;
                Check = checkbox.Done;
                CheckActive = true;
            }
        }
    }
}

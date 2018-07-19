using HookControl;
using UnityEngine;
using UnityEngine.UI;

namespace VT
{
    public class CheckpointHook : Hook
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _dateText;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _daysMissingText;
        [SerializeField] private Toggle _checkToggle;

        public string Name
        {
            get
            {
                if (!_nameText)
                {
                    Debug.LogWarning("No Checkpoint Name found.");
                    return string.Empty;
                }

                return _nameText.text;
            }

            set
            {
                if (!_nameText)
                {
                    Debug.LogWarning("No Checkpoint Name found.");
                    return;
                }

                _nameText.text = value;
            }
        }

        public string Date
        {
            get
            {
                if (!_dateText)
                {
                    Debug.LogWarning("No Checkpoint Date found.");
                    return string.Empty;
                }

                return _dateText.text;
            }

            set
            {
                if (!_dateText)
                {
                    Debug.LogWarning("No Checkpoint Date found.");
                    return;
                }

                _dateText.text = value;
            }
        }

        public string Score
        {
            get
            {
                if (!_scoreText)
                {
                    Debug.LogWarning("No Checkpoint Score found.");
                    return string.Empty;
                }

                return _scoreText.text;
            }

            set
            {
                if (!_scoreText)
                {
                    Debug.LogWarning("No Checkpoint Score found.");
                    return;
                }

                _scoreText.text = value;
            }
        }

        public bool ScoreVisibility
        {
            get
            {
                if (!_scoreText)
                {
                    Debug.LogWarning("No Checkpoint Score found.");
                    return false;
                }

                return _scoreText.gameObject.activeSelf;
            }

            set
            {
                if (!_scoreText)
                {
                    Debug.LogWarning("No Checkpoint Score found.");
                    return;
                }

                _scoreText.gameObject.SetActive(value);
            }
        }

        public string DaysMissing
        {
            get
            {
                if (!_daysMissingText)
                {
                    Debug.LogWarning("No Checkpoint DaysMissing found.");
                    return string.Empty;
                }

                return _daysMissingText.text;
            }

            set
            {
                if (!_daysMissingText)
                {
                    Debug.LogWarning("No Checkpoint DaysMissing found.");
                    return;
                }

                _daysMissingText.text = value;
            }
        }

        public bool DaysMissingVisibility
        {
            get
            {
                if (!_daysMissingText)
                {
                    Debug.LogWarning("No Checkpoint DaysMissing found.");
                    return false;
                }

                return _daysMissingText.gameObject.activeSelf;
            }

            set
            {
                if (!_daysMissingText)
                {
                    Debug.LogWarning("No Checkpoint DaysMissing found.");
                    return;
                }

                _daysMissingText.gameObject.SetActive(value);
            }
        }

        public bool Check
        {
            get
            {
                if (!_checkToggle)
                {
                    Debug.LogWarning("No Checkpoint Check found.");
                    return false;
                }

                return _checkToggle.isOn;
            }

            set
            {
                if (!_checkToggle)
                {
                    Debug.LogWarning("No Checkpoint Check found.");
                    return;
                }

                _checkToggle.isOn = value;
            }
        }

        public bool CheckVisibility
        {
            get
            {
                if (!_checkToggle)
                {
                    Debug.LogWarning("No Checkpoint Check found.");
                    return false;
                }

                return _checkToggle.gameObject.activeSelf;
            }

            set
            {
                if (!_checkToggle)
                {
                    Debug.LogWarning("No Checkpoint Check found.");
                    return;
                }

                _checkToggle.gameObject.SetActive(value);
            }
        }

        public void Set(Checkpoint checkpoint)
        {
            Name = checkpoint.Name;
            Date = checkpoint.Date;

            // check respective conversions to Evaluation or checkbox
            DaysMissingVisibility = false;
            ScoreVisibility = false;
            CheckVisibility = false;

            Evaluation evaluation = checkpoint as Evaluation;
            if (evaluation != null)
            {
                if (string.IsNullOrEmpty(evaluation.Score))
                {
                    DaysMissingVisibility = true;
                }
                else
                {
                    Score = evaluation.Score;
                    ScoreVisibility = true;
                }
            }
            else
            {
                CheckBoxPoint checkbox = checkpoint as CheckBoxPoint;                
                if (checkbox != null)
                {
                    Check = checkbox.Done;
                    CheckVisibility = true;
                }
            }
        }
    }
}
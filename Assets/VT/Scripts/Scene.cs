using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VT
{
	public class Scene
	{
		public List<Agent> agents = new List<Agent> ();
		public Dictionary<string, Topic> topics = new Dictionary<string, Topic> ();
		public ThreePartsControl threePartsControl;
		public ExpressionsControl expressionsControl;
		public CoursesControl coursesControl;
		public Calendar1Control calendar1Control;
		public CourseControl courseControl;
		public Calendar2Control calendar2Control;
		public Calendar3Control calendar3Control;
        private float start = 0.0f;

        private string currentTopicName;

		public float Start {
			get {
				return this.start;
			}
			set {
				start = value;
			}
		}


		public Scene ()
		{
		}

		public void OpenCourses ()
		{
			start = 11.0f;
			coursesControl.SetAndShow (() => {
				OpenCourse ();
				start = 0.0f;
			});
		}

		public void OpenCourse ()
        {
            float evaluationResult = 10;
			courseControl.SetAndShow (() => {
				start = 0.0f;
				expressionsControl.Start = 0.0f;
				if (evaluationResult < 8.5) {
					agents [0].CurrentEmotion = Agent.EmotionType.CRYING;
					agents [1].CurrentEmotion = Agent.EmotionType.CRYING;
					currentTopicName = "badTestTopic";
					var topic2 = topics [currentTopicName];
					threePartsControl.SetAndShow (topic2);
					expressionsControl.SetAndShow (topic2);
					coursesControl.Disable ();
					courseControl.Disable ();

				} else if (evaluationResult >= 8.5 && evaluationResult < 9.5) {
					agents [0].CurrentEmotion = Agent.EmotionType.SAD;
					agents [1].CurrentEmotion = Agent.EmotionType.SAD;
					currentTopicName = "belowAvgTopic";
					var topic2 = topics [currentTopicName];

					threePartsControl.SetAndShow (topic2);
					expressionsControl.SetAndShow (topic2);
					coursesControl.Disable ();
					courseControl.Disable ();

				} else if (evaluationResult >= 9.5 && evaluationResult < 15.0) {
					agents [0].CurrentEmotion = Agent.EmotionType.LIKES;
					agents [1].CurrentEmotion = Agent.EmotionType.SMILING;
					currentTopicName = "expectedTest";
					var topic2 = topics [currentTopicName];
					threePartsControl.SetAndShow (topic2);
					expressionsControl.SetAndShow (topic2);
					coursesControl.Disable ();
					courseControl.Disable ();

				} else if (evaluationResult >= 15.0) {
					agents [0].CurrentEmotion = Agent.EmotionType.LIKES;
					agents [1].CurrentEmotion = Agent.EmotionType.LIKES;
					currentTopicName = "greatTest";
					var topic2 = topics [currentTopicName];
					threePartsControl.SetAndShow (topic2);
					expressionsControl.SetAndShow (topic2);
					coursesControl.Disable ();
					courseControl.Disable ();
				
				}
					
			}, () => {
			}, () => {
			}, () => {
			}, () => {
			}, (string value) => {
//				test1Value =	float.Parse (value);
			},
				(string value) => {
					evaluationResult = float.Parse (value);
				}, () => {
			}, () => {
			});
		
		}

		public void OpenCalendar ()
		{
			start = 0.0f;
			expressionsControl.Start = 0.0f;
			calendar1Control.SetAndShow (() => {
				SaveCalendar ();
			});	
		}

		public void SaveCalendar ()
		{
			calendar2Control.SetAndShow (() => {
				FinalCalendar ();
			});
		}

		public void FinalCalendar ()
		{

			calendar3Control.SetAndShow (() => {
				
				currentTopicName = "enoughPlan";
				var topic3 = topics [currentTopicName];
				threePartsControl.SetAndShow (topic3);
				expressionsControl.SetAndShow (topic3);
				calendar1Control.Disable ();
				calendar2Control.Disable ();
				calendar3Control.Disable ();
				coursesControl.Disable ();
				courseControl.Disable ();
				start = 0.0f;
				expressionsControl.Start = 0.0f;


			});
		}

		public void changeTopic (string topicName)
		{
			start = 0.0f;
			expressionsControl.Start = 0.0f;
			if (!topics.ContainsKey (topicName)) {
				return;
			}
			currentTopicName = topicName;
			var topic = topics [topicName];
			threePartsControl.SetAndShow (topic);
			expressionsControl.SetAndShow (topic);
		}

		public void TimeOutTopic (string topicName)
		{
			agents[0].CurrentEmotion = Agent.EmotionType.IMPATIENT;
			agents[1].CurrentEmotion = Agent.EmotionType.IMPATIENT;
			start = 0.0f;
			expressionsControl.Start = 0.0f;
			if (!topics.ContainsKey (topicName)) {
				return;
			}
			currentTopicName = topicName;
			var topic = topics [topicName];
			expressionsControl.SetAndShow (topic);
		}

		public string CurrentTopicName {
			get {
				return this.currentTopicName;
			}
			set {
				currentTopicName = value;
			}
		}

		public void update (float delta)
		{
			start += delta;
//
			if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 2.0f && currentTopicName != "timeTopic" && currentTopicName != "exit1Topic" && currentTopicName != "exit2Topic" && currentTopicName != "exit3Topic" && currentTopicName != "exit4Topic" && currentTopicName != "badTestTopic") {
				TimeOutTopic ("timeTopic");
			} else if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 2.0f && (currentTopicName == "timeTopic" || currentTopicName == "exit1Topic" || currentTopicName == "exit2Topic" || currentTopicName == "exit3Topic" || currentTopicName == "exit4Topic")) {
				Application.Quit ();
			} else if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 2.0f && currentTopicName == "badTestTopic") {
				changeTopic ("noAnswTest");
			}
            expressionsControl.update (delta);
		}
			
	}
}
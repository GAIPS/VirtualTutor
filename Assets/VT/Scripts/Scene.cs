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
		private float objective = 10.0f;
		private int sceneOffset = 0;

		public float Start {
			get {
				return this.start;
			}
			set {
				start = value;
			}
		}

		public float Objective {
			get {
				return this.objective;
			}
			set {
				objective = value;
			}
		}

		public Scene ()
		{
		}

		private string currentTopicName;

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

			courseControl.SetAndShow (() => {
				start = 0.0f;

				currentTopicName = "badTestTopic";
				var topic2 = topics [currentTopicName];
				threePartsControl.SetAndShow (topic2);
				expressionsControl.SetAndShow (topic2);
				coursesControl.Disable ();
				courseControl.Disable ();
			}, () => {
			}, () => {
			}, () => {
			}, () => {
			});
		
		}

		public void OpenCalendar ()
		{
			start = 11.0f;
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

				start = 0.0f;
				currentTopicName = "enoughPlan";
				var topic3 = topics [currentTopicName];
				threePartsControl.SetAndShow (topic3);
				expressionsControl.SetAndShow (topic3);
				calendar1Control.Disable ();
				calendar2Control.Disable ();
				calendar3Control.Disable ();
				coursesControl.Disable ();
				courseControl.Disable ();


			});
		}

		public void changeTopic (string topicName)
		{
			start = 0.0f;
			objective = 10.0f;
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
			start = 0.0f;
			objective = 10.0f;
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

		public void updateScene (float delta)
		{
			start += delta;
			if (start >= objective && sceneOffset + 2 >= topics [currentTopicName].Lines.Count && currentTopicName != "timeTopic" && currentTopicName != "exit1Topic" && currentTopicName != "exit2Topic" && currentTopicName != "exit3Topic" && currentTopicName != "exit4Topic" && currentTopicName != "badTestTopic") {
				TimeOutTopic ("timeTopic");
			} else if (start >= objective && sceneOffset + 2 >= topics [currentTopicName].Lines.Count && (currentTopicName == "timeTopic" || currentTopicName == "exit1Topic" || currentTopicName == "exit2Topic" || currentTopicName == "exit3Topic" || currentTopicName == "exit4Topic")) { 
				Application.Quit ();
			} else if (start >= objective && sceneOffset + 2 >= topics [currentTopicName].Lines.Count && currentTopicName == "badTestTopic") {
				changeTopic ("noAnswTest");
			} else if (start >= objective && sceneOffset + 2 < topics [currentTopicName].Lines.Count) {
				expressionsControl.UpdateControl ();
				start = 0.0f;
				sceneOffset += 2;
			}
		}
			
	}
}
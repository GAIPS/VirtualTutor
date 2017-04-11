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

		public void changeTopic (string topicName)
		{
			start = 0.0f;
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
			if (start >= objective && sceneOffset + 2 >= topics[currentTopicName].Lines.Count && currentTopicName != "timeTopic" && currentTopicName != "exit1Topic" && currentTopicName != "exit2Topic" && currentTopicName != "exit3Topic" && currentTopicName != "exit4Topic") {
				TimeOutTopic ("timeTopic");
			} else if (start >= objective && sceneOffset + 2 >= topics[currentTopicName].Lines.Count && (currentTopicName == "timeTopic" || currentTopicName == "exit1Topic" || currentTopicName == "exit2Topic" || currentTopicName == "exit3Topic" || currentTopicName == "exit4Topic")) { 
				Application.Quit ();
			} else if (start >= objective && sceneOffset + 2 < topics[currentTopicName].Lines.Count) {
				expressionsControl.UpdateControl ();
				start = 0.0f;
				sceneOffset += 2;
			}
		}
			
	}
}
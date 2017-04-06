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

		// Use this for initialization
		public Scene ()
		{

		}

		public void changeTopic (string topicName)
		{
			if (!topics.ContainsKey (topicName)) {
				return;
			}

			var topic = topics [topicName];
			threePartsControl.SetAndShow (topic.Inputs [0].message, topic.Inputs [1].message, topic.Inputs [2].message, topic.Inputs [0].onClick, topic.Inputs [1].onClick, topic.Inputs [2].onClick);

		}
			
	}
}
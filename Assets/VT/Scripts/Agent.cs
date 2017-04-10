using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VT
{
	public class Agent
	{
		private bool isLeft = true;
		public bool IsLeft {
			get {
				return this.isLeft;
			}
			set {
				isLeft = value;
			}
		}




		public enum EmotionType
		{
			ANGRY,
			CRYING,
			DOMINANT,
			HATES,
			IMPATIENT,
			LIKES,
			POKERFACE,
			SAD,
			SHY,
			SMILING,
			SUBMISSIVE,
			SURPRISED,
			size
		}

		private EmotionType currentEmotion;

		public EmotionType CurrentEmotion {
			get {
				return this.currentEmotion;
			}
			set {
				currentEmotion = value;
			}
		}
	}
}

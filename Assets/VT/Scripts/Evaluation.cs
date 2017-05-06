using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VT
{
	public class Evaluation : Checkpoint
	{
		private string score;

		public Evaluation( string name, string date, int effort, int importance, string score){
			this.Name = name;
			this.Date = date;
			this.Effort = effort;
			this.Importance = importance;
			this.score = score;
		}

		public string Score {
			get {
				return this.score;
			}
			set {
				score = value;
			}
		}
	}
}


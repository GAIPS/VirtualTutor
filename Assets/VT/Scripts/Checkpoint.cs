using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VT
{
	public abstract class Checkpoint
	{
		private string name;
		private string date;
		private int effort;
		private int importance;
		public Checkpoint(){
		}
		public string Date {
			get {
				return this.date;
			}
			set {
				date = value;
			}
		}

	
		public string Name {
			get {
				return this.name;
			}
			set {
				name = value;
			}
		}
		public int Effort {
			get {
				return this.effort;
			}
			set {
				effort = value;
			}
		}

		public int Importance {
			get {
				return this.importance;
			}
			set {
				importance = value;
			}
		}
	}
}

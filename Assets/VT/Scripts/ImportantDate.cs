using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VT
{
	public class ImportantDate :Checkpoint
	{

		private bool done;
		public ImportantDate(string name, string date, int effort, int importance, bool done){
			this.Name = name;
			this.Date = date;
			this.Effort = effort;
			this.Importance = importance;
			this.done = done;
		}
		public bool Done {
			get {
				return this.done;
			}
			set {
				done = value;
			}
		}
	}
}

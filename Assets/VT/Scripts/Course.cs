using System.Collections.Generic;

namespace VT {
    public class Course
	{
		private string name;
		private float like;
		private float know;
		private float importance;
		private Dictionary<string,Checkpoint> checkpoints=new Dictionary<string,Checkpoint>();

		public Course(string name){
			this.name = name;
		}
		public string Name {
			get {
				return this.name;
			}
			set {
				name = value;
			}
		}
		public float Like {
			get {
				return this.like;
			}
			set {
				like = value;
			}
		}

		public float Know {
			get {
				return this.know;
			}
			set {
				know = value;
			}
		}

		public float Importance {
			get {
				return this.importance;
			}
			set {
				importance = value;
			}
		}

		public Dictionary<string,Checkpoint> Checkpoints {
			get {
				return this.checkpoints;
			}
			set {
				checkpoints = value;
			}
		}
	}
}

namespace VT {
    public class CheckBoxPoint :Checkpoint
	{

		private bool done;
		public CheckBoxPoint(string name, string date, int effort, int importance, bool done){
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

namespace VT {
    public class Line
	{
		string content;
		Agent speaker;
		float start;
		float end;

		public	Line (string c, Agent s, float start, float end)
		{
			content = c;
			speaker = s;
			this.start = start;
			this.end = end;
		}

		public Line (string c, Agent s){
			this.content = c;
			this.speaker = s;
		}
		public string Content {
			get {
				return this.content;
			}
			set {
				content = value;
			}
		}

		public Agent Speaker {
			get {
				return this.speaker;
			}
			set {
				speaker = value;
			}
		}
		public float Start {
			get {
				return this.start;
			}
			set {
				start = value;
			}
		}
		public float End {
			get {
				return this.end;
			}
			set {
				end = value;
			}
		}
	}
}
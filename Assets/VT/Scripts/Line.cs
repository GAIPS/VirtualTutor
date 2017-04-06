using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VT
{
	public class Line
	{
		string content;
		Agent speaker;

		public	Line (string c, Agent s)
		{
			content = c;
			speaker = s;
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
	}
}
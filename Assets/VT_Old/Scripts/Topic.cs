using HookControl;
using System.Collections.Generic;

namespace VT {
    public class Topic
    {
        public struct Input
        {
            public string message;

            public VoidFunc onClick;
          

            public Input (string message, VoidFunc onClick)
            {
                this.message = message;
                this.onClick = onClick;
                this.start = 0.0f;
            }
            public float start;

            public Input (string message, VoidFunc onClick, float start){
                this.message = message;
                this.onClick = onClick;
                this.start = start;
            }
        }

        private List<Line> lines = new List<Line> ();
        private Input[] inputs;
        public List<Line> Lines {
            get {
                return this.lines;
            }
            set {
                lines = value;
            }
        }
        public Input[] Inputs {
            get {
                return this.inputs;
            }
            set {
                inputs = value;
            }
        }

        public Topic (List<Line> l, Input[] inp)
        {
            lines = l;
            Inputs = inp;
        }

        public Topic (List<Line> l)
        {
            lines = l;
        }

    

        public override string ToString ()
        {
            string log = string.Empty;
            log+="[Topic: ]\n";
            log += "Lines: \n";
            foreach (Line l in Lines) {
                log += l.Content + "by: " + l.Speaker.IsLeft;
            
            }
            log += "options: \n";
            foreach (Topic.Input i in Inputs) {
                log += i.message + "\n";
            }
            return log;
        }
        
    }
}

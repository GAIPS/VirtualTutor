using System.Collections.Generic;

namespace VT {
    public class Course {
        private string name;
        public float like;
        public float know;
        public float importance;
        private List<Checkpoint> checkpoints = new List<Checkpoint>();

        public Course(string name) {
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

        public List<Checkpoint> Checkpoints {
            get {
                return this.checkpoints;
            }
            set {
                checkpoints = value;
            }
        }
    }
}

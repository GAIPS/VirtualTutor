using HookControl;
using System.Collections.Generic;
using UnityEngine;

namespace VT {
    public class Scene {
        public List<Agent> agents = new List<Agent>();
        public Dictionary<string, Topic> topics = new Dictionary<string, Topic>();
        public BalloonsControl balloonsControl;
        public FacesControl facesControl;

        public VoidFunc preUpdate;
        public VoidFunc postUpdate;

        private float start = 0.0f;

        private string currentTopicName;

        public float Start {
            get {
                return this.start;
            }
            set {
                start = value;
            }
        }

        public Scene() {
        }

        public Scene(GameObject facesPrefab, GameObject balloonsPrefab) {
            balloonsControl = new BalloonsControl(balloonsPrefab);
            facesControl = new FacesControl(facesPrefab);
        }

        public enum ShowOption {
            BOTH,
            HEAD,
            OPTIONS
        }

        public void changeTopic(string topicName, ShowOption option = ShowOption.BOTH) {
            start = 0.0f;

            if (!topics.ContainsKey(topicName)) {
                Debug.LogWarning("Topic named " + topicName + " not found.");
                return;
            }

            // NOTE: Controls Set and Show order are important!
            // If you show the expressions control after the three parts control it will not allow the user to click the options.
            currentTopicName = topicName;
            var topic = topics[topicName];
            if (option == ShowOption.HEAD || option == ShowOption.BOTH) {
                facesControl.SetAndShow(topic);
            }
            if (option == ShowOption.OPTIONS || option == ShowOption.BOTH) {
                balloonsControl.SetAndShow(topic);
            }

        }

        public string CurrentTopicName {
            get {
                return this.currentTopicName;
            }
            set {
                currentTopicName = value;
            }
        }

        public void update(float delta) {

            if (preUpdate != null) {
                preUpdate();
            }

            start += delta;
            facesControl.update(delta);
            balloonsControl.update(delta);

            if (postUpdate != null) {
                postUpdate();
            }
        }

    }
}
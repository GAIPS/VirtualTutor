using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VT
{
	public class Scene
	{
		public List<Agent> agents = new List<Agent> ();
		public Dictionary<string, Topic> topics = new Dictionary<string, Topic> ();
		public ThreePartsControl threePartsControl;
		public ExpressionsControl expressionsControl;
		public CoursesControl coursesControl;
		public Calendar1Control calendar1Control;
		public CourseControl courseControl;
		public Calendar2Control calendar2Control;
		public DiscussControl discussControl;
		public Calendar3Control calendar3Control;
		private float like;
		private float know;
		private float start = 0.0f;
		public Evaluation test1 = new Evaluation ("teste 1", "20/03/2017", 4, 4, "12.0");
		public Evaluation project1 = new Evaluation ("projecto 1", "04/04/2017", 3, 4, "16.0");
		public CheckBoxPoint revision = new CheckBoxPoint ("revisão", "08/04/2017", 1, 3, true);
		public Evaluation test2 = new Evaluation ("teste 2", "07/05/2017", 4, 4, "");
		public Evaluation test3 = new Evaluation ("teste 3", "22/06/2017", 4, 4, "");

		private string currentTopicName;

		public float Start {
			get {
				return this.start;
			}
			set {
				start = value;
			}
		}


		public Scene ()
		{
		}

		public enum ShowOption
		{
			BOTH,
			HEAD,
			OPTIONS

		}

		public void OpenCourses ()
		{
			coursesControl.SetAndShow (() => {
				OpenCourse ();
				start = 0.0f;
			});
		}

		public void OpenCourse ()
		{
			float evaluationResult = 10;
			courseControl.SetAndShow (
				() => {
					start = 0.0f;
					expressionsControl.Start = 0.0f;
					if (evaluationResult < 8.5|| (evaluationResult< 10.0 && like>3.0 && know>3.0)||(like<2&&know<2&&evaluationResult<6.0)) {
						agents [0].CurrentEmotion = Agent.EmotionType.CRYING;
						agents [1].CurrentEmotion = Agent.EmotionType.CRYING;
						currentTopicName = "badTestTopic";
						var topic2 = topics [currentTopicName];
						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();

					} else if ((evaluationResult >= 8.5 && evaluationResult < 11)||(evaluationResult>7.0&&evaluationResult<11&&like<2.0&&know<2.0)||(like>3&&know>3&&evaluationResult>=9.5&&evaluationResult<12)) {
						agents [0].CurrentEmotion = Agent.EmotionType.SAD;
						agents [1].CurrentEmotion = Agent.EmotionType.SAD;
						currentTopicName = "belowAvgTopic";
						var topic2 = topics [currentTopicName];

						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();

					} else if ((evaluationResult >= 11 && evaluationResult < 16.0)||(evaluationResult>9.5&&evaluationResult<14&& like<2&&know<2)||(like>3&&know>3&&evaluationResult>12&&evaluationResult<17.3)) {
						agents [0].CurrentEmotion = Agent.EmotionType.LIKES;
						agents [1].CurrentEmotion = Agent.EmotionType.SMILING;
						currentTopicName = "expectedTest";
						var topic2 = topics [currentTopicName];
						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();

					} else if (evaluationResult >= 16.0) {
						agents [0].CurrentEmotion = Agent.EmotionType.LIKES;
						agents [1].CurrentEmotion = Agent.EmotionType.LIKES;
						currentTopicName = "greatTest";
						var topic2 = topics [currentTopicName];
						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();
    				
					}
    					
				}, 
				() => {
				},
				() => {
				},
				() => {
				},
				() => {
				},
				(string value) => {
					//			test1Value =	float.Parse (value);
				},
				(string value) => {
					evaluationResult = float.Parse (value);
				}, () => {
			},
				() => {
				}, () => {
			}, () => {
			}, () => {
			}, (string value) => {
			}, (bool value) => {
				revision.Done = value;
				}, test1, project1, revision, test2, test3,(float value) => {
					know = value;
				},(float value)=>{
					like = value;
				}

			);
		
		}

		public void OpenCalendar ()
		{
			start = 0.0f;
			expressionsControl.Start = 0.0f;
			calendar1Control.SetAndShow (() => {
				SaveCalendar ();
			});	
		}
		public void OpenList(){
			start = 0.0f;
			expressionsControl.Start = 0.0f;
			discussControl.SetAndShow ("Reconsiderar planos de estudo", "Desistir da Cadeira", "Contactar o Tutor Real", () => {
				changeTopic ("onActivity");
				OpenCalendar();
				discussControl.Disable();
			}, () => {
				discussControl.Disable();
				changeTopic ("quit");

			}, () => {
				changeTopic ("contact");
				discussControl.Disable();
			});
		}
		public void SaveCalendar ()
		{
			calendar2Control.SetAndShow (() => {
				FinalCalendar ();
			});
		}

		public void FinalCalendar ()
		{

			calendar3Control.SetAndShow (() => {
				
				currentTopicName = "enoughPlan";
				var topic3 = topics [currentTopicName];
				threePartsControl.SetAndShow (topic3);
				expressionsControl.SetAndShow (topic3);
				calendar1Control.Disable ();
				calendar2Control.Disable ();
				calendar3Control.Disable ();
				coursesControl.Disable ();
				courseControl.Disable ();
				start = 0.0f;
				expressionsControl.Start = 0.0f;


			});
		}

		public void changeTopic (string topicName, ShowOption option = ShowOption.BOTH)
		{
			start = 0.0f;
			expressionsControl.Start = 0.0f;
			if (!topics.ContainsKey (topicName)) {
				return;
			}
			currentTopicName = topicName;
			var topic = topics [topicName];
			expressionsControl.SetAndShow (topic);
			if (option != ShowOption.HEAD)
				threePartsControl.SetAndShow (topic);

		}

		public string CurrentTopicName {
			get {
				return this.currentTopicName;
			}
			set {
				currentTopicName = value;
			}
		}

		public void update (float delta)
		{
			start += delta;
//
			if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 5.0f && currentTopicName != "timeTopic" && currentTopicName != "exit1Topic" && currentTopicName != "exit2Topic" && currentTopicName != "exit3Topic" && currentTopicName != "exit4Topic" && currentTopicName != "badTestTopic" && currentTopicName != "onActivity") {
				agents [0].CurrentEmotion = Agent.EmotionType.IMPATIENT;
				agents [1].CurrentEmotion = Agent.EmotionType.IMPATIENT;
				changeTopic ("timeTopic", ShowOption.HEAD);
			} else if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 5.0f && (currentTopicName == "timeTopic" || currentTopicName == "exit1Topic" || currentTopicName == "exit2Topic" || currentTopicName == "exit3Topic" || currentTopicName == "exit4Topic")) {
				Application.Quit ();
			} else if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 5.0f && currentTopicName == "badTestTopic") {
				changeTopic ("noAnswTest");
			}
			expressionsControl.update (delta);
			threePartsControl.update (delta);
		}
			
	}
}
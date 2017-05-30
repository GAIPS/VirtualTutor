using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DeadMosquito.AndroidGoodies;

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
		public Course course1 = new Course ("Fundamentos da Programação");
		public Course course2 = new Course ("Álgebra Linear");
		private float start = 0.0f;
		private string wordToParse;
		Course clickedCourse;

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
				clickedCourse = course1;
				OpenCourse ();
				start = 0.0f;
			}, () => {
				clickedCourse = course2;
				OpenCourse ();
				start = 0.0f;
			}, course1, course2);
		}

		public void OpenCourse ()
		{
			float evaluationResult = 10;
			courseControl.SetAndShow (
				() => {
					start = 0.0f;
					expressionsControl.Start = 0.0f;
					if (clickedCourse == course1 && (evaluationResult < 8.5 || (evaluationResult < 10.0 && clickedCourse.Like > 3.0 && clickedCourse.Know > 3.0) || (clickedCourse.Like < 2 && clickedCourse.Know < 2 && evaluationResult < 6.0))) {
						agents [0].CurrentEmotion = Agent.EmotionType.CRYING;
						agents [1].CurrentEmotion = Agent.EmotionType.CRYING;
						currentTopicName = "badTestTopic";
						var topic2 = topics [currentTopicName];
						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();

					} else if (clickedCourse == course1 && (evaluationResult >= 8.5 && evaluationResult < 11) || (evaluationResult > 7.0 && evaluationResult < 11 && clickedCourse.Like < 2.0 && clickedCourse.Know < 2.0) || (clickedCourse.Like > 3 && clickedCourse.Know > 3 && evaluationResult >= 9.5 && evaluationResult < 12)) {
						agents [0].CurrentEmotion = Agent.EmotionType.SAD;
						agents [1].CurrentEmotion = Agent.EmotionType.SAD;
						currentTopicName = "belowAvgTopic";
						var topic2 = topics [currentTopicName];

						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();

					} else if (clickedCourse == course1 && (evaluationResult >= 11 && evaluationResult < 16.0) || (evaluationResult > 9.5 && evaluationResult < 14 && clickedCourse.Like < 2 && clickedCourse.Know < 2) || (clickedCourse.Like > 3 && clickedCourse.Know > 3 && evaluationResult > 12 && evaluationResult < 17.3)) {
						agents [0].CurrentEmotion = Agent.EmotionType.LIKES;
						agents [1].CurrentEmotion = Agent.EmotionType.SMILING;
						currentTopicName = "expectedTest";
						var topic2 = topics [currentTopicName];
						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();

					} else if (clickedCourse == course1 && evaluationResult >= 16.0) {
						agents [0].CurrentEmotion = Agent.EmotionType.LIKES;
						agents [1].CurrentEmotion = Agent.EmotionType.LIKES;
						currentTopicName = "greatTest";
						var topic2 = topics [currentTopicName];
						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();
    				
					} else if (clickedCourse == course2 && (evaluationResult < 6 || (evaluationResult < 10.0 && clickedCourse.Like > 3.0 && clickedCourse.Know > 3.0) || (clickedCourse.Like < 2 && clickedCourse.Know < 2 && evaluationResult < 6.0))) {
						agents [0].CurrentEmotion = Agent.EmotionType.CRYING;
						agents [1].CurrentEmotion = Agent.EmotionType.CRYING;
						currentTopicName = "badTestTopic";
						var topic2 = topics [currentTopicName];
						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();
					} else if(clickedCourse == course2 && (evaluationResult >= 6.1 && evaluationResult < 10) || (evaluationResult > 7.1 && evaluationResult < 9 && clickedCourse.Like < 2.0 && clickedCourse.Know < 2.0) || (clickedCourse.Like > 3 && clickedCourse.Know > 3 && evaluationResult >= 9.5 && evaluationResult < 11)) {
						agents [0].CurrentEmotion = Agent.EmotionType.SAD;
						agents [1].CurrentEmotion = Agent.EmotionType.SAD;
						currentTopicName = "belowAvgTopic";
						var topic2 = topics [currentTopicName];

						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();
					} else if  (clickedCourse == course2 && (evaluationResult >= 10 && evaluationResult <= 14.0) || (evaluationResult > 8.5 && evaluationResult < 13 && clickedCourse.Like < 2 && clickedCourse.Know < 2) || (clickedCourse.Like > 3 && clickedCourse.Know > 3 && evaluationResult > 11 && evaluationResult < 16)){
						agents [0].CurrentEmotion = Agent.EmotionType.LIKES;
						agents [1].CurrentEmotion = Agent.EmotionType.SMILING;
						currentTopicName = "expectedTest";
						var topic2 = topics [currentTopicName];
						threePartsControl.SetAndShow (topic2);
						expressionsControl.SetAndShow (topic2);
						coursesControl.Disable ();
						courseControl.Disable ();
					}else if(clickedCourse == course2 && evaluationResult >= 15.0) {
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
				var revision = clickedCourse.Checkpoints ["revision"] as CheckBoxPoint;
				if (revision != null)
					revision.Done = value;
			}, clickedCourse.Checkpoints ["test1"], clickedCourse.Checkpoints ["project1"], clickedCourse.Checkpoints ["revision"], clickedCourse.Checkpoints ["test2"], clickedCourse.Checkpoints ["test3"], (float value) => {
				clickedCourse.Know = value;
			}, (float value) => {
				clickedCourse.Like = value;
			}, (float value) => {
				clickedCourse.Importance = value;
			}, clickedCourse.Name

			);
		
		}

		public void OpenCalendar ()
		{
			start = 0.0f;
			expressionsControl.Start = 0.0f;

            #if UNITY_ANDROID
            currentTopicName = "enoughPlan";
            var topic3 = topics [currentTopicName];
            threePartsControl.SetAndShow (topic3);
            expressionsControl.SetAndShow (topic3);
            coursesControl.Disable ();
            courseControl.Disable ();
            start = 0.0f;
            expressionsControl.Start = 0.0f;

            Debug.Log("Opening Calendar (Only works on Android)");
            if (AGCalendar.UserHasCalendarApp()) {
                var beginTime = DateTime.Now;
                var endTime = beginTime.AddHours(1.5);
                var eventBuilder = new AGCalendar.EventBuilder("Study Programming - VT", beginTime);
                eventBuilder.SetEndTime(endTime);
                eventBuilder.SetIsAllDay(false);
                eventBuilder.SetDescription("Open Virtual Tutor to continue your experience.");
                eventBuilder.SetAccessLevel(AGCalendar.EventAccessLevel.Public);
                eventBuilder.SetAvailability(AGCalendar.EventAvailability.Busy);
                eventBuilder.BuildAndShow();
            }
            #else
            calendar1Control.SetAndShow (() => {
                SaveCalendar ();
            }); 
            #endif
		}

		public void OpenList ()
		{
			start = 0.0f;
			expressionsControl.Start = 0.0f;
			discussControl.SetAndShow (() => {
				agents[0].CurrentEmotion = Agent.EmotionType.SMILING;
				agents[1].CurrentEmotion = Agent.EmotionType.SMILING;
				discussControl.Disable();
				changeTopic("help");

			},(string value) => {
				wordToParse = value;

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
			if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 5.0f && currentTopicName != "timeTopic" && currentTopicName != "exit1Topic" && currentTopicName != "exit2Topic" && currentTopicName != "exit3Topic" && currentTopicName != "exit4Topic" && currentTopicName != "badTestTopic" && currentTopicName != "onActivity" && currentTopicName != "prePlan") {
				agents [0].CurrentEmotion = Agent.EmotionType.IMPATIENT;
				agents [1].CurrentEmotion = Agent.EmotionType.IMPATIENT;
				changeTopic ("timeTopic", ShowOption.HEAD);
			} else if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 5.0f && (currentTopicName == "timeTopic" || currentTopicName == "exit1Topic" || currentTopicName == "exit2Topic" || currentTopicName == "exit3Topic" || currentTopicName == "exit4Topic")) {
				Application.Quit ();
			} else if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 5.0f && currentTopicName == "badTestTopic") {
				changeTopic ("noAnswTest");
			} else if (start >= topics [currentTopicName].Lines [topics [currentTopicName].Lines.Count - 1].End + 5.0f && currentTopicName == "prePlan")
				OpenCalendar ();
			expressionsControl.update (delta);
			threePartsControl.update (delta);
		}
			
	}
}
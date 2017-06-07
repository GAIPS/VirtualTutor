using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace VT
{
	public class MainScript : MonoBehaviour
	{
		public bool playSplashScreen = true;

		public GameObject splashScreenPrefab;
		public GameObject threeOptionsPrefab;
		public GameObject ExpressionsPrefab;
		public GameObject coursesPrefab;
		public GameObject coursePrefab;
		public GameObject calendar1Prefab;
		public GameObject calendar2Prefab;
		public GameObject calendar3Prefab;
		public GameObject discussPrefab;
		public ToggleImage pauseButton;
		private Scene scene;

		private bool playing = false;

		// Use this for initialization
		void Start ()
		{

			if (splashScreenPrefab != null && playSplashScreen) {
				SplashScreenControl splashScreenControl = new SplashScreenControl (splashScreenPrefab);
				splashScreenControl.SetAndShow (/*OnEndFunction*/() => {
					playing = true;
					splashScreenControl.Destroy ();
				});
			} else {
				playing = true;
			}
			
			if (threeOptionsPrefab == null || ExpressionsPrefab == null
			             || coursesPrefab == null || coursePrefab == null
			             || calendar1Prefab == null || calendar2Prefab == null
			             || calendar3Prefab == null) {
				Debug.LogWarning ("Some prefabs are null");
				return;
			}

			if (pauseButton) {
				pauseButton.onClick = (bool isOn) => {
					playing = isOn;
				};
			}

			scene = new Scene ();
			scene.threePartsControl = new ThreePartsControl (threeOptionsPrefab);
			scene.expressionsControl = new ExpressionsControl (ExpressionsPrefab);
			scene.coursesControl = new CoursesControl (coursesPrefab);
			scene.courseControl = new CourseControl (coursePrefab);
			scene.calendar1Control = new Calendar1Control (calendar1Prefab);
			scene.calendar2Control = new Calendar2Control (calendar2Prefab);
			scene.calendar3Control = new Calendar3Control (calendar3Prefab);
			scene.discussControl = new DiscussControl (discussPrefab);

			PopulateScene (scene);

			//start
			scene.changeTopic ("Hello");
		}

		void Update ()
		{
			if (Input.GetKeyUp (KeyCode.Space)) {
				playing = !playing;
			}
			if (playing) {
				scene.update (Time.deltaTime);
			}
		}

		void PopulateScene (Scene demoScene)
		{
		
			Agent happy = new Agent ();
			Agent grumpy = new Agent ();
			grumpy.IsLeft = false;

			demoScene.agents.Add (happy);
			demoScene.agents.Add (grumpy);
			//Hello
			happy.CurrentEmotion = Agent.EmotionType.SMILING;
			grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
			Evaluation test1 = new Evaluation ("1º Teste",
				                            "20/03/2017",
				                            4,
				                            4,
				                            "12.0");
			Evaluation project1 = new Evaluation ("1º Projecto",
				                               "04/04/2017",
				                               3,
				                               4,
				                               "16.0");
			CheckBoxPoint revision = new CheckBoxPoint ("PDF Aula nº 20",
				                                  "08/04/2017",
				                                  1,
				                                  3,
				                                  true);
			Evaluation test2 = new Evaluation ("2º Teste",
				                            "07/05/2017",
				                            4,
				                            4,
				                            "");
			Evaluation test3 = new Evaluation ("3º Teste",
				                            "22/06/2017",
				                            4,
				                            4,
				                            "");
			demoScene.course1.Checkpoints.Add ("test1", test1);
			demoScene.course1.Checkpoints.Add ("project1", project1);
			demoScene.course1.Checkpoints.Add ("revision", revision);
			demoScene.course1.Checkpoints.Add ("test2", test2);
			demoScene.course1.Checkpoints.Add ("test3", test3);
			Evaluation c2Test1 = new Evaluation ("1º Teste",
				                              "13/03/2017",
				                              3,
				                              4,
				                              "14.0");
			Evaluation c2Test2 = new Evaluation ("2º Teste",
				                              "15/04/2017",
				                              3,
				                              4,
				                              "12.0");
			CheckBoxPoint achievment = new CheckBoxPoint ("Aula de Dúvidas",
				                                    "08/05/2017",
				                                    1,
				                                    3,
				                                    false);
			Evaluation c2Teste3 = new Evaluation ("3º Teste",
				                               "09/05/2017",
				                               3,
				                               4,
				                               "");
			Evaluation c2Test4 = new Evaluation ("4º Teste",
				                              "2/06/2017",
				                              3,
				                              4,
				                              "");
			demoScene.course2.Checkpoints.Add ("test1", c2Test1);
			demoScene.course2.Checkpoints.Add ("project1", c2Test2);
			demoScene.course2.Checkpoints.Add ("revision", achievment);
			demoScene.course2.Checkpoints.Add ("test2", c2Teste3);
			demoScene.course2.Checkpoints.Add ("test3", c2Test4);

			Line l1 = new Line ("Hello. It is good to see you after these 2 days.",
				                   happy,
				                   0.0f,
				                   8.0f);
			Line l2 = new Line ("I was wondering if they would come",
				                   grumpy,
				                   4.0f,
				                   12.0f);
			Line l3 = new Line ("What can we help you with?",
				                   happy,
				                   12.5f,
				                   20.5f);
			Line l4 = new Line ("I wish to remind you that you have a checkpoint in 2 weeks",
				                   grumpy,
				                   16.5f,
				                   24.5f);
			List<Line> lines = new List<Line> ();
			lines.Add (l1);
			lines.Add (l2);
			lines.Add (l3);
			lines.Add (l4);
			lines = lines.OrderBy (l => l.Start).ToList ();
			Topic.Input[] inputs = {
                    new Topic.Input(
                        "Remind me...", () => {
						
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("reminder");
                        }, 1.5f), 
                    new Topic.Input(
                        "I would like to talk about ...",
                        () => {

                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("talkAbout");	
                        }, 1.5f), 
                    new Topic.Input(
                        "I have new information",
                        () => {
						
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("newInfoTopic");
                        }, 1.5f) 
                };
            Topic hello = new Topic(lines, inputs);
            demoScene.topics.Add("Hello", hello);
            //not developed

            Line l5 = new Line("Não podemos fazer nada disso agora. Malditos programadores.",
                               grumpy,
                               0.0f,
                               8.0f);
            Line l6 = new Line("Assim que tivermos esta funcionalidade falaremos contigo.",
                               happy,
                               4.0f,
                               12.0f);
            List<Line> nDLines = new List<Line>();
            nDLines.Add(l5);
            nDLines.Add(l6);
            Topic.Input[] inputs2 =
                { new Topic.Input(
                        "está bem", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("Hello");
                        }, 1.5f),
                    new Topic.Input("", () => {
                            Debug.Log("top");
                        }, 1.5f),
                    new Topic.Input("fechem a aplicação por favor", () => {
                            Application.Quit();
                        }, 3.0f
                    )
                };
            Topic nonDeveloped = new Topic(nDLines, inputs2);
            demoScene.topics.Add("nonDeveloped", nonDeveloped);

            //new Info?
            Line l7 = new Line("New information? Alright, but hurry.",
                               grumpy,
                               0.0f,
                               8.0f);
            Line l8 = new Line("Don't be mean", happy, 4.0f, 12.0f);
            List<Line> newInfo = new List<Line>();
            newInfo.Add(l7);
            newInfo.Add(l8);
            Topic.Input[] inputs1 =
                {new Topic.Input("", () => {
                      
			}), new Topic.Input("", () => {
                        }), new Topic.Input("", () => {
                        })
                };
            Topic newInfoTopic = new Topic(newInfo, inputs1);
            demoScene.topics.Add("newInfoTopic", newInfoTopic);

            // Bad test
//			happy.CurrentEmotion = Agent.EmotionType.CRYING; para por quando houver passagem
//			grumpy.CurrentEmotion = Agent.EmotionType.CRYING;
//
            Line l9 = new Line("Oh no!", happy, 0.0f, 8.0f);
            Line l10 = new Line("You should have studied more...",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l11 = new Line("Can we make a study plan to help you?",
                                happy,
                                12.5f,
                                20.5f);
            Line l56 = new Line("Maybe it can help you improve.",
                                grumpy,
                                16.5f,
                                24.5f);

            Topic.Input[] inputs3 =
                { new Topic.Input("Study Plan? Ok", () => {
                            grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
                            happy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
                            demoScene.changeTopic("onActivity");
                            demoScene.changeTopic("prePlan");
                        }, 14.5f),
                    //new Topic.Input("I want to be told sooner", () => {
                    new Topic.Input("I am not feeling well, contact my tutor", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SHY;
                            grumpy.CurrentEmotion = Agent.EmotionType.SAD;
                            //demoScene.changeTopic("warnTestTopic");
                            demoScene.changeTopic("contact");
                        }, 3.0f), new Topic.Input("I am fine, thank you", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

                            demoScene.changeTopic("help");
                        }, 4.5f)
                };
	
            List<Line> badTest = new List<Line>();
            badTest.Add(l9);
            badTest.Add(l10);
            badTest.Add(l11);
            badTest.Add(l56);
            Topic badTestTopic = new Topic(badTest, inputs3);
            demoScene.topics.Add("badTestTopic", badTestTopic);

            //Below Average
//			happy.CurrentEmotion = Agent.EmotionType.SAD;
//			grumpy.CurrentEmotion = Agent.EmotionType.SAD;
            //colar quando houver mudança
            Line l12 = new Line("You should have studied harder",
                                grumpy,
                                0.0f,
                                8.0f);
            Line l13 = new Line("Oh, this test did not go as expected",
                                happy,
                                4.0f,
                                12.0f);
            Line l14 = new Line("To overcome this challenge we should make a study plan",
                                happy,
                                12.5f,
                                20.5f);
            List<Line> belowAvg = new List<Line>();
            belowAvg.Add(l12);
            belowAvg.Add(l13);
            belowAvg.Add(l14);
            belowAvg.Add(l56);
            Topic belowAvgTopic = new Topic(belowAvg, inputs3);
            demoScene.topics.Add("belowAvgTopic", belowAvgTopic);

//			//mudar data de testes mais cedo

            Line l15 = new Line("Ok, with this you will be notified earlier.",
                                happy,
                                0.0f,
                                8.0f);
            Line l16 = new Line("I still think a study plan would be good.",
                                grumpy,
                                4.0f,
                                12.0f);
            Topic.Input[] inputs4 =
                { new Topic.Input("Study plan? Ok.", () => {
                            demoScene.changeTopic("prePlan");
                        }, 1.5f), new Topic.Input("Thank you", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

                            demoScene.changeTopic("help");
                        }, 3.0f), new Topic.Input("How about...", () => {
                            demoScene.OpenList();
                        }, 4.5f)
                };
            List<Line> warnTest = new List<Line>();
            warnTest.Add(l15);
            warnTest.Add(l16);
            Topic warnTestTopic = new Topic(warnTest, inputs4);
            demoScene.topics.Add("warnTestTopic", warnTestTopic);

//			//expected
//			
            //colocar quando tiver link
            Line l17 = new Line("Congratulations on a good grade.",
                                happy,
                                0.0f,
                                8.0f);
            Line l18 = new Line("There is still room for improvement.",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l19 = new Line("Don't rain on their parade",
                                happy,
                                12.5f,
                                20.5f);
            Line l20 = new Line("Do not start with me, I just mean with a little more work it would be perfect",
                                grumpy,
                                16.5f,
                                24.5f);
            Line l21 = new Line("Alright, but it is still very good.",
                                happy,
                                25.0f,
                                33.0f);
            Line l57 = new Line("Keep up the goof work", grumpy, 29.0f, 37.0f);
            List<Line> expected = new List<Line>();
            expected.Add(l17);
            expected.Add(l18);
            expected.Add(l19);
            expected.Add(l20);
            expected.Add(l21);
            expected.Add(l57);
            Topic.Input[] inputs5 =
                {new Topic.Input("Make a study plan", () => {
                            demoScene.changeTopic("prePlan");
                        }, 1.5f), new Topic.Input("I want to be notified sooner", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SHY;
                            grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
                            demoScene.changeTopic("warnTestTopic");
                        }, 3.0f), new Topic.Input("Thank you", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

                            demoScene.changeTopic("help");
                        }, 4.5f)
                };

            Topic expectedTest = new Topic(expected, inputs5);
            demoScene.topics.Add("expectedTest", expectedTest);

//			//Muito bom
//			happy.CurrentEmotion = Agent.EmotionType.LIKES;
//			grumpy.CurrentEmotion = Agent.EmotionType.LIKES;
            //Colocar quando der
            Line l22 = new Line("Congratulations on an awesome grade!",
                                happy,
                                0.0f,
                                8.0f);
            Line l23 = new Line("Yes, terrific.", grumpy, 4.0f, 12.0f);
            Line l24 = new Line("What? No comment?",
                                happy,
                                12.5f,
                                20.5f);
            Line l25 = new Line("sigh...", grumpy, 16.5f, 24.5f);
            List<Line> great = new List<Line>();
            great.Add(l22);
            great.Add(l23);
            great.Add(l24);
            great.Add(l25);
            Topic greatTest = new Topic(great, inputs5);
            demoScene.topics.Add("greatTest", greatTest);

//			//Horas a menos
//			happy.CurrentEmotion = Agent.EmotionType.SAD;
//			grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
            //Colocar quando der
            Line l26 = new Line("Não sei se só isto é boa ideia, tens a certeza?",
                                happy,
                                0.0f,
                                8.0f);
            Line l27 = new Line("Apenas essas horas é capaz de correr mal.",
                                grumpy,
                                4.0f,
                                12.0f);
            Topic.Input[] inputs6 =
                { new Topic.Input("Tenho a certeza", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

                            demoScene.changeTopic("help");
                        }, 1.5f), new Topic.Input("", () => {
                        }, 3.0f), new Topic.Input("Deixem-me corrigir uma coisa", () => {
                            demoScene.OpenCalendar();
                        }, 4.5f)
                };
            List<Line> fewTime = new List<Line>();
            fewTime.Add(l26);
            fewTime.Add(l27);

            Topic ShortPlan = new Topic(fewTime, inputs6);
            demoScene.topics.Add("ShortPlan", ShortPlan);

//			//Horas a mais
//			happy.CurrentEmotion = Agent.EmotionType.SAD;
//			grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
            //Quando colocar colar
            Line l29 = new Line("Isto não te parece demasiado?",
                                happy,
                                0.0f,
                                8.0f);
            Line l30 = new Line("Eu sei que te digo para estudar mais mas isto é demais.",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l31 = new Line("Se não tiveres tempo para descansar não vais usar o teu potencial máximo.",
                                happy,
                                12.5f,
                                20.5f);
            Line l58 = new Line("Aconselho-te a diminuir um pouco as horas",
                                grumpy,
                                16.5f,
                                24.5f);
            List<Line> muchTime = new List<Line>();
            muchTime.Add(l29);
            muchTime.Add(l30);
            muchTime.Add(l31);
            muchTime.Add(l58);
            Topic BigPlan = new Topic(muchTime, inputs6);
            demoScene.topics.Add("BigPlan", BigPlan);
//
//			//JUst enough
//			happy.CurrentEmotion = Agent.EmotionType.SMILING;
//			grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
            //Quando colocar colar
            Line l32 = new Line("Ok great, it is settled.", happy, 0.0f, 8.0f);
            Line l33 = new Line("I hope you can keep it up.",
                                grumpy,
                                4.0f,
                                12.0f);

            Topic.Input[] inputs7 =
                { new Topic.Input("Thank you.", () => {

                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("help");
                        }, 1.5f), new Topic.Input("", () => {
                        }, 3.0f), new Topic.Input("Let me fix one thing.", () => {
                            demoScene.OpenCalendar();
                        }, 4.5f)
                };
            List<Line> enoughTime = new List<Line>();
            enoughTime.Add(l32);
            enoughTime.Add(l33);
            Topic enoughPlan = new Topic(enoughTime, inputs7);
            demoScene.topics.Add("enoughPlan", enoughPlan);


            //More help?
//			//No need to change?
            Line l34 = new Line("Can we help a little more?",
                                happy,
                                0.0f,
                                8.0f);
            Line l35 = new Line("So it is \" us\" now? ", grumpy, 4.0f, 12.0f);
            Topic.Input[] inputs8 =
                { new Topic.Input("I would like to talk about...", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("talkAbout");	
                        }, 1.5f), new Topic.Input("No, Thank you", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
                            demoScene.changeTopic("twoDaysTopic");
                        }, 1.5f), new Topic.Input("I have new information", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("newInfoTopic");
                        }, 1.5f)
                };
            List<Line> moreHelp = new List<Line>();
            moreHelp.Add(l34);
            moreHelp.Add(l35);
            Topic help = new Topic(moreHelp, inputs8);
            demoScene.topics.Add("help", help);

//		
//			// 2days with plan
            Line l36 = new Line("Come see us in two days so we can check up on your plan",
                                happy,
                                0.0f,
                                8.0f);
            Line l37 = new Line("I hope you show up", grumpy, 4.0f, 12.0f);
            Topic.Input[] inputs9 =
                { new Topic.Input("See you in 2 days", () => {
                            happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            demoScene.changeTopic("exit1Topic");
                        }, 2.5f), new Topic.Input("", () => {
                        }, 2.5f), new Topic.Input("That is too soon", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SHY;
                            grumpy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
                            demoScene.changeTopic("tooEarly");
                        }, 2.5f)
                };
            List<Line> twodays = new List<Line>();
            twodays.Add(l36);
            twodays.Add(l37);
            Topic twoDaysTopic = new Topic(twodays, inputs9);
            demoScene.topics.Add("twoDaysTopic", twoDaysTopic);
	
//			//cedo
            Line l38 = new Line("When do you want to meet with us?",
                                happy,
                                0.0f,
                                8.0f);
            Line l39 = new Line("Maybe they don't want to see you",
                                grumpy,
                                4.0f,
                                12.0f);
            Topic.Input[] inputs10 =
                { new Topic.Input(" 5 days", () => {
                            happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            demoScene.changeTopic("exit2Topic");
                        }, 1.5f), new Topic.Input("7 days", () => {
                            happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            demoScene.changeTopic("exit3Topic");
                        }, 3.0f), new Topic.Input("I am not sure", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SURPRISED;
                            grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
                            demoScene.changeTopic("exit4Topic");
                        }, 4.5f)
                };
            List<Line> early = new List<Line>();
            early.Add(l38);
            early.Add(l39);
            Topic tooEarly = new Topic(early, inputs10);
            demoScene.topics.Add("tooEarly", tooEarly);
//
            Line l40 = new Line("Ok, see you in 2 days!", happy, 0.0f, 8.0f);
            Line l41 = new Line("I will switch this off to save all of us time.",
                                grumpy,
                                4.0f,
                                12.0f);
            List<Line> exit1 = new List<Line>();
            exit1.Add(l40);
            exit1.Add(l41);
//			//nao vai haver inputs just close window
            Topic.Input[] emptyInputs =
                {
                    new Topic.Input("", () => {
                        }),
                    new Topic.Input("", () => {
                        }),
                    new Topic.Input("", () => {
                        })
                };

            Topic exit1Topic = new Topic(exit1, emptyInputs);
            demoScene.topics.Add("exit1Topic", exit1Topic);
//
//			//exit 5 days
            Line l42 = new Line("ok, see you in 5 days!", happy, 0.0f, 8.0f);

            List<Line> exit2 = new List<Line>();
            exit2.Add(l41);
            exit2.Add(l42);
            Topic exit2Topic = new Topic(exit2, emptyInputs);
            demoScene.topics.Add("exit2Topic", exit2Topic);
//
//			//exit 7 days
            Line l43 = new Line("ok,see you in 7 days!", happy, 0.0f, 8.0f);
            List<Line> exit3 = new List<Line>();
            exit3.Add(l43);
            exit3.Add(l41);
            Topic exit3Topic = new Topic(exit3, emptyInputs);
            demoScene.topics.Add("exit3Topic", exit3Topic);

//			//Nao sabe
            Line l44 = new Line("Apparently he does not know, it is normal, not everyone is a machine like us.",
                                grumpy,
                                0.0f,
                                8.0f);
            Line l45 = new Line("See you when you next show up",
                                happy,
                                4.0f,
                                12.0f);
            List<Line> exit4 = new List<Line>();
            exit4.Add(l44);
            exit4.Add(l45);
            Topic exit4Topic = new Topic(exit4, emptyInputs);
            demoScene.topics.Add("exit4Topic", exit4Topic);

            //TimeOutDefault

            Line l46 = new Line("Well perhaps they don't want to talk.",
                                grumpy,
                                0.0f,
                                8.0f);
            Line l47 = new Line("Surely your attitude's fault.",
                                happy,
                                4.0f,
                                12.0f);
            Line l48 = new Line("Yes because you have a great personality.",
                                grumpy,
                                12.5f,
                                20.5f);
            Line l49 = new Line("Now what then?", happy, 16.5f, 24.5f);
            Line l50 = new Line("Just turn it off since you're so chipper, let us go",
                                grumpy,
                                25.0f,
                                33.0f);
            Line l51 = new Line("If you say so...", happy, 29.0f, 37.0f);
            List<Line> timeout1 = new List<Line>();
            timeout1.Add(l46);
            timeout1.Add(l47);
            timeout1.Add(l48);
            timeout1.Add(l49);
            timeout1.Add(l50);
            timeout1.Add(l51);
            Topic timeTopic = new Topic(timeout1, emptyInputs);
            demoScene.topics.Add("timeTopic", timeTopic);

            //not answering test
            Line l52 = new Line("Well, I guess you don't want to talk",
                                happy,
                                0.0f,
                                8.0f);
            Line l53 = new Line("It would help you to do a study plan.",
                                grumpy,
                                4.0f,
                                12.0f);
            Topic.Input[] notAnswTestInputs =
                { new Topic.Input("Make a study plan!", () => {
                            demoScene.changeTopic("prePlan");
                        }, 1.5f), new Topic.Input("", () => {
                        }, 3.0f), new Topic.Input("Thank you", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("help");
                        }, 4.5f)
                };

            List<Line> noAnswerTest = new List<Line>();
            noAnswerTest.Add(l52);
            noAnswerTest.Add(l53);
            Topic noAnswTest = new Topic(noAnswerTest, notAnswTestInputs);
            demoScene.topics.Add("noAnswTest", noAnswTest);

            //			//Not answering Bad plan
            Line l54 = new Line("Ok they wants to insist in this bad plan",
                                grumpy,
                                0.0f,
                                8.0f);
            Line l55 = new Line("We have to trust them", happy, 4.0f, 12.0f);
            List<Line> noAnswerPlan = new List<Line>();

            noAnswerPlan.Add(l54);
            noAnswerPlan.Add(l55);
            Topic noAnswPlan = new Topic(noAnswerPlan, emptyInputs);
            demoScene.topics.Add("noAnswPlan", noAnswPlan);

            Line l59 = new Line("A new information, what will it be? a test?.",
                                happy,
                                0.0f,
                                8.0f);
            Line l60 = new Line("I hope everything went well",
                                grumpy,
                                4.0f,
                                12.0f);
            List<Line> onActivityList = new List<Line>();
            onActivityList.Add(l59);
            onActivityList.Add(l60);
            Topic onActivity = new Topic(onActivityList, emptyInputs);
            demoScene.topics.Add("onActivity", onActivity);

            Line l61 = new Line("Anything you need, do not lean on my partner",
                                grumpy,
                                4.0f,
                                12.0f);
            List<Line> returnlist = new List<Line>();
            returnlist.Add(l34);
            returnlist.Add(l61);
            Topic returnTopic = new Topic(returnlist, inputs);
            demoScene.topics.Add("returnTopic", returnTopic);

            List<Line> remindList = new List<Line>();
            remindList.Add(new Line("We know you have a checkpoint in 10 days",
                                    happy,
                                    0.5f,
                                    10.0f));
            remindList.Add(new Line("That's it for now", grumpy, 5.0f, 12.0f));
            Topic.Input[] remindInputs =
                { new Topic.Input("Thank you", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("help");
                        }, 3.0f), new Topic.Input("", () => {
                        }), new Topic.Input("I have new information", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("newInfoTopic");
                        }, 3.0f) 
                };
            Topic reminder = new Topic(remindList, remindInputs);
            demoScene.topics.Add("reminder", reminder);
				
            Line l66 = new Line("What would you like to talk about?",
                                happy,
                                0.0f,
                                8.0f);
            Line l67 = new Line("Please no personal things, we are study agents",
                                grumpy,
                                4.0f,
                                12.0f);
            List<Line> talkAboutList = new List<Line>();
            talkAboutList.Add(l66);
            talkAboutList.Add(l67);
            Topic.Input[] talkInputs =
                { new Topic.Input("Past evaluations", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("pastTopic");
                        }, 1.5f), new Topic.Input("Semester analysis", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
                            demoScene.changeTopic("balance");
                        }, 3.0f), new Topic.Input("Tips", () => {
                            happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            demoScene.changeTopic("tips");
                        }, 4.5f), new Topic.Input("More...", () => {
                            demoScene.OpenList();
                        }, 6.0f)
                };
            Topic talkAbout = new Topic(talkAboutList, talkInputs);
            demoScene.topics.Add("talkAbout", talkAbout);

            Line l70 = new Line("On FP we know we had a 12 on your test and 16 on a project. ",
                                happy,
                                0.0f,
                                8.0f);
            Line l71 = new Line("And on AL you got a 14 on the first test and a 12 on the second one ",
                                grumpy,
                                4.0f,
                                12.0f);
            List<Line> pastTestList = new List<Line>();
            pastTestList.Add(l70);
            pastTestList.Add(l71);
            Topic.Input[] pastInputs =
                {
                    new Topic.Input("Thank you", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("returnTopic");
                        }, 1.5f), new Topic.Input("", () => {
                        }), new Topic.Input("More...", () => {
                            demoScene.OpenList();
                        })
                };
            Topic pastTopic = new Topic(pastTestList, pastInputs);
            demoScene.topics.Add("pastTopic", pastTopic);

            Line l74 = new Line("Things are going well in FP but sadly you are dropping AL",
                                grumpy,
                                0.0f,
                                8.0f);
            Line l75 = new Line("Keep up the good work", happy, 4.0f, 12.0f);
            List<Line> balanceList = new List<Line>();
		
            balanceList.Add(l74);
            balanceList.Add(l75);
            Topic balance = new Topic(balanceList, pastInputs);
            demoScene.topics.Add("balance", balance);

            Line l76 = new Line("Make a study plan and keep it",
                                happy,
                                0.0f,
                                8.0f);
            Line l77 = new Line("Study ahead of time", grumpy, 4.0f, 12.0f);
            List<Line> tipsList = new List<Line>();
            tipsList.Add(l76);
            tipsList.Add(l77);
            Topic tips = new Topic(tipsList, pastInputs);
            demoScene.topics.Add("tips", tips);

            Line l78 = new Line("If you give up we will drop this course to help you on others",
                                happy,
                                0.0f,
                                8.0f);
            Line l79 = new Line("There is no shame in managing your time properly",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l80 = new Line("Is this what you want?", happy, 12.5f, 20.5f);
            List<Line> quitList = new List<Line>();
            quitList.Add(l78);
            quitList.Add(l79);
            quitList.Add(l80);
            Topic.Input[] quitInputs =
                { new Topic.Input("Yes please", () => {
                            demoScene.changeTopic("help");
                        }), new Topic.Input("", () => {
                        }), new Topic.Input("Maybe not", () => {
                            demoScene.OpenList();
                        })
                };
            Topic quit = new Topic(quitList, quitInputs);
            demoScene.topics.Add("quit", quit);


            Line l84 = new Line("Do not give up just yet", happy, 0.0f, 8.0f);
            Line l85 = new Line("This course is too important for that",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l86 = new Line("We hope we can help you or call your tutor",
                                happy,
                                12.5f,
                                20.5f);
            Line l87 = new Line("Will you really quit?", grumpy, 16.5f, 24.5f);
            List<Line> dontQuitList = new List<Line>();
            dontQuitList.Add(l84);
            dontQuitList.Add(l85);
            dontQuitList.Add(l86);
            dontQuitList.Add(l87);
            Topic dontQuit = new Topic(dontQuitList, quitInputs);
            demoScene.topics.Add("dontQuit", dontQuit);

            Line l81 = new Line("If you really need it, we will contact your tutor",
                                happy,
                                0.0f,
                                8.0f);
            Line l82 = new Line("We will notify them of your situation",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l83 = new Line("Is this what you want?", grumpy, 12.5f, 20.5f);
            List<Line> contactList = new List<Line>();
            contactList.Add(l81);
            contactList.Add(l82);
            contactList.Add(l83);
            Topic contact = new Topic(contactList, quitInputs);
            demoScene.topics.Add("contact", contact);

            Line l88 = new Line(" We will open your google calendar and you should add there the study hours for this week.",
                                happy,
                                0.0f,
                                8.0f);
			Line l89 = new Line("We reccomend you to study " + demoScene.ALHours +" hours weekly this week for Algebra and "+ demoScene.FPHours+" hours of Foundations of Programming",grumpy,4.0f,12.0f);
                           
            List<Line> prePlanList = new List<Line>();
            prePlanList.Add(l88);
            prePlanList.Add(l89);
            Topic prePlan = new Topic(prePlanList, emptyInputs);
            demoScene.topics.Add("prePlan", prePlan);


            Line l90 = new Line("Oh no!", happy, 0.0f, 8.0f);
            Line l91 = new Line("This situation is not right",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l92 = new Line("Can we make a study plan to help you?",
                                happy,
                                12.5f,
                                20.5f);
            Line l93 = new Line("We will notify your tutor of your situation so they can help you further",
                                grumpy,
                                16.5f,
                                24.5f);
            List<Line> terribleLine = new List<Line>();
            terribleLine.Add(l90);
            terribleLine.Add(l91);
            terribleLine.Add(l92);
            terribleLine.Add(l93);
            Topic.Input[] inputsContact =
                { new Topic.Input("Study Plan? Ok", () => {
                            grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
                            happy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
                            demoScene.changeTopic("prePlan");
                        }, 14.5f),
                    new Topic.Input("I want to be told sooner", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SHY;
                            grumpy.CurrentEmotion = Agent.EmotionType.SAD;
                            demoScene.changeTopic("warnTestTopic");	
                        }, 3.0f), new Topic.Input("I am fine, thank you", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

                            demoScene.changeTopic("help");
                        }, 4.5f)
                };
            Topic terrible = new Topic(terribleLine, inputsContact);
            demoScene.topics.Add("terrible", terrible);

        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace VT {
    public class MainScript : MonoBehaviour {
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

        /// <summary>
        /// Selects the the dialog script that should be used.
        /// 0 for formal and 1 for more personal dialog.
        /// </summary>
        public int dialogIndex = 0;

        private bool playing = false;

        // Use this for initialization
        void Start() {

            if (splashScreenPrefab != null && playSplashScreen) {
                SplashScreenControl splashScreenControl = new SplashScreenControl(splashScreenPrefab);
                splashScreenControl.SetAndShow(/*OnEndFunction*/() => {
                        playing = true;
                        splashScreenControl.Destroy();
                    });
            } else {
                playing = true;
            }
			
            if (threeOptionsPrefab == null || ExpressionsPrefab == null
                || coursesPrefab == null || coursePrefab == null
                || calendar1Prefab == null || calendar2Prefab == null
                || calendar3Prefab == null) {
                Debug.LogWarning("Some prefabs are null");
                return;
            }

            if (pauseButton) {
                pauseButton.onClick = (bool isOn) => {
                    playing = isOn;
                };
            }

            scene = new Scene();
            scene.threePartsControl = new ThreePartsControl(threeOptionsPrefab);
            scene.expressionsControl = new ExpressionsControl(ExpressionsPrefab);
            scene.coursesControl = new CoursesControl(coursesPrefab);
            scene.courseControl = new CourseControl(coursePrefab);
            scene.calendar1Control = new Calendar1Control(calendar1Prefab);
            scene.calendar2Control = new Calendar2Control(calendar2Prefab);
            scene.calendar3Control = new Calendar3Control(calendar3Prefab);
            scene.discussControl = new DiscussControl(discussPrefab);

            switch (dialogIndex) {
                case 1:
                    PopulateScene2(scene);
                case 0:
                default:
                    PopulateScene(scene);
            }

            //start
            scene.changeTopic("Hello");
        }

        void Update() {
            if (Input.GetKeyUp(KeyCode.Space)) {
                playing = !playing;
            }
            if (playing) {
                scene.update(Time.deltaTime);
            }
        }

        void PopulateScene2(Scene demoScene) {
            Agent happy = new Agent();
            Agent grumpy = new Agent();
            grumpy.IsLeft = false;

            demoScene.agents.Add(happy);
            demoScene.agents.Add(grumpy);
            //Hello
            happy.CurrentEmotion = Agent.EmotionType.SMILING;
            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
            Evaluation test1 = new Evaluation("1º Teste",
                                              "20/03/2017",
                                              4,
                                              4,
                                              "12.0");
            Evaluation project1 = new Evaluation("1º Projecto",
                                                 "04/04/2017",
                                                 3,
                                                 4,
                                                 "16.0");
            CheckBoxPoint revision = new CheckBoxPoint("PDF Aula nº 20",
                                                       "08/04/2017",
                                                       1,
                                                       3,
                                                       true);
            Evaluation test2 = new Evaluation("2º Teste",
                                              "07/05/2017",
                                              4,
                                              4,
                                              "20.0");
            Evaluation test3 = new Evaluation("3º Teste",
                                              "22/06/2017",
                                              4,
                                              4,
                                              "18.0");
            demoScene.course1.Checkpoints.Add("test1", test1);
            demoScene.course1.Checkpoints.Add("project1", project1);
            demoScene.course1.Checkpoints.Add("revision", revision);
            demoScene.course1.Checkpoints.Add("test2", test2);
            demoScene.course1.Checkpoints.Add("test3", test3);
            Evaluation c2Test1 = new Evaluation("1º Teste",
                                                "13/03/2017",
                                                3,
                                                4,
                                                "14.0");
            Evaluation c2Test2 = new Evaluation("2º Teste",
                                                "15/04/2017",
                                                3,
                                                4,
                                                "12.0");
            CheckBoxPoint achievment = new CheckBoxPoint("Aula de Dúvidas",
                                                         "08/05/2017",
                                                         1,
                                                         3,
                                                         false);
            Evaluation c2Teste3 = new Evaluation("3º Teste",
                                                 "09/05/2017",
                                                 3,
                                                 4,
                                                 "13.0");
            Evaluation c2Test4 = new Evaluation("4º Teste",
                                                "2/06/2017",
                                                3,
                                                4,
                                                "11.0");
            demoScene.course2.Checkpoints.Add("test1", c2Test1);
            demoScene.course2.Checkpoints.Add("project1", c2Test2);
            demoScene.course2.Checkpoints.Add("revision", achievment);
            demoScene.course2.Checkpoints.Add("test2", c2Teste3);
            demoScene.course2.Checkpoints.Add("test3", c2Test4);



            Line l1 = new Line("Bom dia amigo, é bom vê-lo depois destes dois dias, aporveito para o informar que as notas de AL e FP já saíram.",
                               happy,
                               0.0f,
                               8.0f);
            Line l2 = new Line("Estava curioso em saber se vinha, pode consultar as notas das cadeiras nos seus respetivos menus.",
                               grumpy,
                               4.0f,
                               12.0f);
            Line l3 = new Line("Em que o podemos ajudar?",
                               happy,
                               12.5f,
                               20.5f);
            List<Line> lines = new List<Line>();
            lines.Add(l1);
            lines.Add(l2);
            lines.Add(l3);
            lines = lines.OrderBy(l => l.Start).ToList();
            Topic.Input[] inputs =
                {
                    new Topic.Input(
                        "Lembrem-me...", () => {

                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("reminder");
                        }, 1.5f), 
                    new Topic.Input(
                        "Gostaria de falar de...",
                        () => {

                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("talkAbout");	
                        }, 1.5f), 
                    new Topic.Input(
                        "Consultar informação",
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
			Line l7 = new Line("Como sabe, aqui é onde vem para ver a informação das suas cadeiras.",
				grumpy,
				0.0f,
				8.0f);

			List<Line> newInfo = new List<Line>();
			newInfo.Add(l7);
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
			Line l9 = new Line("Epá, que azar! O que aconteceu aqui?", happy, 0.0f, 8.0f);
			Line l10 = new Line("Acho que devia ter estudado mais...",
				grumpy,
				4.0f,
				12.0f);
			Line l11 = new Line("Um plano de estudo vai ajudá-lo.",
				happy,
				12.5f,
				20.5f);
			Line l56 = new Line("Com ele vai poder estudar para o exame final e épica de recurso.",
				grumpy,
				16.5f,
				24.5f);

			Topic.Input[] inputs3 =
			{ new Topic.Input("Plano de estudo? Concordo", () => {
				grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
				happy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
				demoScene.changeTopic("onActivity");
				demoScene.changeTopic("prePlan");
			}, 14.5f),
				//new Topic.Input("I want to be told sooner", () => {
				new Topic.Input("Não me estou a sentir bem, contactem o meu tutor", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.SAD;
					//demoScene.changeTopic("warnTestTopic");
					demoScene.changeTopic("contact");
				}, 3.0f), new Topic.Input("Estou bem, obrigado", () => {
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
			Line l12 = new Line("Devia ter estudado mais.",
				grumpy,
				0.0f,
				8.0f);
			Line l13 = new Line("Isto não é costume para si.",
				happy,
				4.0f,
				12.0f);
			Line l14 = new Line("Uma alternativa é fazer um plano de estudo para tentar melhorar a nota.",
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

			Line l15 = new Line("Ok, será notificado ainda mais cedo.",
				happy,
				0.0f,
				8.0f);
			Line l16 = new Line("Não ignore o plano de estudo, acho que é a melhor coisa para si.",
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
			Line l17 = new Line("Parabéns, que nota boa.",
				happy,
				0.0f,
				8.0f);
			Line l18 = new Line("Ainda há espaço para melhorar, sei que consegue fazer melhor.",
				grumpy,
				4.0f,
				12.0f);
			Line l21 = new Line("Ainda assim é muito bom.",
				happy,
				12.5f,
				20.5f);
			Line l57 = new Line("Continue o bom trabalho", grumpy, 16.5f, 24.5f);
			List<Line> expected = new List<Line>();
			expected.Add(l17);
			expected.Add(l18);
			expected.Add(l21);
			expected.Add(l57);
			Topic.Input[] inputs5 =
			{new Topic.Input("Fazer plano de estudo", () => {
				demoScene.changeTopic("prePlan");
			}, 1.5f), new Topic.Input("Quero ser avisado mais cedo", () => {
				happy.CurrentEmotion = Agent.EmotionType.SHY;
				grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
				demoScene.changeTopic("warnTestTopic");
			}, 3.0f), new Topic.Input("Obrigado", () => {
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
			Line l22 = new Line("Parabéns por uma nota fantástica!",
				happy,
				0.0f,
				8.0f);
			Line l23 = new Line("Sim, sabia que conseguia.", grumpy, 4.0f, 12.0f);
			List<Line> great = new List<Line>();
			great.Add(l22);
			great.Add(l23);
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
			Line l58 = new Line("Aconselho-te a diminuir um pouco as horas.",
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
			Line l32 = new Line("Ok ótimo, já temos plano de estudo.", happy, 0.0f, 8.0f);
			Line l33 = new Line("Sei que vai conseguir mantê-lo.",
				grumpy,
				4.0f,
				12.0f);

			Topic.Input[] inputs7 =
			{ new Topic.Input("Obrigado.", () => {

				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic("help");
			}, 1.5f), new Topic.Input("", () => {
			}, 3.0f), new Topic.Input("Não tenho bem a certeza.", () => {
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
			Line l34 = new Line("Podemos ajudar em algo mais?",
				happy,
				0.0f,
				8.0f);
			Topic.Input[] inputs8 =
			{ new Topic.Input("Gostaria de falar de....", () => {
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic("talkAbout");	
			}, 1.5f), new Topic.Input("Não, obrigado", () => {
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
				demoScene.changeTopic("twoDaysTopic");
			}, 1.5f), new Topic.Input("Conslutar informação", () => {
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic("newInfoTopic");
			}, 1.5f)
			};
			List<Line> moreHelp = new List<Line>();
			moreHelp.Add(l34);
			Topic help = new Topic(moreHelp, inputs8);
			demoScene.topics.Add("help", help);

			//		
			//			// 2days with plan
			Line l36 = new Line("Venha ver-nos em 2 dias para falarmos de outras coisas.",
				happy,
				0.0f,
				8.0f);
			Line l37 = new Line("Tenho confiança que vai tudo correr bem até lá.", grumpy, 4.0f, 12.0f);
			Topic.Input[] inputs9 =
			{ new Topic.Input("Até daqui a 2 dias", () => {
				happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				demoScene.changeTopic("exit1Topic");
			}, 2.5f), new Topic.Input("", () => {
			}, 2.5f), new Topic.Input("é muito cedo", () => {
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
			Line l38 = new Line("Quando quer ter uma nova reunião?",
				happy,
				0.0f,
				8.0f);
			Line l39 = new Line("Zangou-se connosco e já não nos quer.",
				grumpy,
				4.0f,
				12.0f);
			Topic.Input[] inputs10 =
			{ new Topic.Input(" 5 dias", () => {
				happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				demoScene.changeTopic("exit2Topic");
			}, 1.5f), new Topic.Input("7 dias", () => {
				happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				demoScene.changeTopic("exit3Topic");
			}, 3.0f), new Topic.Input("Não tenho a certeza", () => {
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
			Line l40 = new Line("Ok, vejo-o em 2 dias!", happy, 0.0f, 8.0f);
			Line l41 = new Line("Vou desligar a aplicação então.",
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
			Line l42 = new Line("Tudo bem, vemo-nos em 5 dias!", happy, 0.0f, 8.0f);

			List<Line> exit2 = new List<Line>();
			exit2.Add(l41);
			exit2.Add(l42);
			Topic exit2Topic = new Topic(exit2, emptyInputs);
			demoScene.topics.Add("exit2Topic", exit2Topic);
			//
			//			//exit 7 days
			Line l43 = new Line("Tudo bem, vemo-nos em 7 dias", happy, 0.0f, 8.0f);
			List<Line> exit3 = new List<Line>();
			exit3.Add(l43);
			exit3.Add(l41);
			Topic exit3Topic = new Topic(exit3, emptyInputs);
			demoScene.topics.Add("exit3Topic", exit3Topic);

			//			//Nao sabe
			Line l44 = new Line("Ele não sabe, nem toda a gente sabe isso.",
				grumpy,
				0.0f,
				8.0f);
			Line l45 = new Line("Vemo-nos quando aparecer",
				happy,
				4.0f,
				12.0f);
			List<Line> exit4 = new List<Line>();
			exit4.Add(l44);
			exit4.Add(l45);
			Topic exit4Topic = new Topic(exit4, emptyInputs);
			demoScene.topics.Add("exit4Topic", exit4Topic);

			//TimeOutDefault

			Line l46 = new Line("Talvez não queira falar connosco.",
				grumpy,
				0.0f,
				8.0f);
			Line l47 = new Line("E agora?",
				happy,
				4.0f,
				12.0f);
			Line l48 = new Line("É melhor desligarmos então.",
				grumpy,
				12.5f,
				20.5f);

			List<Line> timeout1 = new List<Line>();
			timeout1.Add(l46);
			timeout1.Add(l47);
			timeout1.Add(l48);

			Topic timeTopic = new Topic(timeout1, emptyInputs);
			demoScene.topics.Add("timeTopic", timeTopic);

			//not answering test
			Line l52 = new Line("É normal se não quiser falar sobre isso.",
				happy,
				0.0f,
				8.0f);
			Line l53 = new Line("Vamos fazer um plano de estudo para gerir os problemas da semana que vem.",
				grumpy,
				4.0f,
				12.0f);
			Topic.Input[] notAnswTestInputs =
			{ new Topic.Input("Fazer um plano de estudo!", () => {
				demoScene.changeTopic("prePlan");
			}, 1.5f), new Topic.Input("", () => {
			}, 3.0f), new Topic.Input("Obrigado", () => {
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
			Line l54 = new Line("Se achas que é o melhor para si, eu apoio.",
				grumpy,
				0.0f,
				8.0f);
			Line l55 = new Line("Nós apoiamos!", happy, 4.0f, 12.0f);
			List<Line> noAnswerPlan = new List<Line>();

			noAnswerPlan.Add(l54);
			noAnswerPlan.Add(l55);
			Topic noAnswPlan = new Topic(noAnswerPlan, emptyInputs);
			demoScene.topics.Add("noAnswPlan", noAnswPlan);

			Line l59 = new Line("Será  que vai ver os resultados de uma avaliação?",
				happy,
				0.0f,
				8.0f);
			Line l60 = new Line("Espero que tenha corrido bem.",
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
			remindList.Add(new Line("Estamos à espera que o professor coloque a época de recurso no Moodle.",
				happy,
				0.5f,
				10.0f));
			remindList.Add(new Line("Por agora só sabemos isso.", grumpy, 5.0f, 12.0f));
			Topic.Input[] remindInputs =
			{ new Topic.Input("Obrigado", () => {
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic("help");
			}, 3.0f), new Topic.Input("", () => {
			}), new Topic.Input("Consultar informação", () => {
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic("newInfoTopic");
			}, 3.0f) 
			};
			Topic reminder = new Topic(remindList, remindInputs);
			demoScene.topics.Add("reminder", reminder);

			Line l66 = new Line("Do que gostaria de falar hoje?",
				happy,
				0.0f,
				8.0f);
			Line l67 = new Line("Por favor nada de problemas pessoais.",
				grumpy,
				4.0f,
				12.0f);
			List<Line> talkAboutList = new List<Line>();
			talkAboutList.Add(l66);
			talkAboutList.Add(l67);
			Topic.Input[] talkInputs =
			{ new Topic.Input("Avaliações passadas", () => {
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic("pastTopic");
			}, 1.5f), new Topic.Input("Análise do Semestre", () => {
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
				demoScene.changeTopic("balance");
			}, 1.5f), new Topic.Input("Dicas", () => {
				happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				demoScene.changeTopic("tips");
			}, 1.5f), new Topic.Input("Mais...", () => {
				demoScene.OpenList();
			}, 3.0f)
			};
			Topic talkAbout = new Topic(talkAboutList, talkInputs);
			demoScene.topics.Add("talkAbout", talkAbout);

			Line l70 = new Line("Em FP sabemos que teve um 12 no teste, um 16 no projeto e um 20 no segundo teste, além do teste que acabou de receber.",
				happy,
				0.0f,
				8.0f);
			Line l71 = new Line("Em AL teve um 14 no primeiro teste, um 12 no segundo,um 13 no terceiro e acabou de receber mais um.",
				grumpy,
				4.0f,
				12.0f);
			List<Line> pastTestList = new List<Line>();
			pastTestList.Add(l70);
			pastTestList.Add(l71);
			Topic.Input[] pastInputs =
			{
				new Topic.Input("Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic("help");
				}, 1.5f), new Topic.Input("", () => {
				}), new Topic.Input("Mais...", () => {
					demoScene.OpenList();
				})
			};
			Topic pastTopic = new Topic(pastTestList, pastInputs);
			demoScene.topics.Add("pastTopic", pastTopic);

			Line l74 = new Line("As coisas estão a correr bem a FP mas em AL estão a piorar.",
				grumpy,
				0.0f,
				8.0f);
			Line l75 = new Line("Continue o bom trabalho a FP mas tente melhorar AL talvez fazer uma melhoria de nota.", happy, 4.0f, 12.0f);
			List<Line> balanceList = new List<Line>();

			balanceList.Add(l74);
			balanceList.Add(l75);
			Topic balance = new Topic(balanceList, pastInputs);
			demoScene.topics.Add("balance", balance);

			Line l76 = new Line("Faça um plano de estudo e cumpra-o.",
				happy,
				0.0f,
				8.0f);
			Line l77 = new Line("Estude com antecedência para a época de recurso de AL.", grumpy, 4.0f, 12.0f);
			List<Line> tipsList = new List<Line>();
			tipsList.Add(l76);
			tipsList.Add(l77);
			Topic tips = new Topic(tipsList, pastInputs);
			demoScene.topics.Add("tips", tips);

			Line l78 = new Line("Se desistir da cadeira, vamos deixar de dar apoio nesta em prol das outras.",
				happy,
				0.0f,
				8.0f);
			Line l79 = new Line("Já sabe que não há vergonha em tentar gerir o seu tempo.",
				grumpy,
				4.0f,
				12.0f);
			Line l80 = new Line("É isso que quer?", happy, 12.5f, 20.5f);
			List<Line> quitList = new List<Line>();
			quitList.Add(l78);
			quitList.Add(l79);
			quitList.Add(l80);
			Topic.Input[] quitInputs =
			{ new Topic.Input("Sim por favor", () => {
				demoScene.changeTopic("help");
			}), new Topic.Input("", () => {
			}), new Topic.Input("Talvez não", () => {
				demoScene.OpenList();
			})
			};
			Topic quit = new Topic(quitList, quitInputs);
			demoScene.topics.Add("quit", quit);


			Line l84 = new Line("Não desista, companheiro.", happy, 0.0f, 8.0f);
			Line l85 = new Line("Esta cadeira é demasiado importante para isso.",
				grumpy,
				4.0f,
				12.0f);
			Line l86 = new Line("Quer que contactemos o seu tutor?",
				happy,
				12.5f,
				20.5f);
			Line l87 = new Line("O que pretende fazer?", grumpy, 16.5f, 24.5f);
			List<Line> dontQuitList = new List<Line>();
			dontQuitList.Add(l84);
			dontQuitList.Add(l85);
			dontQuitList.Add(l86);
			dontQuitList.Add(l87);
			Topic dontQuit = new Topic(dontQuitList, quitInputs);
			demoScene.topics.Add("dontQuit", dontQuit);

			Line l81 = new Line("Se precisar, contataremos o seu tutor.",
				happy,
				0.0f,
				8.0f);
			Line l82 = new Line("Notificá-lo-emos da sua situação.",
				grumpy,
				4.0f,
				12.0f);
			Line l83 = new Line("É isso que pretende?", grumpy, 12.5f, 20.5f);
			List<Line> contactList = new List<Line>();
			contactList.Add(l81);
			contactList.Add(l82);
			contactList.Add(l83);
			Topic contact = new Topic(contactList, quitInputs);
			demoScene.topics.Add("contact", contact);

			Line l88 = new Line("Abriremos o seu google calendar para que possa planear as horas de estudo desta semana.",
				happy,
				0.0f,
				8.0f);
			Line l89 = new Line("Para Álgebra recomendamos estudar " + demoScene.ALHours + " horas semanais e para Fundamentos da Programação " + demoScene.FPHours + " horas semanais.",
				grumpy,
				4.0f,
				12.0f);

			List<Line> prePlanList = new List<Line>();
			prePlanList.Add(l88);
			prePlanList.Add(l89);
			Topic prePlan = new Topic(prePlanList, emptyInputs);
			demoScene.topics.Add("prePlan", prePlan);


			Line l90 = new Line("Oh não, foi mesmo abaixo do que estávamos à espera!", happy, 0.0f, 8.0f);
			Line l91 = new Line("Estou muito triste.",
				grumpy,
				4.0f,
				12.0f);
			Line l92 = new Line("Quer que façamos um plano de estudo? Vamos melhorar isto.",
				happy,
				12.5f,
				20.5f);
			Line l93 = new Line("Falaremos com o seu tutor para avisar da situação.",
				grumpy,
				16.5f,
				24.5f);
			List<Line> terribleLine = new List<Line>();
			terribleLine.Add(l90);
			terribleLine.Add(l91);
			terribleLine.Add(l92);
			terribleLine.Add(l93);
			Topic.Input[] inputsContact =
			{ new Topic.Input("Plano de estudo? concordo", () => {
				grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
				happy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
				demoScene.changeTopic("prePlan");
			}, 14.5f),
				new Topic.Input("Gostaria de ser avisado mais cedo", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.SAD;
					demoScene.changeTopic("warnTestTopic");	
				}, 3.0f), new Topic.Input("Estou bem, obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

					demoScene.changeTopic("help");
				}, 4.5f)
			};
			Topic terrible = new Topic(terribleLine, inputsContact);
			demoScene.topics.Add("terrible", terrible);

		}
        void PopulateScene(Scene demoScene) {
		
            Agent happy = new Agent();
            Agent grumpy = new Agent();
            grumpy.IsLeft = false;

            demoScene.agents.Add(happy);
            demoScene.agents.Add(grumpy);
            //Hello
            happy.CurrentEmotion = Agent.EmotionType.SMILING;
            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
            Evaluation test1 = new Evaluation("1º Teste",
                                      "20/03/2017",
                                      4,
                                      4,
                                      "12.0");
            Evaluation project1 = new Evaluation("1º Projecto",
                                         "04/04/2017",
                                         3,
                                         4,
                                         "16.0");
            CheckBoxPoint revision = new CheckBoxPoint("PDF Aula nº 20",
                                               "08/04/2017",
                                               1,
                                               3,
                                               true);
            Evaluation test2 = new Evaluation("2º Teste",
                                      "07/05/2017",
                                      4,
                                      4,
                                      "20.0");
            Evaluation test3 = new Evaluation("3º Teste",
                                      "22/06/2017",
                                      4,
                                      4,
                                      "");
            demoScene.course1.Checkpoints.Add("test1", test1);
            demoScene.course1.Checkpoints.Add("project1", project1);
            demoScene.course1.Checkpoints.Add("revision", revision);
            demoScene.course1.Checkpoints.Add("test2", test2);
            demoScene.course1.Checkpoints.Add("test3", test3);
            Evaluation c2Test1 = new Evaluation("1º Teste",
                                        "13/03/2017",
                                        3,
                                        4,
                                        "14.0");
            Evaluation c2Test2 = new Evaluation("2º Teste",
                                        "15/04/2017",
                                        3,
                                        4,
                                        "12.0");
            CheckBoxPoint achievment = new CheckBoxPoint("Aula de Dúvidas",
                                                 "08/05/2017",
                                                 1,
                                                 3,
                                                 false);
            Evaluation c2Teste3 = new Evaluation("3º Teste",
                                         "09/05/2017",
                                         3,
                                         4,
                                         "13.0");
            Evaluation c2Test4 = new Evaluation("4º Teste",
                                        "2/06/2017",
                                        3,
                                        4,
                                        "");
            demoScene.course2.Checkpoints.Add("test1", c2Test1);
            demoScene.course2.Checkpoints.Add("project1", c2Test2);
            demoScene.course2.Checkpoints.Add("revision", achievment);
            demoScene.course2.Checkpoints.Add("test2", c2Teste3);
            demoScene.course2.Checkpoints.Add("test3", c2Test4);



            Line l1 = new Line("Olá, é bom vê-lo depois destes dois dias.",
                       happy,
                       0.0f,
                       8.0f);
            Line l2 = new Line("Estava curioso em saber se vinha, aproveito para informar que sairam as notas dos testes de AL e FP.",
                       grumpy,
                       4.0f,
                       12.0f);
            Line l3 = new Line("Em que o podemos ajudar?",
                       happy,
                       12.5f,
                       20.5f);
            Line l4 = new Line("Quero lembrá-lo que tem um checkpoint em duas semanas.",
                       grumpy,
                       16.5f,
                       24.5f);
            List<Line> lines = new List<Line>();
            lines.Add(l1);
            lines.Add(l2);
            lines.Add(l3);
            lines.Add(l4);
            lines = lines.OrderBy(l => l.Start).ToList();
            Topic.Input[] inputs =
            {
                new Topic.Input(
                    "Lembrem-me...", () => {
						
                        happy.CurrentEmotion = Agent.EmotionType.SMILING;
                        grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                        demoScene.changeTopic("reminder");
                    }, 1.5f), 
                new Topic.Input(
                    "Gostaria de falar de ...",
                    () => {

                        happy.CurrentEmotion = Agent.EmotionType.SMILING;
                        grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                        demoScene.changeTopic("talkAbout");	
                    }, 1.5f), 
                new Topic.Input(
                    "Consultar informação",
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
            Line l7 = new Line("Aqui poderá consultar informação referente às cadeiras.",
                               grumpy,
                               0.0f,
                               8.0f);
          
            List<Line> newInfo = new List<Line>();
            newInfo.Add(l7);
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
            Line l9 = new Line("Oh não!", happy, 0.0f, 8.0f);
            Line l10 = new Line("Deveria ter estudado mais...",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l11 = new Line("Talvez fazer um plano de estudo o ajude.",
                                happy,
                                12.5f,
                                20.5f);
            Line l56 = new Line("Pode ser que suba a nota.",
                                grumpy,
                                16.5f,
                                24.5f);

            Topic.Input[] inputs3 =
                { new Topic.Input("Plano de estudo? Tudo bem", () => {
                            grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
                            happy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
                            demoScene.changeTopic("onActivity");
                            demoScene.changeTopic("prePlan");
                        }, 14.5f),
                    //new Topic.Input("I want to be told sooner", () => {
                    new Topic.Input("Não me estou a sentir bem, contactem o meu tutor", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SHY;
                            grumpy.CurrentEmotion = Agent.EmotionType.SAD;
                            //demoScene.changeTopic("warnTestTopic");
                            demoScene.changeTopic("contact");
                        }, 3.0f), new Topic.Input("Estou bem, obrigado", () => {
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
            Line l12 = new Line("Talvez deveria ter estudado mais",
                                grumpy,
                                0.0f,
                                8.0f);
            Line l13 = new Line("Este teste não foi o esperado.",
                                happy,
                                4.0f,
                                12.0f);
            Line l14 = new Line("Aconselho a fazer um plano de estudo para tentar melhorar a nota.",
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

            Line l15 = new Line("Tudo bem, com isto será notificado antes.",
                                happy,
                                0.0f,
                                8.0f);
            Line l16 = new Line("Ainda penso que um plano de estudo seria bom.",
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
            Line l17 = new Line("Parabéns por uma boa nota.",
                                happy,
                                0.0f,
                                8.0f);
            Line l18 = new Line("Ainda há espaço para melhorar.",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l21 = new Line("Ainda assim é muito bom.",
                                happy,
                                12.5f,
                                20.5f);
            Line l57 = new Line("Continue o bom trabalho.", grumpy, 16.5f, 24.5f);
            List<Line> expected = new List<Line>();
            expected.Add(l17);
            expected.Add(l18);
            expected.Add(l21);
            expected.Add(l57);
            Topic.Input[] inputs5 =
                {new Topic.Input("Fazer plano de estudo", () => {
                            demoScene.changeTopic("prePlan");
                        }, 1.5f), new Topic.Input("Quero ser avisado mais cedo", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SHY;
                            grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
                            demoScene.changeTopic("warnTestTopic");
                        }, 3.0f), new Topic.Input("Obrigado", () => {
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
            Line l22 = new Line("Parabéns por uma nota brilhante!",
                                happy,
                                0.0f,
                                8.0f);
            Line l23 = new Line("Sim, genial.", grumpy, 4.0f, 12.0f);
            List<Line> great = new List<Line>();
            great.Add(l22);
            great.Add(l23);
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
            Line l58 = new Line("Aconselho-te a diminuir um pouco as horas.",
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
            Line l32 = new Line("Ok ótimo, está marcado.", happy, 0.0f, 8.0f);
            Line l33 = new Line("Espero que o consiga manter.",
                                grumpy,
                                4.0f,
                                12.0f);

            Topic.Input[] inputs7 =
                { new Topic.Input("Obrigado.", () => {

                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("help");
                        }, 1.5f), new Topic.Input("", () => {
                        }, 3.0f), new Topic.Input("Não tenho bem a certeza.", () => {
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
            Line l34 = new Line("Podemos ajudar em algo mais?",
                                happy,
                                0.0f,
                                8.0f);
            Topic.Input[] inputs8 =
                { new Topic.Input("Gostaria de falar de....", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("talkAbout");	
                        }, 1.5f), new Topic.Input("Não, obrigado", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
                            demoScene.changeTopic("twoDaysTopic");
                        }, 1.5f), new Topic.Input("Conslutar informação", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("newInfoTopic");
                        }, 1.5f)
                };
            List<Line> moreHelp = new List<Line>();
            moreHelp.Add(l34);
            Topic help = new Topic(moreHelp, inputs8);
            demoScene.topics.Add("help", help);

//		
//			// 2days with plan
            Line l36 = new Line("Venha ver-nos em 2 dias para vermos se o plano funciona.",
                                happy,
                                0.0f,
                                8.0f);
            Line l37 = new Line("Espero que venha.", grumpy, 4.0f, 12.0f);
            Topic.Input[] inputs9 =
                { new Topic.Input("Até daqui a 2 dias", () => {
                            happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            demoScene.changeTopic("exit1Topic");
                        }, 2.5f), new Topic.Input("", () => {
                        }, 2.5f), new Topic.Input("é muito cedo", () => {
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
            Line l38 = new Line("Quando quer ter uma nova reunião?",
                                happy,
                                0.0f,
                                8.0f);
            Line l39 = new Line("Talvez não nos queira ver.",
                                grumpy,
                                4.0f,
                                12.0f);
            Topic.Input[] inputs10 =
                { new Topic.Input(" 5 dias", () => {
                            happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            demoScene.changeTopic("exit2Topic");
                        }, 1.5f), new Topic.Input("7 dias", () => {
                            happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            demoScene.changeTopic("exit3Topic");
                        }, 3.0f), new Topic.Input("Não tenho a certeza", () => {
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
            Line l40 = new Line("Ok, vejo-o em 2 dias!", happy, 0.0f, 8.0f);
            Line l41 = new Line("Vou desligar a aplicação então.",
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
            Line l42 = new Line("Tudo bem, vemo-nos em 5 dias!", happy, 0.0f, 8.0f);

            List<Line> exit2 = new List<Line>();
            exit2.Add(l41);
            exit2.Add(l42);
            Topic exit2Topic = new Topic(exit2, emptyInputs);
            demoScene.topics.Add("exit2Topic", exit2Topic);
//
//			//exit 7 days
            Line l43 = new Line("Tudo bem, vemo-nos em 7 dias.", happy, 0.0f, 8.0f);
            List<Line> exit3 = new List<Line>();
            exit3.Add(l43);
            exit3.Add(l41);
            Topic exit3Topic = new Topic(exit3, emptyInputs);
            demoScene.topics.Add("exit3Topic", exit3Topic);

//			//Nao sabe
            Line l44 = new Line("Ele não sabe, nem toda a gente sabe isso.",
                                grumpy,
                                0.0f,
                                8.0f);
            Line l45 = new Line("Vemo-nos quando aparecer.",
                                happy,
                                4.0f,
                                12.0f);
            List<Line> exit4 = new List<Line>();
            exit4.Add(l44);
            exit4.Add(l45);
            Topic exit4Topic = new Topic(exit4, emptyInputs);
            demoScene.topics.Add("exit4Topic", exit4Topic);

            //TimeOutDefault

            Line l46 = new Line("Talvez não queira falar connosco.",
                                grumpy,
                                0.0f,
                                8.0f);
            Line l47 = new Line("E agora?",
                                happy,
                                4.0f,
                                12.0f);
            Line l48 = new Line("É melhor desligarmos então.",
                                grumpy,
                                12.5f,
                                20.5f);
          
            List<Line> timeout1 = new List<Line>();
            timeout1.Add(l46);
            timeout1.Add(l47);
            timeout1.Add(l48);
           
            Topic timeTopic = new Topic(timeout1, emptyInputs);
            demoScene.topics.Add("timeTopic", timeTopic);

            //not answering test
            Line l52 = new Line("Suponho que não queira falar sobre isso.",
                                happy,
                                0.0f,
                                8.0f);
            Line l53 = new Line("Aconselho a fazer um plano de estudo para gerir o seu tempo.",
                                grumpy,
                                4.0f,
                                12.0f);
            Topic.Input[] notAnswTestInputs =
                { new Topic.Input("Fazer um plano de estudo!", () => {
                            demoScene.changeTopic("prePlan");
                        }, 1.5f), new Topic.Input("", () => {
                        }, 3.0f), new Topic.Input("Obrigado", () => {
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

            Line l59 = new Line("Será  que vai ver os resultados de uma avaliação?",
                                happy,
                                0.0f,
                                8.0f);
            Line l60 = new Line("Espero que tenha corrido bem.",
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
            remindList.Add(new Line("Sabemos que tem um momento importante daqui a 10 dias.",
                                    happy,
                                    0.5f,
                                    10.0f));
            remindList.Add(new Line("Por agora só sabemos isso.", grumpy, 5.0f, 12.0f));
            Topic.Input[] remindInputs =
                { new Topic.Input("Obrigado", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("help");
                        }, 3.0f), new Topic.Input("", () => {
                        }), new Topic.Input("Consultar informação", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("newInfoTopic");
                        }, 3.0f) 
                };
            Topic reminder = new Topic(remindList, remindInputs);
            demoScene.topics.Add("reminder", reminder);
				
            Line l66 = new Line("Do que gostaria de falar?",
                                happy,
                                0.0f,
                                8.0f);
            Line l67 = new Line("Por favor não de problemas pessoais.",
                                grumpy,
                                4.0f,
                                12.0f);
            List<Line> talkAboutList = new List<Line>();
            talkAboutList.Add(l66);
            talkAboutList.Add(l67);
            Topic.Input[] talkInputs =
                { new Topic.Input("Avaliações passadas", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("pastTopic");
                        }, 1.5f), new Topic.Input("Análise do Semestre", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
                            demoScene.changeTopic("balance");
                        }, 1.5f), new Topic.Input("Dicas", () => {
                            happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
                            demoScene.changeTopic("tips");
                        }, 1.5f), new Topic.Input("Mais...", () => {
                            demoScene.OpenList();
                        }, 3.0f)
                };
            Topic talkAbout = new Topic(talkAboutList, talkInputs);
            demoScene.topics.Add("talkAbout", talkAbout);

            Line l70 = new Line("Em FP sabemos que teve um 12 no teste e um 16 no projeto, além do teste que acabou de receber.",
                                happy,
                                0.0f,
                                8.0f);
            Line l71 = new Line("Em AL teve um 14 no primeiro teste e um 12 no segundo e acabou de receber mais um.",
                                grumpy,
                                4.0f,
                                12.0f);
            List<Line> pastTestList = new List<Line>();
            pastTestList.Add(l70);
            pastTestList.Add(l71);
            Topic.Input[] pastInputs =
                {
                    new Topic.Input("Obrigado", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SMILING;
                            grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                            demoScene.changeTopic("help");
                        }, 1.5f), new Topic.Input("", () => {
                        }), new Topic.Input("Mais...", () => {
                            demoScene.OpenList();
                        })
                };
            Topic pastTopic = new Topic(pastTestList, pastInputs);
            demoScene.topics.Add("pastTopic", pastTopic);

            Line l74 = new Line("As coisas estão a correr bem a FP mas em AL estão a piorar.",
                                grumpy,
                                0.0f,
                                8.0f);
            Line l75 = new Line("Continue o bom trabalho a FP mas tente melhorar AL.", happy, 4.0f, 12.0f);
            List<Line> balanceList = new List<Line>();
		
            balanceList.Add(l74);
            balanceList.Add(l75);
            Topic balance = new Topic(balanceList, pastInputs);
            demoScene.topics.Add("balance", balance);

            Line l76 = new Line("Faça um plano de estudo e cumpra-o.",
                                happy,
                                0.0f,
                                8.0f);
            Line l77 = new Line("Estude com antecedência.", grumpy, 4.0f, 12.0f);
            List<Line> tipsList = new List<Line>();
            tipsList.Add(l76);
            tipsList.Add(l77);
            Topic tips = new Topic(tipsList, pastInputs);
            demoScene.topics.Add("tips", tips);

            Line l78 = new Line("Se desistir da cadeira, vamos deixar de dar apoio nesta em prol das outras.",
                                happy,
                                0.0f,
                                8.0f);
            Line l79 = new Line("Não há vergonha em tentar gerir o seu tempo.",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l80 = new Line("É isso que quer?", happy, 12.5f, 20.5f);
            List<Line> quitList = new List<Line>();
            quitList.Add(l78);
            quitList.Add(l79);
            quitList.Add(l80);
            Topic.Input[] quitInputs =
                { new Topic.Input("Sim por favor", () => {
                            demoScene.changeTopic("help");
                        }), new Topic.Input("", () => {
                        }), new Topic.Input("Talvez não", () => {
                            demoScene.OpenList();
                        })
                };
            Topic quit = new Topic(quitList, quitInputs);
            demoScene.topics.Add("quit", quit);


            Line l84 = new Line("Tente não desistir por agora.", happy, 0.0f, 8.0f);
            Line l85 = new Line("Esta cadeira é demasiado importante para isso.",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l86 = new Line("Se quiser podemos contatar o tutor.",
                                happy,
                                12.5f,
                                20.5f);
            Line l87 = new Line("O que pretende fazer?", grumpy, 16.5f, 24.5f);
            List<Line> dontQuitList = new List<Line>();
            dontQuitList.Add(l84);
            dontQuitList.Add(l85);
            dontQuitList.Add(l86);
            dontQuitList.Add(l87);
            Topic dontQuit = new Topic(dontQuitList, quitInputs);
            demoScene.topics.Add("dontQuit", dontQuit);

            Line l81 = new Line("Se precisar mesmo, podemos contatar o seu tutor.",
                                happy,
                                0.0f,
                                8.0f);
            Line l82 = new Line("Notificá-lo-emos da sua situação.",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l83 = new Line("É isso que pretende?", grumpy, 12.5f, 20.5f);
            List<Line> contactList = new List<Line>();
            contactList.Add(l81);
            contactList.Add(l82);
            contactList.Add(l83);
            Topic contact = new Topic(contactList, quitInputs);
            demoScene.topics.Add("contact", contact);

            Line l88 = new Line("Abriremos o seu google calendar para que possa planear as horas de estudo desta semana.",
                                happy,
                                0.0f,
                                8.0f);
            Line l89 = new Line("Para Álgebra recomendamos estudar " + demoScene.ALHours + " horas semanais e para Fundamentos da Programação " + demoScene.FPHours + " horas semanais.",
                       grumpy,
                       4.0f,
                       12.0f);
                           
            List<Line> prePlanList = new List<Line>();
            prePlanList.Add(l88);
            prePlanList.Add(l89);
            Topic prePlan = new Topic(prePlanList, emptyInputs);
            demoScene.topics.Add("prePlan", prePlan);


            Line l90 = new Line("Oh não!", happy, 0.0f, 8.0f);
            Line l91 = new Line("Estou muito triste.",
                                grumpy,
                                4.0f,
                                12.0f);
            Line l92 = new Line("Quer que façamos um plano de estudo?",
                                happy,
                                12.5f,
                                20.5f);
            Line l93 = new Line("Falaremos com o seu tutor para avisar da situação.",
                                grumpy,
                                16.5f,
                                24.5f);
            List<Line> terribleLine = new List<Line>();
            terribleLine.Add(l90);
            terribleLine.Add(l91);
            terribleLine.Add(l92);
            terribleLine.Add(l93);
            Topic.Input[] inputsContact =
                { new Topic.Input("Plano de estudo? concordo", () => {
                            grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
                            happy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
                            demoScene.changeTopic("prePlan");
                        }, 14.5f),
                    new Topic.Input("Gostaria de ser avisado mais cedo", () => {
                            happy.CurrentEmotion = Agent.EmotionType.SHY;
                            grumpy.CurrentEmotion = Agent.EmotionType.SAD;
                            demoScene.changeTopic("warnTestTopic");	
                        }, 3.0f), new Topic.Input("Estou bem, obrigado", () => {
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

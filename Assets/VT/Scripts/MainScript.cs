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
		private Scene scene;

		private bool playing = false;

		// Use this for initialization
		void Start ()
		{

			if (splashScreenPrefab != null && playSplashScreen) {
				SplashScreenControl splashScreenControl = new SplashScreenControl (splashScreenPrefab);
				splashScreenControl.SetAndShow (() => {
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
			 Evaluation test1 = new Evaluation ("1º Teste", "20/03/2017", 4, 4, "12.0");
			 Evaluation project1 = new Evaluation ("1º Projecto", "04/04/2017", 3, 4, "16.0");
			 CheckBoxPoint revision = new CheckBoxPoint ("PDF Aula nº 20", "08/04/2017", 1, 3, true);
			Evaluation test2 = new Evaluation ("2º Teste", "07/05/2017", 4, 4, "");
			Evaluation test3 = new Evaluation ("3º Teste", "22/06/2017", 4, 4, "");
			demoScene.course1.Checkpoints.Add ("test1",test1);
			demoScene.course1.Checkpoints.Add ("project1",project1);
			demoScene.course1.Checkpoints.Add ("revision",revision);
			demoScene.course1.Checkpoints.Add ("test2",test2);
			demoScene.course1.Checkpoints.Add ("test3",test3);

			Line l1 = new Line ("Olá, é bom ver-te depois destes 2 dias.", happy, 0.0f, 8.0f);
			Line l2 = new Line ("Eu pensava que ele não vinha",
				          grumpy,
				          4.0f,
				          12.0f);
			Line l3 = new Line ("Em que te podemos ajudar hoje?",
				          happy,
				          12.5f,
				          20.5f);
			Line l4 = new Line ("Lembro-te que tens uma avaliação daqui a 2 semanas.",
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
				new Topic.Input (
					"lembrem-me de...", () => {
						
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("reminder");
				}, 1.5f), 
				new Topic.Input (
					"gostaria de falar de ...",
					() => {

						happy.CurrentEmotion = Agent.EmotionType.SMILING;
						grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
						demoScene.changeTopic ("talkAbout");	
					}, 3.0f), 
				new Topic.Input (
					"tenho uma nova informação",
					() => {
						
						happy.CurrentEmotion = Agent.EmotionType.SMILING;
						grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
						demoScene.changeTopic ("newInfoTopic");
					}, 4.5f) 
			};
			Topic hello = new Topic (lines, inputs);
			demoScene.topics.Add ("Hello", hello);
			//not developed

			Line l5 = new Line ("Não podemos fazer nada disso agora. Malditos programadores.",
				          grumpy,
				          0.0f,
				          8.0f);
			Line l6 = new Line ("Assim que tivermos esta funcionalidade falaremos contigo.",
				          happy,
				          4.0f,
				          12.0f);
			List<Line> nDLines = new List<Line> ();
			nDLines.Add (l5);
			nDLines.Add (l6);
			Topic.Input[] inputs2 = { new Topic.Input (
					"está bem", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("Hello");
				}),
				new Topic.Input ("", () => {
					Debug.Log ("top");
				}, 1.5f),
				new Topic.Input ("fechem a aplicação por favor", () => {
					Application.Quit();
				}, 3.0f
				)
			};
			Topic nonDeveloped = new Topic (nDLines, inputs2);
			demoScene.topics.Add ("nonDeveloped", nonDeveloped);

			//new Info?
			Line l7 = new Line ("Uma nova informação? Está bem mas despacha-te.",
				          grumpy,
				          0.0f,
				          8.0f);
			Line l8 = new Line ("Não sejas assim", happy, 4.0f, 12.0f);
			List<Line> newInfo = new List<Line> ();
			newInfo.Add (l7);
			newInfo.Add (l8);
			Topic.Input[] inputs1 = {new Topic.Input ("ok", () => {
					grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
					happy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
					demoScene.changeTopic ("onActivity");
					demoScene.OpenCourses ();
				}, 1.5f), new Topic.Input ("", () => {
				}), new Topic.Input ("", () => {
				})
			};
			Topic newInfoTopic = new Topic (newInfo, inputs1);
			demoScene.topics.Add ("newInfoTopic", newInfoTopic);

			// Bad test
//			happy.CurrentEmotion = Agent.EmotionType.CRYING; para por quando houver passagem
//			grumpy.CurrentEmotion = Agent.EmotionType.CRYING;
//
			Line l9 = new Line ("Ora bolas!", happy, 0.0f, 8.0f);
			Line l10 = new Line ("Devias ter estudado mais...", grumpy, 4.0f, 12.0f);
			Line l11 = new Line ("Podemos sempre fazer um plano de estudo, o que achas?",
				           happy,
				           12.5f,
				           20.5f);
			Line l56 = new Line ("Talvez consigas subir a nota", grumpy, 16.5f, 24.5f);

			Topic.Input[] inputs3 = { new Topic.Input ("Plano de estudo? Parece-me bem.", () => {
					grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
					happy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
					demoScene.changeTopic ("onActivity");
					demoScene.OpenCalendar ();
				}, 1.5f),
				new Topic.Input ("Quero ser avisado mais cedo", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
					demoScene.changeTopic ("warnTestTopic");
				}, 3.0f), new Topic.Input ("Estou bem, obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

					demoScene.changeTopic ("help");
				}, 4.5f)
			};
	
			List<Line> badTest = new List<Line> ();
			badTest.Add (l9);
			badTest.Add (l10);
			badTest.Add (l11);
			badTest.Add (l56);
			Topic badTestTopic = new Topic (badTest, inputs3);
			demoScene.topics.Add ("badTestTopic", badTestTopic);

			//Below Average
//			happy.CurrentEmotion = Agent.EmotionType.SAD;
//			grumpy.CurrentEmotion = Agent.EmotionType.SAD;
			//colar quando houver mudança
			Line l12 = new Line ("Devias ter estudado mais", grumpy, 0.0f, 8.0f);
			Line l13 = new Line ("Oh, este não correu tão bem quanto estávamos à espera",
				           happy,
				           4.0f,
				           12.0f);
			Line l14 = new Line ("Para superar esta barreira,  acho boa ideia fazermos um plano de estudo, o que achas?",
				           happy,
				           12.5f,
				           20.5f);
			List<Line> belowAvg = new List<Line> ();
			belowAvg.Add (l12);
			belowAvg.Add (l13);
			belowAvg.Add (l14);
			belowAvg.Add (l56);
			Topic belowAvgTopic = new Topic (belowAvg, inputs3);
			demoScene.topics.Add ("belowAvgTopic", belowAvgTopic);

//			//mudar data de testes mais cedo

			Line l15 = new Line ("Ok, com isto vais ser relembrado antes e ter mais tempo para estudar.",
				           happy,
				           0.0f,
				           8.0f);
			Line l16 = new Line ("Continuo a achar que um plano  de estudo era uma boa ideia.",
				           grumpy,
				           4.0f,
				           12.0f);
			Topic.Input[] inputs4 = { new Topic.Input ("Plano de estudo? Parece-me bem.", () => {
					demoScene.OpenCalendar ();
				}, 1.5f), new Topic.Input ("Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

					demoScene.changeTopic ("help");
				}, 3.0f), new Topic.Input ("E que tal...", () => {
					demoScene.OpenList ();
				}, 4.5f)
			};
			List<Line> warnTest = new List<Line> ();
			warnTest.Add (l15);
			warnTest.Add (l16);
			Topic warnTestTopic = new Topic (warnTest, inputs4);
			demoScene.topics.Add ("warnTestTopic", warnTestTopic);

//			//expected
//			
			//colocar quando tiver link
			Line l17 = new Line ("Parabéns, foi uma boa nota.", happy, 0.0f, 8.0f);
			Line l18 = new Line ("Acho que podes melhorar um pouco.",
				           grumpy,
				           4.0f,
				           12.0f);
			Line l19 = new Line ("Sim mas não lhe tires a vitória", happy, 12.5f, 20.5f);
			Line l20 = new Line ("Não comeces, só acho que se o trabalho for um bocadinho aumentado a nota é capaz de subir",
				           grumpy,
				           16.5f,
				           24.5f);
			Line l21 = new Line ("Tudo bem, mas ainda assim é muito bom.",
				           happy,
				           25.0f,
				           33.0f);
			Line l57 = new Line ("Continua o bom trabalho", grumpy, 29.0f, 37.0f);
			List<Line> expected = new List<Line> ();
			expected.Add (l17);
			expected.Add (l18);
			expected.Add (l19);
			expected.Add (l20);
			expected.Add (l21);
			expected.Add (l57);
			Topic.Input[] inputs5 = {new Topic.Input ("Vamos fazer um plano de estudo", () => {
					demoScene.OpenCalendar ();
				}, 1.5f), new Topic.Input ("Quero ser avisado mais cedo", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
					demoScene.changeTopic ("warnTestTopic");
				}, 3.0f), new Topic.Input ("Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

					demoScene.changeTopic ("help");
				}, 4.5f)
			};

			Topic expectedTest = new Topic (expected, inputs5);
			demoScene.topics.Add ("expectedTest", expectedTest);

//			//Muito bom
//			happy.CurrentEmotion = Agent.EmotionType.LIKES;
//			grumpy.CurrentEmotion = Agent.EmotionType.LIKES;
			//Colocar quando der
			Line l22 = new Line ("Wow parabéns que óptima nota!", happy, 0.0f, 8.0f);
			Line l23 = new Line ("Sim, espetacular.", grumpy, 4.0f, 12.0f);
			Line l24 = new Line ("O quê ? Não há nenhum comentariozinho?",
				                    happy,
				                    12.5f,
				                    20.5f);
			Line l25 = new Line ("sigh...", grumpy, 16.5f, 24.5f);
			List<Line> great = new List<Line> ();
			great.Add (l22);
			great.Add (l23);
			great.Add (l24);
			great.Add (l25);
			Topic greatTest = new Topic (great, inputs5);
			demoScene.topics.Add ("greatTest", greatTest);

//			//Horas a menos
//			happy.CurrentEmotion = Agent.EmotionType.SAD;
//			grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
			//Colocar quando der
			Line l26 = new Line ("Não sei se só isto é boa ideia, tens a certeza?",
				                    happy,
				                    0.0f,
				                    8.0f);
			Line l27 = new Line ("Apenas essas horas é capaz de correr mal.",
				                    grumpy,
				                    4.0f,
				                    12.0f);
			Topic.Input[] inputs6 = { new Topic.Input("Tenho a certeza", () => {
                        happy.CurrentEmotion = Agent.EmotionType.SMILING;
                        grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

                        demoScene.changeTopic("help");
                    },1.5f), new Topic.Input("", () => {
                    },3.0f), new Topic.Input("Deixem-me corrigir uma coisa", () => {
                        demoScene.OpenCalendar();
                    },4.5f)
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
            Line l29 = new Line("Isto não te parece demasiado?", happy, 0.0f, 8.0f);
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
            Line l32 = new Line("Ok ótimo, está marcado.", happy, 0.0f, 8.0f);
            Line l33 = new Line("Espero que consigas cumpri-lo e subir a tua nota.",
                        grumpy,
                        4.0f,
                        12.0f);

            Topic.Input[] inputs7 =
                { new Topic.Input("Obrigado.", () => {

                        happy.CurrentEmotion = Agent.EmotionType.SMILING;
                        grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
                        demoScene.changeTopic("help");
                    },1.5f), new Topic.Input("", () => {
                    },3.0f), new Topic.Input("Deixem-me corrigir uma coisa.", () => {
                        demoScene.OpenCalendar();
                    },4.5f)
            };
            List<Line> enoughTime = new List<Line>();
            enoughTime.Add(l32);
            enoughTime.Add(l33);
            Topic enoughPlan = new Topic(enoughTime, inputs7);
            demoScene.topics.Add("enoughPlan", enoughPlan);


            //More help?
//			//No need to change?
			Line l34 = new Line ("Podemos ajudar-te em mais alguma coisa?", happy, 0.0f, 8.0f);
			Line l35 = new Line ("Agora já acertas o nós", grumpy, 4.0f, 12.0f);
			Topic.Input[] inputs8 = { new Topic.Input ("gostaria de falar de...", () => {
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic ("talkAbout");	
				},1.5f), new Topic.Input ("Não,Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
					demoScene.changeTopic ("twoDaysTopic");
				},3.0f), new Topic.Input ("tenho nova informação", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("newInfoTopic");
				},4.5f)
			};
			List<Line> moreHelp = new List<Line> ();
			moreHelp.Add (l34);
			moreHelp.Add (l35);
			Topic help = new Topic (moreHelp, inputs8);
			demoScene.topics.Add ("help", help);

//		
//			// 2days with plan
			Line l36 = new Line ("Daqui a dois dias vem cá para vermos como está a correr o plano", happy, 0.0f, 8.0f);
			Line l37 = new Line ("Espero que apareças", grumpy, 4.0f, 12.0f);
			Topic.Input[] inputs9 = { new Topic.Input ("Até daqui a dois dias", () => {
					happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					demoScene.changeTopic ("exit1Topic");
				},1.5f), new Topic.Input ("", () => {
				},3.0f), new Topic.Input ("Isso é muito cedo", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
					demoScene.changeTopic ("tooEarly");
				},4.5f)
			};
			List<Line> twodays = new List<Line> ();
			twodays.Add (l36);
			twodays.Add (l37);
			Topic twoDaysTopic = new Topic (twodays, inputs9);
			demoScene.topics.Add ("twoDaysTopic", twoDaysTopic);
	
//			//cedo
			Line l38 = new Line ("Então quando queres entrar em contacto connosco?", happy, 0.0f, 8.0f);
			Line l39 = new Line ("Se calhar ele não te quer ver", grumpy, 4.0f, 12.0f);
			Topic.Input[] inputs10 = { new Topic.Input (" 5 dias", () => {
					happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					demoScene.changeTopic ("exit2Topic");
				},1.5f), new Topic.Input ("7 dias", () => {
					happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					demoScene.changeTopic ("exit3Topic");
				},3.0f), new Topic.Input ("Ainda não sei", () => {
					happy.CurrentEmotion = Agent.EmotionType.SURPRISED;
					grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
					demoScene.changeTopic ("exit4Topic");
				},4.5f)
			};
			List<Line> early = new List<Line> ();
			early.Add (l38);
			early.Add (l39);
			Topic tooEarly = new Topic (early, inputs10);
			demoScene.topics.Add ("tooEarly", tooEarly);
//
			Line l40 = new Line ("Ok, até daqui a dois dias. Adeeeus!", happy, 0.0f, 8.0f);
			Line l41 = new Line ("Vou desligar para poupar o teu tempo e o nosso.", grumpy, 4.0f, 12.0f);
			List<Line> exit1 = new List<Line> ();
			exit1.Add (l40);
			exit1.Add (l41);
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
			Line l42 = new Line ("ok até daqui a 5 dias. Adeeeus!", happy, 0.0f, 8.0f);

            List<Line> exit2 = new List<Line>();
            exit2.Add(l41);
            exit2.Add(l42);
            Topic exit2Topic = new Topic(exit2, emptyInputs);
            demoScene.topics.Add("exit2Topic", exit2Topic);
//
//			//exit 7 days
			Line l43 = new Line ("ok até daqui a 7 dias. Adeeeus!", happy, 0.0f, 8.0f);
			List<Line> exit3 = new List<Line> ();
			exit3.Add (l43);
			exit3.Add (l41);
			Topic exit3Topic = new Topic (exit3, emptyInputs);
			demoScene.topics.Add ("exit3Topic", exit3Topic);

//			//Nao sabe
			Line l44 = new Line ("Pelos vistos ele não sabe, é normal nem toda a gente tem as coisas tão bem definidas como nós programas.", grumpy, 0.0f, 8.0f);
			Line l45 = new Line ("Então vemo-nos na próxima vez que vieres.", happy, 4.0f, 12.0f);
			List<Line> exit4 = new List<Line> ();
			exit4.Add (l44);
			exit4.Add (l45);
			Topic exit4Topic = new Topic (exit4, emptyInputs);
			demoScene.topics.Add ("exit4Topic", exit4Topic);

            //TimeOutDefault

			Line l46 = new Line ("Bom, eu acho que ele não quer falar connosco.", grumpy, 0.0f, 8.0f);
			Line l47 = new Line ("De certeza que se fartou da tua atitude.", happy, 4.0f, 12.0f);
			Line l48 = new Line ("Sim, porque toda a gente adora a tua personalidade.", grumpy, 12.5f, 20.5f);
			Line l49 = new Line ("Mas e agora, o que fazemos?", happy, 16.5f, 24.5f);
			Line l50 = new Line ("Epá desliga isso, já que estás tão contente e vamos embora.", grumpy, 25.0f, 33.0f);
			Line l51 = new Line ("Se tem mesmo de ser...", happy, 29.0f, 37.0f);
			List<Line> timeout1 = new List<Line> ();
			timeout1.Add (l46);
			timeout1.Add (l47);
			timeout1.Add (l48);
			timeout1.Add (l49);
			timeout1.Add (l50);
			timeout1.Add (l51);
			Topic timeTopic = new Topic (timeout1, emptyInputs);
			demoScene.topics.Add ("timeTopic", timeTopic);

			//not answering test
			Line l52 = new Line ("Bom, não queres falar sobre isso, ok.", happy, 0.0f, 8.0f);
			Line l53 = new Line ("Acho que um plano de estudo era uma boa ideia.", grumpy, 4.0f, 12.0f);
			Topic.Input[] notAnswTestInputs = { new Topic.Input ("Vamos fazer um plano de estudo!", () => {
					demoScene.OpenCalendar ();
				},1.5f), new Topic.Input ("", () => {
				},3.0f), new Topic.Input ("Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("help");
				},4.5f)
			};

            List<Line> noAnswerTest = new List<Line>();
            noAnswerTest.Add(l52);
            noAnswerTest.Add(l53);
            Topic noAnswTest = new Topic(noAnswerTest, notAnswTestInputs);
            demoScene.topics.Add("noAnswTest", noAnswTest);

			//			//Not answering Bad plan
			Line l54 = new Line ("Ok parece que ele quer insistir neste plano.", grumpy, 0.0f, 8.0f);
			Line l55 = new Line ("Temos de confiar nele", happy, 4.0f,12.0f);
			List<Line> noAnswerPlan = new List<Line> ();

            noAnswerPlan.Add(l54);
            noAnswerPlan.Add(l55);
            Topic noAnswPlan = new Topic(noAnswerPlan, emptyInputs);
            demoScene.topics.Add("noAnswPlan", noAnswPlan);

			Line l59 = new Line ("Yaaay ele está a inserir coisas.",happy,0.0f,8.0f);
			Line l60 = new Line ("Será que vai estar tudo bem?", grumpy,4.0f,12.0f);
			List<Line> onActivityList = new List<Line>();
			onActivityList.Add(l59);
			onActivityList.Add(l60);
			Topic onActivity = new Topic(onActivityList,emptyInputs);
			demoScene.topics.Add("onActivity",onActivity);

			Line l61 = new Line ("Qualquer coisa que precisares, não contes com ele",grumpy,4.0f,12.0f);
			List<Line> returnlist = new List<Line>();
			returnlist.Add(l34);
			returnlist.Add(l61);
			Topic returnTopic = new Topic(returnlist,inputs);
			demoScene.topics.Add("returnTopic",returnTopic);

			Line l62 = new Line ("A única informação que temos é que tens uma avaliação daqui a duas semanas",happy,0.0f,8.0f);
			Line l63 = new Line ("Para te lembrarmos de mais coisas tens de nos ajudar com mais informação", grumpy,4.0f,12.0f);
			Line l64 = new Line ("Pára de fazer as pessoas sentirem-se mal, não tarda és apagado", happy,12.5f,20.5f);
			Line l65 = new Line ("Sigh...Nunca me deixas fazer nada, anjinho", grumpy,16.5f,24.5f);
			List<Line> remindList = new List<Line>();
			remindList.Add(l62);
			remindList.Add(l63);
			remindList.Add(l64);
			remindList.Add(l65);
			Topic.Input[] remindInputs = { new Topic.Input("Obrigado pela informação", ()=>{
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic("returnTopic");
			},1.5f), new Topic.Input("",()=>{}), new Topic.Input("Tenho uma nova informação",()=>{
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic ("newInfoTopic");
			}, 3.0f) 
			};
			Topic reminder = new Topic(remindList,remindInputs);
			demoScene.topics.Add("reminder",reminder);
				
			Line l66 = new Line("Do que gostarias de falar connosco?", happy,0.0f,8.0f);
			Line l67 = new Line ("Por favor lembra-te que não devem ser coisas pessoais",grumpy,4.0f,12.0f);
			Line l68 = new Line ("Não sejas mau.", happy,12.5f,20.5f);
			Line l69 = new Line ("Só estou a avisar que somos agentes de estudo",grumpy,16.5f,24.5f);
			List<Line> talkAboutList = new List<Line>();
			talkAboutList.Add(l66);
			talkAboutList.Add(l67);
			talkAboutList.Add(l68);
			talkAboutList.Add(l69);
			Topic.Input[] talkInputs={ new Topic.Input("Avaliações Passadas",()=>{
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
				demoScene.changeTopic("pastTopic");
			},1.5f), new Topic.Input("Balanço do Semestre",()=>{
				happy.CurrentEmotion = Agent.EmotionType.SMILING;
				grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
				demoScene.changeTopic("balance");
			},3.0f), new Topic.Input("Dicas",()=>{
				happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
				demoScene.changeTopic("tips");
			},4.5f),new Topic.Input("Mais...",()=>{
				demoScene.OpenList();
			},6.0f)
				};
			Topic talkAbout = new Topic(talkAboutList,talkInputs);
			demoScene.topics.Add("talkAbout",talkAbout);

			Line l70 = new Line("Sabemos que tiveste um teste com 12 e um projeto com 16",happy,0.0f,8.0f);
			Line l71 = new Line("Esta é toda a informação que temos, por favor insere mais para sabermos mais",grumpy,4.0f,12.0f);
			List<Line> pastTestList = new List<Line>();
			pastTestList.Add(l70);
			pastTestList.Add(l71);
			Topic.Input [] pastInputs={
				new Topic.Input("Obrigado",()=>{
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic("returnTopic");
				},1.5f), new Topic.Input("",()=>{}), new Topic.Input("Mais...",()=>{
				demoScene.OpenList();
				})
			};
			Topic pastTopic = new Topic(pastTestList,pastInputs);
			demoScene.topics.Add("pastTopic",pastTopic);

			Line l72 = new Line ("Sabemos que tiveste um teste com 12 e um projeto com 16",grumpy,0.0f,8.0f);
			Line l73 = new Line ("Também sabemos que foste a uma revisão de prova", happy,4.0f,12.0f);
			Line l74 = new Line ("As coisas estão a ir bem, possivelmente o teu próximo teste será melhor",happy,12.5f,20.5f);
			Line l75 = new Line ("Mas não te esqueças de continuar o trabalho",grumpy,16.5f,24.5f);
			List<Line> balanceList = new List<Line>();
			balanceList.Add(l72);
			balanceList.Add(l73);
			balanceList.Add(l74);
			balanceList.Add(l75);
			Topic balance = new Topic (balanceList,pastInputs);
			demoScene.topics.Add("balance",balance);

			Line l76 = new Line ("Faz um plano de estudo e cumpre-o.",happy,0.0f,8.0f);
			Line l77 = new Line("Não deixes o tempo passar só porque as avaliações ainda estão distantes",grumpy,4.0f,12.0f);
			List<Line> tipsList = new List<Line>();
			tipsList.Add(l76);
			tipsList.Add(l77);
			Topic tips = new Topic (tipsList,pastInputs);
			demoScene.topics.Add("tips",tips);

			Line l78 = new Line("Ao desistires da cadeira deixaremos de te ajudar nela para ajudar de outras",happy,0.0f,8.0f);
			Line l79 = new Line("Não há vergonha nenhuma nisso se não estiveres a conseguir gerir o teu tempo",grumpy,4.0f,12.0f);
			Line l80 = new Line("É isso que queres?",happy,12.5f,20.5f);
			List<Line> quitList = new List<Line>();
			quitList.Add(l78);
			quitList.Add(l79);
			quitList.Add(l80);
			Topic.Input[] quitInputs = { new Topic.Input("Sim, por favor", ()=>{
				demoScene.changeTopic("help");
			}), new Topic.Input("",()=>{}),new Topic.Input("Talvez não",()=>{
				demoScene.OpenList();
			})};
			Topic quit = new Topic(quitList,quitInputs);
			demoScene.topics.Add("quit",quit);


			Line l84 = new Line("Não desistas desta cadeira já",happy,0.0f,8.0f);
			Line l85 = new Line("Ela  é demasiado importante para desistir",grumpy,4.0f,12.0f);
			Line l86 = new Line("Com a nossa ajuda vais conseguir melhorar",happy,12.5f,20.5f);
			Line l87 = new Line("Tens a certeza que queres desistir?",grumpy,16.5f,24.5f);
			List<Line> dontQuitList = new List<Line>();
			dontQuitList.Add(l84);
			dontQuitList.Add(l85);
			dontQuitList.Add(l86);
			dontQuitList.Add(l87);
			Topic dontQuit = new Topic(dontQuitList,quitInputs);
			demoScene.topics.Add("dontQuit",dontQuit);

			Line l81 = new Line ("Se precisares mesmo, falaremos com o teu tutor",happy,0.0f,8.0f);
			Line l82 = new Line("Enviaremos um e-mail com a tua situação",grumpy,4.0f,12.0f);
			Line l83 = new Line("É isto que queres?",grumpy,12.5f,20.5f);
			List<Line> contactList = new List<Line>();
			contactList.Add(l81);
			contactList.Add(l82);
			contactList.Add(l83);
			Topic contact = new Topic(contactList,quitInputs);
			demoScene.topics.Add("contact",contact);
    }
}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace VT
{
	public class MainScript : MonoBehaviour
	{

		public GameObject threeOptionsPrefab;
		public GameObject ExpressionsPrefab;
		public GameObject coursesPrefab;
		public GameObject coursePrefab;
		public GameObject calendar1Prefab;
		public GameObject calendar2Prefab;
		public GameObject calendar3Prefab;
		private Scene myScene;

        private bool playing = true;

		// Use this for initialization
		void Start ()
		{
			
            if (threeOptionsPrefab == null || ExpressionsPrefab == null
                || coursesPrefab == null || coursePrefab == null
                || calendar1Prefab == null || calendar2Prefab == null
                || calendar3Prefab == null) {
                Debug.LogWarning("Some prefabs are null");
				return;
			}

			Scene demoScene = new Scene ();
            demoScene.threePartsControl = new ThreePartsControl (threeOptionsPrefab);
            demoScene.expressionsControl = new ExpressionsControl (ExpressionsPrefab);
            demoScene.coursesControl = new CoursesControl (coursesPrefab);
            demoScene.courseControl = new CourseControl (coursePrefab);
            demoScene.calendar1Control = new Calendar1Control (calendar1Prefab);
            demoScene.calendar2Control = new Calendar2Control (calendar2Prefab);
            demoScene.calendar3Control = new Calendar3Control (calendar3Prefab);

			PopulateScene (demoScene);
			myScene = demoScene;
		}

		void Update ()
		{
            if (Input.GetKeyUp(KeyCode.Space)) {
                playing = !playing;
            }

            if (playing) {
                myScene.update(Time.deltaTime);
            }

		}

		void PopulateScene (Scene demoScene)
		{
		
			//Make topics
			demoScene.agents.Add (new Agent ());
			demoScene.agents.Add (new Agent ());

			Agent happy = demoScene.agents [0];
			Agent grumpy = demoScene.agents [1];
			grumpy.IsLeft = false;
			//Hello
			happy.CurrentEmotion = Agent.EmotionType.SMILING;
			grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
			Line l1 = new Line ("Olá, em que te posso ajudar hoje?", happy, 0.0f, 5.0f);
			Line l2 = new Line ("Podes?No singular? Eu nunca faço nada,é?", grumpy, 2.5f, 7.5f);
			Line l3 = new Line ("Ooooook, desculpa. Em que é que PODEMOS te ajudar hoje?", happy, 8.0f, 13.0f);
			Line l4 = new Line ("Pronto, assim já gosto mais. Egocêntrico.", grumpy, 10.5f, 15.5f);
			List<Line> lines = new List<Line> ();
			lines.Add (l1);
			lines.Add (l2);
			lines.Add (l3);
			lines.Add (l4);
			lines = lines.OrderBy (l => l.Start).ToList ();
			Topic.Input[] inputs = {
				new Topic.Input (
					"lembrem-me de...", () => {
						
					happy.CurrentEmotion = Agent.EmotionType.CRYING;
					grumpy.CurrentEmotion = Agent.EmotionType.CRYING;
					demoScene.changeTopic ("nonDeveloped");
				}),
				new Topic.Input (
					"gostaria de falar de ...",
					() => {

						happy.CurrentEmotion = Agent.EmotionType.CRYING;
						grumpy.CurrentEmotion = Agent.EmotionType.CRYING;
						demoScene.changeTopic ("nonDeveloped");					
					}),
				new Topic.Input (
					"tenho uma nova informação",
					() => {
						
						happy.CurrentEmotion = Agent.EmotionType.SMILING;
						grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
						demoScene.changeTopic ("newInfoTopic");
					}
				)
			};
			Topic hello = new Topic (lines, inputs);
			demoScene.topics.Add ("Hello", hello);
			//not developed

			Line l5 = new Line ("Não podemos fazer nada disso agora. Malditos programadores.", grumpy, 0.0f, 5.0f);
			Line l6 = new Line ("Assim que tivermos esta funcionalidade falaremos contigo.", happy, 2.5f, 7.5f);
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
				}),
				new Topic.Input ("fechem a aplicação por favor", () => {
					Debug.Log ("right");
				}
				)
			};
			Topic nonDeveloped = new Topic (nDLines, inputs2);
			demoScene.topics.Add ("nonDeveloped", nonDeveloped);

			//new Info?
			Line l7 = new Line ("Uma nova informação? Está bem mas despacha-te.", grumpy, 0.0f, 5.0f);
			Line l8 = new Line ("Não sejas assim", happy, 2.5f, 7.5f);
			List<Line> newInfo = new List<Line> ();
			newInfo.Add (l7);
			newInfo.Add (l8);
			Topic.Input[] inputs1 = {new Topic.Input ("ok", () => {
					demoScene.OpenCourses ();
				}), new Topic.Input ("", () => {
				}), new Topic.Input ("", () => {
				})
			};
			Topic newInfoTopic = new Topic (newInfo, inputs1);
			demoScene.topics.Add ("newInfoTopic", newInfoTopic);

			// Bad test
//			happy.CurrentEmotion = Agent.EmotionType.CRYING; para por quando houver passagem
//			grumpy.CurrentEmotion = Agent.EmotionType.CRYING;
//
			Line l9 = new Line ("Ora bolas!", happy, 0.0f, 5.0f);
			Line l10 = new Line ("Devias ter estudado mais...", grumpy, 2.5f, 7.5f);
			Line l11 = new Line ("Podemos sempre fazer um plano de estudo, o que achas?", happy, 8.0f, 13.0f);
			Line l56 = new Line ("Talvez consigas subir a nota", grumpy,10.5f,15.5f);

			Topic.Input[] inputs3 = { new Topic.Input ("Plano de estudo? Parece-me bem.", () => {
					demoScene.OpenCalendar ();
				}),
				new Topic.Input ("Quero ser avisado mais cedo", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
					demoScene.changeTopic ("warnTestTopic");
				}), new Topic.Input ("Estou bem, obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

					demoScene.changeTopic ("help");
				})
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
			Line l12 = new Line ("Devias ter estudado mais", grumpy, 0.0f, 5.0f);
			Line l13 = new Line ("Oh, este não correu tão bem quanto estávamos à espera", happy, 2.5f, 7.5f);
			Line l14 = new Line ("Para superar esta barreira,  acho boa ideia fazermos um plano de estudo, o que achas?", happy, 8.0f, 13.0f);
			List<Line> belowAvg = new List<Line> ();
			belowAvg.Add (l12);
			belowAvg.Add (l13);
			belowAvg.Add (l14);
			belowAvg.Add (l56);
			Topic belowAvgTopic = new Topic (belowAvg, inputs3);
			demoScene.topics.Add ("belowAvgTopic", belowAvgTopic);

//			//mudar data de testes mais cedo

			Line l15 = new Line ("Ok, com isto vais ser relembrado antes e ter mais tempo para estudar.", happy, 0.0f, 5.0f);
			Line l16 = new Line ("Continuo a achar que um plano  de estudo era uma boa ideia.", grumpy, 2.5f, 7.5f);
			Topic.Input[] inputs4 = { new Topic.Input ("Plano de estudo? Parece-me bem.", () => {
					demoScene.OpenCalendar ();
				}), new Topic.Input ("Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

					demoScene.changeTopic ("help");
				}), new Topic.Input ("E que tal...", () => {
					Debug.Log ("Em acabamentos");
				})
			};
			List<Line> warnTest = new List<Line> ();
			warnTest.Add (l15);
			warnTest.Add (l16);
			Topic warnTestTopic = new Topic (warnTest, inputs4);
			demoScene.topics.Add ("warnTestTopic", warnTestTopic);

//			//expected
//			
			//colocar quando tiver link
			Line l17 = new Line ("Parabéns, foi uma boa nota.", happy, 0.0f, 5.0f);
			Line l18 = new Line ("Acho que podes melhorar um pouco.", grumpy, 2.5f, 7.5f);
			Line l19 = new Line ("Sim mas não lhe tires a vitória", happy, 8.0f, 13.0f);
			Line l20 = new Line ("Não comeces, só acho que se o trabalho for um bocadinho aumentado a nota é capaz de subir", grumpy, 10.5f, 15.5f);
			Line l21 = new Line ("Tudo bem, mas ainda assim é muito bom.", happy, 16.0f, 21.0f);
			Line l57 = new Line ("Continua o bom trabalho", grumpy, 18.5f, 23.0f);
			List<Line> expected = new List<Line> ();
			expected.Add (l17);
			expected.Add (l18);
			expected.Add (l19);
			expected.Add (l20);
			expected.Add (l21);
			expected.Add (l57);
			Topic.Input[] inputs5 = {new Topic.Input ("Vamos fazer um plano de estudo", () => {
					demoScene.OpenCalendar ();
				}), new Topic.Input ("Quero ser avisado mais cedo", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
					demoScene.changeTopic ("warnTestTopic");
				}), new Topic.Input ("Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

					demoScene.changeTopic ("help");
				})
			};

			Topic expectedTest = new Topic (expected, inputs5);
			demoScene.topics.Add ("expectedTest", expectedTest);

//			//Muito bom
//			happy.CurrentEmotion = Agent.EmotionType.LIKES;
//			grumpy.CurrentEmotion = Agent.EmotionType.LIKES;
			//Colocar quando der
			Line l22 = new Line ("Wow parabéns que óptima nota!", happy, 0.0f, 5.0f);
			Line l23 = new Line ("Sim, espetacular.", grumpy, 2.5f, 7.5f);
			Line l24 = new Line ("O quê ? Não há nenhum comentariozinho?", happy, 8.0f, 13.0f);
			Line l25 = new Line ("sigh...", grumpy, 10.5f, 15.5f);
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
			Line l26 = new Line ("Não sei se só isto é boa ideia, tens a certeza?", happy, 0.0f, 5.0f);
			Line l27 = new Line ("Apenas essas horas é capaz de correr mal.", grumpy, 2.5f, 7.5f);
			Topic.Input[] inputs6 = { new Topic.Input ("Tenho a certeza", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

					demoScene.changeTopic ("help");
				}), new Topic.Input ("", () => {
				}), new Topic.Input ("Deixem-me corrigir uma coisa", () => {
					demoScene.OpenCalendar ();
				})
			};
			List<Line> fewTime = new List<Line> ();
			fewTime.Add (l26);
			fewTime.Add (l27);

			Topic ShortPlan = new Topic (fewTime, inputs6);
			demoScene.topics.Add ("ShortPlan", ShortPlan);

//			//Horas a mais
//			happy.CurrentEmotion = Agent.EmotionType.SAD;
//			grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
			//Quando colocar colar
			Line l29 = new Line ("Isto não te parece demasiado?", happy, 0.0f, 5.0f);
			Line l30 = new Line ("Eu sei que te digo para estudar mais mas isto é demais.", grumpy, 2.5f, 7.5f);
			Line l31 = new Line ("Se não tiveres tempo para descansar não vais usar o teu potencial máximo.", happy, 8.0f, 13.0f);
			Line l58 = new Line ("Aconselho-te a diminuir um pouco as horas", grumpy, 10.5f, 15.5f);
			List<Line> muchTime = new List<Line> ();
			muchTime.Add (l29);
			muchTime.Add (l30);
			muchTime.Add (l31);
			muchTime.Add (l58);
			Topic BigPlan = new Topic (muchTime, inputs6);
			demoScene.topics.Add ("BigPlan", BigPlan);
//
//			//JUst enough
//			happy.CurrentEmotion = Agent.EmotionType.SMILING;
//			grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
			//Quando colocar colar
			Line l32 = new Line ("Ok ótimo, está marcado.", happy, 0.0f, 5.0f);
			Line l33 = new Line ("Espero que consigas cumpri-lo e subir a tua nota.", grumpy, 2.5f, 7.5f);

			Topic.Input[] inputs7 = { new Topic.Input ("Obrigado.", () => {

					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("help");
				}), new Topic.Input ("", () => {
				}), new Topic.Input ("Deixem-me corrigir uma coisa.", () => {
					demoScene.OpenCalendar ();
				})
			};
			List<Line> enoughTime = new List<Line> ();
			enoughTime.Add (l32);
			enoughTime.Add (l33);
			Topic enoughPlan = new Topic (enoughTime, inputs7);
			demoScene.topics.Add ("enoughPlan", enoughPlan);


			//More help?
//			//No need to change?
			Line l34 = new Line ("Podemos ajudar-te em mais alguma coisa?", happy, 0.0f, 5.0f);
			Line l35 = new Line ("Agora já acertas o nós", grumpy, 2.5f, 7.5f);
			Topic.Input[] inputs8 = { new Topic.Input ("queria falar de...", () => {
					Debug.Log ("In development");
				}), new Topic.Input ("Não,Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
					demoScene.changeTopic ("twoDaysTopic");
				}), new Topic.Input ("tenho nova informação", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("newInfoTopic");
				})
			};
			List<Line> moreHelp = new List<Line> ();
			moreHelp.Add (l34);
			moreHelp.Add (l35);
			Topic help = new Topic (moreHelp, inputs8);
			demoScene.topics.Add ("help", help);

//		
//			// 2days with plan
			Line l36 = new Line ("Daqui a dois dias vem cá para vermos como está a correr o plano", happy, 0.0f, 5.0f);
			Line l37 = new Line ("Espero que apareças", grumpy, 2.5f, 7.5f);
			Topic.Input[] inputs9 = { new Topic.Input ("Até daqui a dois dias", () => {
					happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					demoScene.changeTopic ("exit1Topic");
				}), new Topic.Input ("", () => {
				}), new Topic.Input ("Isso é muito cedo", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.SUBMISSIVE;
					demoScene.changeTopic ("tooEarly");
				})
			};
			List<Line> twodays = new List<Line> ();
			twodays.Add (l36);
			twodays.Add (l37);
			Topic twoDaysTopic = new Topic (twodays, inputs9);
			demoScene.topics.Add ("twoDaysTopic", twoDaysTopic);
	
//			//cedo
			Line l38 = new Line ("Então quando queres entrar em contacto connosco?", happy, 0.0f, 5.0f);
			Line l39 = new Line ("Se calhar ele não te quer ver", grumpy, 2.5f, 7.5f);
			Topic.Input[] inputs10 = { new Topic.Input (" 5 dias", () => {
					happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					demoScene.changeTopic ("exit2Topic");
				}), new Topic.Input ("7 dias", () => {
					happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					demoScene.changeTopic ("exit3Topic");
				}), new Topic.Input ("Ainda não sei", () => {
					happy.CurrentEmotion = Agent.EmotionType.SURPRISED;
					grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
					demoScene.changeTopic ("exit4Topic");
				})
			};
			List<Line> early = new List<Line> ();
			early.Add (l38);
			early.Add (l39);
			Topic tooEarly = new Topic (early, inputs10);
			demoScene.topics.Add ("tooEarly", tooEarly);
//
			Line l40 = new Line ("Ok, até daqui a dois dias. Adeeeus!", happy, 0.0f, 5.0f);
			Line l41 = new Line ("Vou desligar para poupar o teu tempo e o nosso.", grumpy, 2.5f, 7.5f);
			List<Line> exit1 = new List<Line> ();
			exit1.Add (l40);
			exit1.Add (l41);
//			//nao vai haver inputs just close window
			Topic.Input[] emptyInputs = {
				new Topic.Input ("", () => {
				}),
				new Topic.Input ("", () => {
				}),
				new Topic.Input ("", () => {
				})
			};

			Topic exit1Topic = new Topic (exit1, emptyInputs);
			demoScene.topics.Add ("exit1Topic", exit1Topic);
//
//			//exit 5 days
			Line l42 = new Line ("ok até daqui a 5 dias. Adeeeus!", happy, 0.0f, 5.0f);

			List<Line> exit2 = new List<Line> ();
			exit2.Add (l41);
			exit2.Add (l42);
			Topic exit2Topic = new Topic (exit2, emptyInputs);
			demoScene.topics.Add ("exit2Topic", exit2Topic);
//
//			//exit 7 days
			Line l43 = new Line ("ok até daqui a 7 dias. Adeeeus!", happy, 0.0f, 5.0f);
			List<Line> exit3 = new List<Line> ();
			exit3.Add (l43);
			exit3.Add (l41);
			Topic exit3Topic = new Topic (exit3, emptyInputs);
			demoScene.topics.Add ("exit3Topic", exit3Topic);

//			//Nao sabe
			Line l44 = new Line ("Pelos vistos ele não sabe, é normal nem toda a gente tem as coisas tão bem definidas como nós programas.", grumpy, 0.0f, 5.0f);
			Line l45 = new Line ("Então vemo-nos na próxima vez que vieres.", happy, 2.5f, 7.5f);
			List<Line> exit4 = new List<Line> ();
			exit4.Add (l44);
			exit4.Add (l45);
			Topic exit4Topic = new Topic (exit4, emptyInputs);
			demoScene.topics.Add ("exit4Topic", exit4Topic);

			//TimeOutDefault

			Line l46 = new Line ("Bom, eu acho que ele não quer falar connosco.", grumpy, 0.0f, 5.0f);
			Line l47 = new Line ("De certeza que se fartou da tua atitude.", happy, 2.5f, 7.5f);
			Line l48 = new Line ("Sim, porque toda a gente adora a tua personalidade.", grumpy, 8.0f, 13.0f);
			Line l49 = new Line ("Mas e agora, o que fazemos?", happy, 10.5f, 15.5f);
			Line l50 = new Line ("Epá desliga isso, já que estás tão contente e vamos embora.", grumpy, 16.0f, 21.0f);
			Line l51 = new Line ("Se tem mesmo de ser...", happy, 18.5f, 23.5f);
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
			Line l52 = new Line ("Bom, não queres falar sobre isso, ok.", happy, 0.0f, 5.0f);
			Line l53 = new Line ("Acho que um plano de estudo era uma boa ideia.", grumpy, 2.5f, 7.5f);
			Topic.Input[] notAnswTestInputs = { new Topic.Input ("Vamos fazer um plano de estudo!", () => {
					demoScene.OpenCalendar ();
				}), new Topic.Input ("", () => {
				}), new Topic.Input ("Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("help");
				})
			};

			List<Line> noAnswerTest = new List<Line> ();
			noAnswerTest.Add (l52);
			noAnswerTest.Add (l53);
			Topic noAnswTest = new Topic (noAnswerTest, notAnswTestInputs);
			demoScene.topics.Add ("noAnswTest", noAnswTest);

			//			//Not answering Bad plan
			Line l54 = new Line ("Ok parece que ele quer insistir neste plano.", grumpy, 0.0f, 5.0f);
			Line l55 = new Line ("Temos de confiar nele", happy, 2.5f, 7.5f);
			List<Line> noAnswerPlan = new List<Line> ();

			noAnswerPlan.Add (l54);
			noAnswerPlan.Add (l55);
			Topic noAnswPlan = new Topic (noAnswerPlan, emptyInputs);
			demoScene.topics.Add ("noAnswPlan", noAnswPlan);

			//start
			demoScene.changeTopic ("Hello");
	
		}


	}
}


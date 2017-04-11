using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VT
{
	public class MainScript : MonoBehaviour
	{

		public GameObject threeOptionsPrefab;
		public GameObject ExpressionsPrefab;
		private Scene myScene;
		// Use this for initialization
		void Start ()
		{
			
			if (!threeOptionsPrefab) {
				return;
			}
			if (!ExpressionsPrefab) {
				return;
			}
			Scene demoScene = new Scene ();
			ThreePartsControl control = new ThreePartsControl (threeOptionsPrefab);
			ExpressionsControl expControl = new ExpressionsControl (ExpressionsPrefab);
			StartDemoScene (control, expControl, demoScene);
			myScene = demoScene;
		}

		void Update ()
		{
			SceneUpdate (myScene);	
		}
		void SceneUpdate(Scene demoScene){
			demoScene.updateScene (Time.deltaTime);
		}
		void StartDemoScene (ThreePartsControl control, ExpressionsControl expControl, Scene demoScene)
		{
			demoScene.threePartsControl = control;
			demoScene.expressionsControl = expControl;
		
			//Make topics
			demoScene.agents.Add (new Agent ());
			demoScene.agents.Add (new Agent ());

			Agent happy = demoScene.agents [0];
			Agent grumpy = demoScene.agents [1];
			grumpy.IsLeft = false;
			//Hello
			happy.CurrentEmotion = Agent.EmotionType.SMILING;
			grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
			Line l1 = new Line ("Olá, em que te posso ajudar hoje?", happy);
			Line l2 = new Line ("Podes?No singular? Eu nunca faço nada,é?", grumpy);
			Line l3 = new Line ("Ooooook, desculpa. Em que é que PODEMOS te ajudar hoje?", happy);
			Line l4 = new Line ("Pronto, assim já gosto mais. Egocêntrico.", grumpy);
			List<Line> lines = new List<Line> ();
			lines.Add (l1);
			lines.Add (l2);
			lines.Add (l3);
			lines.Add (l4);
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

			Line l5 = new Line ("Não podemos fazer nada disso agora. Malditos programadores.", grumpy);
			Line l6 = new Line ("Assim que tivermos esta funcionalidade falaremos contigo.", happy);
			List<Line> nDLines = new List<Line> ();
			nDLines.Add (l5);
			nDLines.Add (l6);
			Topic.Input[] inputs2 = { new Topic.Input (
					"está bem", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("Hello");
				}),
				new Topic.Input ("empty", () => {
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
			Line l7 = new Line ("Uma nova informação? Está bem mas despacha-te.", grumpy);
			Line l8 = new Line ("Não sejas assim", happy);
			List<Line> newInfo = new List<Line> ();
			newInfo.Add (l7);
			newInfo.Add (l8);
			Topic.Input[] inputs1 = {new Topic.Input ("ok", () => {
					Debug.Log ("Abre number cenas para meter nota do teste");
				}), new Topic.Input ("empty", () => {
				}), new Topic.Input ("empty", () => {
				})
			};
			Topic newInfoTopic = new Topic (newInfo, inputs1);
			demoScene.topics.Add ("newInfoTopic", newInfoTopic);

			// Bad test
//			happy.CurrentEmotion = Agent.EmotionType.CRYING; para por quando houver passagem
//			grumpy.CurrentEmotion = Agent.EmotionType.CRYING;
//
			Line l9 = new Line ("Ora bolas!", happy);
			Line l10 = new Line ("Devias ter estudado mais...", grumpy);
			Line l11 = new Line ("Podemos sempre fazer um plano de estudo, o que achas?", happy);
			Line lEmpty = new Line ("empty", grumpy);

			Topic.Input[] inputs3 = { new Topic.Input ("Plano de estudo? Parece-me bem.", () => {
					Debug.Log ("abrir plano de estudo");
				}),
				new Topic.Input ("Quero ser avisado mais cedo", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
					//				demoScene.changeTopic ("warnTestTopic");
				}), new Topic.Input ("E que tal...", () => {
					Debug.Log ("Em acabamentos");
				})
			};
	
			List<Line> badTest = new List<Line> ();
			badTest.Add (l9);
			badTest.Add (l10);
			badTest.Add (l11);
			badTest.Add (lEmpty);
			Topic badTestTopic = new Topic (badTest, inputs3);
			demoScene.topics.Add ("badTestTopic", badTestTopic);

			//Below Average
//			happy.CurrentEmotion = Agent.EmotionType.SAD;
//			grumpy.CurrentEmotion = Agent.EmotionType.SAD;
			//colar quando houver mudança
			Line l12 = new Line ("Devias ter estudado mais", grumpy);
			Line l13 = new Line ("Oh, este não correu tão bem quanto estávamos à espera", happy);
			Line l14 = new Line ("Para superar esta barreira,  acho boa ideia fazermos um plano de estudo, o que achas?", happy);
			List<Line> belowAvg = new List<Line> ();
			belowAvg.Add (l12);
			belowAvg.Add (l13);
			belowAvg.Add (l14);
			belowAvg.Add (lEmpty);
			Topic belowAvgTopic = new Topic (belowAvg, inputs3);
			demoScene.topics.Add ("belowAvgTopic", belowAvgTopic);

//			//mudar data de testes mais cedo

			Line l15 = new Line ("Ok, com isto vais ser relembrado antes e ter mais tempo para estudar.", happy);
			Line l16 = new Line ("Continuo a achar que um plano  de estudo era uma boa ideia.", grumpy);
			Topic.Input[] inputs4 = { new Topic.Input ("Plano de estudo? Parece-me bem.", () => {
					Debug.Log ("abrir plano de estudo");
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
//			happy.CurrentEmotion = Agent.EmotionType.LIKES;
//			grumpy.CurrentEmotion = Agent.EmotionType.SMILING;
			//colocar quando tiver link
			Line l17 = new Line ("Parabéns, foi uma boa nota.", happy);
			Line l18 = new Line ("Acho que podes melhorar um pouco.", grumpy);
			Line l19 = new Line ("Sim mas não lhe tires a vitória", happy);
			Line l20 = new Line ("Não comeces, só acho que se o trabalho for um bocadinho aumentado a nota é capaz de subir", grumpy);
			Line l21 = new Line ("Tudo bem, mas ainda assim é muito bom.", happy);
			List<Line> expected = new List<Line> ();
			expected.Add (l17);
			expected.Add (l18);
			expected.Add (l19);
			expected.Add (l20);
			expected.Add (l21);
			expected.Add (lEmpty);
			Topic.Input[] inputs5 = {new Topic.Input ("Vamos fazer um plano de estudo", () => {
					Debug.Log ("abre plano");
				}), new Topic.Input ("Quero ser avisado mais cedo", () => {
					happy.CurrentEmotion = Agent.EmotionType.SHY;
					grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
					//				demoScene.changeTopic ("warnTestTopic");
				}), new Topic.Input ("E que tal...", () => {
					Debug.Log ("Em acabamentos");
				})
			};

			Topic expectedTest = new Topic (expected, inputs5);
			demoScene.topics.Add ("expectedTest", expectedTest);

//			//Muito bom
//			happy.CurrentEmotion = Agent.EmotionType.LIKES;
//			grumpy.CurrentEmotion = Agent.EmotionType.LIKES;
			//Colocar quando der
			Line l22 = new Line ("Wow parabéns que óptima nota!", happy);
			Line l23 = new Line ("Sim, espetacular.", grumpy);
			Line l24 = new Line ("O quê ? Não há nenhum comentariozinho?", happy);
			Line l25 = new Line ("sigh...", grumpy);
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
			Line l26 = new Line ("Não sei se só isto é boa ideia, tens a certeza?", happy);
			Line l27 = new Line ("Apenas essas horas é capaz de correr mal.", grumpy);
			Line l28 = new Line ("Eu acredito em ti mas tens mesmo a certeza?", happy);
			Topic.Input[] inputs6 = { new Topic.Input ("Tenho a certeza", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;

				demoScene.changeTopic ("help");
				}), new Topic.Input ("empty", () => {
				}), new Topic.Input ("Deixem-me corrigir uma coisa", () => {
					Debug.Log ("Abre o plano");
				})
			};
			List<Line> fewTime = new List<Line> ();
			fewTime.Add (l26);
			fewTime.Add (l27);
			fewTime.Add (l28);
			fewTime.Add (lEmpty);
			Topic ShortPlan = new Topic (fewTime, inputs6);
			demoScene.topics.Add ("ShortPlan", ShortPlan);

//			//Horas a mais
//			happy.CurrentEmotion = Agent.EmotionType.SAD;
//			grumpy.CurrentEmotion = Agent.EmotionType.ANGRY;
			//Quando colocar colar
			Line l29 = new Line ("Isto não te parece demasiado?", happy);
			Line l30 = new Line ("Eu sei que te digo para estudar mais mas isto é demais.", grumpy);
			Line l31 = new Line ("Se não tiveres tempo para descansar não vais usar o teu potencial máximo.", happy);

			List<Line> muchTime = new List<Line> ();
			muchTime.Add (l29);
			muchTime.Add (l30);
			muchTime.Add (l31);
			muchTime.Add (lEmpty);
			Topic BigPlan = new Topic (muchTime, inputs6);
			demoScene.topics.Add ("BigPlan", BigPlan);
//
//			//JUst enough
//			happy.CurrentEmotion = Agent.EmotionType.SMILING;
//			grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
			//Quando colocar colar
			Line l32 = new Line ("Ok ótimo, está marcado.", happy);
			Line l33 = new Line ("Espero que consigas cumpri-lo e subir a tua nota.", grumpy);

			Topic.Input[] inputs7 = { new Topic.Input ("Obrigado.", () => {

					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("help");
				}), new Topic.Input ("empty", () => {
				}), new Topic.Input ("Deixem-me corrigir uma coisa.", () => {
					Debug.Log ("abre o plano");
				})
			};
			List<Line> enoughTime = new List<Line> ();
			enoughTime.Add (l32);
			enoughTime.Add (l33);
			Topic enoughPlan = new Topic (enoughTime, inputs7);
			demoScene.topics.Add ("enoughPlan", enoughPlan);


			//More help?
//			//No need to change?
			Line l34 = new Line ("Podemos ajudar-te em mais alguma coisa?", happy);
			Line l35 = new Line ("Agora já acertas o nós", grumpy);
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
			Line l36 = new Line ("Daqui a dois dias vem cá para vermos como está a correr o plano", happy);
			Line l37 = new Line ("Espero que apareças", grumpy);
			Topic.Input[] inputs9 = { new Topic.Input ("Até daqui a dois dias", () => {
					happy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					grumpy.CurrentEmotion = Agent.EmotionType.DOMINANT;
					demoScene.changeTopic ("exit1Topic");
				}), new Topic.Input ("empty", () => {
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
			Line l38 = new Line ("Então quando queres entrar em contacto connosco?", happy);
			Line l39 = new Line ("Se calhar ele não te quer ver", grumpy);
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
			Line l40 = new Line ("Ok, até daqui a dois dias. Adeeeus!", happy);
			Line l41 = new Line ("Vou desligar para poupar o teu tempo e o nosso.", grumpy);
			List<Line> exit1 = new List<Line> ();
			exit1.Add (l40);
			exit1.Add (l41);
//			//nao vai haver inputs just close window
			Topic.Input[] emptyInputs = {
				new Topic.Input ("empty", () => {
				}),
				new Topic.Input ("empty", () => {
				}),
				new Topic.Input ("empty", () => {
				})
			};

			Topic exit1Topic = new Topic (exit1, emptyInputs);
			demoScene.topics.Add ("exit1Topic", exit1Topic);
//
//			//exit 5 days
			Line l42 = new Line ("ok até daqui a 5 dias. Adeeeus!", happy);

			List<Line> exit2 = new List<Line> ();
			exit2.Add (l41);
			exit2.Add (l42);
			Topic exit2Topic = new Topic (exit2, emptyInputs);
			demoScene.topics.Add ("exit2Topic", exit2Topic);
//
//			//exit 7 days
			Line l43 = new Line ("ok até daqui a 7 dias. Adeeeus!", happy);
			List<Line> exit3 = new List<Line> ();
			exit3.Add (l43);
			exit3.Add (l41);
			Topic exit3Topic = new Topic (exit3, emptyInputs);
			demoScene.topics.Add ("exit3Topic", exit3Topic);

//			//Nao sabe
			Line l44 = new Line ("Pelos vistos ele não sabe, é normal nem toda a gente tem as coisas tão bem definidas como nós programas.", grumpy);
			Line l45 = new Line ("Então vemo-nos na próxima vez que vieres.", happy);
			List<Line> exit4 = new List<Line> ();
			exit4.Add (l44);
			exit4.Add (l45);
			Topic exit4Topic = new Topic (exit4, emptyInputs);
			demoScene.topics.Add ("exit4Topic", exit4Topic);

			//TimeOutDefault
			happy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
			grumpy.CurrentEmotion = Agent.EmotionType.IMPATIENT;
			Line l46 = new Line ("Bom, eu acho que ele não quer falar connosco.", grumpy);
			Line l47 = new Line ("De certeza que se fartou da tua atitude.", happy);
			Line l48 = new Line ("Sim, porque toda a gente adora a tua personalidade.", grumpy);
			Line l49 = new Line ("Mas e agora, o que fazemos?", happy);
			Line l50 = new Line ("Epá desliga isso, já que estás tão contente e vamos embora.", grumpy);
			Line l51 = new Line ("Se tem mesmo de ser...", happy);
			List<Line> timeout1 = new List<Line> ();
			timeout1.Add (l46);
			timeout1.Add (l47);
			timeout1.Add (l48);
			timeout1.Add (l49);
			timeout1.Add (l50);
			timeout1.Add (l51);
			Topic timeTopic = new Topic (timeout1, emptyInputs);
			demoScene.topics.Add ( "timeTopic",timeTopic);

			//not answering test
			Line l52 = new Line("Bom, não queres falar sobre isso, ok.",happy);
			Line l53 = new Line ("Acho que um plano de estudo era uma boa ideia.", grumpy);
			Topic.Input[] notAnswTestInputs = { new Topic.Input ("Vamos fazer um plano de estudo!", () => {
					Debug.Log ("Abrir plano de estudo");
				}), new Topic.Input ("empty", () => {
				}), new Topic.Input ("Obrigado", () => {
					happy.CurrentEmotion = Agent.EmotionType.SMILING;
					grumpy.CurrentEmotion = Agent.EmotionType.POKERFACE;
					demoScene.changeTopic ("help");
				})
			};

				List<Line> noAnswerTest = new List<Line> ();
					noAnswerTest.Add (l52);
					noAnswerTest.Add (l53);
			Topic noAnswTest = new Topic (noAnswerTest, notAnswTestInputs );
			demoScene.topics.Add ("noAnswTest", noAnswTest);

			//			//Not answering Bad plan
			Line l54 = new Line("Ok parece que ele quer insistir neste plano.",grumpy);
			Line l55 = new Line ("Temos de confiar nele", happy);
			List<Line> noAnswerPlan = new List<Line> ();

					noAnswerPlan.Add (l54);
			noAnswerPlan.Add (l55);
			Topic noAnswPlan = new Topic (noAnswerPlan,emptyInputs);
			demoScene.topics.Add ("noAnswPlan",noAnswPlan);

			//start
			demoScene.changeTopic ("Hello");
	
		}


	}
}


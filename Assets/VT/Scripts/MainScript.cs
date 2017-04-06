using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VT
{
	public class MainScript : MonoBehaviour
	{

		public GameObject threeOptionsPrefab;

		// Use this for initialization
		void Start ()
		{
			
			if (!threeOptionsPrefab) {
				return;
			}


			ThreePartsControl control = new ThreePartsControl (threeOptionsPrefab);

			//control.SetAndShow(t[i].get
			Scene demoScene = new Scene ();
			demoScene.threePartsControl = control;

			//Make topics
			demoScene.agents.Add (new Agent ());
			demoScene.agents.Add (new Agent ());

			Agent happy = demoScene.agents [0];
			Agent grumpy = demoScene.agents [1];
			//Hello
			Line l1 = new Line ("Olá, em que te posso ajudar hoje?\n", happy);
			Line l2 = new Line ("Podes?No singular? Eu nunca faço nada,é?\n", grumpy);
			Line l3 = new Line ("Ooooook, desculpa. Em que é que PODEMOS te ajudar hoje?\n", happy);
			Line l4 = new Line ("Pronto, assim já gosto mais. Egocêntrico.\n", grumpy);
			List<Line> lines = new List<Line> ();
			lines.Add (l1);
			lines.Add (l2);
			lines.Add (l3);
			lines.Add (l4);
			Topic.Input[] inputs = {
				new Topic.Input (
					"lembrem-me de...", () => {
						demoScene.changeTopic("nonDeveloped");
				}),
				new Topic.Input (
					"gostaria de falar de ...",
					() => {
						Debug.Log ("TOP!!!");
					}),
				new Topic.Input (
					"tenho uma nova informação",
					() => {
						Debug.Log ("RIGHT!!!");
					}
				)
			};
			Topic hello = new Topic (lines, inputs);
			demoScene.topics.Add ("Hello", hello);

			//not developed
			l1.Content = "Não podemos fazer nada disso agora. Malditos programadores.\n";
			l1.Speaker = grumpy;
			l2.Content = "Assim que tivermos esta funcionalidade falaremos contigo. \n";
			l2.Speaker = happy;
			List<Line> nDLines = new List<Line> ();
			nDLines.Add (l1);
			nDLines.Add (l2);
			Topic.Input[] inputs2 = { new Topic.Input (
					"está bem", () => {
					Debug.Log ("left");
				}),
				new Topic.Input ("empty", () => {
					Debug.Log ("top");
				}),
				new Topic.Input ("fechem a aplicação por favor", () => {
					Debug.Log ("right)");
				}
				)
			};
			Topic nonDeveloped = new Topic (nDLines, inputs2);
			demoScene.topics.Add ("nonDeveloped",nonDeveloped);
			//
			//			//new Info?
			//			l1.Content = "Uma nova informação? Está bem mas despacha-te.\n";
			//			l1.Speaker = grumpy;
			//			List<Line> newInfo = new List<Line> ();
			//			newInfo.Add (l1);
			//			string[] inputs1 = { "ok (vai sair)" };
			//			Topic newInfoTopic = new Topic (newInfo, inputs1);
			//			topics.Add (newInfoTopic);
			//
			//			// Bad test
			//			l1.Content = "Ora bolas!\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Devias ter estudado mais...\n";
			//			l2.Speaker = grumpy;
			//			l3.Content = "Podemos sempre fazer um plano de estudo, o que achas?\n";
			//			l3.Speaker = happy;
			//			inputs [0] = "Plano de estudo? Parece-me bem.";
			//			inputs [1] = "Gostava que no futuro...";
			//			inputs [2] = "E que tal..";
			//			List<Line> badTest = new List<Line> ();
			//			badTest.Add (l1);
			//			badTest.Add (l2);
			//			badTest.Add (l3);
			//			Topic badTestTopic = new Topic (badTest, inputs);
			//			topics.Add (badTestTopic);
			//
			//			//Below Average
			//			l1.Content = "Devias ter estudado mais.\n";
			//			l1.Speaker = grumpy;
			//			l2.Content = "Oh, este não correu tão bem quanto estávamos à espera\n";
			//			l2.Speaker = happy;
			//			l3.Content = "Para superar esta barreira,  acho boa ideia fazermos um plano de estudo, o que achas?\n";
			//			l3.Speaker = happy;
			//			List<Line> belowAvg = new List<Line> ();
			//			belowAvg.Add (l1);
			//			belowAvg.Add (l2);
			//			belowAvg.Add (l3);
			//			Topic belowAvgTopic = new Topic (belowAvg, inputs);
			//			topics.Add (belowAvgTopic);
			//
			//			//mudar data de testes mais cedo
			//
			//			l1.Content = "Ok, com isto vais ser relembrado antes e ter mais tempo para estudar.\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Continuo a achar que um plano  de estudo era uma boa ideia.\n";
			//			l2.Speaker = grumpy;
			//			inputs [1] = "obrigado";
			//			List<Line> warnTest = new List<Line> ();
			//			warnTest.Add (l1);
			//			warnTest.Add (l2);
			//			Topic warnTestTopic = new Topic (warnTest, inputs);
			//			topics.Add (warnTestTopic);
			//
			//			//expected
			//			l1.Content = "Parabéns, foi uma boa nota.\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Acho que podes melhorar um pouco.\n";
			//			l2.Speaker = grumpy;
			//			l3.Content = "Sim mas não tires a vitória ao rapaz.\n";
			//			l3.Speaker = happy;
			//			l4.Content = "Não comeces, só acho que se o trabalho for um bocadinho aumentado a nota é capaz de subir.\n";
			//			l4.Speaker = grumpy;
			//			Line l5 = new Line ("Tudo bem, mas ainda assim é muito bom.\n", happy);
			//			List<Line> expected = new List<Line> ();
			//			expected.Add (l1);
			//			expected.Add (l2);
			//			expected.Add (l3);
			//			expected.Add (l4);
			//			expected.Add (l5);
			//			inputs [0] = "vamos fazer um plano de estudo?";
			//			Topic expectedTest = new Topic (expected, inputs);
			//			topics.Add (expectedTest);
			//
			//			//Muito bom
			//			l1.Content = "Wow parabéns que óptima nota!\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Sim, espetacular.\n";
			//			l2.Speaker = grumpy;
			//			l3.Content = "O quê ? Não há nenhum comentariozinho?\n";
			//			l3.Speaker = happy;
			//			l4.Content = "sigh…\n";
			//			l4.Speaker = grumpy;
			//			List<Line> great = new List<Line> ();
			//			great.Add (l1);
			//			great.Add (l2);
			//			great.Add (l3);
			//			great.Add (l4);
			//			Topic greatTest = new Topic (great, inputs);
			//			topics.Add (greatTest);
			//
			//			//Horas a menos
			//			l1.Content = "Não sei se só isto é boa ideia, tens a certeza?\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Apenas essas horas é capaz de correr mal.\n";
			//			l2.Speaker = grumpy;
			//			l3.Content = "Eu acredito em ti mas tens mesmo a certeza?\n";
			//			l3.Speaker = happy;
			//			inputs2 [0] = "Tenho a certeza";
			//			inputs2 [1] = "Deixem-me corrigir uma coisa";
			//			List<Line> fewTime = new List<Line> ();
			//			fewTime.Add (l1);
			//			fewTime.Add (l2);
			//			fewTime.Add (l3);
			//			Topic ShortPlan = new Topic (fewTime, inputs2);
			//			topics.Add (ShortPlan);
			//
			//			//Horas a mais
			//			l1.Content = "Isto não te parece demasiado?\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Eu sei que te digo para estudar mais mas isto é demais.\n";
			//			l2.Speaker = grumpy;
			//			l3.Content = "Se não tiveres tempo para descansar não vais usar o teu potencial máximo. Eu acredito em ti mas tens mesmo a certeza? \n";
			//			l3.Speaker = happy;
			//			List<Line> muchTime = new List<Line> ();
			//			muchTime.Add (l1);
			//			muchTime.Add (l2);
			//			muchTime.Add (l3);
			//			Topic BigPlan = new Topic (muchTime, inputs2);
			//			topics.Add (BigPlan);
			//
			//			//JUst enough
			//			l1.Content = "Ok ótimo, está marcado.\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Espero que consigas cumpri-lo e subir a tua nota.\n";
			//			l2.Speaker = grumpy;
			//			inputs2 [0] = "obrigado";
			//			List<Line> enoughTime = new List<Line> ();
			//			enoughTime.Add (l1);
			//			enoughTime.Add (l2);
			//			Topic enoughPlan = new Topic (enoughTime, inputs2);
			//			topics.Add (enoughPlan);
			//
			//			//More help?
			//			l1.Content = "Podemos ajudar-te em mais alguma coisa?";
			//			l1.Speaker = happy;
			//			inputs [0] = "queria falar de...";
			//			inputs [1] = "não obrigado";
			//			inputs [2] = "tenho nova informação";
			//			List<Line> moreHelp = new List<Line> ();
			//			moreHelp.Add (l1);
			//			Topic help = new Topic (moreHelp, inputs);
			//			topics.Add (help);
			//
			//
			//			// 2days with plan
			//			l1.Content = "Daqui a dois dias vem cá para vermos como está a correr o plano";
			//			l1.Speaker = happy;
			//			inputs2 [0] = "Até daqui a dois dias.";
			//			inputs2 [1] = "Isso é muito cedo";
			//			List<Line> twodays = new List<Line> ();
			//			twodays.Add (l1);
			//			Topic twoDaysTopic = new Topic (twodays, inputs2);
			//			topics.Add (twoDaysTopic);
			//
			//			//cedo
			//			l1.Content = "Então quando queres entrar em contacto connosco?";
			//			l1.Speaker = happy;
			//			inputs [0] = "5 dias";
			//			inputs [1] = "7 dias";
			//			inputs [2] = "ainda não sei";
			//			List<Line> early = new List<Line> ();
			//			early.Add (l1);
			//			Topic tooEarly = new Topic (early, inputs);
			//			topics.Add (tooEarly);
			//
			//			//exit 2 days
			//			l1.Content = "ok até daqui a 2 dias. Adeeeus!\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Vou desligar para poupar o teu tempo e o nosso. \n";
			//			l2.Speaker = grumpy;
			//			List<Line> exit1 = new List<Line> ();
			//			exit1.Add (l1);
			//			exit1.Add (l2);
			//			Topic exit1Topic = new Topic (exit1);
			//			topics.Add (exit1Topic);
			//
			//			//exit 5 days
			//			l1.Content = "ok até daqui a 5 dias. Adeeeus!\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Vou desligar para poupar o teu tempo e o nosso. \n";
			//			l2.Speaker = grumpy;
			//			List<Line> exit2 = new List<Line> ();
			//			exit2.Add (l1);
			//			exit2.Add (l2);
			//			Topic exit2Topic = new Topic (exit2);
			//			topics.Add (exit2Topic);
			//
			//			//exit 7 days
			//			l1.Content = "ok até daqui a 7 dias. Adeeeus!\n";
			//			l1.Speaker = happy;
			//			l2.Content = "Vou desligar para poupar o teu tempo e o nosso. \n";
			//			l2.Speaker = grumpy;
			//			List<Line> exit3 = new List<Line> ();
			//			exit3.Add (l1);
			//			exit3.Add (l2);
			//			Topic exit3Topic = new Topic (exit3);
			//			topics.Add (exit3Topic);
			//
			//			//Nao sabe
			//			l1.Content = "Pelos vistos ele não sabe, é normal nem toda a gente tem as coisas tão bem definidas como nós programas.\n";
			//			l1.Speaker = grumpy;
			//			l2.Content = "Então vemo-nos na próxima vez que vieres.\n";
			//			l2.Speaker = happy;
			//			List<Line> exit4 = new List<Line> ();
			//			exit4.Add (l1);
			//			exit4.Add (l2);
			//			Topic exit4Topic = new Topic (exit4);
			//			topics.Add (exit4Topic);
			//
			//			//TimeOutDefault
			//			l1.Content = "Bom, eu acho que ele não quer falar connosco.\n";
			//			l1.Speaker = grumpy;
			//			l2.Content = "De certeza que se fartou da tua atitude.\n";
			//			l2.Speaker = happy;
			//			l3.Content = "Sim, porque toda a gente adora a tua personalidade.\n";
			//			l3.Speaker = grumpy;
			//			l4.Content = "Mas e agora o que fazemos?\n";
			//			l4.Speaker = happy;
			//			l5.Content = "Epá desliga isso, já que estás tão contente e vamos embora. \n";
			//			l5.Speaker = grumpy;
			//			//this case you need to keep the inputs at runtime from the previous interactions and set them again
			//			List<Line> timeout1 = new List<Line> ();
			//			timeout1.Add (l1);
			//			timeout1.Add (l2);
			//			timeout1.Add (l3);
			//			timeout1.Add (l4);
			//			timeout1.Add (l5);
			//			Topic timeTopic = new Topic (timeout1);
			//			topics.Add (timeTopic);
			//
			//			//not answering test
			//			l1.Content = "Bom, não queres falar sobre isso, ok.\n";
			//			l1.Speaker = happy;
			//			l2.Content = " Continuo a achar que um plano de estudo era uma boa ideia.\n";
			//			l2.Speaker = grumpy;
			//			inputs2 [0] = "Vamos fazer um plano de estudo!";
			//			inputs2 [1] = "Obrigado";
			//			List<Line> noAnswerTest = new List<Line> ();
			//			noAnswerTest.Add (l1);
			//			noAnswerTest.Add (l2);
			//			Topic noAnswTest = new Topic (noAnswerTest, inputs2);
			//			topics.Add (noAnswTest);
			//
			//			//Not answering Bad plan
			//			l1.Content = "Ok parece que ele quer insistir neste plano.\n";
			//			l1.Speaker = grumpy;
			//			l2.Content = "Temos de confiar nele\n";
			//			l2.Speaker = happy;
			//			List<Line> noAnswerPlan = new List<Line> ();
			//			noAnswerPlan.Add (l1);
			//			noAnswerPlan.Add (l2);
			//			Topic noAnswPlan = new Topic (noAnswerPlan);
			//			topics.Add (noAnswPlan);
		
			demoScene.changeTopic ("Hello");
		}

	}
}


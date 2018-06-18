using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EmotivectorDebugScript : MonoBehaviour
{
    [SerializeField] private EmotivectorViewDebugger _viewDebugger;

    public bool PopulateEmotivector;

    public List<float> Values;

    private void Start()
    {
        Emotivector emotivector;
//            IPredictor predictor = new MartinhoSimplePredictor();
//            IPredictor predictor = new MovingAveragePredictor();
//            IPredictor predictor = new WeightedMovingAveragePredictor();
//            IPredictor predictor = new ExponencialMovingAveragePredictor();
//            IPredictor predictor = new FirstDerivativeOnlyPredictor(new WeightedMovingAveragePredictor());
//            IPredictor predictor = new AdditiveFirstDerivativePredictor(new WeightedMovingAveragePredictor(),
//                new WeightedMovingAveragePredictor());
        IPredictor predictor = new AdditiveSecondDerivativePredictor(new WeightedMovingAveragePredictor(),
            new WeightedMovingAveragePredictor(), new WeightedMovingAveragePredictor());

        if (PopulateEmotivector)
        {
            emotivector = new Emotivector(predictor, Values);
        }
        else
        {
            emotivector = new Emotivector(predictor);
        }


        RunTutorPersonalityTest(emotivector);


        if (_viewDebugger)
        {
            _viewDebugger.SetEmotivector(emotivector);
        }
    }

    private static void RunTutorPersonalityTest(Emotivector emotivector)
    {
        Tutor joao = new Tutor("Joao");
        Tutor maria = new Tutor("Maria");
        joao.Personality = new ExpectancyPersonality(new Emotion[3, 6]
        {
            {
                /* Negative */
                new Emotion(EmotionEnum.Anger, .5f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Surprise, .2f),
                new Emotion(EmotionEnum.Anger, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Surprise, .8f)
            },
            {
                /* Neutral */
                new Emotion(EmotionEnum.Sadness, .3f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .6f),
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .8f)
            },
            {
                /* Positive */
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .2f),
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, 1f)
            }
        });
        maria.Personality = joao.Personality;

        var emotivectorAppraisal = new EmotivectorAppraisal();
        var updater = new ValuesCheckAffectiveUpdater {Emotivector = emotivector};
        emotivectorAppraisal.AddUpdater(updater);
        IAffectiveAppraisal appraisal = emotivectorAppraisal;
        Debug.Log(joao.Emotion);
        emotivector.AddValue(.65f);
        appraisal.ComputeUserEmotion(null, null);
        appraisal.ComputeTutorEmotion(null, null, joao);
        Debug.Log(joao.Emotion);
        emotivector.AddValue(.4f);
        appraisal.ComputeUserEmotion(null, null);
        appraisal.ComputeTutorEmotion(null, null, joao);
        Debug.Log(joao.Emotion);
    }

    public static void RunExpectancySortingTest()
    {
        List<Emotivector.Expectancy> expectancies = new List<Emotivector.Expectancy>
        {
            new Emotivector.Expectancy
            {
                change = Emotivector.Expectancy.Change.AsExpected,
                valence = Emotivector.Expectancy.Valence.Reward,
                salience = 0.04f
            },
            new Emotivector.Expectancy
            {
                change = Emotivector.Expectancy.Change.AsExpected,
                valence = Emotivector.Expectancy.Valence.Reward,
                salience = 0.02f
            },
            new Emotivector.Expectancy
            {
                change = Emotivector.Expectancy.Change.AsExpected,
                valence = Emotivector.Expectancy.Valence.Reward,
                salience = 0.07f
            }
        };

        string expectancyLog = string.Empty;
        foreach (Emotivector.Expectancy expectancy in expectancies)
        {
            expectancyLog += expectancy + "\n";
        }

        Debug.Log(expectancyLog);

        expectancies.Sort(MathUtils.FloatMaxToMinCompare);

        expectancyLog = string.Empty;
        foreach (Emotivector.Expectancy expectancy in expectancies)
        {
            expectancyLog += expectancy + "\n";
        }

        Debug.Log(expectancyLog);
    }
}
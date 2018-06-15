using System.Collections.Generic;
using UnityEngine;

public class EmotivectorDebugScript : MonoBehaviour
{
    [SerializeField] private EmotivectorViewDebugger _viewDebugger;

    public bool PopulateEmotivector;

    public List<float> Values;

    private void Start()
    {
        if (_viewDebugger)
        {
            Emotivector emotivector;
            if (PopulateEmotivector)
            {
                emotivector = new Emotivector(new MartinhoSimplePredictor(), Values);
            }
            else
            {
                emotivector = new Emotivector(new MartinhoSimplePredictor());
            }

            _viewDebugger.SetEmotivector(emotivector);
        }


        Tutor joao = new Tutor("Joao");
        Tutor maria = new Tutor("Maria");
        joao.Personality = new ExpectancyPersonality(new Emotion[3, 6]
        {
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
                /* Negative */
                new Emotion(EmotionEnum.Anger, .5f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Surprise, .2f),
                new Emotion(EmotionEnum.Anger, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Surprise, .8f)
            },
            {
                /* Positive */
                new Emotion(EmotionEnum.Sadness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, .2f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Neutral, 1f),
                new Emotion(EmotionEnum.Happiness, 1f)
            }
        });
        maria.Personality = joao.Personality;
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

        expectancies.Sort(EmotivectorAppraisal.FloatMaxToMinCompare);

        expectancyLog = string.Empty;
        foreach (Emotivector.Expectancy expectancy in expectancies)
        {
            expectancyLog += expectancy + "\n";
        }

        Debug.Log(expectancyLog);
    }
}
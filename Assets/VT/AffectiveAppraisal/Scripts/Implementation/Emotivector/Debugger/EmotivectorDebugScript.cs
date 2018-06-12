using System.Collections.Generic;
using UnityEngine;

public class EmotivectorDebugScript : MonoBehaviour
{
    [SerializeField] private EmotivectorViewDebugger _viewDebugger;

    public bool populateEmotivector;

    private void Start()
    {
        if (_viewDebugger)
        {
            Emotivector emotivector;
            if (populateEmotivector)
            {
                List<float> values = new List<float>
                {
                    0.5f,
                    0.5f,
                    0.5f,
                    0.4f,
                    0.3f,
                    0.5f,
                    0.7f,
                    0.6f
                };

                emotivector = new Emotivector(new MartinhoSimplePredictor(), values);
            }
            else
            {
                emotivector = new Emotivector(new MartinhoSimplePredictor());
            }

            _viewDebugger.SetEmotivector(emotivector);
        }
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
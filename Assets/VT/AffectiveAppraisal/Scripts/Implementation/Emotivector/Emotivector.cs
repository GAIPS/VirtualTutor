using System.Collections.Generic;
using System.Linq;


public class Emotivector
{
    public class Expectancy
    {
        public enum Valence
        {
            Punishment,
            Reward
        }

        public enum Change
        {
            WorseThanExpected,
            AsExpected,
            BetterThanExpected
        }

        public Valence valence;
        public Change change;
        public float salience;

        public override string ToString()
        {
            return "Emotivector -- Valence: " + valence + " ; Change: " + change + " ; Salience: " + salience + " .";
        }
    }

    private List<float> _values, _predictions;

    private IPredictor _predictor;
    
    public float Epsilon = 0.1f;

    public Emotivector(IPredictor predictor) : this(predictor, new List<float>())
    {
    }

    public Emotivector(IPredictor predictor, List<float> values)
    {
        _predictor = predictor;
        _values = new List<float>();
        _predictions = new List<float>();
        // Add each value at a time to allow for predictions to be made.
        foreach (float value in values)
        {
            AddValue(value);
            Predict();
        }
    }

    public Expectancy ComputeExpectancy()
    {
        Expectancy expectancy = new Expectancy
        {
            valence = Expectancy.Valence.Reward,
            change = Expectancy.Change.AsExpected,
            salience = 0
        };

        if (_values.Count <= 1 || _predictions.Count == 0)
        {
            return expectancy;
        }

        float prediction = _predictions.Last(),
            value = _values.Last(),
            lastValue = _values[_values.Count - 2];

        float expectedResult = prediction - lastValue;
        // if expectedResult < 0 then we expect punishment
        // if expectedResult >= 0 then we expect reward
        expectancy.valence = expectedResult < 0 ? Expectancy.Valence.Punishment : Expectancy.Valence.Reward;

        float sensedResult = value - lastValue;
        // if sensedResult < 0 then we sensed punishment
        // if sensedResult >= 0 then we sensed reward

        if (MathUtils.DeltaAbs(expectedResult, sensedResult) < Epsilon)
        {
            expectancy.change = Expectancy.Change.AsExpected;
        }
        else
        {
            expectancy.change = sensedResult < expectedResult
                ? Expectancy.Change.WorseThanExpected
                : Expectancy.Change.BetterThanExpected;
        }

        expectancy.salience = MathUtils.Exogenous(value, prediction);

        return expectancy;
    }

    public void Predict(float? desiredValue = null)
    {
        _predictions.Add(_predictor.Predict(_values, _predictions, desiredValue));
    }

    public void AddValue(float value)
    {
        _values.Add(value);
    }

    public List<float> GetValues()
    {
        return _values.ToList();
    }
    
    public List<float> GetPredictions()
    {
        return _predictions.ToList();
    }

    public void Clear()
    {
        _values.Clear();
        _predictions.Clear();
    }
}
using System.Collections.Generic;
using System.Linq;


public class Emotivector
{
    public struct Expectancy
    {
        public enum Valence
        {
            Punishment,
            Reward
        }

        public enum Change
        {
            AsExpected,
            WorseThanExpected,
            BetterThanExpected
        }

        public Valence valence;
        public Change change;
        public float salience;
    }

    private List<float> _values, _predictions;

    private IPredictor _predictor;

    public Emotivector(IPredictor predictor) : this(predictor, new List<float>(), new List<float>())
    {
    }

    public Emotivector(IPredictor predictor, List<float> values, List<float> predictions)
    {
        _predictor = predictor;
        _values = values;
        _predictions = predictions;
    }

    public Expectancy ComputeExpectancy()
    {
        Expectancy expectancy = new Expectancy
        {
            valence = Expectancy.Valence.Reward,
            change = Expectancy.Change.AsExpected,
            salience = 0
        };

        if (_values.Count == 0)
        {
            return expectancy;
        }

        if (_values.Count == 1 || _predictions.Count == 0)
        {
            // Predict the next value
            _predictions.Add(_predictor.Predict(_values, _predictions, null));
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

        const float range = 0.05f;
        if (MathUtils.DeltaAbs(expectedResult, sensedResult) < range)
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

        // Predict the next value
        _predictions.Add(_predictor.Predict(_values, _predictions, null));
        return expectancy;
    }

    public void AddValue(float value)
    {
        _values.Add(value);
    }

    public void Clear()
    {
        _values.Clear();
        _predictions.Clear();
    }
}
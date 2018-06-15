using System.Collections.Generic;

public class AdditiveFirstDerivativePredictor : IPredictor
{
    public IPredictor BasePredictor;
    public IPredictor DerivativePredictor;

    public AdditiveFirstDerivativePredictor(IPredictor basePredictor, IPredictor derivativePredictor)
    {
        BasePredictor = basePredictor;
        DerivativePredictor = derivativePredictor;
    }

    public float Predict(List<float> values, List<float> predictions, float? desiredValue = null)
    {
        float basePrediction = BasePredictor.Predict(values, predictions, desiredValue);

        if (values == null || values.Count < 2)
        {
            return basePrediction;
        }

        var firstDerivative = Differentiate(values);

        float derivativePrediction = DerivativePredictor.Predict(firstDerivative, new List<float>(), desiredValue);

        return basePrediction + derivativePrediction;
    }

    public static List<float> Differentiate(List<float> values)
    {
        List<float> firstDerivative = new List<float>();
        for (int i = 1; i < values.Count; i++)
        {
            firstDerivative.Add(values[i] - values[i - 1]);
        }

        return firstDerivative;
    }
}

public class FirstDerivativeOnlyPredictor : IPredictor
{
    public IPredictor DerivativePredictor;

    public FirstDerivativeOnlyPredictor(IPredictor derivativePredictor)
    {
        DerivativePredictor = derivativePredictor;
    }

    public float Predict(List<float> values, List<float> predictions, float? desiredValue = null)
    {
        if (values == null || values.Count == 0)
        {
            return .5f;
        }

        if (values.Count < 2)
        {
            return values[0];
        }

        List<float> firstDerivative = AdditiveFirstDerivativePredictor.Differentiate(values);

        float derivativePrediction = DerivativePredictor.Predict(firstDerivative, new List<float>(), desiredValue);

        return values[values.Count - 1] + derivativePrediction;
    }
}

public class AdditiveSecondDerivativePredictor : IPredictor
{
    public IPredictor BasePredictor;
    public IPredictor DerivativePredictor;
    public IPredictor SecondDerivativePredictor;

    public AdditiveSecondDerivativePredictor(IPredictor basePredictor, IPredictor derivativePredictor,
        IPredictor secondDerivativePredictor)
    {
        BasePredictor = basePredictor;
        DerivativePredictor = derivativePredictor;
        SecondDerivativePredictor = secondDerivativePredictor;
    }

    public float Predict(List<float> values, List<float> predictions, float? desiredValue = null)
    {
        float basePrediction = BasePredictor.Predict(values, predictions, desiredValue);

        if (values == null || values.Count < 2)
        {
            return basePrediction;
        }

        var firstDerivative = AdditiveFirstDerivativePredictor.Differentiate(values);

        float derivativePrediction = DerivativePredictor.Predict(firstDerivative, new List<float>(), desiredValue);

        if (firstDerivative.Count < 2)
        {
            return basePrediction + derivativePrediction;
        }

        var secondDerivative = AdditiveFirstDerivativePredictor.Differentiate(values);

        float secondDerivativePrediction =
            SecondDerivativePredictor.Predict(secondDerivative, new List<float>(), desiredValue);

        return basePrediction + derivativePrediction + secondDerivativePrediction;
    }
}
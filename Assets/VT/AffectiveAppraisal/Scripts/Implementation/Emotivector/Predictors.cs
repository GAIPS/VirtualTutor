using System;
using System.Collections.Generic;
using System.Linq;

public class MathUtils
{
    public static float Exogenous(float value, float expected)
    {
        return DeltaSquared(value, expected);
    }

    public static float Endogenous(float value, float expected, float searched)
    {
        float deltaS = DeltaSquared(value, searched);
        float deltaSExpected = DeltaSquared(expected, searched);

        return deltaSExpected - deltaS;
    }

    public static float DeltaSquared(float a, float b)
    {
        return (float) Math.Pow(a - b, 2);
    }

    public static float DeltaAbs(float a, float b)
    {
        return (float) Math.Abs(a - b);
    }
}

public interface IPredictor
{
    float Predict(List<float> values, List<float> predictions, float? desiredValue);
}

public class MartinhoSimplePredictor : IPredictor
{
    public float Predict(List<float> values, List<float> predictions, float? desiredValue)
    {
        float value;
        // if values are empty assume half value between 0 and 1 (0.5)
        if (values == null || values.Count == 0)
            value = 0.5f;
        else
            value = values.Last();

        float lastPrediction;
        // if there are no previous predictions assume the same value
        if (predictions == null || predictions.Count == 0)
            lastPrediction = value;
        else
            lastPrediction = predictions.Last();

        float a = MathUtils.Exogenous(value, lastPrediction);
        if (desiredValue.HasValue)
        {
            a *= (float) Math.Pow(MathUtils.Endogenous(value, lastPrediction, desiredValue.Value),
                2);
        }

        return lastPrediction * (1 - a) + value * a;
    }
}
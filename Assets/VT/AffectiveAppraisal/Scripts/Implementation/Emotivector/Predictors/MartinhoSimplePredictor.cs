using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

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

        float a = MathUtils.ExogenousScaled(value, lastPrediction);
//        float a = MathUtils.ExogenousInverted(value, lastPrediction);
        if (desiredValue.HasValue)
        {
            a += (float) Math.Pow(MathUtils.Endogenous(value, lastPrediction, desiredValue.Value),
                     2) * a;
        }

        return lastPrediction * (1 - a) + value * a;
    }
}
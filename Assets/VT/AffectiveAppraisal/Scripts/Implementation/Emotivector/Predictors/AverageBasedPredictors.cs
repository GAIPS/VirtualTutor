using System;
using System.Collections.Generic;

public class MovingAveragePredictor : IPredictor
{
    public int ValuesWindow { get; set; }

    public MovingAveragePredictor(int valuesWindow = 5)
    {
        ValuesWindow = valuesWindow;
    }

    public float Predict(List<float> values, List<float> predictions, float? desiredValue)
    {
        // if values are empty assume half value between 0 and 1 (0.5)
        if (values == null || values.Count == 0)
        {
            return 0.5f;
        }

        float average = 0;
        int size = ValuesWindow > values.Count ? values.Count : ValuesWindow;
        for (int i = 0; i < size; i++)
        {
            average += values[values.Count - 1 - i] / size;
        }

        return average;
    }
}

public class WeightedMovingAveragePredictor : IPredictor
{
    public float[] Weights { get; set; }

    public WeightedMovingAveragePredictor() : this(new float[5] {0.5f, .2f, .1f, .1f, .1f})
    {
    }

    public WeightedMovingAveragePredictor(float[] weights)
    {
        Weights = weights;
    }

    public float Predict(List<float> values, List<float> predictions, float? desiredValue)
    {
        // if values are empty assume half value between 0 and 1 (0.5)
        if (values == null || values.Count == 0)
        {
            return 0.5f;
        }

        float average = 0;
        int size = Weights.Length;
        float extra = 0;
        if (Weights.Length > values.Count)
        {
            size = values.Count;
            float diff = Weights.Length - values.Count;
            for (int i = 0; i < diff; i++)
            {
                extra += Weights[Weights.Length - 1 - i];
            }

            extra /= Weights.Length - diff;
        }

        for (int i = 0; i < size; i++)
        {
            average += values[values.Count - 1 - i] * (Weights[i] + extra);
        }

        return average;
    }
}

/// <summary>
/// Exponential moving average predictor.
/// </summary>
/// <remarks>
/// Because it is not looking at a window of values it makes heavy use of the predictions list.
/// </remarks>
public class ExponentialMovingAveragePredictor : IPredictor
{
    public float Predict(List<float> values, List<float> predictions, float? desiredValue)
    {
        float value;
        // if values are empty assume half value between 0 and 1 (0.5)
        if (values == null || values.Count == 0)
        {
            value = 0.5f;
            return value;
        }

        value = values[values.Count - 1];

        float lastPrediction;
        // if there are no previous predictions assume the same value
        if (predictions == null || predictions.Count == 0)
            lastPrediction = value;
        else
            lastPrediction = predictions[predictions.Count - 1];

        return (value - lastPrediction) * (2f / (values.Count + 1)) + lastPrediction;
    }
}
using System.Collections.Generic;

public interface IPredictor
{
    float Predict(List<float> values, List<float> predictions, float? desiredValue = null);
}
using System;
using System.Collections.Generic;
using System.Linq;

public class MathUtils
{
    public static float Exogenous(float value, float expected)
    {
        return DeltaSquared(value, expected);
    }
    
    public static float ExogenousScaled(float value, float expected)
    {
        return DeltaSquared(value, expected) * 10;
    }

    public static float ExogenousInverted(float value, float expected)
    {
        return 1 - DeltaSquared(value, expected);
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
    float Predict(List<float> values, List<float> predictions, float? desiredValue = null);
}
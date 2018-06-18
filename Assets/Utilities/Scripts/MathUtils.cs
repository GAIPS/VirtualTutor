using System;

namespace Utilities
{
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

        public static int FloatMinToMaxCompare(Emotivector.Expectancy x, Emotivector.Expectancy y)
        {
            var diff = x.salience - y.salience;
            if (Math.Abs(diff) < 0.0005f)
            {
                // shortcut, handles infinities
                return 0;
            }

            return diff < 0 ? -1 : 1;
        }

        public static int FloatMaxToMinCompare(Emotivector.Expectancy x, Emotivector.Expectancy y)
        {
            return FloatMinToMaxCompare(x, y) * -1;
        }

        public static float Normalize(float value, float oldMin, float oldMax, float min = 0, float max = 1)
        {
            return ((value - oldMin) / (oldMax - oldMin)) * (max - min) + min;
        }
    }
}
using System;
using System.Collections.Generic;
using Utilities;
using Expectancy = Emotivector.Expectancy;

public class EmotivectorAppraisal : IAffectiveAppraisal
{
    private List<Emotivector> _emotivectors;

    private Expectancy _strongestExpectancy;

    public EmotivectorAppraisal()
    {
        _emotivectors = new List<Emotivector>();
    }

    public void AddEmotivector(Emotivector emotivector)
    {
        _emotivectors.Add(emotivector);
    }

    public void ComputeUserEmotion(History history, User user)
    {
        // Read history and update Emotivectors
        // Add values to the emotivectors using emotivector.AddValue

        // Compute Expectancy for all the emotivectors
        List<Expectancy> expectancies = new List<Expectancy>();
        foreach (Emotivector emotivector in _emotivectors)
        {
            expectancies.Add(emotivector.ComputeExpectancy());
        }

        expectancies.Sort(FloatMinToMaxCompare);

        // Compute emotion using expectancy
        // Compute emotion for user
        // Save expectancy for tutors
        if (expectancies.Count > 0)
        {
            _strongestExpectancy = expectancies[0];
        }

        // Predict new values
        foreach (Emotivector emotivector in _emotivectors)
        {
            emotivector.Predict();
        }
    }

    public static int FloatMinToMaxCompare(Expectancy x, Expectancy y)
    {
        var diff = x.salience - y.salience;
        if (Math.Abs(diff) < 0.0005f)
        {
            // shortcut, handles infinities
            return 0;
        }

        return diff < 0 ? -1 : 1;
    }

    public static int FloatMaxToMinCompare(Expectancy x, Expectancy y)
    {
        return FloatMinToMaxCompare(x, y) * -1;
    }

    public void ComputeTutorEmotion(History history, User user, Tutor tutor)
    {
        if (_strongestExpectancy == null) return;
        // TODO Implement Personality Tables in User/Tutors
    }
}
using System.Collections.Generic;
using Utilities;
using Expectancy = Emotivector.Expectancy;

public class EmotivectorAppraisal : IAffectiveAppraisal
{

    private List<AffectiveUpdater> _updaters;

    private Expectancy _strongestExpectancy;

    public EmotivectorAppraisal()
    {
        _updaters = new List<AffectiveUpdater>();
    }

    public void AddUpdater(AffectiveUpdater affectiveUpdater)
    {
        _updaters.Add(affectiveUpdater);
    }

    public void ComputeUserEmotion(History history, User user)
    {
        List<Emotivector> emotivectors = new List<Emotivector>();
        // Read history and update Emotivectors
        // Add values to the emotivectors using emotivector.AddValue
        foreach (var updater in _updaters)
        {
            var emotivector = updater.Update(history, user);
            if (emotivector != null)
            {
                emotivectors.Add(emotivector);
            }
        }

        // Compute Expectancy for all the emotivectors
        List<Expectancy> expectancies = new List<Expectancy>();
        foreach (Emotivector emotivector in emotivectors)
        {
            expectancies.Add(emotivector.ComputeExpectancy());
        }

        expectancies.Sort(MathUtils.FloatMaxToMinCompare);

        // Compute emotion using expectancy
        // Compute emotion for user
        // Save expectancy for tutors
        _strongestExpectancy = null;
        if (expectancies.Count > 0)
        {
            _strongestExpectancy = expectancies[0];
            history.Set("Expectancy", _strongestExpectancy);
        }

        // Predict new values
        foreach (Emotivector emotivector in emotivectors)
        {
            emotivector.Predict();
        }
    }

    public void ComputeTutorEmotion(History history, User user, Tutor tutor)
    {
        if (_strongestExpectancy == null) return;

        ExpectancyPersonality.EmotionValence valence;
        switch (tutor.Emotion.Name)
        {
            case EmotionEnum.Happiness:
            case EmotionEnum.Surprise:
                valence = ExpectancyPersonality.EmotionValence.Positive;
                break;
            case EmotionEnum.Anger:
            case EmotionEnum.Disgust:
            case EmotionEnum.Fear:
            case EmotionEnum.Sadness:
                valence = ExpectancyPersonality.EmotionValence.Negative;
                break;
            case EmotionEnum.Neutral:
            default:
                valence = ExpectancyPersonality.EmotionValence.Neutral;
                break;
        }

        tutor.Emotion = tutor.Personality.Get(valence,
            _strongestExpectancy.valence, _strongestExpectancy.change);
    }
}
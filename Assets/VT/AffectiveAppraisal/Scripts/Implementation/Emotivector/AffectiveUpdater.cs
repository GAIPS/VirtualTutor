using System.Collections.Generic;
using Utilities;

public abstract class AffectiveUpdater
{
    protected Emotivector _emotivector;

    public virtual Emotivector Emotivector
    {
        get { return _emotivector; }
        set { _emotivector = value; }
    }

    /// <summary>
    /// Updates emotivector given a history and the user info.
    /// </summary>
    /// <param name="history">History to read from</param>
    /// <param name="user">Present user</param>
    /// <returns>Emotivector updated with new information. Null is returned if no new information is given.</returns>
    public abstract Emotivector Update(History history, User user);
}

public class SimpleAffectiveUpdater : AffectiveUpdater
{
    public override Emotivector Update(History history, User user)
    {
        return Emotivector;
    }
}

public class ValuesCheckAffectiveUpdater : AffectiveUpdater
{
    private int _valuesSize = -1;

    public override Emotivector Emotivector
    {
        get { return _emotivector; }
        set
        {
            _emotivector = value;
            if (_emotivector != null)
            {
                _valuesSize = _emotivector.GetValues().Count;
            }
            else
            {
                _valuesSize = -1;
            }
        }
    }

    public override Emotivector Update(History history, User user)
    {
        if (Emotivector == null) return null;

        if (_valuesSize == -1)
        {
            _valuesSize = Emotivector.GetValues().Count;
            return Emotivector;
        }

        return _valuesSize != Emotivector.GetValues().Count ? Emotivector : null;
    }
}

public class GradesAffectiveUpdater : AffectiveUpdater
{
    private int _gradesCount = -1;

    public override Emotivector Update(History history, User user)
    {
        if (Emotivector == null || history == null) return null;

        List<float> grades = history.Get<List<float>>("grades");
        if (grades == null) return null;

        if (_gradesCount < grades.Count)
        {
            if (_gradesCount == -1)
            {
                _gradesCount = 0;
            }

            int i;
            for (i = _gradesCount; i < grades.Count - 1; i++)
            {
                Emotivector.AddValue(MathUtils.Normalize(grades[i], 0f, 20f));
                Emotivector.Predict();
            }

            // Don't predict the last addition
            Emotivector.AddValue(MathUtils.Normalize(grades[i], 0f, 20f));
            _gradesCount = grades.Count;
            
            DebugLog.Log("Found new Grades!");

            return Emotivector;
        }

        return null;
    }
}

public class StudyHoursAffectiveUpdater : AffectiveUpdater
{
    private int _hoursCount = -1;

    public override Emotivector Update(History history, User user)
    {
        if (Emotivector == null || history == null) return null;

        List<float> hours = history.Get<List<float>>("hours");
        if (hours == null) return null;

        if (_hoursCount < hours.Count)
        {
            if (_hoursCount == -1)
            {
                _hoursCount = 0;
            }

            int i;
            for (i = _hoursCount; i < hours.Count - 1; i++)
            {
                Emotivector.AddValue(MathUtils.Normalize(hours[i], 0f, 16f));
                Emotivector.Predict();
            }

            // Don't predict the last addition
            Emotivector.AddValue(MathUtils.Normalize(hours[i], 0f, 16f));
            _hoursCount = hours.Count;

            return Emotivector;
        }

        return null;
    }
}
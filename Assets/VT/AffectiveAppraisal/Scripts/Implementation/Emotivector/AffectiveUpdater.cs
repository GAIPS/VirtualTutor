using System.Collections.Generic;
using SimpleJSON;
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

public class NamedArrayAffectiveUpdater : AffectiveUpdater
{
    private int _count;

    public string Name { get; set; }

    public float Min { get; set; }

    public float Max { get; set; }

    public NamedArrayAffectiveUpdater(string name, float min, float max)
    {
        Name = name;
        Min = min;
        Max = max;
    }

    public override Emotivector Update(History history, User user)
    {
        if (Emotivector == null) return null;

        var state = PersistentDataStorage.Instance.GetState();
        JSONArray values = state[Name].AsArray;

        if (_count < values.Count)
        {
            int i;
            for (i = _count; i < values.Count - 1; i++)
            {
                Emotivector.AddValue(MathUtils.Normalize(values[i], Min, Max));
                Emotivector.Predict();
            }

            // Don't predict the last addition
            Emotivector.AddValue(MathUtils.Normalize(values[i], Min, Max));
            _count = values.Count;

            return Emotivector;
        }

        return null;
    }
}

public class NamedDatedArrayAffectiveUpdater : AffectiveUpdater
{
    private int _count;

    public string Name { get; set; }

    public float Min { get; set; }

    public float Max { get; set; }

    public NamedDatedArrayAffectiveUpdater(string name, float min, float max)
    {
        Name = name;
        Min = min;
        Max = max;
    }

    public override Emotivector Update(History history, User user)
    {
        if (Emotivector == null) return null;

        var values = GetListFromValues();

        if (_count < values.Count)
        {
            int i;
            for (i = _count; i < values.Count - 1; i++)
            {
                Emotivector.AddValue(MathUtils.Normalize(values[i], Min, Max));
                Emotivector.Predict();
            }

            // Don't predict the last addition
            Emotivector.AddValue(MathUtils.Normalize(values[i], Min, Max));
            _count = values.Count;

            return Emotivector;
        }

        return null;
    }

    private List<float> GetListFromValues()
    {
        var state = PersistentDataStorage.Instance.GetState();
        var jsonValues = state["DailyTask"].AsObject["InputSubjective"].AsObject;
        var values = new List<float>();

        for (int i = 0; i < jsonValues.Count; i++)
        {
            JSONNode node = jsonValues[i];
            if (node[Name] != null)
            {
                values.Add(node[Name]);
            }
        }

        return values;
    }
}
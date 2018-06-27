using SimpleJSON;

public struct ExpectancyPersonality
{
    public enum EmotionValence
    {
        Negative,
        Neutral,
        Positive
    }

    private Emotion[,] _table;

    public ExpectancyPersonality(string jsonString) : this(JSON.Parse(jsonString))
    {
    }

    public ExpectancyPersonality(SimpleJSON.JSONNode json)
    {
        _table = new Emotion[3, 6];
        for (int i = 0; i < json.Count; i++)
        {
            for (int j = 0; j < json[i].Count; j++)
            {
                EmotionEnum emotion;
                if (EnumUtils.TryParse(json[i][j]["Name"], out emotion))
                {
                    _table[i, j] = new Emotion(emotion, json[i][j]["Intensity"].AsFloat);
                }
            }
        }
    }

    public ExpectancyPersonality(Emotion[,] table)
    {
        _table = table;
    }

    public void Set(EmotionValence emotionValence, Emotivector.Expectancy.Valence valence,
        Emotivector.Expectancy.Change change,
        Emotion emotion)
    {
        _table[(int) emotionValence, (int) valence * 3 + (int) change] = emotion;
    }

    public Emotion Get(EmotionValence emotionValence, Emotivector.Expectancy.Valence valence,
        Emotivector.Expectancy.Change change)
    {
        return _table[(int) emotionValence, (int) valence * 3 + (int) change];
    }
}
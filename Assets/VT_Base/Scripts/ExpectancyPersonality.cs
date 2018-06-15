
public struct ExpectancyPersonality
{
    public enum EmotionValence
    {
        Neutral,
        Negative,
        Positive
    }

    private Emotion[,] _table;
        
    public ExpectancyPersonality(Emotion[,] table)
    {
        _table = table;
    }

    public void Set(EmotionValence emotionValence, Emotivector.Expectancy.Valence valence, Emotivector.Expectancy.Change change,
        Emotion emotion)
    {
        _table[(int) emotionValence, (int) valence * 3 + (int) change] = emotion;
    }

    public Emotion Get(EmotionValence emotionValence, Emotivector.Expectancy.Valence valence, Emotivector.Expectancy.Change change)
    {
        return _table[(int) emotionValence, (int) valence * 3 + (int) change];
    }
}
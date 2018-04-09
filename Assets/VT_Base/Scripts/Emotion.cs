public enum EmotionEnum
{
    Neutral,
    Happiness,
    Sadness,
    Anger,
    Fear,
    Disgust,
    Surprise
}

[System.Serializable]
public class Emotion
{
    public EmotionEnum Name { get; set; }
    public float Intensity { get; set; }

    public Emotion() { }
    public Emotion(EmotionEnum name)
    {
        Name = name;
    }
    public Emotion(EmotionEnum name, float intensity)
    {
        Name = name;
        Intensity = intensity;
    }
}

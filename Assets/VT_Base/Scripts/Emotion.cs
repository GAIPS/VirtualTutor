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
public struct Emotion
{
    public EmotionEnum Name;
    public float Intensity;

    public Emotion(EmotionEnum name, float intensity = 0.5f)
    {
        Name = name;
        Intensity = intensity;
    }
}

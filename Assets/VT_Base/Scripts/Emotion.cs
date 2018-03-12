[System.Serializable]
public class Emotion : IState
{
    public string Name { get; set; }
    public string Intensity { get; set; }

    public Emotion() { }
    public Emotion(string name)
    {
        Name = name;
    }
    public Emotion(string name, string intensity)
    {
        Name = name;
        Intensity = intensity;
    }

    string IState.Param1
    {
        get { return this.Intensity; }
        set { this.Intensity = value; }
    }
}

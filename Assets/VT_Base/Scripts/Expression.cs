[System.Serializable]
public class Expression : IState
{
    public string Name { get; set; }
    public string Intensity { get; set; }

    public Expression() { }
    public Expression(string name, string intensity)
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

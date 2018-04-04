[System.Serializable]
public class HeadAction : IState
{
    public string Name { get; set; }
    public string Mode { get; set; }

    public HeadAction() { }
    public HeadAction(string name, string mode)
    {
        Name = name;
        Mode = mode;
    }

    string IState.Param1
    {
        get { return this.Mode; }
        set { this.Mode = value; }
    }
}

public class CheckBoxPoint : Checkpoint
{
    private bool _done;

    public CheckBoxPoint(string name, string date, int effort, int importance, bool done)
    {
        this.Name = name;
        this.Date = date;
        this.Effort = effort;
        this.Importance = importance;
        this._done = done;
    }

    public bool Done
    {
        get { return _done; }
        set { _done = value; }
    }
}
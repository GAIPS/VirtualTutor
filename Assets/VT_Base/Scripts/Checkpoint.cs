public abstract class Checkpoint
{
    private string _name;
    private string _date;
    private int _effort;
    private int _importance;

    public Checkpoint()
    {
    }

    public string Date
    {
        get { return this._date; }
        set { _date = value; }
    }


    public string Name
    {
        get { return this._name; }
        set { _name = value; }
    }

    public int Effort
    {
        get { return this._effort; }
        set { _effort = value; }
    }

    public int Importance
    {
        get { return this._importance; }
        set { _importance = value; }
    }
}
public class State
{
    public StateEnum Name { get; set; }

    public State() { }
    public State(StateEnum name)
    {
        Name = name;
    }
}
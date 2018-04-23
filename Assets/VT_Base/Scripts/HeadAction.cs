public enum MovementEnum
{
    Talk,
    Nod,
    Gazeat,
    Gazeback
}

[System.Serializable]
public class Movement
{
    public MovementEnum Name { get; set; }
    public State State { get; set; }
    public Property Property { get; set; }

    public Movement() { }
    public Movement(MovementEnum name)
    {
        Name = name;
    }
    public Movement(MovementEnum name, State state) : this(name)
    {
        State = state;
    }
    public Movement(MovementEnum name, Property property) : this(name)
    {
        Property = property;
    }
}

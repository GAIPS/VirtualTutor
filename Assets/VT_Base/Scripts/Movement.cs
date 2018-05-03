public enum MovementEnum
{
    Talk,
    Nod,
    Gazeat,
    Gazeback
}

public enum PropertyEnum
{
    Speed,
    Frequency
}

public enum StateEnum
{
    Start,
    End
}

public enum TargetEnum
{
    Partner,
    User
}

public interface IMovement
{
    MovementEnum Name { get; set; }
}

public class MovementWithState : IMovement
{
    public MovementEnum Name { get; set; }
    public StateEnum State { get; set; }

    public MovementWithState(StateEnum state)
    {
        State = state;
    }

    public MovementWithState(MovementEnum name, StateEnum state) : this (state)
    {
        Name = name;
    }
}

public class MovementWithProperty : IMovement
{
    public MovementEnum Name { get; set; }
    public PropertyEnum Property { get; set; }
    public float Value { get; set; }

    public MovementWithProperty(PropertyEnum property, float value)
    {
        Property = property;
        Value = value;
    }

    public MovementWithProperty(MovementEnum name, PropertyEnum property, float value) : this(property, value)
    {
        Name = name;
    }
}

public class MovementWithTarget : IMovement
{
    public MovementEnum Name { get; set; }
    public TargetEnum Target { get; set; }

    public MovementWithTarget(MovementEnum name, TargetEnum target)
    {
        Name = name;
        Target = target;
    }
}
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

    public MovementWithState(MovementEnum name, StateEnum state)
    {
        Name = name;
        State = state;
    }
}

public class MovementWithProperty : IMovement
{
    public MovementEnum Name { get; set; }
    public PropertyEnum Property { get; set; }
    public float Value { get; set; }

    public MovementWithProperty(MovementEnum name, PropertyEnum property, float value)
    {
        Name = name;
        Property = property;
        Value = value;
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
public enum PropertyEnum
{
    Speed,
    Frequency
}

public class Property
{
    public PropertyEnum Name { get; set; }
    public float Value { get; set; }

    public Property() { }
    public Property(PropertyEnum name, float value)
    {
        Name = name;
        Value = value;
    }
}
using System;
using Utilities;

public class Checkpoint
{
    public enum CType
    {
        Evaluation,
        Checkbox
    }

    private float _effort;
    private float _importance;

    public Checkpoint()
    {
    }
    
    public CType Type { get; set; }

    public string Name { get; set; }

    public DateTime Date { get; set; }

    public float Effort
    {
        get { return _effort; }
        set { _effort = value.Clamp(0, 1); }
    }

    public float Importance
    {
        get { return _importance; }
        set { _importance = value.Clamp(0, 1); }
    }
    
    // Checkbox
    
    public bool CheckboxDone { get; set; }
    
    // Evaluation
    
    public float? EvaluationScore { get; set; }
}
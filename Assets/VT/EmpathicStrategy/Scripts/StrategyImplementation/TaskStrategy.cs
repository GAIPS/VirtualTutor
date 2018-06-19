using System;
using System.Collections.Generic;
using Yarn;

public class TaskStrategy : IEmpathicStrategy
{
    public bool Completed { get; set; }
    public string Name { get; set; }

    private string _nodeName;

    public string NodeName
    {
        get { return _nodeName; }
        set
        {
            _nodeName = value;
            UpdateComplete();
        }
    }

    public DateTime BeginDate;
    public List<TaskStrategy> DependsOn = new List<TaskStrategy>();

    public VariableStorage VariableStorage;

    public ICollection<Intention> GetIntentions()
    {
        return new List<Intention>(new[] {new Intention(NodeName)});
    }

    public bool IsValid()
    {
        foreach (var taskStrategy in DependsOn)
        {
            if (!taskStrategy.Completed)
            {
                return false;
            }
        }

        UpdateComplete();

        return !Completed && BeginDate < DateTime.Now;
    }

    private void UpdateComplete()
    {
        // TODO Hack - Checks if it is complete.
        if (VariableStorage != null)
        {
            Value value = VariableStorage.GetValue("$" + NodeName + "Complete");
            if (value != null && value.AsBool) Completed = true;
        }
    }
}
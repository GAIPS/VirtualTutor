﻿using System;
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

    public virtual bool IsValid()
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
        if (VariableStorage == null) return;

        string name = "$" + NodeName + "Complete";
        Value value = VariableStorage.GetValue(name);

        bool completed = false;
        // if variable not found try to load it
        if (value != null && value.type != Value.Type.Null)
        {
            completed = value.AsBool;
        }

        Completed = completed;
    }
}

// TODO This class is a huge HACK!
public class OnceADayTaskStrategy : TaskStrategy
{
    public override bool IsValid()
    {
        if (base.IsValid())
        {
            var state = PersistentDataStorage.Instance.GetState();
            return state[Name].AsObject[DateTime.Now.ToShortDateString()] == null;
        }

        return false;
    }
}
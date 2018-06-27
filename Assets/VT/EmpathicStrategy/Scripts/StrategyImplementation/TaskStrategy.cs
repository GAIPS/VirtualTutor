using System;
using System.Collections.Generic;

public class TaskStrategy : IEmpathicStrategy
{
    public IDataStorage DataStorage { get; set; }
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
        string name = "$" + NodeName + "Complete";

        if (DataStorage == null) return;
        var state = DataStorage.GetState();
        var stateObj = state["Yarn"].AsObject[name];

        bool completed = false;
        if (stateObj != null)
        {
            completed = stateObj;
        }

        Completed = completed;
    }
}

public class OnceADayTaskStrategy : TaskStrategy
{
    public override bool IsValid()
    {
        if (base.IsValid())
        {
            if (DataStorage == null) return false;
            var state = DataStorage.GetState();
            return state["DailyTask"].AsObject[Name].AsObject[DateTime.Now.ToShortDateString()] == null;
        }

        return false;
    }
}
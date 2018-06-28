using System;
using System.Collections.Generic;
using System.Globalization;
using SimpleJSON;

public class TaskStrategy : IEmpathicStrategy
{
    private IDataStorage _dataStorage;

    public IDataStorage DataStorage
    {
        get { return _dataStorage; }
        set
        {
            _dataStorage = value;
            UpdateComplete();
        }
    }

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
            // TODO this is hack kek
            return state["DailyTask"].AsObject[Name].AsObject[DateTime.Now.ToShortDateString()] == null;
        }

        return false;
    }
}

public class TaskFactory
{
    private TaskFactory()
    {
    }

    public static List<TaskStrategy> FromJson(string json)
    {
        return FromJson(JSON.Parse(json));
    }

    public static List<TaskStrategy> FromJson(JSONNode json)
    {
        var strategies = new List<TaskStrategy>();

        foreach (var tasksPair in json["Tasks"].AsArray)
        {
            var taskJson = tasksPair.Value.AsObject;
            TaskStrategy task = new TaskStrategy();
            ParseTask(taskJson, ref task, strategies);
            strategies.Add(task);
        }

        foreach (var tasksPair in json["OnceADaysTasks"].AsArray)
        {
            var taskJson = tasksPair.Value.AsObject;
            TaskStrategy task = new OnceADayTaskStrategy();
            ParseTask(taskJson, ref task, strategies);
            strategies.Add(task);
        }

        return strategies;
    }

    private static void ParseTask(JSONNode taskJson, ref TaskStrategy task, List<TaskStrategy> strategies)
    {
        task.Name = taskJson["Name"];
        task.NodeName = taskJson["NodeName"];

        if (taskJson["BeginDate"] == null)
        {
            task.BeginDate = DateTime.Today;
        }
        else
        {
            string dateString = taskJson["BeginDate"];
            var culture = CultureInfo.CreateSpecificCulture("pt-PT");
            var styles = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal;
            DateTime date;
            if (DateTime.TryParse(dateString, culture, styles, out date))
            {
                task.BeginDate = date;
            }
        }

        if (taskJson["DependsOn"] != null)
        {
            for (int i = 0; i < taskJson["DependsOn"].Count; i++)
            {
                foreach (var strategy in strategies)
                {
                    if (string.Equals(strategy.Name, taskJson["DependsOn"][i]))
                    {
                        task.DependsOn.Add(strategy);
                    }
                }
            }
        }
    }
}
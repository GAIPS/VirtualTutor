using System;
using System.Collections.Generic;

[Serializable]
public class History
{
    private Dictionary<string, object> log = new Dictionary<string, object>();

    public object Get(string name)
    {
        return !log.ContainsKey(name) ? null : log[name];
    }
    public T Get<T>(string name) where T : class
    {
        if (!log.ContainsKey(name)) return null;
        return log[name] as T;
    }

    public void Set(string name, object obj)
    {
        log[name] = obj;
    }
}
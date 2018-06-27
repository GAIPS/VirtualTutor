using System.Collections.Generic;
using SimpleJSON;
using Yarn;

public class PersistentVariableStorage : BaseVariableStorage
{
    private readonly string _storageSectionName;
    private readonly IDataStorage _storage;

    public PersistentVariableStorage(IDataStorage storage, string storageSectionName = "Yarn")
    {
        _storageSectionName = storageSectionName;
        _storage = storage;
    }

    public override void SetValue(string variableName, Value value)
    {
        if (_storage == null) return;
        var state = _storage.GetState();
        if (state[_storageSectionName] == null) state[_storageSectionName] = new JSONObject();
        switch (value.type)
        {
            case Value.Type.Bool:
                state[_storageSectionName][variableName] = value.AsBool;
                break;
            case Value.Type.Number:
                state[_storageSectionName][variableName] = value.AsNumber;
                break;
            case Value.Type.String:
                state[_storageSectionName][variableName] = value.AsString;
                break;
        }

        _storage.SaveState();
    }

    public override Value GetValue(string variableName)
    {
        if (_storage == null) return null;
        var state = _storage.GetState();

        Value variable = Value.NULL;
        if (state[_storageSectionName].AsObject[variableName] != null)
        {
            var value = state[_storageSectionName][variableName];
            if (value.IsBoolean)
            {
                variable = new Value(value.AsBool);
            }
            else if (value.IsNumber)
            {
                variable = new Value(value.AsFloat);
            }
            else if (value.IsString)
            {
                variable = new Value(value.Value);
            }
        }

        return variable;
    }

    public override void Clear()
    {
        if (_storage == null) return;
        var state = _storage.GetState();
        if (state[_storageSectionName] != null) state[_storageSectionName] = new JSONObject();
    }
}
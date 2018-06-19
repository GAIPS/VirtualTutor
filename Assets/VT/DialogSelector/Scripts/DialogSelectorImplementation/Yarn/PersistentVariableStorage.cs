using System.Collections.Generic;
using SimpleJSON;
using Yarn;

public class PersistentVariableStorage : BaseVariableStorage
{
    private Dictionary<string, Value> variables = new Dictionary<string, Value>();

    public override void SetValue(string variableName, Value value)
    {
        this.variables[variableName] = value;

        var state = PersistentDataStorage.Instance.GetState();
        if(state["Yarn"] == null) state["Yarn"] = new JSONObject();
        switch (value.type)
        {
            case Value.Type.Bool:
                state["Yarn"][variableName] = value.AsBool;
                break;
            case Value.Type.Number:
                state["Yarn"][variableName] = value.AsNumber;
                break;
            case Value.Type.String:
                state["Yarn"][variableName] = value.AsString;
                break;
        }

        PersistentDataStorage.Instance.SaveState();
    }

    public override Value GetValue(string variableName)
    {
        Value variable = Value.NULL;
        if (this.variables.ContainsKey(variableName))
            variable = this.variables[variableName];

        var state = PersistentDataStorage.Instance.GetState();
        if(state["Yarn"] == null) state["Yarn"] = new JSONObject();
        if (state["Yarn"][variableName] != null)
        {
            var value = state["Yarn"][variableName];
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

            if (!Equals(variable, Value.NULL))
            {
                this.variables[variableName] = variable;
            }
        }

        return variable;
    }

    public override void Clear()
    {
        this.variables.Clear();
    }
}
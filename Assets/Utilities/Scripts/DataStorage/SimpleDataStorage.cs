using SimpleJSON;

public class SimpleDataStorage : IDataStorage
{
    private JSONNode _state = new JSONObject();

    public JSONNode GetState()
    {
        return _state;
    }

    public void ResetState()
    {
        _state = new JSONObject();
    }

    public void SaveState()
    {
    }

    public JSONNode LoadState()
    {
        return GetState();
    }
}
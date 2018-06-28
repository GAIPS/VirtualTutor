using SimpleJSON;
using UnityEngine;

public class UnityDataStorage : IDataStorage
{
    private const string Id = "JSONState";

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
        PlayerPrefs.SetString(Id, _state.ToString());
    }

    public JSONNode LoadState()
    {
        if (PlayerPrefs.HasKey(Id))
        {
            _state = JSON.Parse(PlayerPrefs.GetString(Id));
        }

        return GetState();
    }
}
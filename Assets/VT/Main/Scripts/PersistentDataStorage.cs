using SimpleJSON;
using UnityEngine;

public class PersistentDataStorage
{
    private static PersistentDataStorage _instance;

    public static PersistentDataStorage Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PersistentDataStorage();
                _instance.LoadState();
            }

            return _instance;
        }
    }

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
        
        Utilities.DebugLog.Log(_state.ToString());
        // TODO Save to Website
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
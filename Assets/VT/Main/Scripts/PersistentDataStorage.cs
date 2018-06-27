using System;
using System.Collections.Generic;
using System.Net;
using SimpleJSON;
using UnityEngine;
using Utilities;

public class PersistentDataStorage : IDataStorage
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
    
    public bool StoreDataOnline;

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

        DebugLog.Log(_state.ToString());

        if (!StoreDataOnline) return;
        
        if (_state["UserID"] == null) return;
        
        int id = _state["UserID"];
        string file = _state.ToString();
        string filename = "user" + id + ".json";
        string urlAddress = "http://web.tecnico.ulisboa.pt/ist173960/VTUploadFiles/FileReceiver.php?filename=" +
                            filename + "&file=" +
                            file;

        try
        {
            using (WebClient client = new WebClient())
            {
                // this string contains the webpage's source
                string pagesource = client.DownloadString(urlAddress);
                Debug.Log("Uploaded File Successfully: " + pagesource);
            }
        }
        catch (WebException e)
        {
            DebugLog.Warn(e.Message);
        }
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
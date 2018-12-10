
using System.Net;
using SimpleJSON;
using Utilities;

public class SigmaDataStorage : IDataStorage
{
    private readonly IDataStorage _dataStorage;
    public bool StoreDataOnline;

    public SigmaDataStorage(IDataStorage dataStorage)
    {
        _dataStorage = dataStorage;
    }

    public JSONNode GetState()
    {
        return _dataStorage.GetState();
    }

    public void ResetState()
    {
        _dataStorage.ResetState();
    }

    public void SaveState()
    {
        _dataStorage.SaveState();

        if (!StoreDataOnline) return;

        var state = _dataStorage.GetState();
        if (state["UserID"] == null) return;

        // TODO Refractor code
        string id = state["UserID"];
        string file = state.ToString();
        string filename = "user-" + id + ".json";
        string urlAddress = "http://web.tecnico.ulisboa.pt/ist173960/VTUploadFiles/FileReceiver.php?filename=" +
                            filename + "&file=" +
                            file;

        try
        {
            using (WebClient client = new WebClient())
            {
                // this string contains the webpage's source
                string pagesource = client.DownloadString(urlAddress);
                DebugLog.Log("Uploaded File Successfully: " + pagesource);
            }
        }
        catch (WebException e)
        {
            DebugLog.Warn(e.Message);
        }
    }

    public JSONNode LoadState()
    {
        return _dataStorage.LoadState();
    }
}
using SimpleJSON;

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

    private SigmaDataStorage _dataStorage;

    public bool StoreDataOnline
    {
        get { return _dataStorage.StoreDataOnline; }
        set { _dataStorage.StoreDataOnline = value; }
    }

    private PersistentDataStorage()
    {
        _dataStorage = new SigmaDataStorage(new UnityDataStorage());
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
    }

    public JSONNode LoadState()
    {
        return _dataStorage.LoadState();
    }
}
using SimpleJSON;

public interface IDataStorage
{
    JSONNode GetState();
    void SaveState();
    JSONNode LoadState();
    void ResetState();
}
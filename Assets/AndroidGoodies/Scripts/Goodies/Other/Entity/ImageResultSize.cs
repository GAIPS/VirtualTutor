#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
    /// <summary>
    /// The size of the resulting image. Pick smaller value to save memory.
    /// </summary>
    public enum ImageResultSize
    {
        Original = 0,
        Max256 = 256,
        Max512 = 512,
        Max1024 = 1024,
        Max2048 = 2048
    }
}
#endif
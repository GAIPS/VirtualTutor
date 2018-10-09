using UnityEditor;
using UnityEngine;
using bplib = BuildProcessLib;

public class MainBuildProcess : MonoBehaviour
{
    private static string[] mainLevels = {"Assets/VT/Main/Scenes/VT_Main.unity"};
    private static readonly string Path = bplib.GetProjectPath() + "/Build/Main/";
    private const string ZipName = "Virtual Tutoring";

    [MenuItem("VT Tools/Main Build Tools/Build All", false, 0)]
    public static bool BuildGame()
    {
        if (!BuildGameForAndroid()) return false;
        if (!BuildGameForWindows()) return false;
        if (!BuildGameForLinux()) return false;
        if (!BuildGameForMac()) return false;

        Debug.Log("Completed all builds");
        EditorUtility.RevealInFinder(Path);
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Android", false, 1)]
    public static bool BuildGameForAndroid()
    {
        if (!bplib.BuildGameForPlatform(mainLevels, Path, string.Empty, "VirtualTutoring", BuildTarget.Android))
        {
            Debug.LogError("Failed to build to Android.");
            return false;
        }

        Debug.Log("Completed Android build to " + Path);
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Windows", false, 2)]
    public static bool BuildGameForWindows()
    {
        if (!bplib.BuildGameForPlatform(mainLevels, Path, string.Empty, "VirtualTutoring",
            BuildTarget.StandaloneWindows64))
        {
            Debug.LogError("Failed to build to Windows.");
            return false;
        }

        Debug.Log("Completed Windows build to " + Path);
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Linux", false, 3)]
    public static bool BuildGameForLinux()
    {
        if (!bplib.BuildGameForPlatform(mainLevels, Path, string.Empty, "VirtualTutoring",
            BuildTarget.StandaloneLinux64))
        {
            Debug.LogError("Failed to build to Linux.");
            return false;
        }

        Debug.Log("Completed Linux build to " + Path);
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Mac", false, 4)]
    public static bool BuildGameForMac()
    {
        if (!bplib.BuildGameForPlatform(mainLevels, Path, string.Empty, "VirtualTutoring",
            BuildTarget.StandaloneOSXIntel64))
        {
            Debug.LogError("Failed to build to Mac.");
            return false;
        }

        Debug.Log("Completed Mac build to " + Path);
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Zip All Builds", false, 20)]
    public static void ZipAllBuilds()
    {
        if (!EditorUtility.DisplayDialog("Zipping All Builds",
            "Zipping process is asynchronous and takes a couple of minutes. A message will appear when it is done. Do you wish to continue?",
            "Continue", "Cancel"))
        {
            Debug.Log("Cancelled Zipping by user choice.");
            return;
        }

        ZipForAndroid();
        ZipForWindows();
        ZipForLinux();
        ZipForMac();
    }

    [MenuItem("VT Tools/Main Build Tools/Zip Android Build", false, 22)]
    public static void ZipForAndroid()
    {
        bplib.ZipForPlatform(Path, ZipName, BuildTarget.Android);
    }

    [MenuItem("VT Tools/Main Build Tools/Zip Windows Build", false, 22)]
    public static void ZipForWindows()
    {
        bplib.ZipForPlatform(Path, ZipName, BuildTarget.StandaloneWindows64);
    }

    [MenuItem("VT Tools/Main Build Tools/Zip Linux Build", false, 23)]
    public static void ZipForLinux()
    {
        bplib.ZipForPlatform(Path, ZipName, BuildTarget.StandaloneLinux64);
    }

    [MenuItem("VT Tools/Main Build Tools/Zip Mac Build", false, 24)]
    public static void ZipForMac()
    {
        bplib.ZipForPlatform(Path, ZipName, BuildTarget.StandaloneOSXIntel64);
    }

    [MenuItem("VT Tools/Main Build Tools/Clean All Builds", false, 40)]
    public static void ClearBuilds()
    {
        FileUtil.DeleteFileOrDirectory(Path);
        Debug.Log("Deleted " + Path);
    }
}
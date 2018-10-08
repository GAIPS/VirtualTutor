using System.IO;
using UnityEditor;
using UnityEngine;

using bplib = BuildProcessLib;

public class MainBuildProcess : MonoBehaviour
{
    private static string[] mainLevels = {"Assets/VT/Main/Scenes/VT_Main.unity"};
    private static readonly string zipName = "Virtual Tutoring";

    [MenuItem("VT Tools/Main Build Tools/Build All", false, 0)]
    public static bool BuildGame()
    {
        if (!BuildGameForAndroid()) return false;
        if (!BuildGameForWindows()) return false;
        if (!BuildGameForLinux()) return false;
        if (!BuildGameForMac()) return false;

        Debug.Log("Completed all builds");
        EditorUtility.RevealInFinder(bplib.GetProjectPath() + "/Build/Main/");
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Android", false, 1)]
    public static bool BuildGameForAndroid()
    {
        string path = bplib.GetProjectPath() + "/Build/Main/";
        if (!bplib.BuildGameForPlatform(mainLevels, path, string.Empty, "VirtualTutoring", BuildTarget.Android))
        {
            Debug.LogError("Failed to build to Android.");
            return false;
        }
        Debug.Log("Completed Android build to " + path);
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Windows", false, 2)]
    public static bool BuildGameForWindows()
    {
        string path = bplib.GetProjectPath() + "/Build/Main/";
        if (!bplib.BuildGameForPlatform(mainLevels, path, string.Empty, "VirtualTutoring", BuildTarget.StandaloneWindows64))
        {
            Debug.LogError("Failed to build to Windows.");
            return false;
        }
        Debug.Log("Completed Windows build to " + path);
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Linux", false, 3)]
    public static bool BuildGameForLinux()
    {
        string path = bplib.GetProjectPath() + "/Build/Main/";
        if (!bplib.BuildGameForPlatform(mainLevels, path, string.Empty, "VirtualTutoring", BuildTarget.StandaloneLinux64))
        {
            Debug.LogError("Failed to build to Linux.");
            return false;
        }
        Debug.Log("Completed Linux build to " + path);
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Mac", false, 4)]
    public static bool BuildGameForMac()
    {
        string path = bplib.GetProjectPath() + "/Build/Main/";
        if (!bplib.BuildGameForPlatform(mainLevels, path, string.Empty, "VirtualTutoring", BuildTarget.StandaloneOSXIntel64))
        {
            Debug.LogError("Failed to build to Mac.");
            return false;
        }
        Debug.Log("Completed Mac build to " + path);
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Zip All Builds", false, 20)]
    public static void ZipAllBuilds()
    {
        Debug.LogWarning("Zipping process locks the IDE and takes a couple of minutes. Please wait.");
        string path = bplib.GetProjectPath() + "/Build/Main/";
        ZipForAndroid();
        ZipForWindows();
        ZipForLinux();
        ZipForMac();
        Debug.Log("Zipped all builds to " + path);
    }

    [MenuItem("VT Tools/Main Build Tools/Zip Android Build", false, 22)]
    public static void ZipForAndroid()
    {
        string path = bplib.GetProjectPath() + "/Build/Main/";
        bplib.ZipForPlatform(path, zipName, BuildTarget.Android);
        Debug.Log("Zipped build for Android to " + path);
    }

    [MenuItem("VT Tools/Main Build Tools/Zip Windows Build", false, 22)]
    public static void ZipForWindows()
    {
        string path = bplib.GetProjectPath() + "/Build/Main/";
        bplib.ZipForPlatform(path, zipName, BuildTarget.StandaloneWindows64);
        Debug.Log("Zipped build for Windows to " + path);
    }

    [MenuItem("VT Tools/Main Build Tools/Zip Linux Build", false, 23)]
    public static void ZipForLinux()
    {
        string path = bplib.GetProjectPath() + "/Build/Main/";
        bplib.ZipForPlatform(path, zipName, BuildTarget.StandaloneLinux64);
        Debug.Log("Zipped build for Linux to " + path);
    }

    [MenuItem("VT Tools/Main Build Tools/Zip Mac Build", false, 24)]
    public static void ZipForMac()
    {
        string path = bplib.GetProjectPath() + "/Build/Main/";
        bplib.ZipForPlatform(path, zipName, BuildTarget.StandaloneOSXIntel64);
        Debug.Log("Zipped build for Mac to " + path);
    }

    [MenuItem("VT Tools/Main Build Tools/Clean All Builds", false, 40)]
    public static void ClearBuilds()
    {
        string path = bplib.GetProjectPath() + "/Build/Main/";
        FileUtil.DeleteFileOrDirectory(path);
        Debug.Log("Deleted " + path);
    }
}
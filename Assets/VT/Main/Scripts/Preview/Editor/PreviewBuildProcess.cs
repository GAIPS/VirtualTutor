using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEditor;
using UnityEngine;
using bplib = BuildProcessLib;

public class PreviewBuildProcess : MonoBehaviour
{
    private static string[] previewLevels = {"Assets/VT/Main/Scenes/Preview.unity"};
    private static readonly string Path = bplib.GetProjectPath() + "/Build/Preview/";
    private const string ZipName = "Virtual Tutor Preview";

    [MenuItem("VT Tools/Preview Build Tools/Build All", false, 0)]
    public static bool BuildGame()
    {
        if (!BuildGameForWindows()) return false;
        if (!BuildGameForLinux()) return false;
        if (!BuildGameForMac()) return false;

        Debug.Log("Completed all builds");
        EditorUtility.RevealInFinder(Path);
        return true;
    }

    [MenuItem("VT Tools/Preview Build Tools/Build For Windows", false, 1)]
    public static bool BuildGameForWindows()
    {
        if (!bplib.BuildGameForPlatform(previewLevels, Path, "Preview/", "Preview", BuildTarget.StandaloneWindows64))
        {
            Debug.LogError("Failed to build to Windows.");
            return false;
        }

        Debug.Log("Completed Windows build to " + Path);
        CopyBuildAssetsForWindows();
        return true;
    }

    [MenuItem("VT Tools/Preview Build Tools/Build For Linux", false, 2)]
    public static bool BuildGameForLinux()
    {
        if (!bplib.BuildGameForPlatform(previewLevels, Path, "Preview/", "Preview", BuildTarget.StandaloneLinux64))
        {
            Debug.LogError("Failed to build to Linux.");
            return false;
        }

        Debug.Log("Completed Linux build to " + Path);
        CopyBuildAssetsForLinux();
        return true;
    }

    [MenuItem("VT Tools/Preview Build Tools/Build For Mac", false, 3)]
    public static bool BuildGameForMac()
    {
        if (!bplib.BuildGameForPlatform(previewLevels, Path, "Preview/", "Preview", BuildTarget.StandaloneOSXIntel64))
        {
            Debug.LogError("Failed to build to Mac.");
            return false;
        }

        Debug.Log("Completed Mac build to " + Path);
        CopyBuildAssetsForMac();
        return true;
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy All Build Assets", false, 20)]
    public static void CopyBuildAssets()
    {
        CopyBuildAssetsForWindows();
        CopyBuildAssetsForLinux();
        CopyBuildAssetsForMac();
        Debug.Log("Copied all build assets to " + Path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy Build Assets For Windows", false, 21)]
    public static void CopyBuildAssetsForWindows()
    {
        bplib.CopyBuildAssetsForPlatform(Path, BuildTarget.StandaloneWindows64);
        string buildPath = bplib.GetBuildPath(Path, BuildTarget.StandaloneWindows64);
        FileUtil.ReplaceFile("preview.yarn.txt", buildPath + "preview.yarn.txt");
        Debug.Log("Copied build assets for Windows to " + Path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy Build Assets For Linux", false, 22)]
    public static void CopyBuildAssetsForLinux()
    {
        bplib.CopyBuildAssetsForPlatform(Path, BuildTarget.StandaloneLinux64);
        string buildPath = bplib.GetBuildPath(Path, BuildTarget.StandaloneLinux64);
        FileUtil.ReplaceFile("preview.yarn.txt", buildPath + "preview.yarn.txt");
        Debug.Log("Copied build assets for Linux to " + Path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy Build Assets For Mac", false, 23)]
    public static void CopyBuildAssetsForMac()
    {
        bplib.CopyBuildAssetsForPlatform(Path, BuildTarget.StandaloneOSXIntel64);
        string buildPath = bplib.GetBuildPath(Path, BuildTarget.StandaloneOSXIntel64);
        FileUtil.ReplaceFile("preview.yarn.txt", buildPath + "preview.yarn.txt");
        Debug.Log("Copied build assets for Mac to " + Path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip All Builds", false, 40)]
    public static void ZipAllBuilds()
    {
        if (!EditorUtility.DisplayDialog("Zipping All Builds",
            "Zipping process is asynchronous and takes a couple of minutes. A message will appear when it is done. Do you wish to continue?",
            "Continue", "Cancel"))
        {
            Debug.Log("Cancelled Zipping by user choice.");
            return;
        }

        ZipForWindows();
        ZipForLinux();
        ZipForMac();
        EditorUtility.RevealInFinder(Path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip Windows Build", false, 41)]
    public static void ZipForWindows()
    {
        bplib.ZipForPlatform(Path, ZipName, BuildTarget.StandaloneWindows64);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip Linux Build", false, 42)]
    public static void ZipForLinux()
    {
        bplib.ZipForPlatform(Path, ZipName, BuildTarget.StandaloneLinux64);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip Mac Build", false, 43)]
    public static void ZipForMac()
    {
        bplib.ZipForPlatform(Path, ZipName, BuildTarget.StandaloneOSXIntel64);
    }

    [MenuItem("VT Tools/Preview Build Tools/Clean All Builds", false, 60)]
    public static void ClearBuilds()
    {
        FileUtil.DeleteFileOrDirectory(Path);
        Debug.Log("Deleted " + Path);
    }
}
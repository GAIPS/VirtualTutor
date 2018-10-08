using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEditor;
using UnityEngine;
using bplib = BuildProcessLib;

public class PreviewBuildProcess : MonoBehaviour
{
    private static string[] previewLevels = {"Assets/VT/Main/Scenes/Preview.unity"};
    private static readonly string zipName = "Virtual Tutor Preview";

    [MenuItem("VT Tools/Preview Build Tools/Build All", false, 0)]
    public static bool BuildGame()
    {
        if (!BuildGameForWindows()) return false;
        if (!BuildGameForLinux()) return false;
        if (!BuildGameForMac()) return false;

        Debug.Log("Completed all builds");
        EditorUtility.RevealInFinder(bplib.GetProjectPath() + "/Build/Preview/");
        return true;
    }

    [MenuItem("VT Tools/Preview Build Tools/Build For Windows", false, 1)]
    public static bool BuildGameForWindows()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        if (!bplib.BuildGameForPlatform(previewLevels, path, "Preview/", "Preview", BuildTarget.StandaloneWindows64))
        {
            Debug.LogError("Failed to build to Windows.");
            return false;
        }

        Debug.Log("Completed Windows build to " + path);
        CopyBuildAssetsForWindows();
        return true;
    }

    [MenuItem("VT Tools/Preview Build Tools/Build For Linux", false, 2)]
    public static bool BuildGameForLinux()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        if (!bplib.BuildGameForPlatform(previewLevels, path, "Preview/", "Preview", BuildTarget.StandaloneLinux64))
        {
            Debug.LogError("Failed to build to Linux.");
            return false;
        }

        Debug.Log("Completed Linux build to " + path);
        CopyBuildAssetsForLinux();
        return true;
    }

    [MenuItem("VT Tools/Preview Build Tools/Build For Mac", false, 3)]
    public static bool BuildGameForMac()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        if (!bplib.BuildGameForPlatform(previewLevels, path, "Preview/", "Preview", BuildTarget.StandaloneOSXIntel64))
        {
            Debug.LogError("Failed to build to Mac.");
            return false;
        }

        Debug.Log("Completed Mac build to " + path);
        CopyBuildAssetsForMac();
        return true;
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy All Build Assets", false, 20)]
    public static void CopyBuildAssets()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        CopyBuildAssetsForWindows();
        CopyBuildAssetsForLinux();
        CopyBuildAssetsForMac();
        Debug.Log("Copied all build assets to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy Build Assets For Windows", false, 21)]
    public static void CopyBuildAssetsForWindows()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        bplib.CopyBuildAssetsForPlatform(path, BuildTarget.StandaloneWindows64);
        string buildPath = bplib.GetBuildPath(path, BuildTarget.StandaloneWindows64);
        FileUtil.ReplaceFile("preview.yarn.txt", buildPath + "preview.yarn.txt");
        Debug.Log("Copied build assets for Windows to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy Build Assets For Linux", false, 22)]
    public static void CopyBuildAssetsForLinux()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        bplib.CopyBuildAssetsForPlatform(path, BuildTarget.StandaloneLinux64);
        string buildPath = bplib.GetBuildPath(path, BuildTarget.StandaloneLinux64);
        FileUtil.ReplaceFile("preview.yarn.txt", buildPath + "preview.yarn.txt");
        Debug.Log("Copied build assets for Linux to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy Build Assets For Mac", false, 23)]
    public static void CopyBuildAssetsForMac()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        bplib.CopyBuildAssetsForPlatform(path, BuildTarget.StandaloneOSXIntel64);
        string buildPath = bplib.GetBuildPath(path, BuildTarget.StandaloneOSXIntel64);
        FileUtil.ReplaceFile("preview.yarn.txt", buildPath + "preview.yarn.txt");
        Debug.Log("Copied build assets for Mac to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip All Builds", false, 40)]
    public static void ZipAllBuilds()
    {
        Debug.LogWarning("Zipping process locks the IDE and takes a couple of minutes. Please wait.");
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        ZipForWindows();
        ZipForLinux();
        ZipForMac();
        Debug.Log("Zipped all builds to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip Windows Build", false, 41)]
    public static void ZipForWindows()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        bplib.ZipForPlatform(path, zipName, BuildTarget.StandaloneWindows64);
        Debug.Log("Zipped build for Windows to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip Linux Build", false, 42)]
    public static void ZipForLinux()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        bplib.ZipForPlatform(path, zipName, BuildTarget.StandaloneLinux64);
        Debug.Log("Zipped build for Linux to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip Mac Build", false, 43)]
    public static void ZipForMac()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        bplib.ZipForPlatform(path, zipName, BuildTarget.StandaloneOSXIntel64);
        Debug.Log("Zipped build for Mac to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Clean All Builds", false, 60)]
    public static void ClearBuilds()
    {
        string path = bplib.GetProjectPath() + "/Build/Preview/";
        FileUtil.DeleteFileOrDirectory(path);
        Debug.Log("Deleted " + path);
    }
}
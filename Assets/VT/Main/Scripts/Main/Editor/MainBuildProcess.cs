using System.IO;
using UnityEditor;
using UnityEngine;

public class MainBuildProcess : MonoBehaviour
{
    private static string[] mainLevels = {"Assets/VT/Main/Scenes/VT_Main.unity"};

    [MenuItem("VT Tools/Main Build Tools/Build All", false, 0)]
    public static void BuildGame()
    {
        // Get filename.
        //  string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "Main");
        string path = GetProjectPath() + "/Build/Main/";

        if (!BuildGameForPlatform(mainLevels, path, BuildTarget.Android)) return;
        if (!BuildGameForPlatform(mainLevels, path, BuildTarget.StandaloneWindows64)) return;
        if (!BuildGameForPlatform(mainLevels, path, BuildTarget.StandaloneLinux64)) return;
        if (!BuildGameForPlatform(mainLevels, path, BuildTarget.StandaloneOSXIntel64)) return;

        Debug.Log("Completed all builds to " + path);
        EditorUtility.RevealInFinder(path);
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Android", false, 1)]
    public static void BuildGameForAndroid()
    {
        string path = GetProjectPath() + "/Build/Main/";
        BuildGameForPlatform(mainLevels, path, BuildTarget.Android);
        Debug.Log("Completed Mac build to " + path);
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Windows", false, 2)]
    public static void BuildGameForWindows()
    {
        string path = GetProjectPath() + "/Build/Main/";
        BuildGameForPlatform(mainLevels, path, BuildTarget.StandaloneWindows64);
        Debug.Log("Completed Windows build to " + path);
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Linux", false, 3)]
    public static void BuildGameForLinux()
    {
        string path = GetProjectPath() + "/Build/Main/";
        BuildGameForPlatform(mainLevels, path, BuildTarget.StandaloneLinux64);
        Debug.Log("Completed Linux build to " + path);
    }

    [MenuItem("VT Tools/Main Build Tools/Build For Mac", false, 4)]
    public static void BuildGameForMac()
    {
        string path = GetProjectPath() + "/Build/Main/";
        BuildGameForPlatform(mainLevels, path, BuildTarget.StandaloneOSXIntel64);
        Debug.Log("Completed Mac build to " + path);
    }

    private static bool BuildGameForPlatform(string[] levels, string path, BuildTarget target)
    {
        string buildPath = path;
        string extention;
        switch (target)
        {
            case BuildTarget.Android:
                buildPath += "Android/";
                extention = "apk";
                break;
            case BuildTarget.StandaloneWindows64:
                buildPath += "Windows/";
                extention = "exe";
                break;
            case BuildTarget.StandaloneLinux64:
                buildPath += "Linux/";
                extention = "x86_64";
                break;
            case BuildTarget.StandaloneOSXIntel64:
                buildPath += "Mac/";
                extention = "app";
                break;
            default:
                Debug.LogWarning("Building for an unsupported plartform.");
                buildPath += target + "/";
                extention = "run";
                break;
        }

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = levels,
            locationPathName = buildPath + "VirtualTutoring." + extention,
            target = target,
            options = BuildOptions.None
        };
        // Build player.
        string result = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (!string.IsNullOrEmpty(result))
        {
            return false;
        }

        // Copy files here
        return true;
    }

    [MenuItem("VT Tools/Main Build Tools/Clean All Builds", false, 40)]
    public static void ClearBuilds()
    {
        string path = GetProjectPath() + "/Build/Main/";
        FileUtil.DeleteFileOrDirectory(path);
        Debug.Log("Deleted " + path);
    }

    private static string GetProjectPath()
    {
        DirectoryInfo projectDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        string projectPath = projectDirectory.FullName.Replace('\\', '/');
        return projectPath;
    }
}
using System.IO;
using UnityEditor;
using UnityEngine;

public class PreviewBuildProcess : MonoBehaviour
{
    private static string[] previewLevels = {"Assets/VT/Main/Scenes/Preview.unity"};

    [MenuItem("VT Tools/Preview Build Tools/Build All", false, 0)]
    public static void BuildGame()
    {
        // Get filename.
        //  string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "Preview");
        string path = GetProjectPath() + "/Build/Preview/";

        if (!BuildGameForPlatform(previewLevels, path, BuildTarget.StandaloneWindows64)) return;
        if (!BuildGameForPlatform(previewLevels, path, BuildTarget.StandaloneLinux64)) return;
        if (!BuildGameForPlatform(previewLevels, path, BuildTarget.StandaloneOSXIntel64)) return;

        Debug.Log("Completed all builds to " + path);
        EditorUtility.RevealInFinder(path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Build For Windows", false, 1)]
    public static void BuildGameForWindows()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        if (BuildGameForPlatform(previewLevels, path, BuildTarget.StandaloneWindows64))
            Debug.Log("Completed Windows build to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Build For Linux", false, 2)]
    public static void BuildGameForLinux()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        if (BuildGameForPlatform(previewLevels, path, BuildTarget.StandaloneLinux64))
            Debug.Log("Completed Linux build to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Build For Mac", false, 3)]
    public static void BuildGameForMac()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        if (BuildGameForPlatform(previewLevels, path, BuildTarget.StandaloneOSXIntel64))
            Debug.Log("Completed Mac build to " + path);
    }

    private static bool BuildGameForPlatform(string[] levels, string path, BuildTarget target)
    {
        string buildPath = path;
        string extention;
        switch (target)
        {
            case BuildTarget.StandaloneWindows64:
                buildPath += "Windows/Preview/";
                extention = "exe";
                break;
            case BuildTarget.StandaloneLinux64:
                buildPath += "Linux/Preview/";
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
            locationPathName = buildPath + "Preview." + extention,
            target = target,
            options = BuildOptions.None
        };
        // Build player.
        string result = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (!string.IsNullOrEmpty(result))
        {
            return false;
        }

        CopyBuildAssetsForPlatform(path, target);
        return true;
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy All Build Assets", false, 20)]
    public static void CopyBuildAssets()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        CopyBuildAssetsForPlatform(path, BuildTarget.StandaloneWindows64);
        CopyBuildAssetsForPlatform(path, BuildTarget.StandaloneLinux64);
        CopyBuildAssetsForPlatform(path, BuildTarget.StandaloneOSXIntel64);
        Debug.Log("Copied all build assets to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy Build Assets For Windows", false, 21)]
    public static void CopyBuildAssetsForWindows()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        CopyBuildAssetsForPlatform(path, BuildTarget.StandaloneWindows64);
        Debug.Log("Copied build assets for Windows to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy Build Assets For Linux", false, 22)]
    public static void CopyBuildAssetsForLinux()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        CopyBuildAssetsForPlatform(path, BuildTarget.StandaloneLinux64);
        Debug.Log("Copied build assets for Linux to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Copy Build Assets For Mac", false, 23)]
    public static void CopyBuildAssetsForMac()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        CopyBuildAssetsForPlatform(path, BuildTarget.StandaloneOSXIntel64);
        Debug.Log("Copied build assets for Mac to " + path);
    }

    private static void CopyBuildAssetsForPlatform(string path, BuildTarget target)
    {
        string buildPath = path;
        string os = string.Empty;
        switch (target)
        {
            case BuildTarget.StandaloneWindows64:
                buildPath += "Windows/";
                os += "Windows/";
                break;
            case BuildTarget.StandaloneLinux64:
                buildPath += "Linux/";
                os += "Linux/";
                break;
            case BuildTarget.StandaloneOSXIntel64:
                buildPath += "Mac/";
                os += "Mac/";
                break;
        }

        CheckDirectory(buildPath);

        // Copy example file
        FileUtil.ReplaceFile("preview.yarn.txt", buildPath + "preview.yarn.txt");

        string projectPath = GetProjectPath();
        // Copying Misc files like examples and documentation
        CopyAllDirectory(projectPath + "/BuildAssets/Misc/", buildPath);
        // Copying OS specific files like Yarn
        CopyAllDirectory(projectPath + "/BuildAssets/" + os + "/", buildPath);
    }

    /// <summary>
    /// Copy all files in a directory to the other. It replaces files that already exist.
    /// </summary>
    /// <param name="sourcePath">Path to directory to copy from.</param>
    /// <param name="destPath">Path to directory to copy to.</param>
    private static void CopyAllDirectory(string sourcePath, string destPath)
    {
        var osAssetsInfo = new DirectoryInfo(sourcePath);
        foreach (var file in osAssetsInfo.GetFiles())
        {
            FileUtil.ReplaceFile(file.FullName, destPath + file.Name);
        }

        foreach (var file in osAssetsInfo.GetDirectories())
        {
            FileUtil.ReplaceDirectory(file.FullName, destPath + file.Name);
        }
    }

    /// <summary>
    /// If the directory doesn't exist, create it.
    /// </summary>
    /// <param name="path">Path to check</param>
    private static void CheckDirectory(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
    }

    [MenuItem("VT Tools/Preview Build Tools/Clean All Builds", false, 40)]
    public static void ClearBuilds()
    {
        string path = GetProjectPath() + "/Build/Preview/";
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
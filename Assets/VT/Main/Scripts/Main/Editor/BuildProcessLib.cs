using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildProcessLib
{
    public static bool BuildGameForPlatform(string[] levels, string path, string subfolder, string name,
        BuildTarget target)
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
                buildPath += "Windows/" + subfolder;
                extention = "exe";
                break;
            case BuildTarget.StandaloneLinux64:
                buildPath += "Linux/" + subfolder;
                extention = "x86_64";
                break;
            case BuildTarget.StandaloneOSX:
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
            locationPathName = buildPath + name + "." + extention,
            target = target,
            options = BuildOptions.None
        };
        // Build player.
        var result = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = result.summary;
        
        if (summary.result == BuildResult.Succeeded)
        {
            return true;
        }

        return false;
    }

    public static string GetBuildPath(string path, BuildTarget target)
    {
        string buildPath = path;
        switch (target)
        {
            case BuildTarget.Android:
                buildPath += "Android/";
                break;
            case BuildTarget.StandaloneWindows64:
                buildPath += "Windows/";
                break;
            case BuildTarget.StandaloneLinux64:
                buildPath += "Linux/";
                break;
            case BuildTarget.StandaloneOSX:
                buildPath += "Mac/";
                break;
        }

        return buildPath;
    }

    public static void CopyBuildAssetsForPlatform(string path, BuildTarget target)
    {
        string buildPath = path;
        string os = string.Empty;
        switch (target)
        {
            case BuildTarget.Android:
                buildPath += "Android/";
                os += "Android/";
                break;
            case BuildTarget.StandaloneWindows64:
                buildPath += "Windows/";
                os += "Windows/";
                break;
            case BuildTarget.StandaloneLinux64:
                buildPath += "Linux/";
                os += "Linux/";
                break;
            case BuildTarget.StandaloneOSX:
                buildPath += "Mac/";
                os += "Mac/";
                break;
        }

        CheckDirectory(buildPath);

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

    public static void ZipForPlatform(string path, string filename, BuildTarget target)
    {
        string buildPath = path;
        switch (target)
        {
            case BuildTarget.Android:
                buildPath += "Android/";
                filename += " Android";
                break;
            case BuildTarget.StandaloneWindows64:
                buildPath += "Windows/";
                filename += " Windows";
                break;
            case BuildTarget.StandaloneLinux64:
                buildPath += "Linux/";
                filename += " Linux";
                break;
            case BuildTarget.StandaloneOSX:
                buildPath += "Mac/";
                filename += " Mac";
                break;
        }

        filename += ".zip";
        CompressDirectoryAsync(buildPath, path + filename);
    }

    public static string GetProjectPath()
    {
        DirectoryInfo projectDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        string projectPath = projectDirectory.FullName.Replace('\\', '/');
        return projectPath;
    }

    private static void CompressDirectoryAsync(string sInDir, string sOutFile)
    {
        Debug.Log("Zipping build to " + sOutFile + ". Please wait...");
        var asyncAction = new Action(() => CompressDirectory(sInDir, sOutFile));
        asyncAction.BeginInvoke(result => Debug.Log("Finished Zipping file " + sOutFile), null);
    }

    private static void CompressDirectory(string sInDir, string sOutFile)
    {
        FastZip fastZip = new FastZip();
        fastZip.CreateZip(sOutFile, sInDir, true, "");
    }
}
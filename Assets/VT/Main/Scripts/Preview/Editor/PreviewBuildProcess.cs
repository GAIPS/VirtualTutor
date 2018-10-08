using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using UnityEditor;
using UnityEngine;

public class PreviewBuildProcess : MonoBehaviour
{
    private static string[] previewLevels = {"Assets/VT/Main/Scenes/Preview.unity"};
    private static string filename = "Virtual Tutor Preview";

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

    [MenuItem("VT Tools/Preview Build Tools/Zip All Builds", false, 40)]
    public static void ZipAllBuilds()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        ZipForPlatform(path, BuildTarget.StandaloneWindows64);
        ZipForPlatform(path, BuildTarget.StandaloneLinux64);
        ZipForPlatform(path, BuildTarget.StandaloneOSXIntel64);
        Debug.Log("Zipped all builds to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip Windows Build", false, 41)]
    public static void ZipForWindows()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        ZipForPlatform(path, BuildTarget.StandaloneWindows64);
        Debug.Log("Zipped build for Windows to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip Linux Build", false, 42)]
    public static void ZipForLinux()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        ZipForPlatform(path, BuildTarget.StandaloneLinux64);
        Debug.Log("Zipped build for Linux to " + path);
    }

    [MenuItem("VT Tools/Preview Build Tools/Zip Mac Build", false, 43)]
    public static void ZipForMac()
    {
        string path = GetProjectPath() + "/Build/Preview/";
        ZipForPlatform(path, BuildTarget.StandaloneOSXIntel64);
        Debug.Log("Zipped build for Mac to " + path);
    }

    public static void ZipForPlatform(string path, BuildTarget target)
    {
        string buildPath = path;
        string filename = "Virtual Tutor Preview ";
        switch (target)
        {
            case BuildTarget.StandaloneWindows64:
                buildPath += "Windows/";
                filename += "Windows";
                break;
            case BuildTarget.StandaloneLinux64:
                buildPath += "Linux/";
                filename += "Linux";
                break;
            case BuildTarget.StandaloneOSXIntel64:
                buildPath += "Mac/";
                filename += "Mac";
                break;
        }

        filename += ".zip";
        CompressDirectory(buildPath, path + filename);
    }

    [MenuItem("VT Tools/Preview Build Tools/Clean All Builds", false, 60)]
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

    static void CompressDirectory(string sInDir, string sOutFile)
    {
        try
        {
            // 'using' statements guarantee the stream is closed properly which is a big source
            // of problems otherwise.  Its exception safe as well which is great.
            using (ZipOutputStream s = new ZipOutputStream(File.Create(sOutFile)))
            {
                s.SetLevel(9); // 0 - store only to 9 - means best compression

                CompressInnerDirectory(s, sInDir, sInDir);

                // Finish/Close arent needed strictly as the using statement does this automatically

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                s.Finish();

                // Close is important to wrap things up and unlock the file.
                s.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Exception during processing " + ex);

            // No need to rethrow the exception as for our purposes its handled.
        }
    }

    static void CompressInnerDirectory(ZipOutputStream s, string directory, string rootDirectory)
    {
        // Depending on the directory this could be very large and would require more attention
        // in a commercial package.
        string[] filenames = Directory.GetFiles(directory);
        string[] directories = Directory.GetDirectories(directory);

        byte[] buffer = new byte[4096];

        foreach (string file in filenames)
        {
            // Using GetFileName makes the result compatible with XP
            // as the resulting path is not absolute.
            string filename = file.Replace(rootDirectory, "");
            var entry = new ZipEntry(filename);

            // Setup the entry data as required.

            // Crc and size are handled by the library for seakable streams
            // so no need to do them here.

            // Could also use the last write time or similar for the file.
            entry.DateTime = DateTime.Now;
            s.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(file))
            {
                // Using a fixed size buffer here makes no noticeable difference for output
                // but keeps a lid on memory usage.
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    s.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }

        foreach (var dir in directories)
        {
            CompressInnerDirectory(s, dir, rootDirectory);
        }
    }
}
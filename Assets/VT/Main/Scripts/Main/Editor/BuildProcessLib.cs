using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEditor;
using UnityEngine;

public static class BuildProcessLib
{
    public static bool BuildGameForPlatform(string[] levels, string path, string subfolder, string name, BuildTarget target)
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
            locationPathName = buildPath + name + "." + extention,
            target = target,
            options = BuildOptions.None
        };
        // Build player.
        string result = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (!string.IsNullOrEmpty(result))
        {
            return false;
        }

        return true;
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
            case BuildTarget.StandaloneOSXIntel64:
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
            case BuildTarget.StandaloneOSXIntel64:
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
            case BuildTarget.StandaloneOSXIntel64:
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
        try
        {
            FastZip fastZip = new FastZip();

            bool recurse = true;  // Include all files by recursing through the directory structure
            string filter = @"";
            fastZip.CreateZip(sOutFile, sInDir, recurse, filter);
            
//            // 'using' statements guarantee the stream is closed properly which is a big source
//            // of problems otherwise.  Its exception safe as well which is great.
//            using (ZipOutputStream s = new ZipOutputStream(File.Create(sOutFile)))
//            {
//                s.SetLevel(9); // 0 - store only to 9 - means best compression
////                s.UseZip64 = UseZip64.Off;
//
//                CompressInnerDirectory(s, sInDir, sInDir);
//
//                // Finish/Close arent needed strictly as the using statement does this automatically
//
//                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
//                // the created file would be invalid.
//                s.Finish();
//
//                // Close is important to wrap things up and unlock the file.
//                s.Close();
//            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Exception during processing " + ex);

            // No need to rethrow the exception as for our purposes its handled.
        }
    }

    private static void CompressInnerDirectory(ZipOutputStream s, string directory, string rootDirectory)
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
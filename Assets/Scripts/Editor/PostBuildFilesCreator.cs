using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

using UnityEngine;
namespace Society.Editor
{
    /// <summary>
    /// Класс копирует нужные данные в папку с билдом из папки проекта
    /// </summary>
    internal sealed class PostBuildFilesCreator
    {
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            string[] srcPaths = {$"{Directory.GetCurrentDirectory()}\\Localization",
            $"{Directory.GetCurrentDirectory()}\\Saves"};

            string[] destPaths ={$"{Directory.GetCurrentDirectory()}\\Builds\\Localization",
            $"{Directory.GetCurrentDirectory()}\\Builds\\Saves"};
            for (int i = 0; i < srcPaths.Length; i++)
            {
                DirectoryCopy(srcPaths[i], destPaths[i], true);
            }

            Debug.Log("Files were added");
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
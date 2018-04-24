using System.Collections.Generic;
using System.IO;

namespace Common.Utils
{
    public static class FileHelper
    {
        public static void DirectoryCopy(string srcDir, string dstDir, bool copySubDirs, bool overwrite = false)
        {
            var srcDirInfo = new DirectoryInfo(srcDir);
            if (!srcDirInfo.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + srcDir);

            if (!Directory.Exists(dstDir))
                Directory.CreateDirectory(dstDir);

            //Copy files
            foreach (FileInfo file in srcDirInfo.EnumerateFiles())
            {
                string dstPath = Path.Combine(dstDir, file.Name);
                file.CopyTo(dstPath, overwrite);
            }

            if (!copySubDirs)
                return;

            //Copy subdirectories and their child directories and files
            foreach (DirectoryInfo subdir in srcDirInfo.EnumerateDirectories())
            {
                string dstPath = Path.Combine(dstDir, subdir.Name);
                DirectoryCopy(subdir.FullName, dstPath, true, overwrite);
            }
        }

        public static void CopyFilesToDirectory(IEnumerable<string> files, string dstDir, bool overwrite)
        {
            if (!Directory.Exists(dstDir))
                Directory.CreateDirectory(dstDir);

            foreach (string srcFilePath in files)
            {
                string dstFileName = Path.GetFileName(srcFilePath);
                File.Copy(srcFilePath, Path.Combine(dstDir, dstFileName), overwrite);
            }
        }

        public static string GetRandomFolderName()
        {
            string name = Path.GetRandomFileName();
            return name.Remove(name.Length - 4, 1);
        }

        public static string Rename(string filePath, string newFileName)
        {
            string newFilePath = Path.GetDirectoryName(filePath);
            newFilePath = Path.Combine(newFilePath, newFileName);
            File.Move(filePath, newFilePath);
            return newFilePath;
        }

    }
}

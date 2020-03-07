using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Framework.Utils
{
    /// <summary>
    /// File system utils.
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// Gets path to current project.
        /// </summary>
        public static string ProjectRoot
        {
            get
            {
                var binDebugFolder = Environment.CurrentDirectory;
                var projectRoot = new DirectoryInfo(binDebugFolder);
                while (projectRoot != null && !projectRoot.GetFiles("*.csproj").Any())
                {
                    projectRoot = projectRoot.Parent;
                }

                return projectRoot.FullName;
            }
        }

        /// <summary>
        /// Create folder.
        /// </summary>
        /// <param name="path">Path.</param>
        public static void CreateFolder(string path)
        {
            FileInfo folder = new FileInfo(path.ResolveVariables());
            if (!folder.Exists)
            {
                Directory.CreateDirectory(folder.FullName);
            }
        }

        /// <summary>
        /// Delete folder (recursively).
        /// </summary>
        /// <param name="path">Path.</param>
        public static void DeleteFolder(string path)
        {
            FileInfo folder = new FileInfo(path.ResolveVariables());
            if (folder.Exists)
            {
                Directory.Delete(folder.Directory.FullName, true);
            }
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="path">Path.</param>
        public static void DeleteFile(string path)
        {
            FileInfo file = new FileInfo(path.ResolveVariables());
            if (file.Exists)
            {
                File.Delete(file.FullName);
            }
        }

        /// <summary>
        /// Read file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>Content of file as string.</returns>
        public static string ReadFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path.ResolveVariables());
            if (fileInfo.Exists)
            {
                return File.ReadAllText(fileInfo.FullName);
            }
            else
            {
                throw new FileNotFoundException(path + " do not exists!");
            }
        }

        /// <summary>
        /// Read file lines.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>Content of file as List of strings.</returns>
        public static List<string> ReadFileLines(string path)
        {
            FileInfo fileInfo = new FileInfo(path.ResolveVariables());
            if (fileInfo.Exists)
            {
                IEnumerable<string> lines = File.ReadLines(fileInfo.FullName);
                return lines.ToList();
            }
            else
            {
                throw new FileNotFoundException(path + " do not exists!");
            }
        }
    }
}

using System;
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
            FileInfo folder = new FileInfo(path);
            if (!folder.Exists)
            {
                Directory.CreateDirectory(folder.Directory.FullName);
            }
        }

        /// <summary>
        /// Delete folder (recursively).
        /// </summary>
        /// <param name="path">Path.</param>
        public static void DeleteFolder(string path)
        {
            FileInfo folder = new FileInfo(path);
            if (folder.Exists)
            {
                Directory.Delete(folder.Directory.FullName, true);
            }
        }

        /// <summary>
        /// Remove all invalid PATH symbols of given string.
        /// </summary>
        /// <param name="path">String.</param>
        /// <returns>String with escaped invalid PATH symbols.</returns>
        public static string NormalizePath(string path)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(), string.Empty);
            }

            return path;
        }

        /// <summary>
        /// Read file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>Content of file as string.</returns>
        public static string ReadFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                return File.ReadAllText(path);
            }
            else
            {
                throw new FileNotFoundException(path + " do not exists!");
            }
        }
    }
}

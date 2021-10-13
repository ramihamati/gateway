using Digitteck.Gateway.Service.Abstractions;
using Digitteck.Gateway.Service.Exceptions;
using System;
using System.IO;
using System.Reflection;

namespace Digitteck.Gateway.Service.Common.Helpers
{
    internal static class FileHelper
    {
        public static string GetFilePathInFolder(string folderPath, string filePath)
        {
            string folderPathClean = CleanPathRightside(folderPath.ToCharArray());
            string filePathClean = CleanPathLeftside(filePath.ToCharArray());

            var absolutePath = Path.Combine(folderPathClean, filePathClean);
            return absolutePath.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        public static string GetFilePathInCurrentAssembly(string filePath)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            FileInfo assemblyFileInfo = new FileInfo(assemblyLocation);
            DirectoryInfo assemblyDirectory = assemblyFileInfo.Directory;

            string assemblyPathClean = CleanPathRightside(assemblyDirectory.FullName.ToCharArray());
            string filePathClean = CleanPathLeftside(filePath.ToCharArray());

            //Uri uri = new Uri(new Uri(assemblyPathClean), new Uri(filePathClean));
            var absolutePath = Path.Combine(assemblyPathClean, filePathClean);

            return absolutePath.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        private static string CleanPathRightside(Span<char> path)
        {
            if (path.EndsWith("/") || path.EndsWith("\\"))
            {
                return CleanPathRightside(path.Slice(0, path.Length - 1));
            }

            return path.ToString();
        }

        private static string CleanPathLeftside(Span<char> path)
        {
            if (path.StartsWith("/") || path.StartsWith("\\"))
            {
                return CleanPathLeftside(path.Slice(1, path.Length - 1));
            }

            return path.ToString();
        }

        /// <summary>
        /// Gets the directory of the file. The file will be searched based on the path kind
        /// </summary>
        public static string GetFileDirectory(string filePath, FilePathKind kind)
        {
            try
            {
                if (kind == FilePathKind.ExecutingAssembly)
                {
                    string fileAbsolutePath = FileHelper.GetFilePathInCurrentAssembly(filePath);
                    FileInfo fileInfo = new FileInfo(fileAbsolutePath);
                    return fileInfo.Directory.FullName;
                }

                throw new GatewayException(ErrorCode.FileHelper, $"The file kind must be set");
            }
            catch (GatewayException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.FileHelper, ex.Message);
            }
        }

        /// <summary>
        /// Gets the contnet of the file. The file will be searched based on the path kind
        /// </summary>
        public static string GetFileContent(string filePath, FilePathKind kind)
        {
            try
            {
                if (kind == FilePathKind.ExecutingAssembly)
                {
                    string fileAbsolutePath = FileHelper.GetFilePathInCurrentAssembly(filePath);

                    return File.ReadAllText(fileAbsolutePath);
                }
             
                throw new GatewayException(ErrorCode.FileHelper, $"The file kind must be set");
            }
            catch (GatewayException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.ContentProvider, ex.Message);
            }
        }

        /// <summary>
        /// Gets the content of a file. The filepath is relative to the directory path
        /// </summary>
        public static string GetFileContent(string filePath, string directoryPath)
        {
            try
            {
                string fileAbsolutePath = FileHelper.GetFilePathInFolder(directoryPath, filePath);

                return File.ReadAllText(fileAbsolutePath);
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.ContentProvider, ex.Message);
            }
        }
    }
}

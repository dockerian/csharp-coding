using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public static class FileHelper
    {
        public static List<string> GetFilesRecursive(string path, string pattern)
        {
            // 1.
            // Store results in the file results list.
            List<string> result = new List<string>();

            // 2.
            // Store a stack of our directories.
            Stack<string> stack = new Stack<string>();

            // 3.
            // Add initial directory.
            stack.Push(path);

            // 4.
            // Continue while there are directories to process
            while (stack.Count > 0)
            {
                // A.
                // Get top directory
                string dir = stack.Pop();

                try
                {
                    // B
                    // Add all files at this directory to the result List.
                    result.AddRange(Directory.GetFiles(dir, pattern));

                    // C
                    // Add all directories at this directory.
                    foreach(string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                    // D
                    // Could not open the directory
                }
            }
            return result;
        }

        public static void DeleteAllDirectoryFiles(string directoryPath)
        {
            // Note: We recursively delete all files (only) contained within the directory tree, instead of 
            //       using the problematic Directory.Delete() MS API:
            //
            // KNOWN MS API ISSUE:  There is a know issue regarding usage of System.IO.Directory.Delete(), which states:
            //                      "In some cases, if you have the specified directory open in Windows Explorer, the 
            //                      Delete method may not be able to delete it"
            //
            // For more info see the following links:
            //   http://msdn.microsoft.com/en-us/library/fxeahc5f.aspx
            //   http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/a2fcc569-1835-471f-b731-3fe9c6bcd2d9#de0cbae1-adbf-4d0f-88de-ea02e3a0b62f
            //

            foreach(string dir in Directory.EnumerateDirectories(directoryPath))
                DeleteAllDirectoryFiles(dir);

            foreach(string file in Directory.EnumerateFiles(directoryPath))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
        }

        public static IEnumerable<string> GetFiles(string path, string searchPattern, SortOrder sortOrder, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            if (sortOrder == SortOrder.Ascending)
            {
                var files = (from file in dir.EnumerateFiles(searchPattern, searchOption)
                             orderby file.CreationTime ascending
                             select file.Name).Distinct(); // Don't need <string> here, since it's implied
                return files;
            }
            else
            {
                var files = (from file in dir.EnumerateFiles(searchPattern, searchOption)
                             orderby file.CreationTime descending
                             select file.Name).Distinct(); // Don't need <string> here, since it's implied
                return files;
            }
        }
    }

    public static class FilePath
    {
        public static char[] FileNameTrimChars;
        public static char[] PathTrimChars;

        public static string ErrorMessage { get; set; }

        static FilePath()
        {
            int count_fc = Path.GetInvalidFileNameChars().Length;
            int count_pc = Path.GetInvalidPathChars().Length;

            FileNameTrimChars = new char[count_fc + 1];
            PathTrimChars = new char[count_pc + 1];

            FileNameTrimChars[0] = ' ';
            PathTrimChars[0] = ' ';

            Path.GetInvalidFileNameChars().CopyTo(FileNameTrimChars, 1);
            Path.GetInvalidPathChars().CopyTo(PathTrimChars, 1);
        }

        public static bool CopyFromFile(string filePath, string targetServerPath, bool disableProxy = false)
        {
            return CopyFromFile(filePath, targetServerPath, "", "", disableProxy);
        }
        public static bool CopyFromFile(string filePath, string targetServerPath, string userName, string password, bool disableProxy = false)
        {
            string domain = (userName != null &&
                userName.IndexOf('\\') > 0) ?
                userName.Substring(0, userName.IndexOf('\\')) : "";
            string userid = (userName != null) ?
                userName.Replace(domain + "\\", "") : "";

            bool succeeded = false;
            WebClient client = new WebClient();

            try
            {
                if (IsWindowsPath(targetServerPath))
                {
                    using (new Impersonator(userid, password, domain))
                    {
                        File.Copy(filePath, targetServerPath, true);
                    }
                }
                else // uploading
                {
                    if (disableProxy)
                    {
                        client.Proxy = null; // Elimates delay caused by auto-detecting proxies.
                    }
                    if (String.IsNullOrEmpty(userName))
                    {
                        client.UseDefaultCredentials = true;
                    }
                    else // assume using userName and password
                    {
                        client.Credentials = new NetworkCredential(userid, password, domain);
                    }
                    client.UploadFile(targetServerPath, "PUT", filePath);
                    succeeded = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception on copying from file \r\n" + filePath + "\r\n" + ex.ToString();
                System.Diagnostics.Trace.WriteLine(ErrorMessage);
            }

            return succeeded;
        }

        public static void CopyFromUrl(string urlPath, string localPath, Action<object, AsyncCompletedEventArgs> callback, bool disableProxy = false)
        {
            WebClient client = new WebClient();

            if (disableProxy)
            {
                client.Proxy = null;
            }
            if (callback != null)
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(callback);
                client.DownloadFileAsync(new Uri(urlPath), localPath);
            }
            else // download file directly
            {
                client.DownloadFile(new Uri(urlPath), localPath);
            }
        }

        public static bool CopyToFile(string filePath, string sourceServerPath)
        {
            bool succeeded = false;
            WebClient client = new WebClient();

            try
            {
                if (IsWindowsPath(sourceServerPath))
                {
                    File.Copy(sourceServerPath, filePath, true);
                }
                else // downloading
                {
                    client.Proxy = null; // Elimates delay caused by auto-detecting proxies.
                    client.DownloadFile(sourceServerPath, filePath);
                    succeeded = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception on copying to file \r\n" + filePath + "\r\n" + ex.ToString();
                System.Diagnostics.Trace.WriteLine(ErrorMessage);
            }

            return succeeded;
        }

        public static string GetFileName(string path)
        {
            if (String.IsNullOrEmpty(path)) return "";

            if (String.IsNullOrEmpty(path = GetValidPath(path))) return "";

            int idx1 = path.TrimEnd('\\').LastIndexOf('\\');
            int idx2 = path.Replace("://", ":::").TrimEnd('/').LastIndexOf('/');
            int indx = idx1 >= 0 ? idx1 : idx2;

            string dirUp = (indx >= 0) ? path.Substring(indx).Trim(FileNameTrimChars) : "";

            return dirUp;
        }

        public static char GetSlashChar(string path)
        {
            if (String.IsNullOrEmpty(path)) return '\\';

            path = path.Trim();

            if (path.StartsWith("\\") || Regex.IsMatch(path, @"^([A-Za-z]:|\\\\)"))
            {
                return '\\';
            }
            return '/';
        }

        public static string GetRootPath(string path)
        {
            if (String.IsNullOrEmpty(path)) return "";

            if (String.IsNullOrEmpty(path = GetValidPath(path))) return "";

            int index = 2; // for local path or network path

            if (path.IndexOf("://") > 0) index = path.IndexOf("://") + 3;

            for (int i = index; i < path.Length; i++)
            {
                if (path[i] == '\\' || path[i] == '/' || path[i] == ':')
                {
                    index = i; break;
                }
            }

            if (index > path.Length) index = path.Length;

            string dirRoot = path.Substring(0, index);

            return dirRoot;
        }

        public static string GetUpperPath(string path)
        {
            if (String.IsNullOrEmpty(path)) return "";

            if (String.IsNullOrEmpty(path = GetValidPath(path))) return "";

            int idx1 = path.TrimEnd('\\').LastIndexOf('\\');
            int idx2 = path.Replace("://", ":::").TrimEnd('/').LastIndexOf('/');
            int indx = idx1 >= 0 ? idx1 : idx2;

            string dirUp = (indx >= 0) ? path.Substring(0, indx) : "";

            return dirUp;
        }

        public static string GetValidPath(string path)
        {
            string validPath = (path == null) ? "" : path.Trim();
            string charSlash = GetSlashChar(path).ToString();
            string chInvalid = new string(Path.GetInvalidPathChars());

            if (validPath.Length < 4) return validPath;

            //validPath = Regex.Replace(validPath, @"["+chInvalid+"]", "");
            validPath = Regex.Replace(validPath, @"[\\/]", charSlash);

            //validPath = validPath.TrimEnd(FilePath.PathTrimChars);

            return validPath;
        }

        public static bool IsWindowsPath(string path)
        {
            path = GetValidPath(path);

            return (path.Contains('\\') || Regex.IsMatch(path, @"^([A-Za-z]:|\\\\)"));
        }

        public static bool IsUrlPath(string path)
        {
            path = GetValidPath(path);

            return path.Contains("://");
        }

    }// class FilePath


    public static class FileSystem
    {
        public static void CleanDirectoryTree(string rootPath)
        {
            if (String.IsNullOrEmpty(rootPath)) return;
            if (Directory.Exists(rootPath) == false) return;

            DirectoryInfo dir = new DirectoryInfo(rootPath);
            DirectoryInfo[] directories = dir.GetDirectories();
            FileInfo[] files = dir.GetFiles();

            if (files.Length > 0) return;

            foreach(DirectoryInfo dirInfo in directories)
            {
                CleanDirectoryTree(dirInfo.FullName);
            }

            directories = dir.GetDirectories(); // refresh after processed sub dirs

            if (directories.Length == 0 && files.Length == 0)
            {
                Directory.Delete(rootPath);
            }
        }


        public static void ClearAttributes(string currentDir)
        {
            if (Directory.Exists(currentDir))
            {
                string[] subDirs = Directory.GetDirectories(currentDir);

                foreach(string dir in subDirs)
                {
                    ClearAttributes(dir);
                }

                string[] files = files = Directory.GetFiles(currentDir);

                foreach(string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }
            }
        }

        public static void CopyDirectory(string source, string target, bool recursive, bool overwrite)
        {
            if (String.IsNullOrEmpty(source) || String.IsNullOrEmpty(target)) return;
            if (Directory.Exists(source) == false) return;

            source = Path.GetFullPath(source);
            target = Path.GetFullPath(target);

            if (String.Compare(source, target, true) == 0) return;

            if (target[target.Length - 1] != Path.DirectorySeparatorChar)
            {
                target += Path.DirectorySeparatorChar;
            }
            if (Directory.Exists(target) == false)
            {
                Directory.CreateDirectory(target);
            }

            String[] entries = Directory.GetFileSystemEntries(source);

            foreach(string element in entries)
            {
                string targetFile = target + Path.GetFileName(element);

                if (File.Exists(element))
                {
                    if (overwrite || !File.Exists(targetFile))
                    {
                        File.Copy(element, targetFile, overwrite);
                    }
                }
                else if (recursive) // assume sub directory
                {
                    CopyDirectory(element, targetFile, recursive, overwrite);
                }
            }
        }

        public static void DeleteDirectory(string target_dir)
        {
            DeleteDirectory(target_dir, true, false);
        }
        public static void DeleteDirectory(string target_dir, bool recursive)
        {
            DeleteDirectory(target_dir, recursive, false);
        }
        public static void DeleteDirectory(string target_dir, bool recursive, bool emptyDirectoryOnly)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            if (recursive)
            {
                foreach(string dir in dirs)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    dirInfo.Attributes &= ~FileAttributes.ReadOnly;
                    DeleteDirectory(dir, true, emptyDirectoryOnly);
                }
            }
            foreach(string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            if (emptyDirectoryOnly == false)
            {
                Directory.Delete(target_dir, true);
            }
        }

        public static FileInfo[] GetMostRecentlyFileLinks()
        {
            string folderName = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            DirectoryInfo recentFolder = new DirectoryInfo(folderName);
            FileInfo[] files = recentFolder.GetFiles();

            return files;
        }

    }

}
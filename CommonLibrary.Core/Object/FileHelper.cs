using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace hwj.CommonLibrary.Object
{
    /// <summary>
    /// 文件类
    /// </summary>
    public class FileHelper
    {
        public static bool RegisterFile(string fileName, bool displayMessage)
        {
            try
            {
                if (displayMessage)
                    System.Diagnostics.Process.Start("regsvr32.exe", fileName);
                else
                    System.Diagnostics.Process.Start("regsvr32.exe", "/s " + fileName);
                return true;
            }
            catch { return false; }
        }

        public static bool UnRegisterFile(string fileName, bool displayMessage)
        {
            try
            {
                if (displayMessage)
                    System.Diagnostics.Process.Start("regsvr32.exe", "/u " + fileName);
                else
                    System.Diagnostics.Process.Start("regsvr32.exe", "/s /u " + fileName);
                return true;
            }
            catch { return false; }
        }

        #region 文件操作

        public static List<string> ReadFileList(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                List<string> lines = new List<string>();
                String line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                return lines;
            }
        }

        public static string ReadFile(string fileName)
        {
            while (true)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default))
                    {
                        return sr.ReadToEnd();
                    }
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }

        public static string ReadFile(string fileName, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(fileName, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName">要写入的完整文件路径。</param>
        public static void CreateFile(string fileName)
        {
            CreateFile(fileName, null);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName">要写入的完整文件路径。</param>
        /// <param name="text">文本</param>
        public static void CreateFile(string fileName, string text)
        {
            CreateFile(fileName, text, false);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName">要写入的完整文件路径。</param>
        /// <param name="text">文本</param>
        /// <param name="append"> 确定是否将数据追加到文件。如果该文件存在，并且 append 为 false，则该文件被覆盖。如果该文件存在，并且 append 为 true，则数据被追加到该文件中。否则，将创建新文件。</param>
        public static void CreateFile(string fileName, string text, bool append)
        {
            if (!File.Exists(fileName))
            {
                string directory = Path.GetDirectoryName(fileName);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                else
                    using (File.Create(fileName)) { }
            }
            if (!string.IsNullOrEmpty(text))
            {
                using (StreamWriter sw = new StreamWriter(fileName, append, System.Text.Encoding.UTF8))
                {
                    sw.Write(text);
                }
            }
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceFolder">源文件夹</param>
        /// <param name="targetFolder">目标文件夹</param>
        /// <returns></returns>
        public static bool CopyFolder(string sourceFolder, string targetFolder)
        {
            string[] filenames = Directory.GetFileSystemEntries(FormatFolder(sourceFolder));
            string tmpTargetFolder = FormatFolder(targetFolder);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                string currFolder = GetCurrentFolder(file);

                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    currFolder = tmpTargetFolder + "\\" + currFolder;

                    if (!Directory.Exists(currFolder))
                    {
                        Directory.CreateDirectory(currFolder);
                    }

                    CopyFolder(file, currFolder);
                }
                else // 否则直接copy文件
                {
                    currFolder = tmpTargetFolder + "\\" + currFolder;

                    if (!Directory.Exists(tmpTargetFolder))
                    {
                        Directory.CreateDirectory(tmpTargetFolder);
                    }

                    File.Copy(file, currFolder, true);
                }
            }
            return true;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static bool DeleteFolder(string folder)
        {
            DirectoryInfo path = new DirectoryInfo(folder);
            return DeleteFolder(path);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static bool DeleteFolder(DirectoryInfo directory)
        {
            foreach (System.IO.DirectoryInfo d in directory.GetDirectories())
            {
                DeleteFolder(d);
            }
            foreach (System.IO.FileInfo f in directory.GetFiles())
            {
                f.Delete();
            }
            if (directory.GetDirectories() == null || directory.GetDirectories().Length == 0)
            {
                directory.Delete();
            }

            return true;
        }

        #region Private Function

        private static string GetCurrentFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            string[] arr = path.Split('\\');

            if (arr != null && arr.Length <= 1)
                return arr[0];

            if (arr != null && string.IsNullOrEmpty(arr[arr.Length - 1]))
            {
                return arr[arr.Length - 2];
            }
            else
            {
                return arr[arr.Length - 1];
            }
        }

        private static string FormatFolder(string path)
        {
            return path.EndsWith("\\") ? path.Substring(0, path.Length - 1) : path;
        }

        #endregion Private Function

        #endregion 文件操作

        #region 文件转换

        /// <summary>
        /// 将 Stream 写入文件
        /// </summary>
        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[]
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// 从文件读取 Stream
        /// </summary>
        public static Stream FileToStream(string fileName)
        {
            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        #endregion 文件转换
    }
}
using DotLiquid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallCoder
{
    internal static class Utils
    {
        internal static string _tempPath = Application.StartupPath + @"\Templates";
        internal static string _confPath = Application.StartupPath + @"\config.json";
        internal static string _outPath = Application.StartupPath + @"\Out";
        internal static string _tempFileSuffix = ".nxt";
        internal static string _defaultFileSuffix = ".cs";
        internal static Random _random = new Random();

        /// <summary>
        /// 表名转大写
        /// </summary>
        public static string UpperTable(string str)
        {
            // 字符串缓冲区
            StringBuilder sbf = new StringBuilder();
            if (str.Contains(".")) str = str.Replace('.', '_');
            // 如果字符串包含 下划线
            if (str.Contains("_"))
            {
                // 按下划线来切割字符串为数组
                string[] split = str.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                // 循环数组操作其中的字符串
                for (int i = 0, index = split.Length; i < index; i++)
                {
                    // 递归调用本方法
                    string upperTable = Utils.UpperTable(split[i]);
                    // 添加到字符串缓冲区
                    sbf.Append(upperTable);
                }
            }
            else
            {// 字符串不包含下划线
             // 转换成字符数组
                char[] ch = str.ToCharArray();
                // 判断首字母是否是字母
                if (ch[0] >= 'a' && ch[0] <= 'z')
                {
                    // 利用ASCII码实现大写
                    ch[0] = (char)(ch[0] - 32);
                }
                // 添加进字符串缓存区
                sbf.Append(ch);
            }
            return sbf.ToString();
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        public static string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文件并装载模板
        /// </summary>
        public static Template ReadFileToTemplate(string filePath)
        {
            var strTemplate = ReadFile(filePath);
            return Template.Parse(strTemplate);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        public static void WriteToFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }

        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="subPath">子文件夹</param>
        public static List<FileInformation> GetAllFiles(string subPath = "")
        {
            List<FileInformation> InnerGetAllFiles(DirectoryInfo dir)
            {
                var listFiles = new List<FileInformation>();
                FileInfo[] allFile = dir.GetFiles($"*{_tempFileSuffix}");
                foreach (FileInfo fi in allFile)
                {
                    var outPath = _outPath + fi.DirectoryName.Replace(_tempPath, String.Empty);
                    var relativePath = fi.FullName.Remove(0, _tempPath.Length + 1);
                    //if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);

                    var arrName = fi.Name.Remove(fi.Name.Length - _tempFileSuffix.Length, _tempFileSuffix.Length).Split('.');
                    var outName = arrName[0];
                    var suffix = _defaultFileSuffix;
                    if (arrName.Length > 1)
                    {
                        suffix = "." + arrName[arrName.Length - 1];
                    }

                    listFiles.Add(new FileInformation
                    {
                        FileName = fi.Name,
                        OutFileName = outName == "Entity" ? String.Empty : outName,
                        FilePath = fi.FullName,
                        FolderPath = fi.DirectoryName,
                        RootPath = relativePath,
                        OutPath = outPath,
                        Suffix = suffix
                    });
                }
                DirectoryInfo[] allDir = dir.GetDirectories();
                foreach (DirectoryInfo d in allDir)
                {
                    listFiles.AddRange(InnerGetAllFiles(d));
                }
                return listFiles;
            }
            return InnerGetAllFiles(new DirectoryInfo(_tempPath + subPath));
        }

        /// <summary>
        /// 删除文件夹下所有文件
        /// </summary>
        public static bool DeleteDir(string path, bool delRoot = false)
        {
            try
            {
                DirectoryInfo fileInfo = new DirectoryInfo(path);
                //判断文件夹是否还存在
                if (Directory.Exists(path))
                {
                    foreach (string f in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDir(f, true);
                        }
                    }

                    //删除空文件夹
                    if (delRoot) Directory.Delete(path);
                }
                return true;
            }
            catch (Exception ex) // 异常处理
            {
                return false;
            }
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        public static void ChangeDataType(List<TableColumn> tableColumns)
        {
            foreach (var item in tableColumns)
            {
                switch (item.data_type_code)
                {
                    case "varchar":
                    case "char":
                    case "text":
                    case "tinytext":
                    case "longtext":
                    case "json":
                        item.data_type = "string"; break;
                    case "int":
                    case "tinyint":
                    case "smallint":
                    case "mediumint":
                    case "integer":
                        item.data_type = "int"; break;
                    case "bigint":
                        item.data_type = "long"; break;
                    case "decimal":
                        item.data_type = "decimal"; break;
                    case "double":
                        item.data_type = "double"; break;
                    case "float":
                        item.data_type = "float"; break;
                    case "date":
                    case "datetime":
                    case "timestamp":
                        item.data_type = "DateTime"; break;
                    case "bit":
                        item.data_type = "bool"; break;
                    default:
                        item.data_type = "string";
                        break;
                }
            }
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        public static AppConfig LoadConfFile()
        {
            if (File.Exists(_confPath))
            {
                var strConfig = ReadFile(_confPath);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<AppConfig>(strConfig);
            }
            return new AppConfig();
        }

        /// <summary>
        /// 获取输出模板
        /// </summary>
        /// <returns></returns>
        public static List<string> GetOutTemplates()
        {
            var dirInfo = new DirectoryInfo(_tempPath);
            if (!dirInfo.Exists) Directory.CreateDirectory(_tempPath);
            return dirInfo.GetDirectories().Select(s => s.Name).ToList();
        }

        /// <summary>
        /// 获取当前版本号
        /// </summary>
        /// <returns></returns>
        public static string ShowVersionInfo()
        {
            //获取文件版本信息
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            var ver = $"{fvi.FileMajorPart}.{fvi.FileMinorPart}";
            return fvi.FileBuildPart > 0 ? $"{ver}.{fvi.FileBuildPart}" : ver;
        }

        /// <summary>
        /// 判断是文件夹
        /// </summary>
        public static bool IsDir(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            return (fi.Attributes & FileAttributes.Directory) != 0;
        }

        public static void writeToConfFile(AppConfig _appConfig)
        {
            Task.Run(() =>
            {
                var strApp = Newtonsoft.Json.JsonConvert.SerializeObject(_appConfig, Newtonsoft.Json.Formatting.Indented);
                WriteToFile(_confPath, strApp);
            });
        }

        public static void OpenWebUrl(string url = "https://blog.renzicu.com/2022/small-coder/")
        {
            System.Diagnostics.Process.Start(url);
        }

        /// <summary>
        /// 当有多显示器时，获取程序所在屏幕上最佳显示位置
        /// </summary>
        /// <param name="control">指定作为参考的窗体</param>
        public static System.Drawing.Point GetBestScreenLocation(Control control, int? curWidth = null, int? curHeight = null)
        {
            var s = Screen.FromControl(control);//当前所在屏幕
            var topX = s.Bounds.X;
            var topY = s.Bounds.Y;

            #region 计算同屏幕上最佳显示位置

            if (curWidth.HasValue && curHeight.HasValue)
            {
                var sWidth = s.WorkingArea.Width;
                var sHeight = s.WorkingArea.Height;
                var remainWidth = (int)Math.Ceiling((decimal)(sWidth - curWidth) / 2);//可利用宽度
                var remainHeigt = (int)Math.Ceiling((decimal)(sHeight - curHeight) / 2);
                var avgRemainWidth = (int)Math.Ceiling((decimal)(remainWidth) / 4);//将可利用宽度均分4份
                var avgRemainHeight = (int)Math.Ceiling((decimal)(remainHeigt) / 4);

                if (avgRemainWidth < 0) avgRemainWidth = 0;
                if (avgRemainHeight < 0) avgRemainHeight = 0;

                topX = _random.Next(topX + avgRemainWidth, topX + avgRemainWidth * 3);//位置在1-3之间是便于阅读的最佳位置
                topY = _random.Next(topY + avgRemainHeight, topY + avgRemainHeight * 3);
            }

            #endregion

            return new System.Drawing.Point(topX, topY);
        }

        public static void CopyDirectory(string sourceDirPath, string SaveDirPath)
        {
            if (!Directory.Exists(SaveDirPath))
            {
                Directory.CreateDirectory(SaveDirPath);
            }
            string[] files = Directory.GetFiles(sourceDirPath);
            //遍历子文件夹的所有文件
            foreach (string file in files)
            {
                string pFilePath = SaveDirPath + "\\" + Path.GetFileName(file);
                if (File.Exists(pFilePath))
                    continue;
                File.Copy(file, pFilePath, true);
            }
            string[] dirs = Directory.GetDirectories(sourceDirPath).Where(w => w != SaveDirPath).ToArray();
            //递归，遍历文件夹
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, SaveDirPath + "\\" + Path.GetFileName(dir));
            }
        }

        /// <summary>
        /// 获取Json中的键
        /// </summary>
        public static MatchCollection RegexJsonKeys(string jsonString)
        {
            MatchCollection matches = Regex.Matches(jsonString ?? string.Empty, @"(\"".+\"":)", RegexOptions.IgnoreCase);
            return matches;
        }

        /// <summary>
        /// 获取屏幕缩放比例
        /// </summary>
        public static double GetScreenScaleValue(Control control)
        {
            Graphics graphics = control.CreateGraphics();
            return Math.Round(graphics.DpiX / 96, 2);
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        /// <summary>
        /// 刷新物理内存到虚拟内存
        /// </summary>
        public static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
    }

    public class FileInformation
    {
        public string FileName { get; set; }
        public string OutFileName { get; set; }
        public string FilePath { get; set; }
        public string FolderPath { get; set; }
        public string OutPath { get; set; }
        /// <summary>
        /// 文件根路径
        /// </summary>
        public string RootPath { get; set; }
        /// <summary>
        /// 文件后缀
        /// </summary>
        public string Suffix { get; set; }
    }
}

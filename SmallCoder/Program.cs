using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SmallCoder
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        internal static string _AppName = "SmallCoder";
        internal static string _Version = Utils.ShowVersionInfo();
        internal static Assembly _Assembly;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            _Assembly = Assembly.GetExecutingAssembly();
            _AppName = _Assembly.GetName().Name;
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Mutex instance = new Mutex(true, _AppName + _Version, out bool createdNew);
            if (!createdNew)
            {
                MessageBox.Show($"{_AppName} 已在其它窗口运行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit(); return;
            }

            Application.Run(new MainForm());
            //Application.Run(new TemplateEditor());
            //Application.Run(new AboutForm());
            instance.ReleaseMutex();
        }
    }
}

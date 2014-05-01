using System;
using System.Windows.Forms;

namespace PrintServer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            CheckRun();
        }
        /// <summary>
        /// 单进程检测
        /// </summary>
        private static void CheckRun()
        {

            bool isapprunning = false;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, System.Diagnostics.Process.GetCurrentProcess().ProcessName,
            out isapprunning);
            if (!isapprunning)
            {
                MessageBox.Show("本程序已经在运行了，请不要重复运行！");
                Environment.Exit(1);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new BackgrounPrintForm());
            }
        }
    }
}

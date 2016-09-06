using System;
using System.Threading;
using System.Windows.Forms;

namespace iQQ.Net.BatchHangQQ
{
    static class Program
    {
        // 保证只能运行一个窗体实例
        private static readonly Mutex MainFormMutex = new Mutex(true, typeof(Program).FullName);

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (MainFormMutex.WaitOne(0, false))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FmQQList());
            }
            else
            {
                MessageBox.Show("程序已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
    }
}

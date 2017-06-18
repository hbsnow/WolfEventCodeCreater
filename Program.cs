using System;
using System.Windows.Forms;
using WolfEventCodeCreater;

namespace WolfEventCodeCreater
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var CodeCreater = new CodeCreater();
            Application.Run(new MainWindow(CodeCreater));
        }
    }
}

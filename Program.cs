using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace SmartFalcon
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
            Form1 form = new Form1();

            System.Windows.Forms.Timer timer = new Timer();
            timer.Tick += new EventHandler(form.Interval);
            timer.Interval = 1000;
            timer.Enabled = true;

            Application.Run();
        }
    }
}

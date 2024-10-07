using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using log4net.Config;
using System.IO;

namespace INA_ACS_Server
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                ConfigData.Load();


                if (IsNewProgram() == false)
                {
                    MessageBox.Show("프로그램을 하나 이상 실행할 수 없습니다.");
                    Application.Exit();
                    return;
                }

                XmlConfigurator.ConfigureAndWatch(new FileInfo("Config/log4net.config"));

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        static bool IsNewProgram()
        {
            using (new Semaphore(0, 1, "INA_ACS_Server", out bool isNewProgram))
            {
                return isNewProgram;
            }
        }
    }
}

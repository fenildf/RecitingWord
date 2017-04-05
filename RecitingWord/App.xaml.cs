using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RecitingWord
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                //数据库初始化
                Mysql.dbConnectInit("", "");
            }
            catch (Exception)
            {
            }

            WordMode wm = new WordMode("Asynchronous");

            wm.AsynTrans();

            MainWindow mw = new MainWindow();
            mw.Show();
            mw.Closed += Closed;
            WordPlayViewMode.Instance.Word = wm;
            Application.Current.Exit += Exit;
            base.OnStartup(e);
        }

        private void Exit(object sender, ExitEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

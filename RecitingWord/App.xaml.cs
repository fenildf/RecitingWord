using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
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
            NORMAN.NRM411S7.CommandLineDebug.Instance.ToString();
            var LoadWindow = new View.Load();
            LoadWindow.Show();
            SettingViewMode.Instance.MySqlConnectionString.ToString();
            Task.Run(()=> {
                try
                {
                    Mysql.dbConnectInit("", "");//数据库初始化

                    Thread.Sleep(5000);
                    MainWindowViewMode.Instance.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });


            MainWindow mw = new MainWindow();
            mw.Show();
            mw.Closed += Closed;
            //WordPlayViewMode.Instance.Word = wm;
            Application.Current.Exit += Exit;
            base.OnStartup(e);
            LoadWindow.Close();
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

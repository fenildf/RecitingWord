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
            MainWindow mw = new MainWindow();
            mw.Show();
            base.OnStartup(e);
        }
    }
}

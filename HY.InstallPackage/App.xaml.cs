using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;

namespace HY.InstallPackage
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool createdNew;
            string mutexName = "InstallPackage";
            Mutex singleInstanceWatcher = new Mutex(false, mutexName, out createdNew);
            if (!createdNew)
            {
                Environment.Exit(-1);
            }
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("HY Main");
            if (p != null && p.Count() > 0)
            {
                MessageBox.Show("请退出正在运行的“黑鹰 Hub”程序");
                Environment.Exit(0);
            }
        }
    }
}

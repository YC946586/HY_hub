﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using UpdateWindowProgram.Helpers;

namespace HY.UpdateWindowProgram
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                ApplicationPath.JavaApiHttpAddress = e.Args[0];
            }
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
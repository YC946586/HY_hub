using HY.Application.Base;
using HY_Main.Common.Unity;
using HY_Main.ViewModel.Sign;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HY_Main
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //http://v.bidsneo.com/hub/index.html#id=sfmm4t&p=%E7%89%88%E6%9C%AC%E8%AF%B4%E6%98%8E

        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            //IOC接口注册
            BootStrapper.Initialize();
            LoginViewModel view = new LoginViewModel();
            view.ReadConfigInfo(); //读写配置参数
            var Dialog = ServiceProvider.Instance.Get<IModelDialog>("LoginViewDlg");
            Dialog.BindViewModel(view);
            Dialog.ShowDialog();
        }
    }
}

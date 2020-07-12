using HY.Application.Base;
using HY.Client.Execute.Commons;
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
        //http://118.31.16.221:8012/eagle/swagger-ui.html


        protected override void OnStartup(StartupEventArgs e)
        {
            ////加载主题文件
            System.Windows.Application.Current.Resources.MergedDictionaries.Clear();
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri(
                    "pack://application:,,,/HY.CustomControl;component/Themes/SkinDefault.xaml",
                    UriKind.RelativeOrAbsolute)
            });
            var MacAddress = MacAddressHelper.GetMacByIpConfig() ?? MacAddressHelper.GetMacByWmi().FirstOrDefault() ?? "unknown";
            if (!string.IsNullOrEmpty(MacAddress))
            {
                Loginer.LoginerUser.macAdd = MacAddress;
            }
            else
            {
                MessageBox.Show("网络连接失败,请您稍后重新运行");
                Current.Shutdown();
                return;
            }
          
            base.OnStartup(e);
            //IOC接口注册
            BootStrapper.Initialize();
            LoginViewModel view = new LoginViewModel();
            var Dialog = ServiceProvider.Instance.Get<IModelDialog>("LoginViewDlg");
            view.ReadConfigInfo(); //读写配置参数
            Dialog.BindViewModel(view);
            Dialog.ShowDialog();
        }
    }
}

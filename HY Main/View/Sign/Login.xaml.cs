using GalaSoft.MvvmLight.Messaging;
using HY_Main.ViewModel.Sign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HY_Main.View.Sign
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            Messenger.Default.Register<object>(this, "ShowPassword", ShowPassword);

        }

        private void ShowPassword(object obj)
        {
            //pass.Password = obj;
        } 
    }

    public class ValueEditorTemplateSelector : DataTemplateSelector
    {
        ///// <summary>
        ///// 基本Tableitem模板
        ///// </summary>
        //public DataTemplate RegisteredDataTemplate { get; set; }

        ///// <summary>
        ///// 特殊Tableitem模板
        ///// </summary>
        //public DataTemplate ResetDataTemplate { get; set; }
        ///// <summary>
        ///// 重写模板选择器方法
        ///// </summary>
        ///// <param name="item"></param>
        ///// <param name="container"></param>
        ///// <returns></returns>
        //public override DataTemplate SelectTemplate(object item, DependencyObject container)
        //{
        //    try
        //    {
        //        var tab = item as LoginViewModel;
        //        switch (tab.TemplateType)
        //        {
        //            case "RegisteredDataTemplate":
        //                return RegisteredDataTemplate;
        //            case "ResetDataTemplate":
        //                return ResetDataTemplate;
        //            default:
        //                break;
        //        }
        //        return base.SelectTemplate(item, container);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
    }


}

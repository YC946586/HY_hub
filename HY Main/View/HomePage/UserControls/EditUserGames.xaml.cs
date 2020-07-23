using GalaSoft.MvvmLight.Messaging;
using HY.Application.Base;
using HY_Main.ViewDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HY_Main.View.HomePage.UserControls
{
    /// <summary>
    /// EditUserGames.xaml 的交互逻辑
    /// </summary>
    public partial class EditUserGames : UserControl
    {
        public EditUserGames()
        {
            InitializeComponent();
        }
    }
    /// <summary>
    /// 消息处理-HOST
    /// </summary>
    public class EditUserGamesDlg : BaseViewDialog<EditUserGames>, IModelDialog
    {
    }
    //public class EditUserGamesDlg : BaseViewDialog<EditUserGames>, IModelDialog
    //{
    //    public override void BindViewModel<TViewModel>(TViewModel viewModel)
    //    {
    //        GetDialogWindow().DataContext = viewModel;
    //    }

    //    public override void Close()
    //    {
    //        GetDialogWindow().Close();
    //    }

    //    public override Task<bool> ShowDialog()
    //    {
    //        GetDialogWindow().ShowDialog();
    //        return Task.FromResult(true);
    //    }

    //    public override void RegisterDefaultEvent()
    //    {
    //        GetDialogWindow().MouseDown += (sender, e) => { if (e.LeftButton == MouseButtonState.Pressed) { GetDialogWindow().DragMove(); } };
    //        Messenger.Default.Register<string>(GetDialogWindow(), "EditPwdClose", new Action<string>((msg) =>
    //        {
    //            this.Close();
    //        }));

    //    }

    //    private Window GetDialogWindow()
    //    {
    //        return GetDialog() as Window;
    //    }
    //}
}

using GalaSoft.MvvmLight.Messaging;
using HY.Application.Base;
using HY_Main.View.Sign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HY_Main.ViewDialog.Sign
{
    /// <summary>
    /// 登录窗口
    /// </summary>
    public class LoginViewDlg : BaseViewDialog<Login>, IModelDialog
    {
        public override void BindViewModel<TViewModel>(TViewModel viewModel)
        {
            GetDialogWindow().DataContext = viewModel;
        }

        public override void Close()
        {
            GetDialogWindow().Close();
        }

        public override Task<bool> ShowDialog()
        {
            GetDialogWindow().ShowDialog();
            return Task.FromResult(true);
        }

        public override void RegisterDefaultEvent()
        {
            GetDialogWindow().MouseDown += (sender, e) => { if (e.LeftButton == MouseButtonState.Pressed) { GetDialogWindow().DragMove(); } };
            Messenger.Default.Register(GetDialogWindow(), "ApplicationHiding", new Action<string>((msg) => { GetDialogWindow().Close(); }));
            Messenger.Default.Register(GetDialogWindow(), "ApplicationShutdown", new Action<string>((arg) => { Application.Current.Shutdown(); }));
        }

        private Window GetDialogWindow()
        {
            return GetDialog() as Window;
        }
    }
}

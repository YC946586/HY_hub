using GalaSoft.MvvmLight.Messaging;
using HY.Application.Base;
using HY_Main.Common.CoreLib;
using HY_Main.Common.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HY_Main.ViewDialog.Step
{
    /// <summary>
    /// 消息处理-Window
    /// </summary>
    public class MsgBoxViewDlg : BaseViewDialog<MsgBox>, IModelDialog
    {

        public override void BindViewModel<TViewModel>(TViewModel viewModel)
        {
            GetDialogWindow().DataContext = viewModel;
        }

        public override Task<bool> ShowDialog()
        {
            var dialog = GetDialogWindow();
            dialog.ShowDialog();
            return Task.FromResult((dialog.DataContext as BaseDialogOperation).Result);
        }

        public override void RegisterDefaultEvent()
        {
            GetDialogWindow().MouseDown += (sender, e) => { if (e.LeftButton == MouseButtonState.Pressed) { GetDialogWindow().DragMove(); } };
            Messenger.Default.Register<string>(GetDialogWindow(), "DialogClose", new Action<string>((msg) =>
            {
                GetDialogWindow().Close();
            }));

        }

        private Window GetDialogWindow()
        {
            return GetDialog() as Window;
        }
    }
}

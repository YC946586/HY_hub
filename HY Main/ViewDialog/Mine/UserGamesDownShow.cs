using GalaSoft.MvvmLight.Messaging;
using HY.Application.Base;
using HY_Main.View.Mine.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HY_Main.ViewDialog.Mine
{
    public class UserGamesDownShowDlg : BaseViewDialog<UserGamesDownShow>, IModelDialog
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
            GetDialogWindow().Show();
            return Task.FromResult(true);
        }

        public override void RegisterDefaultEvent()
        {
            GetDialogWindow().MouseDown += (sender, e) => { if (e.LeftButton == MouseButtonState.Pressed) { GetDialogWindow().DragMove(); } };
            Messenger.Default.Register<string>(GetDialogWindow(), "UserGamesDownShowClose", new Action<string>((msg) =>
            {
                this.Close();
            }));

        }

        private Window GetDialogWindow()
        {
            return GetDialog() as Window;
        }
    }
}

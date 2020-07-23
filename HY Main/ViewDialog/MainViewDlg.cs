using GalaSoft.MvvmLight.Messaging;
using HY.Application.Base;
using HY.Client.Execute.Commons;
using HY_Main.Common.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HY_Main.ViewDialog
{
    /// <summary>
    /// 首页
    /// </summary>
    public class MainViewDlg : BaseViewDialog<MainWindow>, IModelDialog
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
            Messenger.Default.Register<string>(GetDialogWindow(), "MainExit", new Action<string>(async (msg) =>
            {
                if (CommonsCall.UserGames.Count!=0)
                {
                    if (await Msg.Question("当前有正在下载/安装的游戏,是否继续退出"))
                    {
                        this.Close();
                    }
                }
                else
                {
                    if (await Msg.Question("是否退出黑鹰Hub"))
                    {
                        this.Close();
                    }
                }
            }));
            Messenger.Default.Register<string>(GetDialogWindow(), "MinWindow", new Action<string>((msg) => { GetDialogWindow().WindowState = WindowState.Minimized; }));
            Messenger.Default.Register<bool>(GetDialogWindow(), "MaxWindow", new Action<bool>((arg) =>
            {
                var win = GetDialogWindow();
                if (win.WindowState == WindowState.Maximized)
                    win.WindowState = WindowState.Normal;
                else
                    win.WindowState = WindowState.Maximized;
            }));
        }

        private Window GetDialogWindow()
        {
            return GetDialog() as Window;
        }
    }
}

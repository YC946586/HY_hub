using GalaSoft.MvvmLight.Messaging;
using HY.Application.Base;
using HY.Client.Entity.CommonEntitys;
using HY.Client.Execute.Commons;
using HY_Main.Common.CoreLib;
using HY_Main.Common.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HY_Main.ViewModel.Sign
{
    public class UpdateViewModel : BaseDialogOperation
    {
        private Visibility _progress = Visibility.Collapsed;
        private double _proData;
        private string _speed = "开始更新程序...";
        public Visibility Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                RaisePropertyChanged();
            }
        }
     
        /// <summary>
        /// 进度条
        /// </summary>
        public double ProData
        {
            get { return _proData; }
            set
            {
                _proData = value;
                RaisePropertyChanged();
            }
        }
        public string Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                RaisePropertyChanged();
            }
        }
        public VersionEntity Version { get; set; } = new VersionEntity();
        public void InitViewModel(VersionEntity _version)
        {
            Version = _version;
        }
        /// <summary>
        /// 确定更新
        /// </summary>
        public override async void Save()
        {
            try
            {
                ProData = 10;
                Progress = Visibility.Visible;
                Speed = "检测文件数量";
                var appURL = AppDomain.CurrentDomain.BaseDirectory;
                File.Move(appURL+ "HY Main.exe", appURL+"程序更新.txt");
                //foreach (var item in Version.attaches)
                //{
                
                //}
                WebClient web = new WebClient();
                Speed = "正在下载文件";
                web.DownloadFile(
                    new Uri(Version.attaches.First().fileUrl),
                    appURL + "HY Main.jpg");
                Thread.Sleep(5000);
                Speed = "更新完成!";
                CommonsCall.RecordVersion(Version.version);
                if (await Msg.Question("更新成功是否现在启动？"))
                {
                    Application.Current.Shutdown(0);
                    System.Diagnostics.Process.Start(appURL + "HY Main.exe");
                    //LoginViewModel view = new LoginViewModel();
                    //var Dialog = ServiceProvider.Instance.Get<IModelDialog>("LoginViewDlg");
                    //view.ReadConfigInfo(); //读写配置参数
                    //Dialog.BindViewModel(view);
                    //Messenger.Default.Send(string.Empty, "ApplicationHiding");
                    //await Dialog.ShowDialog();
                }
                else
                {
                    base.Save();
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
            finally
            {
                ProData = 100;
                Progress = Visibility.Collapsed;
            }

        }
        
    }
}

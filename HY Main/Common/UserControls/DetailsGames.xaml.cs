using GalaSoft.MvvmLight.Messaging;
using HY.Client.Entity.HomeEntitys;
using HY_Main.ViewModel.Step;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace HY_Main.Common.UserControls
{
    /// <summary>
    /// DetailsGames.xaml 的交互逻辑
    /// </summary>
    public partial class DetailsGames : UserControl
    {
        public DetailsGames()
        {
            InitializeComponent();
            Messenger.Default.Register<object>(this, "ShowVideo", ShowVideo);
        }

        private void ShowVideo(object model)
        {
            Load.Visibility = Visibility.Visible;
            Task.Run(() =>
            {
                try
                {
                    var viewModel = model as Recommendgame;
                    string _videoPath = System.IO.Directory.GetCurrentDirectory() + @"\DownLoad_Video" + viewModel.title + ".mp4";
                    if (!File.Exists(_videoPath))
                    {
                        _videoPath = DownLoad_Video(viewModel);
                    }
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MediaPlayer.Source = new Uri(_videoPath);
                        MediaPlayer.Play();
                    }));
                

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Load.Visibility = Visibility.Collapsed;
                    }));
                  
                }
            });
           
           
        }

        private string DownLoad_Video(Recommendgame model)
        {
            try
            {
                string pathUrl = model.videoUrl;
                System.Net.HttpWebRequest request = null;
                System.Net.HttpWebResponse response = null;
                //请求网络路径地址
                request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(pathUrl);
                request.Timeout = 5000; // 超时时间
                                        //获得请求结果
                response = (System.Net.HttpWebResponse)request.GetResponse();
                //文件下载地址
                string path = System.IO.Directory.GetCurrentDirectory() + @"\DownLoad_Video";

                // 如果不存在就创建file文件夹
                if (!Directory.Exists(path))
                {
                    if (path != null) Directory.CreateDirectory(path);
                }
                path = path + model.title+".mp4";
                Stream stream = response.GetResponseStream();
                //先创建文件
                Stream sos = new System.IO.FileStream(path, System.IO.FileMode.Create);
                byte[] img = new byte[1024];
                int total = stream.Read(img, 0, img.Length);
                while (total > 0)
                {
                    //之后再输出内容
                    sos.Write(img, 0, total);
                    total = stream.Read(img, 0, img.Length);
                }
                stream.Close();
                stream.Dispose();
                sos.Close();
                sos.Dispose();
                return path;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}

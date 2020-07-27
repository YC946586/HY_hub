using HY.Client.Entity.CommonEntitys;
using HY.Client.Entity.UserEntitys;
using HY.Client.Execute.Commons;
using HY.Client.Execute.Commons.Download;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using HY_Main.ViewModel.HomePage.UserControls;
using ICSharpCode.SharpZipLib.Zip;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HY_Main.ViewModel.Mine.UserControls
{
    public class GameDwonloadViewModel : BaseOperation<UserGamesEntity>
    {

        public event Action ShowList;

        public event Action<string, string> stuepEnd;
        private UserGamesEntity _pageCollection = new UserGamesEntity();


        public UserGamesEntity PageCollection
        {
            get { return _pageCollection; }
            set
            {
                _pageCollection = value;
                RaisePropertyChanged();
            }
        }

        public List<DwonloadEntity> dwonloadEntities { get; set; }
        public void InitAsyncViewModel()
        {
            base.InitViewModel();
            PageCollection.fileSize = dwonloadEntities.Sum(s => s.size);
            var drive = DriveInfo.GetDrives();
            if (drive.Length != 0)
            {
                var driveDate = drive.Where(s => s.IsReady).ToList();
                if (driveDate.Any())
                {
                    if (driveDate.Count > 1)
                    {
                        PageCollection.StrupPath = driveDate[1].Name + "HYhubInstallation";
                    }
                    else
                    {
                        PageCollection.StrupPath = driveDate[0].Name + "HYhubInstallation";
                    }
                }
            }
            PageCollection.IsSelected = false;
            PageCollection.GameSize = CommonsCall.ConvertByG(PageCollection.fileSize);


        }
        /// <summary>
        /// 选择安装路径
        /// </summary>
        public override void Reset()
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择安装路径";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    var drive = fbd.SelectedPath.Substring(0, 3);
                    DriveInfo d = new DriveInfo(drive);
                    if (d.TotalFreeSpace <= PageCollection.fileSize)
                    {
                        Msg.Info("磁盘大小不足,请您选择其他路径");
                        return;
                    }
                    if (CommonsCall.WordsIScn(fbd.SelectedPath))
                    {
                        Msg.Info("当前路径包含中文,请重新选择");
                        return;
                    }
                    PageCollection.StrupPath = fbd.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }

        public List<Task> ParallelTasks { get; set; }

        public static List<MeterInfo> takMeter = new List<MeterInfo>();
        /// <summary>
        /// 下载文件
        /// </summary>
        public override async void Query()
        {
            try
            {
              
                //Task.Run(async () =>
                //{
                foreach (var item in dwonloadEntities)
                {
                    var strFileName = AppDomain.CurrentDomain.BaseDirectory + @"DownloadGeam\" + item.name;
                    var file = new FileInfo(strFileName);
                    var progress = new Progress<DownloadProgress>();
                    DownloadProgress downloadProgress = null;
                    var obj = new object();
                    progress.ProgressChanged += (sender, p) =>
                    {
                        lock (obj)
                        {
                            downloadProgress = p;
                        }
                    };
                    bool finished = false;
                    long lastLength = 0;
                    DateTime lastTime = DateTime.Now;

                    _ = Task.Run(async () =>
                    {
                        while (!finished)
                        {
                            lock (obj)
                            {
                                if (downloadProgress == null)
                                {
                                    continue;
                                }

                       
                                Console.WriteLine($"Download url = {item.url}");
                                Console.WriteLine($"Output = {strFileName}");
                                var ss= ($"{downloadProgress.DownloadedLength}/{downloadProgress.FileLength}");
                                var ee= ( $"Process {downloadProgress.DownloadedLength * 100.0 / downloadProgress.FileLength:0.00}");
                                var dd= ($"{(downloadProgress.DownloadedLength - lastLength) * 1000.0 / (DateTime.Now - lastTime).TotalMilliseconds / 1024 / 1024:0.00} MB/s");
                                lastLength = downloadProgress.DownloadedLength;
                                lastTime = DateTime.Now;

                                foreach (var userGames in CommonsCall.UserGames)
                                {
                                    if (downloadProgress.DwonloadModel.gameId.Equals(userGames.gameId))
                                    {
                                        userGames.SurplusSize = CommonsCall.ConvertByG(downloadProgress.DownloadedLength);
                                        userGames.Speed = dd;
                                    }
                                }
                                //首页下载进度
                                var SurplusSizeList = CommonsCall.UserGames.Sum(s => Convert.ToDouble(s.SurplusSize));

                                CommonsCall.DownProgress = CommonsCall.UserGames.Sum(s => Convert.ToDouble(s.GameSize)) + "G / " + SurplusSizeList + "G";
                                //foreach (var downloadSegment in downloadProgress.GetCurrentDownloadSegmentList())
                                //{
                                //    Console.WriteLine(downloadSegment);
                                //}
                            }
                            await Task.Delay(500);
                        }
                    });

                    var segmentFileDownloader = new SegmentFileDownloader(item, file, progress);
                    CommonsCall.UserGames.Add(PageCollection);
                    await segmentFileDownloader.DownloadFile(PageCollection);
                    finished = true;
                    Compress(PageCollection.StrupPath, strFileName);
                    CommonsCall.DeleteDir(strFileName);
                }

                //ParallelTasks = new List<Task>();
                //var MeterInfo = new MeterInfo();
                //CancellationTokenSource tokenSource = new CancellationTokenSource();
                //CancellationToken token = tokenSource.Token;
                //ManualResetEvent resetEvent = new ManualResetEvent(true);
                //MeterInfo.gamesId = PageCollection.gameId;
                //MeterInfo.manualReset = resetEvent;
                //takMeter.Add(MeterInfo);

                //foreach (var item in dwonloadEntities)
                //{
                //    Task task = new Task( () =>
                //    {
                //        while (true)
                //        {
                //            if (token.IsCancellationRequested)
                //            {
                //                return;
                //            }
                //            resetEvent.WaitOne();
                //            DownloadFile(item);
                //            GC.Collect();
                //            var strFileName = AppDomain.CurrentDomain.BaseDirectory + @"DownloadGeam\" + item.name;
                //            Compress(PageCollection.StrupPath, strFileName);
                //            CommonsCall.DeleteDir(strFileName);
                //            return;
                //        }

                //    }, token);
                //    task.Start();
                //    ParallelTasks.Add(task);
                //}
                //Task.WaitAll(ParallelTasks.ToArray());
                //resetEvent.Close();
                //GC.Collect();
                ////strFileName
                //System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    CommonsCall.UserGames.Remove(PageCollection);
                //}));
                //var pathName = dwonloadEntities.First().name;
                //pathName = pathName.Substring(0, pathName.Length - 4);
                //var path = PageCollection.StrupPath +"\\" + pathName;
                //if (PageCollection.IsSelected)
                //{
                //    CreateShortcut(PageCollection.title+".lnk", path, PageCollection.title);
                //}
                //stuepEnd?.Invoke(path + @"\" + PageCollection.startFileName, PageCollection.startFileName);
                //CommonsCall.HyGameInstall(PageCollection.gameId.ToString(), path + @"\" + PageCollection.startFileName, PageCollection.startFileName);
                //});


            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
            finally
            {
                ShowList?.Invoke();
            }


            //IStore store = BridgeFactory.BridgeManager.GetStoreManager();
            //var genrator = await store.GetGameFiles(PageCollection.id);
            //if (!genrator.code.Equals("000"))
            //{
            //    HY.Client.Execute.Commons.Msg.Info(genrator.Msg);
            //}
            //else
            //{
            //    var Results = JsonConvert.DeserializeObject<List<string>>(genrator.result.ToString());
            //    var gramPath = AppDomain.CurrentDomain.BaseDirectory + @"DownloadGeam\" + PageCollection.fileDir+"\\";

            //}

        }

        public static void ResetTask(int gamesId, string content)
        {
            foreach (var item in takMeter)
            {
                if (item.gamesId.Equals(gamesId))
                {
                    if (content.Equals("暂停"))
                    {
                        item.manualReset.Reset();
                    }
                    else
                    {
                        item.manualReset.Set();
                    }

                }
            }


        }

        #region  解压
        /// <summary>
        /// 解压Zip
        /// </summary>
        /// <param name="DirPath">解压后存放路径</param>
        /// <param name="ZipPath">Zip的存放路径</param>
        /// <param name="ZipPWD">解压密码（null代表无密码）</param>
        /// <returns></returns>
        public static string Compress(string DirPath, string ZipPath)
        {
            FastZip fz = new FastZip();
            string state = "Fail...";
            try
            {
                fz.ExtractZip(ZipPath, DirPath, null);

                state = "Success !";
            }
            catch (Exception ex)
            {
                state += "," + ex.Message;
            }
            return state;
        }

        /// <summary>
        /// 创建快捷方式
        /// </summary>
        /// <param name="shortcutPath"></param>
        /// <param name="path">快捷方式的保存路径</param>
        public void CreateShortcut(string shortcutPath, string path, string fileName)
        {
            try
            {
                RegistryKey hkeyCurrentUser =
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders");
                if (hkeyCurrentUser != null)
                {
                    string desktopPath = hkeyCurrentUser.GetValue("Desktop").ToString(); //获取桌面文件夹路径
                                                                                         //实例化WshShell对象 
                    WshShell shell = new WshShell();

                    //通过该对象的 CreateShortcut 方法来创建 IWshShortcut 接口的实例对象 
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(desktopPath + @"\" + shortcutPath);

                    //设置快捷方式的目标所在的位置(源程序完整路径) 
                    var dd = path + @"\" + PageCollection.startFileName;
                    shortcut.TargetPath = dd;
                    shortcut.WindowStyle = 1;//设置运行方式，默认为常规窗口
                    shortcut.Description = "洋葱";//设置备注
                                                //快捷方式的描述 
                    shortcut.Description = PageCollection.title;
                    shortcut.IconLocation = path + @"\" + "Icon.ico";  //快捷方式图标

                    //保存快捷方式 
                    shortcut.Save();

                    Process.Start(path + @"\" + PageCollection.startFileName);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region  下载

        /// <summary>
        /// 以断点续传方式下载文件
        /// </summary>
        /// <param name="strFileName">下载文件的保存路径</param>
        /// <param name="strUrl">文件下载地址</param>
        public void DownloadFile(DwonloadEntity dwonloadEntity)
        {
            string strUrl = dwonloadEntity.url;
            var strFileName = AppDomain.CurrentDomain.BaseDirectory + @"DownloadGeam\" + dwonloadEntity.name;
            //打开上次下载的文件或新建文件
            long SPosition = 0;
            FileStream FStream;
            if (System.IO.File.Exists(strFileName))
            {
                FStream = System.IO.File.OpenWrite(strFileName);
                SPosition = FStream.Length;
                FStream.Seek(SPosition, SeekOrigin.Current);//移动文件流中的当前指针
                dwonloadEntity.SurplusSize = SPosition;
            }
            else
            {
                FStream = new FileStream(strFileName, FileMode.Create);
                SPosition = 0;
            }
            Down(strUrl, SPosition, FStream, dwonloadEntity);
        }

        //锁
        private readonly object _objLock = new object();

        private void Down(string strUrl, long SPosition, FileStream FStream, DwonloadEntity dwonloadEntity)
        {
            //打开网络连接
            try
            {
                WebClient webClient = new WebClient();
                using (Stream read = webClient.OpenRead(strUrl))
                {
                    byte[] mbyte = new byte[1024 * 1024];
                    int readL = read.Read(mbyte, 0, 1024 * 1024);
                    using (Stream fs = FStream)
                    {
                        //读取流 
                        while (readL != 0)
                        {
                            fs.Write(mbyte, 0, readL);
                            readL = read.Read(mbyte, 0, 1024 * 1024);
                            dwonloadEntity.SurplusSize += readL;
                            Task.Run(() => AddSupSize(dwonloadEntity));
                        }
                    }
                    GC.Collect();
                }


            }
            catch (Exception ex)
            {
                //FStream.Close();
                Down(strUrl, SPosition, FStream, dwonloadEntity);
            }
            finally
            {
                if (dwonloadEntity.size == dwonloadEntity.SurplusSize)
                {
                    FStream.Close();
                }
            }
        }
        private void AddSupSize(DwonloadEntity dwonloadEntity)
        {
            try
            {
                PageCollection.SurplusSize = CommonsCall.ConvertByG((dwonloadEntities.Sum(s => s.SurplusSize)));
                foreach (var item in CommonsCall.UserGames)
                {
                    if (dwonloadEntity.gameId.Equals(item.id))
                    {
                        item.SurplusSize = CommonsCall.ConvertByG(dwonloadEntity.SurplusSize);
                    }

                    //if (PageCollection.gameId.Equals(item.gameId))
                    //{
                    //    item.SurplusSize = PageCollection.SurplusSize;
                    //}
                }

                //首页下载进度
                var SurplusSizeList = CommonsCall.UserGames.Sum(s => Convert.ToDouble(s.SurplusSize));

                CommonsCall.DownProgress = CommonsCall.UserGames.Sum(s => Convert.ToDouble(s.GameSize)) + "G / " + SurplusSizeList + "G";

            }
            catch (Exception ex)
            {

            }
        } /// <summary>
          /// 获取文件下载速度
          /// </summary>
          /// <param name="fileSizeBytpeLength"></param>
          /// <param name="elapsedMilliseconds"></param>
          /// <returns></returns>
        public static double GetDonwloadRate(long fileSizeBytpeLength, long elapsedMilliseconds)
        {
            if (fileSizeBytpeLength == 0) return 0;
            if (elapsedMilliseconds == 0) return 0;
            double fe = fileSizeBytpeLength / elapsedMilliseconds;
            return Math.Round(fe * 1000 / 1024d, 2);
        }
        #endregion



    }
    public class MeterInfo
    {
        public MeterInfo()
        {

        }
        public ManualResetEvent manualReset { get; set; }
        public int gamesId { get; set; }

    }
}

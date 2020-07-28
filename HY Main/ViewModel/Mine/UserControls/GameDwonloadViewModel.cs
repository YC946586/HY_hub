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
using System.Timers;
using System.Windows.Forms;

namespace HY_Main.ViewModel.Mine.UserControls
{
    public class GameDwonloadViewModel : BaseOperation<UserGamesEntity>
    {

        public event Action ShowList;
        public event Action stuepGo;

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

        public Dictionary<int, long> DicLastDic = new Dictionary<int, long>();

        private System.Timers.Timer aTimer;
        private int threadCount = 0;

        private Thread thread;
        private AutoResetEvent autoViewEvent;
        /// <summary>
        /// 下载文件
        /// </summary>
        public override void Query()
        {
            try
            {
                stuepGo?.Invoke();
                //计时器  
                System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
                tm.Interval = 1;
                tm.Tick += new EventHandler(tm_Tick);
                autoViewEvent = new AutoResetEvent(false);
                MeterInfo meterInfo = new MeterInfo() { gamesId = PageCollection.gameId, manualReset = tm };
                takMeter.Add(meterInfo);
                tm.Start();


                #region  下载方法
                List<dynamic> dynamics = new List<dynamic>();
                dwonloadEntities.ForEach((ary) =>
                {
                    ary.downStuep = PageCollection.StrupPath;
                    dynamics.Add(ary);
                });
                PageCollection.dwonloadAllEntities = dynamics;
                CommonsCall.UserGames.Add(PageCollection);
                thread = new Thread(() =>
                {
                    autoViewEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行  
                    threadCount = 0;
                    if (dwonloadEntities.Count==1)
                    {
                        threadCount = 1;
                    }
                    dwonloadEntities.ForEach(d =>
                    {
                        autoViewEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行  
                        d.Down_tm = new System.Windows.Forms.Timer();
                        d.Down_tm.Interval = 1;
                        d.autoEvent.Set(); //通知阻塞的线程继续执行  
                        d.Down_tm.Tick += new EventHandler(d.tm_Tick);
                        d.Down_tm.Start();
                        threadCount++;

                        d.td = new Thread(() =>
                        {
                            threadCount= d.DownFile(d.url, threadCount);
                        });
                        d.td.IsBackground = false;
                        d.td.Start();
                        while (threadCount > 1)
                        {
                            Thread.Sleep(2);
                        }
                        GC.Collect();
                    });
                    GC.Collect();
                    //創建快捷方式

                    var curGameId = dwonloadEntities.First().gameId;

                    var curModel = CommonsCall.UserGames.Where(s => s.gameId.Equals(curGameId)).First();
                    
                    var pathName = curModel.dwonloadAllEntities.First().name.Substring(0, curModel.dwonloadAllEntities.First().name.Length - 4);
                    var path = curModel.StrupPath + "\\" + pathName+"\\"+ curModel.startFileName;
                    if (curModel.IsSelected)
                    {
                        CommonsCall.CreateShortcut(curModel);
                    }
                     stuepEnd?.Invoke(path, curModel.startFileName);
                     CommonsCall.HyGameInstall(curGameId.ToString(), path, curModel.startFileName);

                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CommonsCall.UserGames.Remove(curModel);
                    }));
                });
                thread.IsBackground = false;
                thread.Start();

                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = 2000;
                aTimer.Enabled = true;
                aTimer.Start();
                #endregion
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
            finally
            {
                ShowList?.Invoke();
            }
        }

        private void tm_Tick(object sender, EventArgs e)
        {
            try
            {
                autoViewEvent.Set(); //通知阻塞的线程继续执行  

            }
            catch (Exception)
            {

                throw;
            }
        }

        private string DownPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + @"DownloadGeam\";
        /// <summary>
        /// 检查速度和进度还有剩余时间
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                autoViewEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行  
                //总大小
                long allSize = 0;
                //在2秒钟下载了多少次
                int downCount = 0;
                //已经下载的字节数
                long allReadyDownsize = 0;
                var curGuid = dwonloadEntities.First().gameId;
                dwonloadEntities.ForEach(d =>
                {
                    allSize += d.size;
                    downCount += d.downCount;
                    string StrFileName = DownPath + d.name; //根据实际情况设置 
                    if (System.IO.File.Exists(StrFileName))
                    {
                        allReadyDownsize += new FileInfo(StrFileName).Length;
                    }
                    d.downCount = 0;
                });
                //每秒下载的字节数
                long BytesOneSecondDownload = downCount * 512 / 2;

                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    #region 计算速度
                    double speed = downCount * 512 / 2048;
                    string Speed = string.Empty;
                    if (speed > 1024)
                    {
                        Speed = (speed / 1024).ToString("0.00") + " MB/S";
                    }
                    else
                    {
                        Speed = speed.ToString() + " KB/S";
                    }
                    #endregion
                    //计算已经下载了多少字节
                    var Progress = ConvertFileSize(allReadyDownsize);
                    long rt = 0;
                    if (BytesOneSecondDownload != 0)
                    {
                        rt = (allSize - allReadyDownsize) / BytesOneSecondDownload;
                        //lbRemainingTime.Content = "剩余时间" + rt + "秒";
                    }
                    foreach (var temo in CommonsCall.UserGames)
                    {
                        if (curGuid.Equals(temo.gameId))
                        {
                            temo.downCont = allReadyDownsize;
                            temo.SurplusSize = Progress;
                            temo.Speed = Speed;
                            temo.RemainingTime = rt.ToString();
                        }
                    }
                    var dlastList = ConvertFileSize(CommonsCall.UserGames.Sum(s => s.downCont));
                    CommonsCall.DownProgress = CommonsCall.UserGames.Sum(s => Convert.ToDouble(s.GameSize)) + "G / " + dlastList + "   下载速度为:" + Speed;
                }));

                downCount = 0;
            }
            catch (Exception ex)
            {

            }
        }  /// <summary>
           /// 将文件大小(字节)转换为最适合的显示方式
           /// </summary>
           /// <param name="size"></param>
           /// <returns></returns>
        private string ConvertFileSize(long size)
        {
            string result = "0KB";
            int filelength = size.ToString().Length;
            if (filelength < 4)
                result = size + "byte";
            else if (filelength < 7)
                result = Math.Round(Convert.ToDouble(size / 1024d), 2) + "KB";
            else if (filelength < 10)
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
            else if (filelength < 13)
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
            else
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
            return result;
        }


        public void ResetTask(string content, UserGamesEntity userGames)
        {
            foreach (var item in takMeter)
            {
                if (item.gamesId.Equals(userGames.gameId))
                {
                    switch (content)
                    {
                        case "暂停":
                            {
                                item.manualReset.Stop();
                                userGames.dwonloadAllEntities.ForEach((d) =>
                                {
                                    d.StopDown();
                                });
                                Thread.Sleep(1000);

                                break;
                            }
                        case "继续":
                            {
                                item.manualReset.Start();
                                userGames.dwonloadAllEntities.ForEach((d) =>
                                {
                                    d.StartDown();
                                });
                                Thread.Sleep(1000);


                                break;
                            }
                        case "取消":
                            {
                                item.manualReset.Stop();
                                userGames.dwonloadAllEntities.ForEach((d) =>
                                {
                                    d.CloseDown();
                                });
                                Thread.Sleep(1000);
                                var curRemove = CommonsCall.UserGames.Where(s => s.gameId.Equals(userGames.gameId));
                                if (curRemove.Any())
                                {
                                    CommonsCall.UserGames.Remove(curRemove.First());
                                }
                                if (CommonsCall.UserGames.Count==0)
                                {
                                    CommonsCall.DownProgress = "";

                                }
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }
    }

   
    public class MeterInfo
    {
        public MeterInfo()
        {

        }
        public System.Windows.Forms.Timer manualReset { get; set; }
        public int gamesId { get; set; }

    }
}

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
                            d.DownFile(d.url, threadCount);
                        });
                        d.td.IsBackground = false;
                        d.td.Start();
                        while (threadCount > 0)
                        {
                            Thread.Sleep(2);
                        }
                        GC.Collect();
                    });
                    GC.Collect();
                    //創建快捷方式
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CommonsCall.UserGames.Remove(PageCollection);
                    }));
                    var path = PageCollection.StrupPath + "\\" + PageCollection.cateName;
                    if (PageCollection.IsSelected)
                    {
                        //CreateShortcut(PageCollection.title + ".lnk", path, PageCollection.title);
                    }
                    //    stuepEnd?.Invoke(path + @"\" + PageCollection.startFileName, PageCollection.startFileName);
                    //    CommonsCall.HyGameInstall(PageCollection.gameId.ToString(), path + @"\" + PageCollection.startFileName, PageCollection.startFileName);
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
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class DwonloadEntity
    {

        public int id { get; set; }
        public int gameId { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public long size { get; set; }
        public int createTime { get; set; }
        public long SurplusSize { get; set; }
        public long lastLength { get; set; }
        /// <summary>
        /// 安裝地址
        /// </summary>
        public string downStuep { get; set; } = string.Empty;
        #region 下载文件相关
        /// <summary>
        /// 字节下载次数计数
        /// </summary>
        public int downCount = 0;

        /// <summary>
        /// 已经下载的字节数
        /// </summary>
        public long allReadyDownSize = 0;

        /// <summary>
        /// 文件下载文件流
        /// </summary>
        public System.IO.FileStream fs { get; set; }

        public System.IO.Stream ns { get; set; }

        /// <summary>
        /// 文件云端地址
        /// </summary>
        public string fileCloudUrl { get; set; }

        /// <summary>
        /// 文件下载到本地地址
        /// </summary>
        public string fileLocalPath { get; set; }

        /// <summary>
        /// 文件下载线程
        /// </summary>
        public Thread td { get; set; }

        private string DownPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + @"DownloadGeam\";
        #endregion

        #region  线程管理
        //计时器  
        public System.Windows.Forms.Timer Down_tm = new System.Windows.Forms.Timer();

        public AutoResetEvent autoEvent = new AutoResetEvent(false);


        #endregion

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="localFilePath"></param>
        /// <param name="url"></param>
        public void DownFile(string url, int threadCount)
        {
            autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行  
            string StrFileName = DownPath + name; //根据实际情况设置 
            string StrUrl = url; //根据实际情况设置
            //打开上次下载的文件或新建文件 
            long lStartPos = 0;
            if (System.IO.File.Exists(StrFileName))//另外如果文件已经下载完毕，就不需要再断点续传了，不然请求的range 会不合法会抛出异常。
            {
                fs = System.IO.File.OpenWrite(StrFileName);
                lStartPos = fs.Length;
                fs.Seek(lStartPos, System.IO.SeekOrigin.Current); //移动文件流中的当前指针 
            }
            else
            {
                fs = new FileStream(StrFileName, System.IO.FileMode.Create);
                lStartPos = 0;
            }
            //打开网络连接 
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(StrUrl);
                if (lStartPos > 0)
                {
                    request.AddRange((int)lStartPos); //设置Range值
                }
                request.Timeout = 20000;
                System.Net.WebResponse response = request.GetResponse();
                //向服务器请求，获得服务器回应数据流 
                ns = response.GetResponseStream();
                long totalSize = response.ContentLength;
                long hasDownSize = 0;
                byte[] nbytes = new byte[512];//521,2048 etc
                int nReadSize = 0;
                nReadSize = ns.Read(nbytes, 0, nbytes.Length);
                while (nReadSize > 0)
                {
                    fs.Write(nbytes, 0, nReadSize);
                    downCount++;
                    nReadSize = ns.Read(nbytes, 0, 512);
                    hasDownSize += nReadSize;
                }
                CommonsCall.Compress(downStuep, StrFileName);
                CommonsCall.DeleteDir(StrFileName);
                fs.Close();
                ns.Close();
                response.Dispose();
                GC.Collect();
            }
            catch (ThreadAbortException e)
            {
                Thread.Sleep(10000);
                DownFile(url, threadCount);
            }
            catch (Exception ex)
            {
                fs.Close();
                MessageBox.Show("下载过程中出现错误,请重试！");
            }
            finally
            {
                threadCount--;
            }
        }

        //计时器 事件  
        public void tm_Tick(object sender, EventArgs e)
        {
            autoEvent.Set(); //通知阻塞的线程继续执行  
        }
        /// <summary>
        /// 暂停下载
        /// </summary>
        public void StopDown()
        {
            Down_tm.Stop();
            //if (td != null)
            //{
            //    td.Abort();
            //}
            //if (fs != null)
            //{
            //    fs.Close();
            //}
            //if (ns != null)
            //{
            //    ns.Close();
            //}
        }
        /// <summary>
        /// 继续下载
        /// </summary>
        public void StartDown()
        {
            Down_tm.Start();
            //if (td != null)
            //{
            //    td.Start();
            //}
        }
        /// <summary>
        /// 取消下载
        /// </summary>
        public void CloseDown()
        {
            if (td != null)
            {
                td.Abort();
            }
            if (fs != null)
            {
                fs.Close();
            }
            if (ns != null)
            {
                ns.Close();
            }
            Down_tm = new System.Windows.Forms.Timer();
            autoEvent = new AutoResetEvent(false);
            string StrFileName = DownPath + name; //根据实际情况设置 
            CommonsCall.DeleteDir(StrFileName);

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

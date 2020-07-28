using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons
{
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
                byte[] nbytes = new byte[1024 * 2];//521,2048 etc
                int nReadSize = 0;
                nReadSize = ns.Read(nbytes, 0, nbytes.Length);
                while (nReadSize > 0)
                {
                    fs.Write(nbytes, 0, nReadSize);
                    downCount++;
                    nReadSize = ns.Read(nbytes, 0, 1024 * 2);
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
                Thread.Sleep(10000);
                DownFile(url, threadCount);

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

        }
        /// <summary>
        /// 继续下载
        /// </summary>
        public void StartDown()
        {
            Down_tm.Start();

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
}

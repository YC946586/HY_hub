using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons
{
   public  class DownLoadHelp
    {
        #region 以断点续传方式下载文件
        /// <summary>
        /// 以断点续传方式下载文件
        /// </summary>
        /// <param name="strFileName">下载文件的保存路径</param>
        /// <param name="strUrl">文件下载地址</param>
        public void DownloadFile(string strFileName, string strUrl)
        {
            float percent = 0;
            //打开上次下载的文件或新建文件
            long SPosition = 0;
            FileStream FStream;
            if (File.Exists(strFileName))
            {
                FStream = File.OpenWrite(strFileName);
                SPosition = FStream.Length;
                FStream.Seek(SPosition, SeekOrigin.Current);//移动文件流中的当前指针
            }
            else
            {
                FStream = new FileStream(strFileName, FileMode.Create);
                SPosition = 0;
            }
            //打开网络连接
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                if (SPosition > 0)
                    myRequest.AddRange((int)SPosition);//设置Range值
                long totalBytes = myRequest.ContentLength;
                
                //向服务器请求，获得服务器的回应数据流
                Stream myStream = myRequest.GetResponse().GetResponseStream();
                byte[] btContent = new byte[1024];
                int intSize = 0;
                long totalDownloadedByte = 0;
                intSize = myStream.Read(btContent, 0, 1024);
                while (intSize > 0)
                {
                    totalDownloadedByte = intSize + totalDownloadedByte;
                    FStream.Write(btContent, 0, intSize);
                    intSize = myStream.Read(btContent, 0, 1024);

                    percent = totalDownloadedByte / (float)totalBytes * 100;
                }
                FStream.Close();
                myStream.Close();
                
            }
            catch
            {
                FStream.Close();
            }
        }
        #endregion
    }
}

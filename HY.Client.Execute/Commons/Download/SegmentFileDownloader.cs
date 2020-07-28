using HY.Client.Execute.Commons.Download.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons.Download
{
    public class SegmentFileDownloader
    {
        public SegmentFileDownloader(string url, FileInfo file,  IProgress<DownloadProgress> progress)
        {
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _progress = progress ?? throw new ArgumentNullException(nameof(progress));

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            Url = url;
            File = file ?? throw new ArgumentNullException(nameof(file));
 
        }

        public string Url { get; }

        private FileInfo File { get; }

        public long lStartPos { get; set; }
        /// <summary>
        /// 开始下载文件
        /// </summary>
        /// <returns></returns>
        public async Task DownloadFile()
        {
          
            var (response, contentLength) = await GetContentLength();
             lStartPos = 0;
            if (System.IO.File.Exists(File.FullName))//另外如果文件已经下载完毕，就不需要再断点续传了，不然请求的range 会不合法会抛出异常。
            {
                FileStream = System.IO.File.OpenWrite(File.FullName);
                lStartPos = FileStream.Length;
                FileStream.Seek(lStartPos, System.IO.SeekOrigin.Current); //移动文件流中的当前指针 
            }
            else
            {
                FileStream = new FileStream(File.FullName, System.IO.FileMode.Create);
                lStartPos = 0;
            }
            FileStream.SetLength(contentLength);
            FileWriter = new RandomFileWriter(FileStream);

            SegmentManager = new SegmentManager(contentLength);

            _progress.Report(new DownloadProgress($"file length = {contentLength}", SegmentManager));

            var downloadSegment = SegmentManager.GetNewDownloadSegment();

            // 下载第一段
            Download(response, downloadSegment);

            var supportSegment = await TryDownloadLast(contentLength);

            var threadCount = 1;

            if (supportSegment)
            {
                // 多创建几个线程下载
                threadCount = 10;

                for (var i = 0; i < threadCount; i++)
                {
                    Download(SegmentManager.GetNewDownloadSegment());
                }
            }

            for (var i = 0; i < threadCount; i++)
            {
                _ = Task.Run(DownloadTask);
            }

            await FileDownloadTask.Task;
        }

      
        private readonly IProgress<DownloadProgress> _progress;

        private bool _isDisposed;

        private RandomFileWriter FileWriter { set; get; }

        private FileStream FileStream { set; get; }

        private TaskCompletionSource<bool> FileDownloadTask { get; } = new TaskCompletionSource<bool>();

        private SegmentManager SegmentManager { set; get; }

        /// <summary>
        /// 获取整个下载的长度
        /// </summary>
        /// <returns></returns>
        private async Task<(WebResponse response, long contentLength)> GetContentLength()
        {
         

            var response = await GetWebResponseAsync();

            if (response == null)
            {
                return default;
            }

            var contentLength = response.ContentLength;

            //_logger.LogInformation(
            //    $"完成获取文件长度，文件长度 {contentLength} {contentLength / 1024}KB {contentLength / 1024.0 / 1024.0:0.00}MB");

            return (response, contentLength);
        }

        private async Task<WebResponse> GetWebResponseAsync(Action<HttpWebRequest> action = null)
        {
            for (var i = 0; !_isDisposed; i++)
            {
                try
                {
                    var url = Url;
                    var webRequest = (HttpWebRequest)WebRequest.Create(url);
                    if (lStartPos > 0)
                    {
                        webRequest.AddRange((int)lStartPos); //设置Range值
                    }
                    webRequest.Method = "GET";
                    webRequest.Timeout = 20000;
                    action?.Invoke(webRequest);

                    var response = await webRequest.GetResponseAsync();
                    return response;
                }
                catch (InvalidCastException)
                {
                    throw;
                }
                catch (NotSupportedException)
                {
                    throw;
                }
                catch (ArgumentException)
                {
                    throw;
                }
                catch (Exception e)
                {
                   
                }

                // 后续需要配置不断下降时间
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }

            return null;
        }

        /// <summary>
        /// 尝试获取链接响应
        /// </summary>
        /// <param name="downloadSegment"></param>
        /// <returns></returns>
        private async Task<WebResponse> GetWebResponse(DownloadSegment downloadSegment)
        {
          

            // 为什么不使用 StartPoint 而是使用 CurrentDownloadPoint 是因为需要处理重试

            var response = await GetWebResponseAsync(webRequest => webRequest.AddRange(downloadSegment.CurrentDownloadPoint, downloadSegment.RequirementDownloadPoint));
            return response;
        }

        private async Task DownloadTask()
        {
            while (!SegmentManager.IsFinished())
            {
                var data = await DownloadDataList.DequeueAsync();

                var downloadSegment = data.DownloadSegment;
 

                using var response = data.WebResponse ?? await GetWebResponse(downloadSegment);

                try
                {
                    using Stream responseStream = response.GetResponseStream();
                    const int length = 1024*2;
                    Debug.Assert(responseStream != null, nameof(responseStream) + " != null");

                    while (!downloadSegment.Finished)
                    {
                        var buffer = SharedArrayPool.Rent(length);
                        var n = await responseStream.ReadAsync(buffer, 0, length);

                        if (n < 0)
                        {
                            break;
                        }

                    
                        var task = FileWriter.WriteAsync(downloadSegment.CurrentDownloadPoint, buffer, 0, n);
                        _ = task.ContinueWith(_ => SharedArrayPool.Return(buffer));

                        downloadSegment.DownloadedLength += n;

                        _progress.Report(new DownloadProgress(SegmentManager));

                        if (downloadSegment.Finished)
                        {
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                  

                    // 下载失败了，那么放回去继续下载
                    Download(downloadSegment);
                }

                // 下载比较快，尝试再分配一段下载
                if (downloadSegment.RequirementDownloadPoint - downloadSegment.StartPoint > 1024 * 1024)
                {
                    Download(SegmentManager.GetNewDownloadSegment());
                }
            }

            await FinishDownload();
        }

        private void Download(WebResponse webResponse, DownloadSegment downloadSegment)
        {
            DownloadDataList.Enqueue(new DownloadData(webResponse, downloadSegment));
        }

        private void Download(DownloadSegment downloadSegment)
        {
            Download(null, downloadSegment);
        }

        private AsyncQueue<DownloadData> DownloadDataList { get; } = new AsyncQueue<DownloadData>();

        private async Task FinishDownload()
        {
            if (_isDisposed)
            {
                return;
            }

            lock (FileDownloadTask)
            {
                if (_isDisposed)
                {
                    return;
                }

                _isDisposed = true;
            }

            await FileWriter.DisposeAsync();
          

            FileDownloadTask.SetResult(true);
        }

        private async Task<bool> TryDownloadLast(long contentLength)
        {
            // 尝试下载后部分，如果可以下载后续的 100 个字节，那么这个链接支持分段下载
            const int downloadLength = 100;

            var startPoint = contentLength - downloadLength;

            var responseLast = await GetWebResponseAsync(webRequest =>
            {
                webRequest.AddRange(startPoint, contentLength);
            });

            if (responseLast.ContentLength == downloadLength)
            {
                var downloadSegment = new DownloadSegment(startPoint, contentLength);
                SegmentManager.RegisterDownloadSegment(downloadSegment);

                Download(responseLast, downloadSegment);

                return true;
            }

            return false;
        }

        private class DownloadData
        {
            public DownloadData(WebResponse webResponse, DownloadSegment downloadSegment)
            {
                WebResponse = webResponse;
                DownloadSegment = downloadSegment;
            }

            public WebResponse WebResponse { get; }

            public DownloadSegment DownloadSegment { get; }
        }
    }
}

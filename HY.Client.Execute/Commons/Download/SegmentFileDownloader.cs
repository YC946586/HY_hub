using HY.Client.Entity.CommonEntitys;
using HY.Client.Entity.UserEntitys;
using HY.Client.Execute.Commons.Download.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons.Download
{
    public class SegmentFileDownloader
    {
        public SegmentFileDownloader(DwonloadEntity item, FileInfo file, IProgress<DownloadProgress> progress)
        {
          
            _progress = progress ?? throw new ArgumentNullException(nameof(progress));

            if (string.IsNullOrEmpty(item.url))
            {
                throw new ArgumentNullException(nameof(item.url));
            }
    
            Url = item.url;
            Dwonload = item;
            File = file ?? throw new ArgumentNullException(nameof(file));
        }

        public string Url { get; }

        private FileInfo File { get; }

        private DwonloadEntity Dwonload { get; }
        /// <summary>
        /// 开始下载文件
        /// </summary>
        /// <returns></returns>
        public async Task DownloadFile()
        {
          
            var (response, contentLength) = await GetContentLength();

            FileStream = File.Create();
            FileStream.SetLength(contentLength);
            FileWriter = new RandomFileWriter(FileStream);

            SegmentManager = new SegmentManager(contentLength);
          
        
            //_progress.Report(new DownloadProgress($"file length = {contentLength}", SegmentManager));

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
            //开始获取整个下载长度
            var response = await GetWebResponseAsync();

            if (response == null)
            {
                return default;
            }

            var contentLength = response.ContentLength;

            //_logger.LogInformation(
            //    $"完成获取文件长度，文件长度 {contentLength} {contentLength / 1024}KB {contentLength / 1024.0 / 1024.0:0.00}MB");
            //var dd = "完成获取文件长度，文件长度 "{contentLength} {contentLength / 1024}"KB"+ {contentLength / 1024.0 / 1024.0:0.00}MB";
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
                    //次获取长度失败
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
            //_logger.LogInformation(
            //    $"Start Get WebResponse{downloadSegment.StartPoint}-{downloadSegment.CurrentDownloadPoint}/{downloadSegment.RequirementDownloadPoint}");

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
                downloadSegment.DwonloadModel = data.DownloadModel;
                //_logger.LogInformation(
                //    $"Download {downloadSegment.StartPoint}-{downloadSegment.CurrentDownloadPoint}/{downloadSegment.RequirementDownloadPoint}");

                using var response = data.WebResponse ?? await GetWebResponse(downloadSegment);

                try
                {
                    using Stream responseStream = response.GetResponseStream();
                    const int length = 1024;
                    //Debug.Assert(responseStream != null, nameof(responseStream) + " != null");

                    while (!downloadSegment.Finished)
                    {
                        var buffer = SharedArrayPool.Rent(length);
                        var n = await responseStream.ReadAsync(buffer, 0, length);

                        if (n < 0)
                        {
                            break;
                        }

                        //_logger.LogInformation(
                        //    $"Download  {downloadSegment.CurrentDownloadPoint * 100.0 / downloadSegment.RequirementDownloadPoint:0.00} Thread {Thread.CurrentThread.ManagedThreadId} {downloadSegment.StartPoint}-{downloadSegment.CurrentDownloadPoint}/{downloadSegment.RequirementDownloadPoint}");
                        var dd = ($"Download  {downloadSegment.CurrentDownloadPoint * 100.0 / downloadSegment.RequirementDownloadPoint:0.00} Thread {Thread.CurrentThread.ManagedThreadId} {downloadSegment.StartPoint}-{downloadSegment.CurrentDownloadPoint}/{downloadSegment.RequirementDownloadPoint}");
                        var task = FileWriter.WriteAsync(downloadSegment.CurrentDownloadPoint, buffer, 0, n);
                        _ = task.ContinueWith(_ => SharedArrayPool.Return(buffer));

                        downloadSegment.DownloadedLength += n;
                        //downloadSegment.DwonloadModel.SurplusSize = downloadSegment.DownloadedLength;
                        //AddSupSize(downloadSegment.DwonloadModel);
                        _progress.Report(new DownloadProgress(SegmentManager, data.DownloadModel));

                        if (downloadSegment.Finished)
                        {
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    //_logger.LogInformation(
                    //    $"Download {downloadSegment.StartPoint}-{downloadSegment.RequirementDownloadPoint} error {e}");

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
        private void AddSupSize(DwonloadEntity dwonloadEntity)
        {
            try
            {
                //foreach (var item in CommonsCall.UserGames)
                //{
                //    if (dwonloadEntity.gameId.Equals(item.gameId))
                //    {
                //        item.SurplusSize = CommonsCall.ConvertByG(dwonloadEntity.SurplusSize);
                //    }

                //    //if (PageCollection.gameId.Equals(item.gameId))
                //    //{
                //    //    item.SurplusSize = PageCollection.SurplusSize;
                //    //}
                //}

                ////首页下载进度
                //var SurplusSizeList = CommonsCall.UserGames.Sum(s => Convert.ToDouble(s.SurplusSize));

                //CommonsCall.DownProgress = CommonsCall.UserGames.Sum(s => Convert.ToDouble(s.GameSize)) + "G / " + SurplusSizeList + "G";

            }
            catch (Exception ex)
            {

            }
        } /// <summary>
        private void Download(WebResponse webResponse, DownloadSegment downloadSegment)
        {
            DownloadDataList.Enqueue(new DownloadData(webResponse, downloadSegment, Dwonload));
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
            FileStream.Close();

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
            public DownloadData(WebResponse webResponse, DownloadSegment downloadSegment, DwonloadEntity dwonload)
            {
                WebResponse = webResponse;
                DownloadSegment = downloadSegment;
                DownloadModel = dwonload;
            }

            public WebResponse WebResponse { get; }

            public DownloadSegment DownloadSegment { get; }

            public DwonloadEntity DownloadModel { get; }
        }
    }
}

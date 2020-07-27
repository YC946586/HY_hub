using HY.Client.Entity.CommonEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons.Download
{
    public class DownloadProgress
    {
        public DownloadProgress(SegmentManager segmentManager, DwonloadEntity dwonload)
        {
            SegmentManager = segmentManager;
            DwonloadModel = dwonload;
        }

        public DownloadProgress(string message, SegmentManager segmentManager)
        {
            Message = message;
            SegmentManager = segmentManager;
        }


        public DwonloadEntity DwonloadModel { get; set; } = new DwonloadEntity();
        public string Message { get; } = "";

        public long DownloadedLength => SegmentManager.GetDownloadedLength();

        public long FileLength => SegmentManager.FileLength;

        private SegmentManager SegmentManager { get; }

        /// <summary>
        /// 获取当前所有下载段
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<DownloadSegment> GetCurrentDownloadSegmentList()
        {
            return SegmentManager.GetCurrentDownloadSegmentList();
        }
    }
}

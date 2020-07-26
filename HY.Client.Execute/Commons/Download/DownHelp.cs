using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons.Download
{
   public  class DownHelp
    {
        /// <summary>
        /// 根据文件大小和下载速度计算剩余下载时间
        /// </summary>
        /// <param name="Size">文件大小，单位MB</param>
        /// <param name="Speed">下载速度，单位KB/s</param>
        /// <returns>返回剩余时间（含单位）</returns>
        public static string DownloadTime(double Size, double Speed)
        {
            //MessageBox.Show("70/60:" + 59 / 60 + "\n70%60:" + 59 % 60);
            double secondsRemaining = Size * 1024 / Speed;//剩余秒数
            int minutesRemaining = Convert.ToInt32(secondsRemaining) / 60;//剩余分钟
            int hoursRemaining = minutesRemaining / 60;//剩余小时
            int daysRemaining = hoursRemaining / 24;//剩余天数


            //MessageBox.Show((time % 60).ToString());
            if (secondsRemaining < 60)//不超过1分钟
            {
                return secondsRemaining + "秒";
            }
            else//超过1分钟
            {
                if (minutesRemaining < 60)//不超过1小时
                {
                    //double[] minsec = intdec(minutesRemaining);
                    //MessageBox.Show("1:" + minsec[0] + "\n2:" + Math.Round(minsec[1]*60,7) + "\n3:" + Math.Ceiling(50.6));
                    //return minsec[0] + "分钟" + Math.Ceiling(minsec[1] * 60) + "秒";
                    return minutesRemaining + "分钟" + Math.Ceiling(secondsRemaining % 60) + "秒";
                }
                else//超过1小时
                {
                    if (hoursRemaining < 24)//不超过1天
                    {
                        return hoursRemaining + "小时" + minutesRemaining % 60 + "分钟" + Math.Ceiling(secondsRemaining % 60) + "秒";
                    }
                    else//超过1天
                    {
                        return daysRemaining + "天" + hoursRemaining % 24 + "小时" + minutesRemaining % 60 + "分钟" + Math.Ceiling(secondsRemaining % 60) + "秒";
                    }
                }
            }
        }
    }
}

using HandyControl.Controls;
using HY.Client.Entity.HomeEntitys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons
{
    public class CommonsCall
    {
        ///<summary>
        /// 通过WMI读取系统信息里的网卡MAC
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByWMI()
        {
            List<string> macs = new List<string>();
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        macs.Add(mac);
                    }
                }
                moc = null;
                mc = null;
            }
            catch
            {
            }

            return macs;
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed(int next)
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng =
                new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            var seed = BitConverter.ToInt32(bytes, 0);
            var random = new Random(seed).Next(next);

            return random;

        }
        /// <summary>
        /// 点击获取游戏
        /// </summary>
        public static void GaemeDownload(GetCommonUseGamesEntity gamesEntity)
        {
            try
            {
                //判断是否为会员
                if (Loginer.LoginerUser.IsAdmin)
                {
                    if (Loginer.LoginerUser.vipValidTo != null)
                    {


                    }
                    else
                    {
                        if (Message.Question("会员已经到期,是否续费"))
                        {
                            Message.Info("跳到充值");
                        }
                    }

                    //if (Loginer.LoginerUser.vipType.Equals("月费用户"))
                    //{

                    //}
                }
                else
                {
                    if (Loginer.LoginerUser.balance <= 0 || Loginer.LoginerUser.balance < gamesEntity.price)
                    {
                        if (Message.Question("黑鹰币不足，是否立即充值"))
                        {
                            Message.Info("跳到充值");
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }


        public static string ConvertByG(float fileSize)
        {
            try
            {
                var dd = fileSize / 1024;
                return dd.ToString("F2");
            }
            catch (Exception)
            {

                throw;
            }
        }

    }  
}

using HY.Client.Execute.Commons.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons
{
    /// <summary>
    /// 配置消息参数
    /// </summary>
    public class Configuration
    {
        public const string ADD_MSG = "添加完成";

        public const string UPDATE_MSG = "更新完成";

        public const string DELETE_MSG = "删除完成";
    }

    /// <summary>
    /// 
    /// </summary>
    public class SerivceFiguration
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        public const string INI_CFG = "config\\user.ini";

        /// <summary>
        /// 获取本地样式参数
        /// </summary>
        /// <returns></returns>
        public static string GetSkin()
        {
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + INI_CFG;
            if (File.Exists(cfgINI))
            {
                IniFile ini = new IniFile(cfgINI);
                string SkinName = ini.IniReadValue("Skin", "Skin");
                return SkinName;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 设置样式
        /// </summary>
        /// <param name="SkinName"></param>
        public static void SetKin(string SkinName)
        {
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + INI_CFG;
            IniFile ini = new IniFile(cfgINI);
            ini.IniWriteValue("Skin", "Skin", SkinName);
        }
    }
}

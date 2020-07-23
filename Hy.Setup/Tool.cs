using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Hy.Setup
{
    public class Tool
    {
        /// <summary>
        /// 获取用户当前操作系统
        /// </summary>
        /// <returns></returns>
        public static string GetWindows()
        {
            try
            {
                Version ver = Environment.OSVersion.Version;
                string strClient = "";
                if (ver.Major == 5 && ver.Minor == 1)
                {
                    strClient = "Windows XP";
                }
                else if (ver.Major == 6 && ver.Minor == 1)
                {
                    strClient = "Windows 7";
                }
                else if (ver.Major == 5 && ver.Minor == 0)
                {
                    strClient = "Windows 2000";
                }
                else if (ver.Major == 6 && ver.Minor == 2)
                {
                    strClient = "Windows 8 ";
                }
                else if (ver.Major == 6 && ver.Minor == 3)
                {
                    strClient = "Windows 8.1";
                }
                else if (ver.Major == 10 && ver.Minor == 0)
                {
                    strClient = "Windows 10";
                }
                else
                {
                    strClient = "其他操作系统";
                }
                return strClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 检查是否已经安装了NET中文语言包
        /// </summary>
        /// <returns>返回检查结果</returns>
        public static bool CheckNetLanguage()
        {
            try
            {
                //获取注册表的HKEY_LOCAL_MACHINE节点
                RegistryKey hkeyLocalMachine = Registry.LocalMachine;
                //获取注册表HKEY_LOCAL_MACHINE节点下面的Language节点
                RegistryKey netFrameworkLanguage =
                    hkeyLocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\");
                if (netFrameworkLanguage != null)
                {
                    int releaseKey = Convert.ToInt32(netFrameworkLanguage.GetValue("Release"));
                    if (releaseKey >= 393295)
                    {
                        return true; //"4.6 or later";
                    }
                    if ((releaseKey >= 379893))
                    {
                        return true; //"4.5.2 or later";
                    }
                    if ((releaseKey >= 378675))
                    {
                        return true; //"4.5.1 or later";
                    }
                    if ((releaseKey >= 378389))
                    {
                        return true; //"4.5 or later";
                    }
                    hkeyLocalMachine.Close();
                    netFrameworkLanguage.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 检测VC+=2013是否安装
        /// </summary>
        /// <returns></returns>
        public static bool CheckVc2013()
        {
            try
            {
                //*2013(X64) { 929FBD26 - 9020 - 399B - 9A7A - 751D61F0B942}
                //*2013(X86) { 13A4EE12 - 23EA - 3371 - 91EE - EFB36DDFFF3E}
                RegistryKey cuKey = Registry.LocalMachine;
                RegistryKey cnctkjptKey = cuKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\", true);
                if (cnctkjptKey != null)
                {
                    var check =
                        cnctkjptKey.GetSubKeyNames()
                            .Where(
                                s =>
                                    s.Contains("FF66E9F6-83E7-3A3E-AF14-8DE9A809A6A4") ||
                                    s.Contains("5e4b593b-ca3c-429c-bc49-b51cbf46e72a") ||
                                    s.Contains("13A4EE12-23EA-3371-91EE-EFB36DDFFF3E") ||
                                    s.Contains("929FBD26-9020-399B-9A7A-751D61F0B942"));
                    if (check.Any())
                    {
                        return true;
                    }
                    cuKey.Close();
                    cnctkjptKey.Close();
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 打开VC++2013安装
        /// </summary>
        public static void OpenVcredist()
        {
            var curPath = AppDomain.CurrentDomain.BaseDirectory + "\\EnvLibrary\\";
            Process.Start(curPath + "vcredist2013_x86.exe");
        }





        /// <summary>
        /// 创建快捷方式
        /// </summary>
        /// <param name="shortcutPath"></param>
        /// <param name="path">快捷方式的保存路径</param>
        public static void CreateShortcut(string shortcutPath, string path, string fileName)
        {
            try
            {
                //实例化WshShell对象 
                WshShell shell = new WshShell();

                //通过该对象的 CreateShortcut 方法来创建 IWshShortcut 接口的实例对象 
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

                //设置快捷方式的目标所在的位置(源程序完整路径) 
                shortcut.TargetPath = path + @"\" + fileName;

                //快捷方式的描述 
                shortcut.Description = "黑鹰Hub";

                //保存快捷方式 
                shortcut.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using GalaSoft.MvvmLight;
using HY.MAIN.Properties;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Hy.Setup.ViewModel
{

    public class MainViewModel : baseViewModel
    {
        public MainViewModel()
        {
            ////检测.net版本 如果没有安装 就去下载
            if (!Tool.CheckNetLanguage())
            {
                Process.Start(@"https://dotnet.microsoft.com/download/thank-you/net452?survey=false");
                MessageBox.Show("检测到您暂未安装.NET4.5,请先安装.NET4.5", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //检测VC++2013
            //if (!Tool.CheckVc2013())
            //{
            //    Process.Start(@"https://www.microsoft.com/en-us/download/confirmation.aspx?id=40784");

            //    MessageBox.Show("检测到您暂未安装VC++2013,请先安装VC++2013", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            //获取电脑盘符
            var drive = DriveInfo.GetDrives();
            if (drive.Length != 0)
            {
                var driveDate = drive.Where(s => s.IsReady).ToList();
                if (driveDate.Any())
                {
                    if (driveDate.Count > 1)
                    {
                        PageCollection.StrupPath = driveDate[1].Name + "HyInstallPackage";
                    }
                    else
                    {
                        PageCollection.StrupPath = driveDate[0].Name + "HyInstallPackage";
                    }
                }
            }
        }

        /// <summary>
        /// 选择安装目录
        /// </summary>
        public override void Browse()
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择安装路径";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    PageCollection.StrupPath = fbd.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 开始解压 实现安装
        /// </summary>
        public override void GetAllDirFiles()
        {
            try
            {
                //将软件解压到用户指定目录
                var filesPath = Resources.Release;
                Extract(filesPath);
                Adddesktop();
            }
            catch (Exception ex)
            {
                _notsucces = "失败";
                Msg = ex.Message;
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 增加桌面图标
        /// </summary>
        private void Adddesktop()
        {
            try
            {
                //删除注册表数据
                DeleteStartMenuShortcuts(PageCollection.StrupPath);
                //添加开始菜单快捷方式
                RegistryKey hkeyCurrentUser =
                    Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders");

                if (hkeyCurrentUser != null)
                {

                    string programsPath = hkeyCurrentUser.GetValue("Programs").ToString(); //获取开始菜单程序文件夹路径
                    Directory.CreateDirectory(programsPath + @"\黑鹰Hub"); //在程序文件夹中创建快捷方式的文件夹

                    PageCollection.Message = "添加开始菜单快捷方式";

                    Tool.CreateShortcut(programsPath + @"\黑鹰Hub.lnk", PageCollection.StrupPath, appName);

                    PageCollection.Message = "添加卸载目录";
                    Tool.CreateShortcut(programsPath + @"\黑鹰Hub.lnk", PageCollection.StrupPath,
                        uninstallName); //创建卸载快捷方式
                    //添加桌面快捷方式
                    string desktopPath = hkeyCurrentUser.GetValue("Desktop").ToString(); //获取桌面文件夹路径
                    PageCollection.Message = "添加桌面图标";
                    Tool.CreateShortcut(desktopPath + @"\黑鹰Hub.lnk", PageCollection.StrupPath, appName); //创建快捷方式

                    PageCollection.Schedule = 100;
                    PageCollection.Plah = "100";
                    PageCollection.GridHide = 3;
                    PageCollection.Message = "黑鹰Hub安装完成";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 删除开始栏快捷方式
        /// </summary>
        /// <param name="bBase"></param>
        /// <returns></returns>
        public override void DeleteStartMenuShortcuts(string stpPath)
        {
            try
            {
                RegistryKey cuKey = Registry.LocalMachine;
                RegistryKey cnctkjptKey = cuKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall", true);

                //获取AutoCAD软件快捷方式在桌面上的路径
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string pathCom = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                //返回桌面上*.lnk文件的集合
                string[] items = Directory.GetFiles(path, "*.lnk");
                string[] itemsCom = Directory.GetFiles(pathCom, "*.lnk");

                #region 删除Uninstall注册表

                if (cnctkjptKey != null)
                {
                    foreach (string aimKey in cnctkjptKey.GetSubKeyNames())
                    {
                        if (aimKey == "HyInstallPackage")
                            cnctkjptKey.DeleteSubKeyTree("HyInstallPackage");
                    }
                    cnctkjptKey.Close();
                }

                cuKey.Close();


                #endregion

                #region 删除桌面快捷方式

                foreach (string item in items)
                {
                    Console.WriteLine(item);
                    if (item.Contains("黑鹰Hub") && item.Contains(".lnk"))
                    {
                        File.Delete(item);
                    }
                    else if (item.Contains("卸载黑鹰Hub") && item.Contains(".lnk"))
                    {
                        File.Delete(item);
                    }
                }
                foreach (string item in itemsCom)
                {
                    Console.WriteLine(item);
                    if (item.Contains("黑鹰Hub") && item.Contains(".lnk"))
                    {
                        File.Delete(item);
                    }
                    else if (item.Contains("卸载黑鹰Hub") && item.Contains(".lnk"))
                    {
                        File.Delete(item);
                    }

                }

                #endregion


            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
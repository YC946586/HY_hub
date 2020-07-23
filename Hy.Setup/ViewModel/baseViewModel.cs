using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace Hy.Setup.ViewModel
{
    public class baseViewModel : ViewModelBase
    {
        #region 属性

        /// <summary>
        /// 是否安装成功
        /// </summary>
        public string _notsucces = "成功";

        public string Msg = "";

        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string appName = "HY Main.exe";

        /// <summary>
        /// 卸载程序名称
        /// </summary>
        public string uninstallName = "Uninstall.exe";


        public SetupModel _pageCollection = new SetupModel();

        /// <summary>
        /// 界面数据
        /// </summary>
        public SetupModel PageCollection
        {
            get { return _pageCollection; }
            set
            {
                _pageCollection = value;
                RaisePropertyChanged("PageCollection");
            }
        }

        #endregion

        #region  命令

        private RelayCommand _setupCommand;

        /// <summary>
        ///  安装按钮
        /// </summary>
        public RelayCommand SetupCommand
        {
            get
            {
                if (_setupCommand == null)
                {
                    _setupCommand = new RelayCommand(Setup);
                }
                return _setupCommand;
            }
        }

        private RelayCommand _exitCommand;

        /// <summary>
        /// 关闭页
        /// </summary>
        public RelayCommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand(Close);
                }
                return _exitCommand;
            }
            set { _exitCommand = value; }
        }

        private RelayCommand _customCommand;

        /// <summary>
        /// 自定义安装
        /// </summary>
        public RelayCommand CustomCommand
        {
            get
            {
                if (_customCommand == null)
                {
                    _customCommand = new RelayCommand(Custom);
                }
                return _customCommand;
            }
            set { _customCommand = value; }
        }

        private RelayCommand _browseCommand;

        /// <summary>
        /// 选择安装目录
        /// </summary>
        public RelayCommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                {
                    _browseCommand = new RelayCommand(Browse);
                }
                return _browseCommand;
            }
            set { _browseCommand = value; }
        }

        private RelayCommand _sigeCommand;

        /// <summary>
        /// 立即体验
        /// </summary>
        public RelayCommand SigeCommand
        {
            get
            {
                if (_sigeCommand == null)
                {
                    _sigeCommand = new RelayCommand(Sige);
                }
                return _sigeCommand;
            }
            set { _sigeCommand = value; }
        }


        #endregion

        #region 方法


        /// <summary>
        /// 立即安装 GGOGOGOGO
        /// </summary>
        public void Setup()
        {
            try
            {
                PageCollection.GridHide = 2;
                //创建用户指定的安装目录文件夹
                if (!Directory.Exists(PageCollection.StrupPath))
                {
                    Directory.CreateDirectory(PageCollection.StrupPath);
                    DirectoryInfo dir = new DirectoryInfo(PageCollection.StrupPath);
                    dir.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                }
                else
                {
                    FileInfo fi = new FileInfo(PageCollection.StrupPath);
                    //判断文件属性是否只读?是则修改为一般属性再删除
                    fi.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                }
                //解压文件
                Thread thread = new Thread(GetAllDirFiles);
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 选择安装目录
        /// </summary>
        public virtual void Browse()
        {

        }
        public virtual void GetAllDirFiles()
        {

        }
        /// <summary>
        /// 立即体验
        /// </summary>
        public void Sige()
        {
            Process.Start(PageCollection.StrupPath + @"\" + appName);
            Application.Current.Shutdown(0);
        }
        /// <summary>
        /// 自定义安装
        /// </summary>
        public void Custom()
        {
            try
            {
                PageCollection.GridHide = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 关闭当前窗体
        /// </summary>
        /// <param name="win"></param>
        public void Close()
        {
            try
            {
                if (MessageBox.Show("你确定退出安装程序吗？", "温馨提示", MessageBoxButton.YesNo, MessageBoxImage.Information) ==
                    MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        /// <summary>
        /// 解压缩zip文件
        /// </summary>
        /// <param name="zipFile">解压的zip文件流</param>
        public bool Extract(byte[] zipFile)
        {
            try
            {
                PageCollection.Schedule = 0;
                PageCollection.StrupPath = PageCollection.StrupPath.TrimEnd('/') + "//";
                byte[] data = new byte[1024 * 1204];
                int size; //缓冲区的大小（字节）
                double fileCount = 0; //带待压文件的大小（字节）
                double osize = 0; //每次解压读取数据的大小（字节）
                using (ZipInputStream s = new ZipInputStream(new System.IO.MemoryStream(zipFile)))
                {
                    ZipEntry entry;
                    while ((entry = s.GetNextEntry()) != null)
                    {
                        fileCount += entry.Size; //获得待解压文件的大小
                    }
                }
                PageCollection.Maximum = fileCount;
                using (var s = new ZipInputStream(new System.IO.MemoryStream(zipFile)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        if (theEntry.IsDirectory)
                        {
                            continue;
                        }
                        string directorName = Path.Combine(PageCollection.StrupPath,
                            Path.GetDirectoryName(theEntry.Name));
                        string fileName = Path.Combine(directorName, Path.GetFileName(theEntry.Name));
                        if (!Directory.Exists(directorName))
                        {
                            Directory.CreateDirectory(directorName);
                        }
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            //设置文件为可读
                            if (File.Exists(fileName))
                            {
                                File.SetAttributes(fileName, FileAttributes.Normal);
                            }

                            using (FileStream streamWriter = System.IO.File.Create(fileName))
                            {
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        osize += size;
                                        streamWriter.Write(data, 0, size);

                                        PageCollection.Plah =
                                            Math.Round((osize / fileCount * 100), 0).ToString(CultureInfo.InvariantCulture);
                                        PageCollection.Schedule = osize;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                //if (theEntry.Name.Contains("dll"))
                                //{
                                //    PageCollection.Message = theEntry.Name.Substring(0, theEntry.Name.Length - 3);
                                //}
                                //else
                                //{
                                PageCollection.Message = theEntry.Name;
                                //}
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
       



        /// <summary>
        /// 删除开始栏快捷方式
        /// </summary>
        /// <param name="bBase"></param>
        /// <returns></returns>
        public virtual void DeleteStartMenuShortcuts(string stpPath)
        {

        }

       
    }
}

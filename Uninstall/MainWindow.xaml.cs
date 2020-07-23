using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uninstall
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region  属性
        /// <summary>
        /// 当前文件的目录
        /// </summary>
        private readonly string _path;
        /// <summary>
        /// 卸载残余文件的脚本路径
        /// </summary>
        private string _batPath = null;
        /// <summary>
        /// 执行BAT脚本文件
        /// 
        /// </summary>
        /// <param name="lpCmdLine">BAT脚本路径</param>
        /// <param name="uCmdShow">CMD界面是否显示</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);

        private double _proData;
        /// <summary>
        /// 进度条
        /// </summary>
        public double ProData
        {
            get { return _proData; }
            set
            {
                _proData = value;
                OnPropertyChanged("ProData");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            IsRemember.IsChecked = true;
            _path = AppDomain.CurrentDomain.BaseDirectory;
        }
        /// <summary>
        /// 点击卸载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                IsRemember.Visibility = Visibility.Collapsed;
                Process[] pOrange = Process.GetProcessesByName("Hy Main");
                if (pOrange.Length > 0)
                {
                    OrxMessageBox orx = null;
                    orx = new OrxMessageBox("是否要删除 " + "黑鹰Hub" + " 程序?", "温馨提示");

                    orx.Owner = this;
                    orx.ShowDialog();
                    if (Common.isBtn)
                    {
                        foreach (Process item in pOrange)
                        {
                            item.Kill(); //关闭所有橘子进程
                        }
                    }
                }
 
                btnConfig.Visibility = Visibility.Collapsed;
                pro.Visibility = Visibility.Visible;
                leblJdt.Visibility = Visibility.Visible;
         
                Task.Factory.StartNew(() =>
                {
                    ProData = 10;
                    Thread.Sleep(500);
                
                    ProData = 25;
                    DeleteStartMenuShortcuts();
                    DelectShortcut(_path);
                    ProData = 40;
                    Thread.Sleep(500);
                    DelectDir(_path);
                    TestForKillMyself();
                    ProData = 80;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        btnWc.Visibility = Visibility.Visible;

                    }));
                    ProData = 100;

                });

        
            }
            catch (Exception)
            {
            }
        }

        #region  注册表操作
       

        /// <summary>
        /// 删除开始栏快捷方式
        /// </summary>
        /// <param name="fileFold"></param>
        /// <returns></returns>
        public static bool DeleteStartMenuShortcuts()
        {
            try
            {
                string strJkname = "黑鹰Hub";
               

                if (!string.IsNullOrEmpty(strJkname))
                {
                    string strFold = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\" +
                                     "Programs\\" + strJkname;
                    //判断文件夹是否存在，存在就删除
                    if (Directory.Exists(strFold))
                    {
                        Directory.Delete(strFold, true);
                    }
                    string strFold2 = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\" +
                                     "Programs\\" + "黑鹰Hub";
                    //判断文件夹是否存在，存在就删除
                    if (Directory.Exists(strFold2))
                    {
                        Directory.Delete(strFold2, true);
                    }
                }
                return true;
            }
            catch (Exception)
            {

                MessageBox.Show("删除开始栏快捷方式");
                return false;
            }
        }

        /// <summary>
        /// 删除桌面快捷方式
        /// </summary>
        public void DelectShortcut(string filePath)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    if (File.Exists(filePath + "Hy Main.exe"))
                    {
                        //获取AutoCAD软件快捷方式在桌面上的路径
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string pathCom = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);

                        //返回桌面上*.lnk文件的集合
                        string[] items = Directory.GetFiles(path, "*.lnk");
                        string[] itemsCom = Directory.GetFiles(pathCom, "*.lnk");

                        //遍历集合中的每个文件，如果名称包括“AutoCAD”则将其快捷方式删除。
                        foreach (string item in items)
                        {
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
                            if (item.Contains("黑鹰Hub") && item.Contains(".lnk"))
                            {
                                File.Delete(item);
                            }
                            else if (item.Contains("卸载黑鹰Hub") && item.Contains(".lnk"))
                            {
                                File.Delete(item);
                            }

                        }
                    }
                });
            }
            catch (Exception)
            {
                MessageBox.Show("错了删除桌面快捷方式");
            }
        }
        /// <summary>
        /// 正在关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(_batPath))
                {
                    //执行卸载残留文件的脚本
                    WinExec(_batPath, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "正在关闭发生错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 运行完毕后删除自己
        /// </summary>
        public void TestForKillMyself()
        {
            StringBuilder sb = new StringBuilder(); //创建BAT内容
            sb.Append("choice /t:y,2 /n >nul" + Environment.NewLine); //开始执行删除前等待2秒用于关闭Uninstall.exe程序
            sb.Append("rd/s/q " + AppDomain.CurrentDomain.BaseDirectory); //删除橘子文件夹命令
            _batPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString("N") + ".bat"; //创建BAT保存目录
            FileStream fsBAT = System.IO.File.Open(_batPath, FileMode.Create); //创建BAT文件
            StreamWriter swBAT = new StreamWriter(fsBAT, Encoding.Default); //将BAT内容写入BAT文件中
            swBAT.Write(sb.ToString());
            swBAT.Close();
            fsBAT.Close();
        }

        /// <summary>
        /// 删除文件所在目录
        /// </summary>
        private void DelectDir(string filePath)
        {
            try
            {
                string fileName = System.IO.Path.GetTempPath() + "remove.bat";
                //string fileName = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
                //fileName = fileName.Substring(0, fileName.LastIndexOf('\\')) + "\\remove.bat";
                StreamWriter bat = new StreamWriter(fileName, false, Encoding.Default);
                string exePath = _path;
                bat.WriteLine("cd..");
                bat.WriteLine("ping -n 1 -w 100 192.186.221.125");
                bat.WriteLine("ping -n 1 -w 100 192.186.221.125");
                bat.WriteLine(string.Format("del \"{0}\" /q", exePath));
                bat.WriteLine(string.Format("del \"{0}\" /q", fileName));
                bat.WriteLine(string.Format("rd \"{0}\" /s /q", exePath.Substring(0, exePath.LastIndexOf('\\'))));
                bat.Close();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误删除文件所在目录", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// 拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        /// <summary>
        /// 不卸载了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnNoClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }
    }
}

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using HY.Client.Execute.Commons;
using HY.Client.Execute.Commons.Files;
using HY_Main.Common.CoreLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel.Sign
{
     public class LoginViewModel: BaseDialogOperation
    {
        #region 用户名/密码

        private string _Report;
        private string userName = string.Empty;
        private string passWord = string.Empty;
        private bool _IsCancel = true;
        private string _SkinName;
        private string _Title;

        /// <summary>
        /// 背景图片
        /// </summary>
        public string SkinName
        {
            get { return _SkinName; }
        }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
        }
        
        /// <summary>
        /// 进度报告
        /// </summary>
        public string Report
        {
            get { return _Report; }
            set { _Report = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        ///// <summary>
        ///// 记住密码
        ///// </summary>
        //public bool UserChecked
        //{
        //    get { return _UserChecked; }
        //    set { _UserChecked = value; RaisePropertyChanged(); }
        //}

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 禁用按钮
        /// </summary>
        public bool IsCancel
        {
            get { return _IsCancel; }
            set { _IsCancel = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 命令(Binding Command)

        private RelayCommand _signCommand;

        public RelayCommand SignCommand
        {
            get
            {
                if (_signCommand == null)
                {
                    _signCommand = new RelayCommand(() => Login());
                }
                return _signCommand;
            }
        }

        private RelayCommand _exitCommand;

        public RelayCommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand(() => ApplicationShutdown());
                }
                return _exitCommand;
            }
        }

        #endregion

        #region Login/Exit

        /// <summary>
        /// 登陆系统
        /// </summary>
        public   void Login()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                {
                    this.IsCancel = false;
                    MessageBox.Show("登录成功");
                }
            }
            catch (Exception ex)
            {
                this.Report = ExceptionLibrary.GetErrorMsgByExpId(ex);
            }
            finally
            {
                this.IsCancel = true;
            }
        }

        /// <summary>
        /// 关闭系统
        /// </summary>
        public void ApplicationShutdown()
        {
            Messenger.Default.Send("", "ApplicationShutdown");
        }

        #endregion

        #region 记住密码

        /// <summary>
        /// 读取本地配置信息
        /// </summary>
        public void ReadConfigInfo()
        {
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + SerivceFiguration.INI_CFG;
            if (File.Exists(cfgINI))
            {
                IniFile ini = new IniFile(cfgINI);
                UserName = ini.IniReadValue("Login", "User");
                Password = CEncoder.Decode(ini.IniReadValue("Login", "Password"));
                _SkinName = ini.IniReadValue("Skin", "Skin");
            }
        }

        /// <summary>
        /// 保存登录信息
        /// </summary>
        private void SaveLoginInfo()
        {
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + SerivceFiguration.INI_CFG;
            IniFile ini = new IniFile(cfgINI);
            ini.IniWriteValue("Login", "User", UserName);
            ini.IniWriteValue("Login", "Password", CEncoder.Encode(Password));
        }

        #endregion
    }
}

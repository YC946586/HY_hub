using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.Model.Sign
{
    public class loginModel: ViewModelBase
    {
        #region 用户名/密码

     
        private string _Report;
        private string userName = string.Empty;
        private string _passWord = string.Empty;
        private string _confirmPassword = string.Empty;
        private bool _IsCancel = true;
        private string _verification = string.Empty;
        private string _verificationContent = "获取验证码";
        private bool _verificationEnbled = true;
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
        /// <summary>
        /// 验证码
        /// </summary>
        public string Verification
        {
            get { return _verification; }
            set { _verification = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 验证码内容
        /// </summary>
        public string VerificationContent
        {
            get { return _verificationContent; }
            set { _verificationContent = value; RaisePropertyChanged(); }
        }
         /// <summary>
         /// 验证码内容
         /// </summary>
        public bool VerificationEnbled
        {
            get { return _verificationEnbled; }
            set { _verificationEnbled = value; RaisePropertyChanged(); }
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
            get { return _passWord; }
            set { _passWord = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; RaisePropertyChanged(); }
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
    }
}

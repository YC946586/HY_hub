using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons
{
    /// <summary>
    /// 登录用户信息
    /// </summary>
    public class Loginer : ViewModelBase
    {
        private Loginer() { }
        private static Loginer _Loginer = new Loginer(); //饿汉式单例

        public string macAdd { get; set; }
        public string Authorization { get; set; }

        public string token { get; set; }
        public string phone { get; set; }
        // 0：普通用户  1：月费用户  2：年费用户
        public string vipType { get; set; }
        //是免费下载次数
        public int freeCount { get; set; }
        //VIP有效期
        public string vipInfo { get; set; }
        /// <summary>
        /// 当前用户
        /// </summary>
        public static Loginer LoginerUser
        {
            get { return _Loginer; }
        }

        private string _Account = string.Empty;
        private string _UserName = string.Empty;
        private string _Email = string.Empty;
        private bool _Admin = false;
        private float _balance = 0;

        /// <summary>
        /// 登录名
        /// </summary>
        public string Account
        {
            get { return _Account; }
            set { _Account = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set { _UserName = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 是否属于管理员
        /// </summary>
        public bool IsAdmin
        {
            get { return _Admin; }
            set { _Admin = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 月
        /// </summary>
        public float balance
        {
            get { return _balance; }
            set { _balance = value; RaisePropertyChanged(); }
        }
        

    }
}

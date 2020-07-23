using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hy.Setup
{
    public class SetupModel : ViewModelBase
    {

        private int _gridHide = 0;

        /// <summary>
        ///  0 直接安装界面 1 自定义安装界面
        /// </summary>
        public int GridHide
        {
            get { return _gridHide; }
            set
            {
                _gridHide = value;
                RaisePropertyChanged("GridHide");
            }
        }

        private string _winver = string.Empty;

        /// <summary>
        /// 系统版本
        /// </summary>
        public string Winver
        {
            get { return _winver; }
            set
            {
                _winver = value;
                RaisePropertyChanged("Winver");
            }
        }


        private string _strupPath = string.Empty;

        /// <summary>
        /// 安装目录
        /// </summary>
        public string StrupPath
        {
            get { return _strupPath; }
            set
            {
                _strupPath = value;
                RaisePropertyChanged("StrupPath");
            }
        }
        private string _message;
        ///<summary>
        ///显示的消息
        ///</summary>
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged("Message");

            }
        }

        private double _schedule = 0;
        ///<summary>
        ///进度百分比
        ///</summary>
        public double Schedule
        {
            get { return _schedule; }
            set
            {
                _schedule = value;
                RaisePropertyChanged("Schedule");

            }
        }

        private double _maximum = 0;
        ///<summary>
        ///进度最大值
        ///</summary>
        public double Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                RaisePropertyChanged("Maximum");

            }
        }



        private string _plah;
        ///<summary>
        ///进度条说明
        ///</summary>
        public string Plah
        {
            get { return _plah; }
            set
            {
                _plah = value;
                RaisePropertyChanged("Plah");
            }
        }

    }
}

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity.UserEntitys
{
    public class UserGamesEntity : ViewModelBase
    {
        public int id { get; set; }
        public int gameId { get; set; }
        public int userId { get; set; }
        public int isInOften { get; set; }
        public int downLoadCount { get; set; }
        public object downMacAdd { get; set; }
        public int createTime { get; set; }
        public int updateTime { get; set; }
        public string title { get; set; }
        public string fileDir { get; set; }
        public string pict { get; set; }
        public object logo { get; set; }
        public float price { get; set; }
        public string cateName { get; set; }
        public string startFileName { get; set; }
        public string setUpFile { get; set; }
        public long fileSize { get; set; }
        public object version { get; set; }

        public bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; RaisePropertyChanged(); }
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
                RaisePropertyChanged();
            }
        }
        public string GameSize { get; set; }

        private string _mineContent = "安装游戏";
        /// <summary>
        /// 
        /// </summary>
        public string MineContent
        {
            get { return _mineContent; }
            set
            {
                _mineContent = value;
                RaisePropertyChanged();
            }
        }


        private string _content = "暂停";

        /// <summary>
        /// 
        /// </summary>
        public string content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged();
            }
        }
        private string _SurplusSize = string.Empty;
        /// <summary>
        ///  已经下载
        /// </summary>
        public string SurplusSize
        {
            get { return _SurplusSize; }
            set
            {
                _SurplusSize = value;
                RaisePropertyChanged();
            }
        }
        private string _Speed = string.Empty;
        /// <summary>
        ///  下载速度
        /// </summary>
        public string Speed
        {
            get { return _Speed; }
            set
            {
                _Speed = value;
                RaisePropertyChanged();
            }
        }
        private string _RemainingTime = string.Empty;
        /// <summary>
        ///  剩余时间
        /// </summary>
        public string RemainingTime
        {
            get { return _RemainingTime; }
            set
            {
                _RemainingTime = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 下载了好多
        /// </summary>
        public long downCont { get; set; }
        public string gameName { get; set; }

        public List<dynamic> dwonloadAllEntities { get; set; }
    }

     

}

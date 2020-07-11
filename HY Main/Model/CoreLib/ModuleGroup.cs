using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.Model.CoreLib
{
    /// <summary>
    /// 模块组
    /// </summary>
    public class ModuleGroup : ViewModelBase
    {
        private int groupid;
        private string _groupIcon = "BlockHelper";
        private string groupName;
     

        /// <summary>
        /// 模块ICO
        /// </summary>
        public string GroupIcon
        {
            get { return _groupIcon; }
            set { _groupIcon = value; RaisePropertyChanged(); }
        }

        
        /// <summary>
        /// 父模块ID
        /// </summary>
        public int GroupId
        {
            get { return groupid; }
            set { groupid = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 父模块名称
        /// </summary>
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; RaisePropertyChanged(); }
        }

       
    }

    
}

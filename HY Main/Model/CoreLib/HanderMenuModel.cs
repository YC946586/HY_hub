using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.Model.CoreLib
{
    public class HanderMenuModel
    {
        private string headerName;
        private string headerIcon;
        private object body;
        private bool _isChecked;
        
        /// <summary>
        /// 标题
        /// </summary>
        public string HeaderName
        {
            get { return headerName; }
            set { headerName = value; }
        }
        /// <summary>
        ///  
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }
        
        /// <summary>
        /// 标题图标
        /// </summary>
        public string HeaderIcon
        {
            get { return headerIcon; }
            set { headerIcon = value; }
        }
        /// <summary>
        /// 窗口内容
        /// </summary>
        public object Body
        {
            get { return body; }
            set { body = value; }
        }
    }
}

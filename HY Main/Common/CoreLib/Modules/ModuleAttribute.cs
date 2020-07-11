using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.Common.CoreLib.Modules
{
    /// <summary>
    /// 模块类型
    /// </summary>
    public enum ModuleType
    {
        [Desc("MainHander")]
        Hander = 0,
        [Desc("default")]
        None = 1,
    }

    /// <summary>
    /// 模块特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleAttribute : Attribute
    {
        /// <summary>
        /// 模块构造函数
        /// </summary>
        /// <param name="code">模块编码</param>
        /// <param name="name">模块名称</param>
        /// <param name="Namespace">模块位置</param> 
        /// <param name="IviewModel">对应ViewModel</param> 
        /// <param name="Sort">排序</param> 
        public ModuleAttribute(ModuleType type, string name, string Namespace, string Icon, int Sort)
        {
            _Name = name;
            _ModuleType = type;
            _ModuleNameSpace = Namespace;
            _Sort = Sort;
            _ICON = Icon;
        }

        #region private

        private string _Name;
        private string _IviewModel;
        private int _Sort;
        private string _ModuleNameSpace;
        private string _Remark;
        private ModuleType _ModuleType;
        private string _ICON;

        #endregion

        #region 只读属性

        /// <summary>
        /// 图标
        /// </summary>
        public string ICON
        {
            get { return _ICON; }
        }

        /// <summary>
        /// Viewm命名空间
        /// </summary>
        public string ModuleNameSpace
        {
            get { return _ModuleNameSpace; }
        }
        /// <summary>
        /// Viewm命名空间
        /// </summary>
        public string IviewModel
        {
            get { return _IviewModel; }
        }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort
        {
            get { return _Sort; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
        }

        /// <summary>
        /// 模块类型
        /// </summary>
        public ModuleType ModuleType
        {
            get { return _ModuleType; }
        }

        #endregion
    }


    /// <summary>
    /// 模块类型特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DescAttribute : Attribute
    {
        protected string _ModuleName = string.Empty;
        protected string _ModuleIcon = string.Empty;

        public string ModuleName { get { return _ModuleName; } }
        public string ModuleIcon { get { return _ModuleIcon; } }

        public DescAttribute(string caption, string ico = "BorderAll")
        {
            this._ModuleName = caption;
            this._ModuleIcon = ico;
        }

    }
}

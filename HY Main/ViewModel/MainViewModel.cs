using GalaSoft.MvvmLight.Command;
using HY_Main.Common.CoreLib;
using HY_Main.Common.CoreLib.Modules;
using HY_Main.ViewModel.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel
{
    /// <summary>
    /// 首页
    /// </summary>
    public class MainViewModel : BaseHostDialogOperation
    {

        #region 模块系统

        private ModuleManager _ModuleManager;

        /// <summary>
        /// 模块管理器
        /// </summary>
        public ModuleManager ModuleManager
        {
            get { return _ModuleManager; }
        }

        #endregion
        #region 工具栏

        private NoticeViewModel _NoticeView;

        /// <summary>
        /// 通知模块
        /// </summary>
        public NoticeViewModel NoticeView
        {
            get { return _NoticeView; }
        }

        #endregion

        #region 命令(Binding Command)

        private object _CurrentPage;

        /// <summary>
        /// 当前选择页
        /// </summary>
        public object CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 初始化/页面相关

        /// <summary>
        /// 初始化首页
        /// </summary>
        public async void InitDefaultView()
        {
            //初始化工具栏,通知窗口
            _NoticeView = new NoticeViewModel();
            ////加载窗体模块
            _ModuleManager = new ModuleManager();
            CurrentPage = await _ModuleManager.LoadModulesAsync();
        }



        #endregion

    }
}

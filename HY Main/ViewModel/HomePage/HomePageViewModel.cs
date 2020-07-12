using HY_Main.Common.CoreLib;
using HY_Main.Model.HomePage;
using HY_Main.ViewModel.HomePage.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel.HomePage
{
     public class HomePageViewModel: BaseOperation<HomePageModel>
    {
        private Download _DownloadManager;

        /// <summary>
        /// 模块管理器
        /// </summary>
        public Download DownloadManager
        {
            get { return _DownloadManager; }
        }

        public override void InitViewModel()
        {
            base.InitViewModel();
            _DownloadManager = new Download();
        }

    }
}

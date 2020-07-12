using HY.Application.BaseModel;
using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel.HomePage.UserControls
{
    public class Download
    {

        public Download()
        {
            LoadModulesAsync();
        }

        private ObservableCollection<DownloadModel> _Groups = new ObservableCollection<DownloadModel>();

        /// <summary>
        /// 加载模块
        /// </summary>
        public ObservableCollection<DownloadModel> Groups
        {
            get { return _Groups; }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public  void  LoadModulesAsync()
        {
            try
            {
                List<GameModel> MenuList = new List<GameModel>();
                for (int i = 1; i < 14; i++)
                {
                    GameModel loadModel = new GameModel() { 
                      Sort=i,
                      GameName="超凡双笙"+i,
                    };
                    MenuList.Add(loadModel);
                }
                ObservableCollection<GameModel> MenuModels = new ObservableCollection<GameModel>();
                MenuList.Take(7).ForEach((ary) =>
                {
                    MenuModels.Add(ary);
                });
                DownloadModel model = new DownloadModel() { MenuModels = MenuModels };
                Groups.Add(model);
                MenuModels = new ObservableCollection<GameModel>();
                MenuList.Skip(7).ForEach((ary) =>
                {
                    MenuModels.Add(ary);
                });
                model = new DownloadModel() { MenuModels = MenuModels };
                Groups.Add(model);
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
    public class DownloadModel
    {
        public string Region { get; set; }

        private ObservableCollection<GameModel> _ModulMenus = new ObservableCollection<GameModel>();

        /// <summary>
        /// 加载模块
        /// </summary>
        public ObservableCollection<GameModel> MenuModels
        {
            get { return _ModulMenus; }
            set
            {
                _ModulMenus = value;
            }
        }
    }
}

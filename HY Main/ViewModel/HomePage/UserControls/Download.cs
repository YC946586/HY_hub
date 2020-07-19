using HY.Application.BaseModel;
using HY.Client.Entity.HomeEntitys;
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

        /// <summary>
        /// 加载模块
        /// </summary>
        public ObservableCollection<DownloadModel> Groups { get; } = new ObservableCollection<DownloadModel>();
        /// <summary>
        /// 初始化
        /// </summary>
        public  void  LoadModulesAsync (List<Hotgame> hotGames)
        {
            try
            {
                int i =1;
                ObservableCollection<Hotgame> MenuModels = new ObservableCollection<Hotgame>();

                var ItemsSource = hotGames.Skip(0).Take(7);
                ItemsSource.ForEach((ary) =>
                {
                    ary.Sort = i++;
                    MenuModels.Add(ary);
                });
                DownloadModel model = new DownloadModel() { MenuModels = MenuModels };
                Groups.Add(model);
                MenuModels = new ObservableCollection<Hotgame>();
                i = 1;
                ItemsSource = hotGames.Skip(7).Take(7);
                ItemsSource.ForEach((ary) =>
                {
                    ary.Sort = i++;
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

        /// <summary>
        /// 加载模块
        /// </summary>
        public ObservableCollection<Hotgame> MenuModels { get; set; } = new ObservableCollection<Hotgame>();
    }
}

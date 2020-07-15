using HY.Client.Entity.HomeEntitys;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using HY_Main.Model.HomePage;
using HY_Main.ViewModel.HomePage.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel.HomePage
{
     public class HomePageViewModel: BaseOperation<HomePageModel>
    {
        private Download _DownloadManager;
        private ObservableCollection<Hotgame> _hotGames = new ObservableCollection<Hotgame>();
        private ObservableCollection<Recommendgame> _recommendGames = new ObservableCollection<Recommendgame>();
        /// <summary>
        /// 模块管理器
        /// </summary>
        public Download DownloadManager
        {
            get { return _DownloadManager; }
        }
        
        public ObservableCollection<Hotgame> HotGames
        {
            get { return _hotGames; }
            set
            {
                _hotGames = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Recommendgame> RecommendGames
        {
            get { return _recommendGames; }
            set
            {
                _recommendGames = value;
                RaisePropertyChanged();
            }
        }


        public override void InitViewModel()
        {
            base.InitViewModel();
            _DownloadManager = new Download();
            InitHotRecomenAsync();
        }
        public   async  void InitHotRecomenAsync()
        {
            try
            {
                IHome home = BridgeFactory.BridgeManager.GetHomeManager();
                var genrator = await home.GetHomeGames();
                if (genrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<GetHomeResultEntity>(genrator.result.ToString());
                    if (Results.hotGames!=null&& Results.hotGames.Length!=0)
                    {
                        Results.hotGames.ToList().ForEach((ary) => HotGames.Add(ary));
                    }
                    if (Results.recommendGames != null && Results.recommendGames.Length != 0)
                    {
                        Results.recommendGames.ToList().ForEach((ary) => RecommendGames.Add(ary));
                    } 
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}

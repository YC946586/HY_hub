using GalaSoft.MvvmLight.Command;
using HandyControl.Data;
using HY.Client.Entity.HomeEntitys;
using HY.Client.Execute.Commons;
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
using System.Windows;


namespace HY_Main.ViewModel.HomePage
{
    public class HomePageViewModel : BaseOperation<HomePageModel>
    {
        private Download _DownloadManager;
        private ObservableCollection<GetCommonUseGamesEntity> _hotGames = new ObservableCollection<GetCommonUseGamesEntity>();
        private ObservableCollection<Recommendgame> _recommendGames = new ObservableCollection<Recommendgame>();

        private string _describe;
        private string _toolDes;
        /// <summary>
        ///  
        /// </summary>
        public string about
        {
            get { return _describe; }
            set { _describe = value; RaisePropertyChanged(); }
        }

        /// <summary>
        ///  
        /// </summary>
        public string toolDes
        {
            get { return _toolDes; }
            set { _toolDes = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 模块管理器
        /// </summary>
        public Download DownloadManager
        {
            get { return _DownloadManager; }
        }

        public ObservableCollection<GetCommonUseGamesEntity> HotGames
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
        public async void InitHotRecomenAsync()
        {
            try
            {
                IHome home = BridgeFactory.BridgeManager.GetHomeManager();
                var genrator = await home.GetHomeGames();
                if (genrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<GetHomeResultEntity>(genrator.result.ToString());
                    if (Results.recommendGames != null && Results.recommendGames.Length != 0)
                    {
                        Results.recommendGames.OrderBy(s => s.displayOrder).ToList().ForEach((ary) => RecommendGames.Add(ary));
                    }
                }
                var comkmogenrator = await home.GetCommonUseGames();
                if (comkmogenrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<GetCommonUseGamesEntity>>(comkmogenrator.result.ToString());
                    if (Results != null && Results.Count != 0)
                    {
                        Results.ToList().ForEach((ary) => HotGames.Add(ary));
                    }
                }

                ICommon common = BridgeFactory.BridgeManager.GetCommonManager();
                var Commongenrator = await common.GetCommonDes();
                if (Commongenrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<HomePageModel>(Commongenrator.result.ToString());
                    about = Results.about;
                    toolDes = Results.toolDes;

                }
            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }


        public override void GainGames(Recommendgame tmodel)
        {
            try
            {
                if (Message.Question("是否使用黑鹰币获取游戏"))
                {

                }

            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }

    }
}

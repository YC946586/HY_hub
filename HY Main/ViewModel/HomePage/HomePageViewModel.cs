using GalaSoft.MvvmLight.Command;
using HandyControl.Data;
using HY.Application.Base;
using HY.Client.Entity.HomeEntitys;
using HY.Client.Execute.Commons;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using HY_Main.Common.Unity;
using HY_Main.Model.HomePage;
using HY_Main.View.HomePage.UserControls;
using HY_Main.ViewModel.HomePage.UserControls;
using Microsoft.Practices.ObjectBuilder2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using HandyControl.Tools;
using System.Windows.Documents;
using HY.Client.Entity.UserEntitys;
using System.IO;
using System.Diagnostics;
using HY.Client.Entity.CommonEntitys;
using HY_Main.ViewModel.Mine.UserControls;
using HY_Main.ViewModel.Step;

namespace HY_Main.ViewModel.HomePage
{
    public class HomePageViewModel : BaseOperation<HomePageModel>
    {
        #region  命令

        private RelayCommand<string> _OpenEditCommand;
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand<string> OpenEditCommand
        {
            get
            {
                if (_OpenEditCommand == null)
                {
                    _OpenEditCommand = new RelayCommand<string>(t => OpenEdit(t));
                }
                return _OpenEditCommand;
            }
            set { _OpenEditCommand = value; }
        }

        private RelayCommand<GetCommonUseGamesEntity> _openCommand;
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand<GetCommonUseGamesEntity> OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand<GetCommonUseGamesEntity>(t => Open(t));
                }
                return _openCommand;
            }
            set { _openCommand = value; }
        }
        private RelayCommand<Hotgame> _GainGamesHotgame;
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand<Hotgame> GainGamesHotgameCommand
        {
            get
            {
                if (_GainGamesHotgame == null)
                {
                    _GainGamesHotgame = new RelayCommand<Hotgame>(t => GainGamesHotgame(t));
                }
                return _GainGamesHotgame;
            }
            set { _GainGamesHotgame = value; }
        }
        private new RelayCommand<Recommendgame> _DetailsCommond;
        /// <summary>
        /// 详情
        /// </summary>
        public new RelayCommand<Recommendgame> DetailsCommond
        {
            get
            {
                if (_DetailsCommond == null)
                {
                    _DetailsCommond = new RelayCommand<Recommendgame>(t => Details(t));
                }
                return _DetailsCommond;
            }
            set { _DetailsCommond = value; }
        }


        #endregion
        private Download _DownloadManager;
        private ObservableCollection<GetCommonUseGamesEntity> _hotGames = new ObservableCollection<GetCommonUseGamesEntity>();
        private ObservableCollection<Recommendgame> _recommendGames = new ObservableCollection<Recommendgame>();
        private ObservableCollection<Recommendgame> _recommendSkipGames = new ObservableCollection<Recommendgame>();
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

        public ObservableCollection<Recommendgame> RecommendSkipGames
        {
            get { return _recommendSkipGames; }
            set
            {
                _recommendSkipGames = value;
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
                //获取我的游戏
              
                DisplayMetro = Visibility.Visible;
                IHome home = BridgeFactory.BridgeManager.GetHomeManager();
                var genrator = await home.GetHomeGames();
                if (genrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<GetHomeResultEntity>(genrator.result.ToString());
                    if (Loginer.LoginerUser.ToolEntities.Count!=0)
                    {
                        foreach (var item in Loginer.LoginerUser.ToolEntities)
                        {
                            foreach (var recommen in Results.recommendGames)
                            {
                                if (recommen.id.Equals(item.gamesId))
                                {
                                    recommen.Purchased = 1;
                                }
                                else if(recommen.isPurchased)
                                {
                                    recommen.Purchased = 2;
                                }
                            }
                            foreach (var hotgameItem in Results.hotGames)
                            {
                                if (hotgameItem.id.Equals(item.gamesId))
                                {
                                    hotgameItem.Purchased = 1;
                                }
                                else if (hotgameItem.isPurchased)
                                {
                                    hotgameItem.Purchased = 2;
                                }
                            }
                        }
                    }
                    if (Results.recommendGames != null && Results.recommendGames.Length != 0)
                    {
                        Results.recommendGames.OrderBy(s => s.displayOrder).ToList().ForEach((ary) => RecommendGames.Add(ary));
                        var ItemsSource = Results.recommendGames.OrderBy(s => s.displayOrder).Skip(0).Take(4);
                        ItemsSource.ForEach((ary) => RecommendSkipGames.Add(ary));
                    }
                    if (Results.hotGames != null && Results.hotGames.Length != 0)
                    {
                        DownloadManager.LoadModulesAsync(Results.hotGames.OrderBy(s => s.displayOrder).ToList());
                    }
                }
                //常用游戏
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
                Msg.Error(ex);
            }
            finally
            {
                DisplayMetro = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 常用功能双击
        /// </summary>
        /// <param name="t"></param>
        private void Open(GetCommonUseGamesEntity model)
        {
            try
            {
                var GameRoute = CommonsCall.ReadUserGameInfo(model.gameId.ToString());
                if (!string.IsNullOrEmpty(GameRoute.Key))
                {
                    if (File.Exists(GameRoute.Key))//判断文件是否存在
                    {
                        Process.Start(GameRoute.Key);
                    }
                    else
                    {
                        CommonsCall.DeleteSubKeyTree(GameRoute.Key);
                        Msg.Info("游戏损坏,请重新安装游戏");
                    }
                }
                else
                {
                    Msg.Info("请您先安装游戏");
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }

        private void OpenEdit(string t)
        {
            try
            {
                EditUserGamesViewModel viewModel = new EditUserGamesViewModel();
                if (t.Equals("Add"))
                {
                    viewModel.AddViewModel();
                }
                else
                {
                    viewModel.UpdateViewModel(HotGames);
                    viewModel.Display = "UpdateViewModel";
                }
                var dialog = ServiceProvider.Instance.Get<IModelDialog>("EditUserGamesDlg");
                dialog.BindViewModel(viewModel);

                var d = Dialog.Show(dialog.GetDialog());

                viewModel.ShowList += (async () =>
                {
                    d.Close();
                    HotGames.Clear();
                    IHome home = BridgeFactory.BridgeManager.GetHomeManager();
                    var comkmogenrator = await home.GetCommonUseGames();
                    if (comkmogenrator.code.Equals("000"))
                    {
                        var Results = JsonConvert.DeserializeObject<List<GetCommonUseGamesEntity>>(comkmogenrator.result.ToString());
                        if (Results != null && Results.Count != 0)
                        {
                            Results.ToList().ForEach((ary) => HotGames.Add(ary));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }
        /// <summary>
        /// 推荐游戏购买
        /// </summary>
        /// <param name="gameId"></param>
        public override async void GainGames(object gameId)
        {
            try
            {
                var model = gameId as Recommendgame;
                if (model.Purchased==2)
                {
                    IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                    var genrator = await store.GetGameFiles(model.id);
                    if (!genrator.code.Equals("000"))
                    {
                        Msg.Info(genrator.Message);
                        return;
                    }
                    var Results = JsonConvert.DeserializeObject<List<DwonloadEntity>>(genrator.result.ToString());
                    if (Results.Count == 0)
                    {
                        Msg.Info("游戏获取失败,请重试");
                        return;
                    }
                    if (CommonsCall.UserGames.Any(s => s.gameId.Equals(model.id)))
                    {
                        Msg.Info(model.title + "已经进入下载队列中");
                        return;
                    }
                    GameDwonloadViewModel viewModel = new GameDwonloadViewModel();
                    viewModel.PageCollection = Loginer.LoginerUser.UserGameList.Where(s=>s.gameId.Equals(model.id)).First() ;
                    viewModel.dwonloadEntities = Results;
                    viewModel.InitAsyncViewModel();
                    var dialog = ServiceProvider.Instance.Get<IModelDialog>("EGameDwonloadDlg");
                    
                    dialog.BindViewModel(viewModel);

                    var d = Dialog.Show(dialog.GetDialog());
                    viewModel.ShowList += (async () =>
                    {
                        d.Close();
                    });
                    return;
                }
                var GameRoute = CommonsCall.ReadUserGameInfo(model.id.ToString());
                if (!string.IsNullOrEmpty(GameRoute.Key))
                {
                    if (File.Exists(GameRoute.Key))//判断文件是否存在
                    {
                        Process.Start(GameRoute.Key);
                    }
                    else
                    {
                        model.Purchased = 2;
                        CommonsCall.DeleteSubKeyTree(GameRoute.Key);
                        Msg.Info("游戏损坏,请重新安装游戏");
                    }
                }
                else
                {
                    if (await Msg.Question("是否购买游戏"))
                    {
                        IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                        var genrator = await store.BuyGame(model.id);
                        model.Purchased = 2;
                        Msg.Info(genrator.Message);
                        if (genrator.code.Equals("000"))
                        {
                            if (genrator.result.Equals("888"))
                            {
                                return;
                            }
                            CommonsCall.BuyGame(genrator.result.ToString());
                        }
                    }
                }
              
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }

        /// <summary>
        //常用游戏购买
        /// </summary>
        /// <param name="gameId"></param>
        public  async void GainGamesHotgame(Hotgame model)
        {
            try
            {
                if (model.Purchased == 2)
                {
                    IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                    var genrator = await store.GetGameFiles(model.id);
                    if (!genrator.code.Equals("000"))
                    {
                        Msg.Info(genrator.Message);
                        return;
                    }
                    var Results = JsonConvert.DeserializeObject<List<DwonloadEntity>>(genrator.result.ToString());
                    if (Results.Count == 0)
                    {
                        Msg.Info("游戏获取失败,请重试");
                        return;
                    }
                    if (CommonsCall.UserGames.Any(s => s.gameId.Equals(model.id)))
                    {
                        Msg.Info(model.title + "已经进入下载队列中");
                        return;
                    }
                    GameDwonloadViewModel viewModel = new GameDwonloadViewModel();
                    viewModel.PageCollection = Loginer.LoginerUser.UserGameList.Where(s => s.gameId.Equals(model.id)).First();
                    viewModel.dwonloadEntities = Results;
                    viewModel.InitAsyncViewModel();
                    var dialog = ServiceProvider.Instance.Get<IModelDialog>("EGameDwonloadDlg");

                    dialog.BindViewModel(viewModel);

                    var d = Dialog.Show(dialog.GetDialog());

                    viewModel.ShowList += (async () =>
                    {
                        d.Close();
                    });
                    return;
                }
                var GameRoute = CommonsCall.ReadUserGameInfo(model.id.ToString());
                if (!string.IsNullOrEmpty(GameRoute.Key))
                {
                    if (File.Exists(GameRoute.Key))//判断文件是否存在
                    {
                        Process.Start(GameRoute.Key);
                    }
                    else
                    {
                        model.Purchased = 2;
                        CommonsCall.DeleteSubKeyTree(GameRoute.Key);
                        Msg.Info("游戏损坏,请重新安装游戏");
                    }
                }
                else
                {
                    if (await Msg.Question("是否购买游戏"))
                    {
                        IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                        var genrator = await store.BuyGame(model.id);
                        model.Purchased = 2;
                        Msg.Info(genrator.Message);
                        if (genrator.code.Equals("000"))
                        {
                            if (genrator.result.Equals("888"))
                            {
                                return;
                            }
                            CommonsCall.BuyGame(genrator.result.ToString());
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }

        public new  async void Details<TModel>(TModel model)
        {
            try
            {
                var showModel = model as Recommendgame;
                if (!string.IsNullOrEmpty(showModel.description) &&!string.IsNullOrEmpty(showModel.videoUrl))
                {
                    DetailsGamesViewModel viewModel = new DetailsGamesViewModel();
                    var dialog = ServiceProvider.Instance.Get<IModelDialog>("DetailsGamesDlg");
                    dialog.BindViewModel(viewModel);
                    viewModel.InitViewModel(showModel);
                    var d = Dialog.Show(dialog.GetDialog());
                    
                }
             
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }
    }
}

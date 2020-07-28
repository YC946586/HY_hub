using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;
using HY.Application.Base;
using HY.Client.Entity.CommonEntitys;
using HY.Client.Entity.HomeEntitys;
using HY.Client.Entity.StoreEntitys;
using HY.Client.Entity.ToolEntitys;
using HY.Client.Execute.Commons;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using HY_Main.Common.Unity;
using HY_Main.ViewModel.Mine.UserControls;
using HY_Main.ViewModel.Step;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HY_Main.ViewModel.ShopMall
{
    public class ShopMallViewModel : BaseOperation<Recommendgame>
    {
        public override void InitViewModel()
        {
            base.InitViewModel();
            PageIndex = 1;
            InitHotRecomenAsync();
        }

        #region 属性



        private ToolEntity _SelectCombox;

        public ToolEntity SelectCombox
        {
            get => _SelectCombox;
            set { _SelectCombox = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<ToolEntity> _catesList = new ObservableCollection<ToolEntity>() { new ToolEntity() { Key = "全部游戏" } };

        /// <summary>
        /// 表单数据
        /// </summary>
        public ObservableCollection<ToolEntity> CatesList
        {
            get { return _catesList; }
            set { _catesList = value; RaisePropertyChanged(); }
        }
        #endregion

        public async void InitHotRecomenAsync()
        {
            try
            {
                DisplayMetro = Visibility.Visible;
                IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                var genrator = await store.GetCates();
                if (genrator.code.Equals("000"))
                {
                    //游戏分类
                    var Results = JsonConvert.DeserializeObject<List<CatesEntity>>(genrator.result.ToString());
                    Results.ForEach((ary) => { CatesList.Add(new ToolEntity() { Key = ary.name, Values = ary }); });
                    SelectCombox = CatesList.First();
                }
                var gamesGetGames = await store.GetGames(1212121211, "", 1, 100000);
                if (gamesGetGames.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<Recommendgame>>(gamesGetGames.result.ToString());
                    if (Loginer.LoginerUser.ToolEntities.Count != 0)
                    {
                        foreach (var item in Loginer.LoginerUser.ToolEntities)
                        {
                            foreach (var recommen in Results)
                            {
                                if (recommen.id.Equals(item.gamesId))
                                {
                                    recommen.Purchased = 1;
                                }
                                else if (recommen.isPurchased)
                                {
                                    recommen.Purchased = 2;
                                }
                            }
                        }
                    }

                    PageCount = Convert.ToInt32(Math.Ceiling(Results.Count / (double)8));
                    var curShowmodel = Results.Skip(0).Take(8);
                    curShowmodel.OrderBy(s => s.displayOrder).ToList().ForEach((ary) => GridModelList.Add(ary));
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
        ///     页码改变命令
        /// </summary>
        public RelayCommand<FunctionEventArgs<int>> PageUpdatedCmd =>
            new Lazy<RelayCommand<FunctionEventArgs<int>>>(() =>
                new RelayCommand<FunctionEventArgs<int>>(PageUpdated)).Value;

        /// <summary>
        /// 页码改变
        /// </summary>
        private async void PageUpdated(FunctionEventArgs<int> info)
        {
            if (PageCount < 1)
            {
                return;
            }
            try
            {
                DisplayMetro = Visibility.Visible;
                GridModelList.Clear();
                string cateId = string.Empty;
                if (SelectCombox != null && SelectCombox.Values != null)
                {
                    cateId = SelectCombox.Values.id;
                }
                else
                {
                    cateId = "1212121211";
                }
                IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                var gamesGetGames = await store.GetGames(int.Parse(cateId), SearchText, info.Info, 8);
                if (gamesGetGames.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<Recommendgame>>(gamesGetGames.result.ToString());
                    if (Results.Count == 0)
                    {
                        PageCount = 0;
                        Msg.Info("暂未查询出数据,请您重新查询");
                        return;
                    }
                    if (Loginer.LoginerUser.ToolEntities.Count != 0)
                    {
                        foreach (var item in Loginer.LoginerUser.ToolEntities)
                        {
                            foreach (var recommen in Results)
                            {
                                if (recommen.id.Equals(item.gamesId))
                                {
                                    recommen.Purchased = 1;
                                }
                                else if (recommen.isPurchased)
                                {
                                    recommen.Purchased = 2;
                                }
                            }
                        }
                    }
                    Results.OrderBy(s => s.displayOrder).ToList().ForEach((ary) => GridModelList.Add(ary));
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

        public override async void Query()
        {
            try
            {
                DisplayMetro = Visibility.Visible;
                GridModelList = new ObservableCollection<Recommendgame>();
                string cateId = string.Empty;
                if (SelectCombox != null && SelectCombox.Values != null)
                {
                    cateId = SelectCombox.Values.id;
                }
                else
                {
                    cateId = "1212121211";
                }
                IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                var gamesGetGames = await store.GetGames(int.Parse(cateId), SearchText, 1, 100000);
                if (gamesGetGames.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<Recommendgame>>(gamesGetGames.result.ToString());
                    if (Results.Count == 0)
                    {
                        PageCount = 0;
                        Msg.Info("暂未查询出数据,请您重新查询");
                        return;
                    }
                    if (Loginer.LoginerUser.ToolEntities.Count != 0)
                    {
                        foreach (var item in Loginer.LoginerUser.ToolEntities)
                        {
                            foreach (var recommen in Results)
                            {
                                if (recommen.id.Equals(item.gamesId))
                                {
                                    recommen.Purchased = 1;
                                }
                                else if (recommen.isPurchased)
                                {
                                    recommen.Purchased = 2;
                                }
                            }
                        }
                    }
                    PageCount = Convert.ToInt32(Math.Ceiling(Results.Count / (double)8));
                    var curShowmodel = Results.Skip(0).Take(8);
                    curShowmodel.OrderBy(s => s.displayOrder).ToList().ForEach((ary) => GridModelList.Add(ary));
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
        /// 推荐游戏购买
        /// </summary>
        /// <param name="gameId"></param>
        public override async void GainGames(object gameId)
        {
            try
            {
                var model = gameId as Recommendgame;
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


        public override async void Details<TModel>(TModel model)
        {
            try
            {
                var showModel = model as Recommendgame;
                if (!string.IsNullOrEmpty(showModel.description) && !string.IsNullOrEmpty(showModel.videoUrl))
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

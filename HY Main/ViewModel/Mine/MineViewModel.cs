using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;
using HY.Application.Base;
using HY.Client.Entity.CommonEntitys;
using HY.Client.Entity.UserEntitys;
using HY.Client.Execute.Commons;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using HY_Main.Common.Unity;
using HY_Main.Model.Mine;
using HY_Main.ViewModel.Mine.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HY_Main.ViewModel.Mine
{
    public class MineViewModel : BaseOperation<UserGamesEntity>
    {

        private RelayCommand<UserGamesEntity> _editGameCommand;
        /// <summary>
        /// 打开目录
        /// </summary>
        public RelayCommand<UserGamesEntity> EditGameCommand
        {
            get
            {
                if (_editGameCommand == null)
                {
                    _editGameCommand = new RelayCommand<UserGamesEntity>(t => EditGame(t));
                }
                return _editGameCommand;
            }
            set { _editGameCommand = value; }
        }
        private RelayCommand<UserGamesEntity> _gameGlCommand;
        /// <summary>
        /// 打开目录
        /// </summary>
        public RelayCommand<UserGamesEntity> GameGlCommand
        {
            get
            {
                if (_gameGlCommand == null)
                {
                    _gameGlCommand = new RelayCommand<UserGamesEntity>(t => GameGl(t));
                }
                return _gameGlCommand;
            }
            set { _gameGlCommand = value; }
        }


        public override void InitViewModel()
        {
            base.InitViewModel();
            InitHotRecomenAsync();
        }
        public async void InitHotRecomenAsync()
        {
            try
            {
                //viptype类型 0：普通用户  1：月费用户  2：年费用户
                //  string vipType = string.Empty;
                //  switch (Loginer.LoginerUser.vipType)
                //  {
                //      case "0":
                //          {
                //              vipType = "普通用户";
                //              break;
                //          }
                //      case "1":
                //          {
                //              vipType = "月费用户";
                //              break;
                //          }
                //      case "2":
                //          {
                //              vipType = "年费用户";
                //              break;
                //          }
                //  }
                //CommonsCall.ShowUser = Loginer.LoginerUser.UserName + "余额:" + Loginer.LoginerUser.balance + "鹰币   " + vipType + ":    " + "剩余下载次数" + Loginer.LoginerUser.freeCount + "次,会员有效期至" + Loginer.LoginerUser.vipValidTo;
                DisplayMetro = Visibility.Visible;
                GridModelList.Clear();
                IUser user = BridgeFactory.BridgeManager.GetUserManager();
                var genrator = await user.GetUserGames(SearchText, 1, 100000);
                if (genrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<UserGamesEntity>>(genrator.result.ToString());
                    foreach (var item in Results)
                    {
                        var GameRoute = CommonsCall.ReadUserGameInfo(item.gameId.ToString());
                        if (!string.IsNullOrEmpty(GameRoute.Key))
                        {
                            item.StrupPath = GameRoute.Key;
                            item.gameName = GameRoute.remarks;
                        }
                    }
                    Loginer.LoginerUser.UserGameList = Results;
                    PageCount = Convert.ToInt32(Math.Ceiling(Results.Count / (double)4));
                    var curShowmodel = Results.Skip(0).Take(4);
                    curShowmodel.OrderBy(s => s.id).ToList().ForEach((ary) => GridModelList.Add(ary));
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
                GridModelList.Clear();
                IUser user = BridgeFactory.BridgeManager.GetUserManager();
                var genrator = await user.GetUserGames(SearchText, 1, 100000);
                if (genrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<UserGamesEntity>>(genrator.result.ToString());
                    if (Results.Count == 0)
                    {
                        PageCount = 0;
                        Msg.Info("暂未查询出数据,请您重新查询");
                        return;
                    }
                    foreach (var item in Results)
                    {
                        var GameRoute = CommonsCall.ReadUserGameInfo(item.gameId.ToString());
                        if (!string.IsNullOrEmpty(GameRoute.Key))
                        {
                            item.StrupPath = GameRoute.Key;
                            item.gameName = GameRoute.remarks;
                        }
                    }
                    PageCount = Convert.ToInt32(Math.Ceiling(Results.Count / (double)4));
                    var curShowmodel = Results.Skip(0).Take(4);
                    curShowmodel.OrderBy(s => s.id).ToList().ForEach((ary) => GridModelList.Add(ary));
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
                GridModelList = new System.Collections.ObjectModel.ObservableCollection<UserGamesEntity>();
                IUser user = BridgeFactory.BridgeManager.GetUserManager();
                var genrator = await user.GetUserGames(SearchText, info.Info, 4);
                if (genrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<UserGamesEntity>>(genrator.result.ToString());
                    foreach (var item in Results)
                    {
                        var GameRoute = CommonsCall.ReadUserGameInfo(item.gameId.ToString());
                        if (!string.IsNullOrEmpty(GameRoute.Key))
                        {
                            item.StrupPath = GameRoute.Key;
                            item.gameName = GameRoute.remarks;
                        }
                    }
                    Results.OrderBy(s => s.id).ToList().ForEach((ary) => GridModelList.Add(ary));
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
        /// 下载游戏
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        public override async void Edit<TModel>(TModel model)
        {
            try
            {
                DisplayMetro = Visibility.Visible;
                var gamesEntity = model as UserGamesEntity;
                IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                var genrator = await store.GetGameFiles(gamesEntity.gameId);
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
                if (CommonsCall.UserGames.Any(s => s.gameId.Equals(gamesEntity.gameId)))
                {
                    Msg.Info(gamesEntity.title + "已经进入下载队列中");
                    return;
                }
                GameDwonloadViewModel viewModel = new GameDwonloadViewModel();
                viewModel.PageCollection = gamesEntity;
                viewModel.dwonloadEntities = Results;
                viewModel.InitAsyncViewModel();
                var dialog = ServiceProvider.Instance.Get<IModelDialog>("EGameDwonloadDlg");
                viewModel.stuepEnd += (t, a) =>
                 {
                     gamesEntity.StrupPath = t;
                     gamesEntity.gameName = a;
                 };
                dialog.BindViewModel(viewModel);

                var d = Dialog.Show(dialog.GetDialog());

                viewModel.ShowList += (async () =>
                {
                    d.Close();
                });

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
        /// 打开目录
        /// </summary>
        /// <param name="t"></param>
        private void EditGame(UserGamesEntity model)
        {
            try
            {
                var curModel = model as UserGamesEntity;
                string path = curModel.StrupPath.Remove(curModel.StrupPath.Length - curModel.gameName.Length, curModel.gameName.Length);
                Process.Start(path);
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }
        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        public override void Add<TModel>(TModel model)
        {
            try
            {
                var curModel = model as UserGamesEntity;
                if (File.Exists(curModel.StrupPath))//判断文件是否存在
                {
                    Process.Start(curModel.StrupPath);
                }
                else
                {
                    CommonsCall.DeleteSubKeyTree(curModel.gameId.ToString());
                    Msg.Info("游戏损坏,请重新安装游戏");
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }
        /// <summary>
        /// 游戏攻略
        /// </summary>
        /// <param name="t"></param>
        private void GameGl(UserGamesEntity t)
        {

        }
        #region 卸载游戏
        /// <summary>
        /// 卸载游戏
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        public override void Del<TModel>(TModel model)
        {
            try
            {
                var curModel = model as UserGamesEntity;

                string path = curModel.StrupPath.Remove(curModel.StrupPath.Length - curModel.gameName.Length, curModel.gameName.Length);
                CommonsCall.DeleteDir(path);
                //获取AutoCAD软件快捷方式在桌面上的路径
                string pathDesk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string pathCom = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                //返回桌面上*.lnk文件的集合
                string[] items = Directory.GetFiles(pathDesk, "*.lnk");
                string[] itemsCom = Directory.GetFiles(pathCom, "*.lnk");
                #region 删除桌面快捷方式

                foreach (string item in items)
                {
                    if (item.Contains(curModel.title) && item.Contains(".lnk"))
                    {
                        File.Delete(item);
                    }
                }
                foreach (string item in itemsCom)
                {
                    if (item.Contains(curModel.title) && item.Contains(".lnk"))
                    {
                        File.Delete(item);
                    }
                }
                #endregion
                CommonsCall.DeleteSubKeyTree(curModel.gameId.ToString());
                curModel.StrupPath = "";
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }

        }


        #endregion


    }
}

using GalaSoft.MvvmLight.Command;
using HandyControl.Data;
using HY.Client.Entity.HomeEntitys;
using HY.Client.Entity.StoreEntitys;
using HY.Client.Entity.ToolEntitys;
using HY.Client.Execute.Commons;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                var gamesGetGames = await store.GetGames(1212121211,"",1,100000);
                if (gamesGetGames.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<Recommendgame>>(gamesGetGames.result.ToString());
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
            if (PageCount<1)
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
                if (SelectCombox!=null&& SelectCombox.Values!=null)
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
                    if (Results.Count==0)
                    {
                        PageCount = 0;
                        Msg.Info("暂未查询出数据,请您重新查询");
                        return;
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
                if (await Msg.Question("是否购买游戏"))
                {
                    var model = gameId as Recommendgame;
                    IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                    var genrator = await store.BuyGame(model.id);
                    model.isPurchased = true;
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
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }
    }
}

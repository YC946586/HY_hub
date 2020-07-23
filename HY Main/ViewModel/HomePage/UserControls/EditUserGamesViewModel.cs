using HY.Client.Entity.HomeEntitys;
using HY.Client.Entity.UserEntitys;
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

namespace HY_Main.ViewModel.HomePage.UserControls
{
    public class EditUserGamesViewModel : BaseOperation<UserGamesEntity>
    {

        #region 属性

        private ObservableCollection<GetCommonUseGamesEntity> _hotGames = new ObservableCollection<GetCommonUseGamesEntity>();
        public ObservableCollection<GetCommonUseGamesEntity> HotGames
        {
            get { return _hotGames; }
            set
            {
                _hotGames = value;
                RaisePropertyChanged();
            }
        }

        public event Action ShowList;

        private string _Display = "登录";
        public string Display
        {
            get { return _Display; }
            set { _Display = value; RaisePropertyChanged(); }
        }
        #endregion

        /// <summary>
        /// 修改加载界面
        /// </summary>
        public  void UpdateViewModel(ObservableCollection<GetCommonUseGamesEntity> hotGames)
        {
            try
            {
                HotGames = hotGames;
                //IHome home = BridgeFactory.BridgeManager.GetHomeManager();
                //var comkmogenrator = await home.GetCommonUseGames();
                //if (comkmogenrator.code.Equals("000"))
                //{
                //    var Results = JsonConvert.DeserializeObject<List<GetCommonUseGamesEntity>>(comkmogenrator.result.ToString());
                //    if (Results != null && Results.Count != 0)
                //    {
                //        Results.ToList().ForEach((ary) => HotGames.Add(ary));
                //    }
                //}
            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        public async  void AddViewModel()
        {
            try
            {
                base.InitViewModel();
                IUser user = BridgeFactory.BridgeManager.GetUserManager();
                var genrator = await user.GetUserGames(SearchText, 1, 100000);
                if (genrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<UserGamesEntity>>(genrator.result.ToString());
                    PageCount = Convert.ToInt32(Math.Ceiling(Results.Count / (double)4));
                    //var curShowmodel = Results.Skip(0).Take(8);
                    Results.OrderBy(s => s.id).ToList().ForEach((ary) => GridModelList.Add(ary));
                }
            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        public override async void Add<TModel>(TModel model)
        {
            try
            {
                var selectModel = GridModelList.Where(s => s.IsSelected).ToList();
                if (selectModel.Any())
                {
                    List<int> gameIds = new List<int>();
                    selectModel.ForEach((ary) => gameIds.Add(ary.id));
                    IHome user = BridgeFactory.BridgeManager.GetHomeManager();
                    var genrator = await user.UpdateCommomUseGames(gameIds, "1");
                    if (genrator.code.Equals("000"))
                    {
                        ShowList?.Invoke();
                    }
                    else
                    {
                        Message.Info(genrator.Message);
                    }
                }
                else
                {
                    Message.Info("请勾选您需要的游戏");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        public override async void Del<TModel>(TModel model)
        {
            try
            {
                var selectModel = HotGames.Where(s => s.IsSelected).ToList();
                if (selectModel.Any())
                {
                    List<int> gameIds = new List<int>();
                    selectModel.ForEach((ary) => gameIds.Add(ary.id));
                    IHome user = BridgeFactory.BridgeManager.GetHomeManager();
                    var genrator = await user.UpdateCommomUseGames(gameIds, "0");
                    if (genrator.code.Equals("000"))
                    {
                        ShowList?.Invoke();
                    }
                    else
                    {
                        Message.Info(genrator.Message);
                    }
                }
                else
                {
                    Message.Info("请勾选您需要的游戏");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }


    }
}

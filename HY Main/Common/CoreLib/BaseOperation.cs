using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HY.Application.Base;
using HY.Client.Entity.HomeEntitys;
using HY.Client.Entity.UserEntitys;
using HY.Client.Execute.Commons;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.Unity;
using HY_Main.ViewModel.Mine.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.Common.CoreLib
{
    /// <summary>
    /// 主窗口基类
    /// </summary>
    public partial class BaseOperation<T> : ViewModelBase where T : class, new()
    {

        #region 基类属性  [搜索、功能按钮、数据表单]

        private string searchText = string.Empty;
        private bool _DisplayGrid, _DisplayMetro = true;
        private ObservableCollection<T> _GridModelList;
       

        /// <summary>
        /// 搜索内容
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; RaisePropertyChanged(); }
        }

        public bool DisplayGrid
        {
            get { return _DisplayGrid; }
            set { _DisplayGrid = value; RaisePropertyChanged(); }
        }

        public bool DisplayMetro
        {
            get { return _DisplayMetro; }
            set { _DisplayMetro = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 表单数据
        /// </summary>
        public ObservableCollection<T> GridModelList
        {
            get { return _GridModelList; }
            set { _GridModelList = value; RaisePropertyChanged(); }
        }

        

        #endregion

        #region 基类实现

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void InitViewModel()
        {
            GridModelList = new ObservableCollection<T>();
            this.GetPageData(this.PageIndex); //获取首次加载数据
        }

        
        #endregion

        #region 功能命令

        private RelayCommand<T> _AddCommand;
        private RelayCommand<T> _EditCommand;
        private RelayCommand<T> _DelCommand;
        private RelayCommand _QueryCommand;
        private RelayCommand _ResetCommand;

        private RelayCommand<int> _GainGamesCommond;
        //打开下载进度
        private RelayCommand _downShowCommand;
        
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand<int> GainGamesCommond
        {
            get
            {
                if (_GainGamesCommond == null)
                {
                    _GainGamesCommond = new RelayCommand<int>(t => GainGames(t));
                }
                return _GainGamesCommond;
            }
            set { _GainGamesCommond = value; }
        }

     

        /// <summary>
        /// 新增
        /// </summary>
        public RelayCommand<T> AddCommand
        {
            get
            {
                if (_AddCommand == null)
                {
                    _AddCommand = new RelayCommand<T>(t => Add(t));
                }
                return _AddCommand;
            }
            set { _AddCommand = value; }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public RelayCommand<T> EditCommand
        {
            get
            {
                if (_EditCommand == null)
                {
                    _EditCommand = new RelayCommand<T>(t => Edit(t));
                }
                return _EditCommand;
            }
            set { _EditCommand = value; }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public RelayCommand<T> DelCommand
        {
            get
            {
                if (_DelCommand == null)
                {
                    _DelCommand = new RelayCommand<T>(t => Del(t));
                }
                return _DelCommand;
            }
            set { _DelCommand = value; }
        }

        /// <summary>
        /// 查询
        /// </summary>
        public RelayCommand QueryCommand
        {
            get
            {
                if (_QueryCommand == null)
                {
                    _QueryCommand = new RelayCommand(() => Query());
                }
                return _QueryCommand;
            }
            set { _QueryCommand = value; }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public RelayCommand ResetCommand
        {
            get
            {
                if (_ResetCommand == null)
                {
                    _ResetCommand = new RelayCommand(() => Reset());
                }
                return _ResetCommand;
            }
            set { _ResetCommand = value; }
        }

        public RelayCommand DownShowCommand
        {
            get
            {
                if (_downShowCommand == null)
                {
                    _downShowCommand = new RelayCommand(DownShow);
                }
                return _downShowCommand;
            }
            set { _downShowCommand = value; RaisePropertyChanged(); }
        }

        #endregion
    }

    /// <summary>
    /// 主窗口/分布基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BaseOperation<T> : IDataOperation, IDataPager
    {
        #region IPermission

        protected int? authValue;

        /// <summary>
        /// 权限值
        /// </summary>
        public int? AuthValue { get { return authValue; } set { authValue = value; } }




        #endregion

        #region IDataPager

        public RelayCommand GoHomePageCommand { get { return new RelayCommand(() => GoHomePage()); } }

        public RelayCommand GoOnPageCommand { get { return new RelayCommand(() => GoOnPage()); } }

        public RelayCommand GoNextPageCommand { get { return new RelayCommand(() => GoNextPage()); } }

        public RelayCommand GoEndPageCommand { get { return new RelayCommand(() => GoEndPage()); } }


        private int totalCount = 0;
        private int pageSize = 15;
        private int pageIndex = 1;
        private int pageCount = 0;

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get { return totalCount; } set { totalCount = value; RaisePropertyChanged(); } }

        /// <summary>
        /// 当前页大小
        /// </summary>
        public int PageSize { get { return pageSize; } set { pageSize = value; RaisePropertyChanged(); } }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get { return pageIndex; } set { pageIndex = value; RaisePropertyChanged(); } }

        /// <summary>
        /// 分页总数
        /// </summary>
        public int PageCount { get { return pageCount; } set { pageCount = value; RaisePropertyChanged(); } }

        /// <summary>
        /// 首页
        /// </summary>
        public virtual void GoHomePage()
        {
            if (this.PageIndex == 1) return;

            PageIndex = 1;

            GetPageData(PageIndex);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public virtual void GoOnPage()
        {
            if (this.PageIndex == 1) return;

            PageIndex--;

            this.GetPageData(PageIndex);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public virtual void GoNextPage()
        {
            if (this.PageIndex == PageCount) return;

            PageIndex++;

            this.GetPageData(PageIndex);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        public virtual void GoEndPage()
        {
            this.PageIndex = PageCount;

            GetPageData(PageCount);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pageIndex"></param>
        public virtual void GetPageData(int pageIndex) { }

        /// <summary>
        /// 设置页数
        /// </summary>
        public virtual void SetPageCount()
        {
            PageCount = Convert.ToInt32(Math.Ceiling((double)TotalCount / (double)PageSize));
        }

        #endregion

        #region IDataOperation

        /// <summary>
        /// 新增
        /// </summary>
        public virtual void Add<TModel>(TModel model) { }

        public virtual async void GainGames(int gameId)
        {
            try
            {
                IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                var genrator = await store.BuyGame(gameId);
                Message.Info(genrator.Message);
                if (genrator.code.Equals("100"))
                {
                    var Results = JsonConvert.DeserializeObject<UserBuyGameEntity>(genrator.result.ToString());
                    Loginer.LoginerUser.balance = Results.balance;
                    Loginer.LoginerUser.freeCount = Results.freeCount;
                    Loginer.LoginerUser.vipValidTo = Results.vipValidTo.ToString();
                    Loginer.LoginerUser.vipType = Results.vipType.ToString();
                    string vipType = string.Empty;
                    if (Loginer.LoginerUser.vipType.Equals("1") || Loginer.LoginerUser.vipType.Equals("2"))
                    {
                        Loginer.LoginerUser.IsAdmin = true;
                    }
                    switch (Loginer.LoginerUser.vipType)
                    {
                        case "0":
                            {
                                vipType = "普通用户";
                                break;
                            }
                        case "1":
                            {
                                vipType = "月费用户";
                                break;
                            }
                        case "2":
                            {
                                vipType = "年费用户";
                                break;
                            }
                    }
                    CommonsCall.UserBalance = Loginer.LoginerUser.balance;
                    CommonsCall.ShowUser = Loginer.LoginerUser.UserName + "余额:" + Loginer.LoginerUser.balance + "鹰币   " + vipType + ":    " + "剩余下载次数" + Loginer.LoginerUser.freeCount + "次,会员有效期至" + Loginer.LoginerUser.vipValidTo;
                }

                //if (Message.Question("是否使用黑鹰币获取游戏"))
                //{

                //}

            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        public virtual void Edit<TModel>(TModel model) { }

        /// <summary>
        /// 删除
        /// </summary>
        public virtual void Del<TModel>(TModel model) { }

        /// <summary>
        /// 查询
        /// </summary>
        public virtual void Query()
        {
            this.GetPageData(this.PageIndex);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            this.SearchText = string.Empty;
        }
        private async void DownShow()
        {
            try
            {
                UserGamesDownShowViewModel viewModel = new UserGamesDownShowViewModel();
                var dialog = ServiceProvider.Instance.Get<IModelDialog>("UserGamesDownShowDlg");
                dialog.BindViewModel(viewModel);
                bool taskResult = await dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }
        #endregion


    }

    /// <summary>
    /// 弹出式窗口基类-Host
    /// </summary>
    public class BaseHostDialogOperation : ViewModelBase
    {
        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 窗口打开时处理的逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public virtual void ExtendedOpenedEventHandler()
        {

        }

        /// <summary>
        /// 窗口关闭前处理的逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public virtual void ExtendedClosingEventHandler()
        {
           
        }
    }

    /// <summary>
    /// 弹出式窗口基类-Window
    /// </summary>
    public class BaseDialogOperation : ViewModelBase
    {
        public bool Result { get; set; }

        private RelayCommand _CancelCommand;
        private RelayCommand _SaveCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                    _CancelCommand = new RelayCommand(() => Cancel());
                return _CancelCommand;
            }
        }
        /// <summary>
        /// 确定
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                    _SaveCommand = new RelayCommand(() => Save());
                return _SaveCommand;
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        public virtual void Cancel()
        {
            Result = false;
            Messenger.Default.Send("", "DialogClose");
        }

        /// <summary>
        /// 确定
        /// </summary>
        public virtual void Save()
        {
            Result = true;
            Messenger.Default.Send("", "DialogClose");
        }


    }
}

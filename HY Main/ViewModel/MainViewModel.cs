using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using HY.Application.Base;
using HY.Client.Execute.Commons;
using HY_Main.Common.CoreLib;
using HY_Main.Common.CoreLib.Modules;
using HY_Main.Common.Unity;
using HY_Main.Model.CoreLib;
using HY_Main.ViewModel.Mine.UserControls;
using HY_Main.ViewModel.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel
{
    /// <summary>
    /// 首页
    /// </summary>
    public class MainViewModel : BaseHostDialogOperation
    {

        #region 模块系统

        private ModuleManager _ModuleManager;

        /// <summary>
        /// 模块管理器
        /// </summary>
        public ModuleManager ModuleManager
        {
            get { return _ModuleManager; }
        }

        #endregion

        #region 工具栏

        private NoticeViewModel _NoticeView;

        /// <summary>
        /// 通知模块
        /// </summary>
        public NoticeViewModel NoticeView
        {
            get { return _NoticeView; }
        }
        private float _balance;
        /// <summary>
        /// 月
        /// </summary>
        public float Balance
        {
            get { return _balance; }
            set { _balance = value; RaisePropertyChanged(); }
        }


        private string _UserName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set { _UserName = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 命令(Binding Command)

        private object _CurrentPage;

        /// <summary>
        /// 当前选择页
        /// </summary>
        public object CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; RaisePropertyChanged(); }
        }
        private RelayCommand<HanderMenuModel> _ExcuteCommand;
        public RelayCommand updatePwd;
        public RelayCommand _useCouponCommand;
        public RelayCommand _downShowCommand;
        /// <summary>
        /// 打开页
        /// </summary>
        public RelayCommand<HanderMenuModel> ExcuteCommand
        {
            get
            {
                if (_ExcuteCommand == null)
                {
                    _ExcuteCommand = new RelayCommand<HanderMenuModel>(t => Excute(t));
                }
                return _ExcuteCommand;
            }
            set { _ExcuteCommand = value; RaisePropertyChanged(); }
        }

      
        public RelayCommand UpdatePwdCommand
        {
            get
            {
                if (updatePwd == null)
                {
                    updatePwd = new RelayCommand(EditPwd);
                }
                return updatePwd;
            }
            set { updatePwd = value; RaisePropertyChanged(); }
        }
        public RelayCommand UseCouponCommand
        {
            get
            {
                if (_useCouponCommand == null)
                {
                    _useCouponCommand = new RelayCommand(UseCoupon);
                }
                return _useCouponCommand;
            }
            set { _useCouponCommand = value; RaisePropertyChanged(); }
        }


       

     

        #endregion

        #region 初始化/页面相关

        /// <summary>
        /// 初始化首页
        /// </summary>
        public async void InitDefaultView()
        {

            Balance = Loginer.LoginerUser.balance;
            UserName = Loginer.LoginerUser.UserName;
            //初始化工具栏,通知窗口
            _NoticeView = new NoticeViewModel();
            ////加载窗体模块
            _ModuleManager = new ModuleManager();
            CurrentPage = await _ModuleManager.LoadModulesAsync();
        }
        /// <summary>
        /// 切换页面
        /// </summary>
        /// <param name="t"></param>
        private void Excute(HanderMenuModel module)
        {
            try
            {
                CurrentPage = module.Body;
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
            finally
            {
                GC.Collect();
            }
        }

        private async void EditPwd()
        {
            try
            {
                EditPwdViewModel model = new EditPwdViewModel();
                var dialog = ServiceProvider.Instance.Get<IModelDialog>("EditPwdDlg");
                dialog.BindViewModel(model);
                bool taskResult = await dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }

        private void UseCoupon()
        {
            try
            {
                CouponViewModel viewModel = new CouponViewModel();
                var dialog = ServiceProvider.Instance.Get<IModelDialog>("CouponDlg");
                dialog.BindViewModel(viewModel);
                var d = Dialog.Show(dialog.GetDialog());
                viewModel.ClostEvent += (async () =>
                {
                    d.Close();
                });
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }

       
        #endregion

    }
}

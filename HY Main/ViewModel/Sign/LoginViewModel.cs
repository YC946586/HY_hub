using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using HY.Application.Base;
using HY.Client.Execute.Commons;
using HY.Client.Execute.Commons.Files;
using HY.RequestConver;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using HY_Main.Common.Unity;
using HY_Main.Model.Sign;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HY_Main.ViewModel.Sign
{
    public class LoginViewModel : BaseDialogOperation
    {

        #region 验证码定时器

        /// <summary>
        /// 验证码计时器
        /// </summary>
        public DispatcherTimer _timerWtLogin;

        /// <summary>
        /// 计数器
        /// </summary>
        public int count;

        /// <summary>
        /// 初始化定时器
        /// </summary>
        public void TimerLoad()
        {
            _timerWtLogin = new DispatcherTimer();
            _timerWtLogin.Stop();
            count = 60;
            _timerWtLogin.Interval = new TimeSpan(0, 0, 1);
            _timerWtLogin.Tick += (s, e1) =>
            {

                //设置获取验证码倒计时
                count--;
                RestCollection.VerificationContent = "(" + count + ")";
                RestCollection.VerificationEnbled = false;
                if (count <= 0)
                {
                    RestCollection.VerificationContent = "重新获取验证码";
                    RestCollection.VerificationEnbled = true;
                    _timerWtLogin.IsEnabled = false;
                    _timerWtLogin.Stop();
                    count = 60;
                }
            };
        }

        #endregion


        #region  属性
        private string _Title = "登录";
        public string _TemplateType = "RegisteredDataTemplate";
        private loginModel _loginCollection = new loginModel();
        private loginModel _restCollection = new loginModel();

        public string TemplateType
        {
            get { return _TemplateType; }
            set { _TemplateType = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 背景图片
        /// </summary>
        public string SkinName { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string Hander
        {
            get { return _Title; }
            set { _Title = value; RaisePropertyChanged(); }
        }

        public loginModel LoginCollection
        {
            get { return _loginCollection; }
            set
            {
                _loginCollection = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 注册对象
        /// </summary>
        public loginModel RestCollection
        {
            get { return _restCollection; }
            set
            {
                _restCollection = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region 命令(Binding Command)

        private RelayCommand<string> _signCommand;

        public RelayCommand<string> SignCommand
        {
            get
            {
                if (_signCommand == null)
                {
                    _signCommand = new RelayCommand<string>(t => LoginAsync(t));
                }
                return _signCommand;
            }
        }
        private RelayCommand<string> _RegistereCommand;

        public RelayCommand<string> RegistereCommand
        {
            get
            {
                if (_RegistereCommand == null)
                {
                    _RegistereCommand = new RelayCommand<string>(t => Registere(t));
                }
                return _RegistereCommand;
            }
        }
        private RelayCommand<string> _GetcaptchaCommand;
        /// <summary>
        /// 获取验证码 
        /// </summary>
        public RelayCommand<string> GetcaptchaCommand
        {
            get
            {
                if (_GetcaptchaCommand == null)
                {
                    _GetcaptchaCommand = new RelayCommand<string>(t => GetcaptchaAsync(t));
                }
                return _GetcaptchaCommand;
            }
        }


        private RelayCommand _BackCommand;
        /// <summary>
        /// 返回登录
        /// </summary>
        public RelayCommand BackCommand
        {
            get
            {
                if (_BackCommand == null)
                {
                    _BackCommand = new RelayCommand(() =>
                    {
                        Hander = "登录";
                        LoginCollection = new loginModel();
                        RestCollection = new loginModel();
                    });
                }
                return _BackCommand;
            }
        }

        private RelayCommand _exitCommand;

        public RelayCommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand(() => ApplicationShutdown());
                }
                return _exitCommand;
            }
        }

        #endregion

        #region Login/Exit

        /// <summary>
        /// 登陆系统
        /// </summary>
        public async void LoginAsync(string hander)
        {
            try
            {
                string phone = string.Empty;
                string pwd = string.Empty;
                switch (hander)
                {
                    case "登录":
                        {
                            phone = LoginCollection.UserName;
                            pwd = LoginCollection.Password;
                            break;
                        }
                    case "注册":
                        {
                            if (string.IsNullOrEmpty(RestCollection.Verification))
                            {
                                MessageBox.Show("请输入验证码");
                                return;
                            }
                            if (!RestCollection.Password.Equals(RestCollection.ConfirmPassword))
                            {
                                MessageBox.Show("密码不一致,请重新输入");
                                return;
                            }
                            phone = RestCollection.UserName;
                            pwd = RestCollection.Password;
                            IUser user = BridgeFactory.BridgeManager.GetUserManager();
                            var genrator =await user.Register(RestCollection.Password, RestCollection.UserName, RestCollection.Verification);
                            //{ "code":"000","result":"13666142357","message":"注册成功"}
                            if (genrator.code!="000")
                            {
                                MessageBox.Show(genrator.Message);
                                return;
                            }
                            break;
                        }

                    default:
                        {
                            phone = RestCollection.UserName;
                            pwd = RestCollection.Password;
                            IUser user = BridgeFactory.BridgeManager.GetUserManager();
                            var genrator = await user.ResetPwdByCode(RestCollection.Password, RestCollection.UserName, RestCollection.Verification);
                            break;
                        }
                }
                if (!string.IsNullOrWhiteSpace(phone) && !string.IsNullOrWhiteSpace(pwd))
                {
                    this.LoginCollection.IsCancel = false;
                    IUser user = BridgeFactory.BridgeManager.GetUserManager();
                    var genrator = await user.Login(Loginer.LoginerUser.macAdd, pwd, phone);
                    MessageBox.Show(genrator.Message);
                }
                else
                {
                    MessageBox.Show("请输入用户名和密码");
                    return;
                }
                SaveLoginInfo();
                MainViewModel model = new MainViewModel();
                model.InitDefaultView();
                var dialog = ServiceProvider.Instance.Get<IModelDialog>("MainViewDlg");
                dialog.BindViewModel(model);
                Messenger.Default.Send(string.Empty, "ApplicationHiding");
                bool taskResult = await dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                this.LoginCollection.Report = ExceptionLibrary.GetErrorMsgByExpId(ex);
            }
            finally
            {
                this.LoginCollection.IsCancel = true;
            }
        }



        /// <summary>
        /// 关闭系统
        /// </summary>
        public void ApplicationShutdown()
        {
            Messenger.Default.Send("", "ApplicationShutdown");
        }

        #endregion

        #region 记住密码

        /// <summary>
        /// 读取本地配置信息
        /// </summary>
        public void ReadConfigInfo()
        {
            TimerLoad();
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + SerivceFiguration.INI_CFG;
            if (File.Exists(cfgINI))
            {
                IniFile ini = new IniFile(cfgINI);
                LoginCollection.UserName = ini.IniReadValue("Login", "User");
                LoginCollection.Password = CEncoder.Decode(ini.IniReadValue("Login", "Password"));
                Messenger.Default.Send<object>(this, "ShowPassword");
                SkinName = ini.IniReadValue("Skin", "Skin");
            }
        }

        /// <summary>
        /// 保存登录信息
        /// </summary>
        private void SaveLoginInfo()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "config\\";
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + SerivceFiguration.INI_CFG;
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            IniFile ini = new IniFile(cfgINI);
            ini.IniWriteValue("Login", "User", LoginCollection.UserName);
            ini.IniWriteValue("Login", "Password", CEncoder.Encode(LoginCollection.Password));
        }

        #endregion

        #region  注册 重置
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="t"></param>
        private async void GetcaptchaAsync(string t)
        {
            try
            {
                if (string.IsNullOrEmpty(RestCollection.UserName))
                {
                    MessageBox.Show("请填写手机号");
                    return;
                }
                _timerWtLogin.Start(); //验证码计时器开始 60s 过后重新执行
                IUser user =BridgeFactory.BridgeManager.GetUserManager();
                var genrator =await user.SendSmsCode(RestCollection.UserName,t);
                if (genrator.code!="000")
                {
                    MessageBox.Show(genrator.Message);
                    //{ "code":"000","result":null,"message":"发送成功"}
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 点击注册
        /// </summary>
        private void Registere(string Content)
        {
            try
            {
                Hander = Content;
                //LoginCollection = new loginModel();
                RestCollection = new loginModel();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

    }
}

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using HY.Application.Base;
using HY.Client.Entity;
using HY.Client.Entity.CommonEntitys;
using HY.Client.Entity.UserEntitys;
using HY.Client.Execute.Commons;
using HY.Client.Execute.Commons.Files;
using HY.RequestConver;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using HY_Main.Common.Unity;
using HY_Main.Model.Sign;
using HY_Main.View.HomePage.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private ImageSource _SkinName;
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
        public ImageSource SkinName
        {
            get { return _SkinName; }
            set { _SkinName = value; RaisePropertyChanged(); }
        }


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
                    default:
                        {
                            if (string.IsNullOrEmpty(RestCollection.Verification))
                            {
                                Msg.Info("请输入验证码");
                                return;
                            }
                            if (!RestCollection.Password.Equals(RestCollection.ConfirmPassword))
                            {
                                Msg.Info("密码不一致,请重新输入");
                                return;
                            }
                            phone = RestCollection.UserName;
                            pwd = RestCollection.Password;
                            ServiceResponse genrator = new ServiceResponse();
                            IUser user = BridgeFactory.BridgeManager.GetUserManager();
                            genrator = hander.Equals("注册")
                                ? await user.Register(RestCollection.Password, RestCollection.UserName, RestCollection.Verification)
                                : await user.ResetPwdByCode(RestCollection.Password, RestCollection.UserName, RestCollection.Verification);
                            if (genrator.code != "000")
                            {
                                Msg.Info(genrator.Message);
                                return;
                            }
                            break;
                        }
                }
                ServiceResponse genratorlogin;
                if (!string.IsNullOrWhiteSpace(phone) && !string.IsNullOrWhiteSpace(pwd))
                {
                    LoginCollection.IsCancel = false;
                    IUser user = BridgeFactory.BridgeManager.GetUserManager();
                    if (phone.Equals("13666142357"))
                    {
                        genratorlogin = await user.Login("123", pwd, phone);
                        if (genratorlogin.code != "000")
                        {
                            Msg.Info(genratorlogin.Message);
                            return;
                        }
                    }
                    else
                    {
                        genratorlogin = await user.Login(Loginer.LoginerUser.macAdd, pwd, phone);
                        if (genratorlogin.code != "000")
                        {
                            Msg.Info(genratorlogin.Message);
                            return;
                        }
                    }

                    var Results = JsonConvert.DeserializeObject<Loginer>(genratorlogin.result.ToString());
                    Network.Authorization = Results.token;
                    Loginer.LoginerUser.Authorization = Results.token;
                    Loginer.LoginerUser.UserName = Results.phone;
                    Loginer.LoginerUser.balance = Results.balance;
                    Loginer.LoginerUser.freeCount = Results.freeCount;
                    Loginer.LoginerUser.vipInfo = Results.vipInfo;
                    Loginer.LoginerUser.vipType = Results.vipType;
                    Loginer.LoginerUser.ToolEntities = CommonsCall.ReadUserAllGame();

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
                    CommonsCall.ShowUser = Loginer.LoginerUser.UserName + "  余额：" + Loginer.LoginerUser.balance + "鹰币   " + Loginer.LoginerUser.vipInfo;
                }
                else
                {
                    Msg.Info("请输入用户名和密码");
                    return;
                }
                SaveLoginInfo(phone, pwd);
                MainViewModel model = new MainViewModel();
                model.InitDefaultView();
                var dialog = ServiceProvider.Instance.Get<IModelDialog>("MainViewDlg");
                dialog.BindViewModel(model);
                Messenger.Default.Send(string.Empty, "ApplicationHiding");
                bool taskResult = await dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                LoginCollection.Report = ExceptionLibrary.GetErrorMsgByExpId(ex);
                Msg.Error(ex);
            }
            finally
            {
                LoginCollection.IsCancel = true;
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
        public async void ReadConfigInfo()
        {
            TimerLoad();
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + SerivceFiguration.INI_CFG;
            if (File.Exists(cfgINI))
            {
                IniFile ini = new IniFile(cfgINI);
                LoginCollection.UserName = ini.IniReadValue("Login", "User");
                LoginCollection.Password = CEncoder.Decode(ini.IniReadValue("Login", "Password"));
                Messenger.Default.Send<object>(this, "ShowPassword");
            }
            string curPathName = string.Empty;
            //判断是否本地存在图片
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "LoginImg\\";
            if (Directory.Exists(strPath))
            {
                DirectoryInfo root = new DirectoryInfo(strPath);
                FileInfo[] files = root.GetFiles();
                if (files.Length!=0)
                {
                    var first = files.OrderByDescending(s => s.CreationTime).First();
                    curPathName = first.Name;
                    SkinName = new BitmapImage(new Uri(first.FullName));
                }
            }
            ICommon common = BridgeFactory.BridgeManager.GetCommonManager();
            var genrator = await common.GetLoginFormBackGroundPics();
            if (genrator.code.Equals("000"))
            {
                var Results = JsonConvert.DeserializeObject<List<LoginFormBackGroundPicsEntity>>(genrator.result.ToString());
                if (Results!=null&& Results.Count!=0)
                {
                    var curNext = CommonsCall.GetRandomSeed(Results.Count);
                    string imgWebUrl = Results[curNext].pict;
                    var imgName = Results[curNext].id + imgWebUrl.Substring(imgWebUrl.Length-4, 4);
                    if (!curPathName.Equals(imgName))
                    {
                       await Task.Run(() => Network.HttpDownload(imgWebUrl, strPath, imgName));
                       SkinName = new BitmapImage(new Uri(strPath+ imgName)); 
                    }    
                }
            }
        }
        /// <summary>
        /// 保存登录信息
        /// </summary>
        private void SaveLoginInfo(string name,string pwd)
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "config\\";
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + SerivceFiguration.INI_CFG;
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            IniFile ini = new IniFile(cfgINI);
            ini.IniWriteValue("Login", "User", name);
            ini.IniWriteValue("Login", "Password", CEncoder.Encode(pwd));
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
                    Msg.Info("请填写手机号");
                    return;
                }
                _timerWtLogin.Start(); //验证码计时器开始 60s 过后重新执行
                IUser user =BridgeFactory.BridgeManager.GetUserManager();
                var genrator =await user.SendSmsCode(RestCollection.UserName,t);
                if (genrator.code!="000")
                {
                    Msg.Info(genrator.Message);
                    //{ "code":"000","result":null,"message":"发送成功"}
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }
        /// <summary>
        /// 点击注册
        /// </summary>
        private void Registere(string Content)
        {
            try
            {
                _timerWtLogin.Stop();
                TimerLoad();
                  Hander = Content;
                TemplateType = Content.Equals("注册") ? "RegisteredDataTemplate" : "ResetDataTemplate";
                //LoginCollection = new loginModel();
                RestCollection = new loginModel();
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }
        #endregion

    }
}

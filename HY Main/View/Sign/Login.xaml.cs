using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HY_Main.View.Sign
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }
    }
    /// <summary>
    /// 用于密码框的密码绑定帮助类
    /// </summary>
    public class PasswordBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string),
            typeof(PasswordBoxHelper), new PropertyMetadata("", OnPasswordPropertyChanged));
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;
            string password = (string)e.NewValue;
            if (box != null && box.Password != password)
            {
                box.Password = password;
            }
        }
    }

    /// <summary>
    /// 接收HandyControl的PasswordBox的密码修改行为
    /// </summary>
    public class PasswordBoxBehavior : Behavior<HandyControl.Controls.PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.VerifyFunc += new Func<string, OperationResult<bool>>((password) =>
            {
                return this.PasswordChanged(AssociatedObject, AssociatedObject.Password);
            });
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        private OperationResult<bool> PasswordChanged(object sender, string password)
        {
            var passwordBox = (HandyControl.Controls.PasswordBox)sender;
            if (passwordBox != null)
            {
                PasswordBoxHelper.SetPassword(passwordBox, passwordBox.Password);
            }
            return new OperationResult<bool>() { ResultType = ResultType.Success, Data = true };
        }
    }
}

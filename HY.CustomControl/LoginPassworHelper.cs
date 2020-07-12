
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace HY.CustomControl
{
    /// <summary>
    /// 接收HandyControl的PasswordBox的密码修改行为
    /// </summary>
    public class PasswordBoxBehavior : Behavior<PasswordBox>
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
            var passwordBox = (PasswordBox)sender;
            if (passwordBox != null)
            {
                PasswordBoxHelper.SetPassword(passwordBox, passwordBox.Password);
            }
            return new OperationResult<bool>() { ResultType = ResultType.Success, Data = true };
        }
    } /// <summary>
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





}

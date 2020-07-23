
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace HY.CustomControl
{
    ///// <summary>
    ///// 接收HandyControl的PasswordBox的密码修改行为
    ///// </summary>
    //public class PasswordBoxBehavior : Behavior<PasswordBox>
    //{
    //    protected override void OnAttached()
    //    {
    //        base.OnAttached();

    //        AssociatedObject.VerifyFunc += new Func<string, OperationResult<bool>>((password) =>
    //        {
    //            return this.PasswordChanged(AssociatedObject, AssociatedObject.Password);
    //        });
    //    }

    //    protected override void OnDetaching()
    //    {
    //        base.OnDetaching();
    //    }

    //    private OperationResult<bool> PasswordChanged(object sender, string password)
    //    {
    //        var passwordBox = (PasswordBox)sender;
    //        if (passwordBox != null)
    //        {
    //            PasswordBoxHelper.SetPassword(passwordBox, passwordBox.Password);
    //        }
    //        return new OperationResult<bool>() { ResultType = ResultType.Success, Data = true };
    //    }
    //} /// <summary>
    //  /// 用于密码框的密码绑定帮助类
    //  /// </summary>
    //public class PasswordBoxHelper
    //{
    //    public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string),
    //        typeof(PasswordBoxHelper), new PropertyMetadata("", OnPasswordPropertyChanged));
    //    public static string GetPassword(DependencyObject obj)
    //    {
    //        return (string)obj.GetValue(PasswordProperty);
    //    }

    //    public static void SetPassword(DependencyObject obj, string value)
    //    {
    //        obj.SetValue(PasswordProperty, value);
    //    }

    //    private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    //    {
    //        PasswordBox box = sender as PasswordBox;
    //        string password = (string)e.NewValue;
    //        if (box != null && box.Password != password)
    //        {
    //            box.Password = password;
    //        }
    //    }
    //}

    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password",  typeof(string), typeof(PasswordBoxHelper),  new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));
        public static readonly DependencyProperty PasswordLengthProperty =  DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxHelper), new UIPropertyMetadata(0));
        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            string password = (string)e.NewValue;
            if (passwordBox != null && passwordBox.Password != password)
            {
                passwordBox.Password = password;
            }
            //passwordBox.Tag = password;
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }
        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

      
    }

    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            PasswordBoxHelper.SetPasswordLength(passwordBox, passwordBox.Password.Length);
            //passwordBox.Tag = passwordBox.Password;
            string password = PasswordBoxHelper.GetPassword(passwordBox);

            if (passwordBox != null && passwordBox.Password != password)
            {
                PasswordBoxHelper.SetPassword(passwordBox, passwordBox.Password);
            }
          
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }
    }



}

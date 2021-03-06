﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HY.CustomControl.Converters
{
    /// <summary>
    /// 0/1 隐藏显示转换器 1隐藏 0 显示
    /// </summary>
    public class FlagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                switch (parameter.ToString())
                {
                    case "Login":
                        {
                            switch (value.ToString())
                            {
                                case "登录":
                                    return Visibility.Visible;
                                default:
                                    return Visibility.Collapsed;
                            }
                        }
                    case "Registere":
                        {
                            switch (value.ToString())
                            {
                                case "登录":
                                    return Visibility.Collapsed;
                                default:
                                    return Visibility.Visible;
                            }
                        }
                    case "Progress":
                        {
                            if (value!=null)
                            {
                                return Visibility.Visible;
                            }
                            else
                            {
                                return Visibility.Collapsed;
                            }   
                        }
                    case "MaineView":
                        {
                            if (value != null&&!string.IsNullOrEmpty(value.ToString()))
                            {
                                return Visibility.Collapsed;
                            }
                            else
                            {
                                return Visibility.Visible;
                            }
                        }
                    case "MaineView1":
                        {
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                return Visibility.Visible;
                            }
                            else
                            {
                                return Visibility.Collapsed;
                            }
                        }
                    default:
                        {
                            int result;
                            if (value != null && int.TryParse(value.ToString(), out result))
                            {
                                if (result.Equals(1))
                                    return Visibility.Collapsed;
                                if (result.Equals(2))
                                    return "Hidden";
                                return Visibility.Visible;
                            }
                            return Visibility.Collapsed;
                        }
                }
            }
            int resultPage;
            if (value != null && int.TryParse(value.ToString(), out resultPage))
            {
                if (resultPage.Equals(1))
                    return Visibility.Collapsed;
                if (resultPage.Equals(2))
                    return Visibility.Hidden;
                return Visibility.Visible;
            }
            return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

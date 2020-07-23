using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Hy.Setup.Converter
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
                    case "IndeVisible":
                        {
                            if (value.ToString().Equals("0"))
                            {
                                return Visibility.Visible;
                            }
                            else
                            {
                                return Visibility.Collapsed;
                            }
                        }
                    case "isVisible":
                        {
                            if (value.ToString().Equals("1"))
                            {
                                return Visibility.Visible;
                            }
                            else
                            {
                                return Visibility.Collapsed;
                            }
                        }
                    case "GridVisible":
                        {
                            if (value.ToString().Equals("2"))
                            {
                                return Visibility.Visible;
                            }
                            else
                            {
                                return Visibility.Collapsed;
                            }
                        }
                    case "SigeVisible":
                        {
                            if (value.ToString().Equals("3"))
                            {
                                return Visibility.Visible;
                            }
                            else
                            {
                                return Visibility.Collapsed;
                            }
                        }
                    case "CloseVisible":
                        {
                            if (value.ToString().Equals("2"))
                            {
                                return Visibility.Collapsed;
                            }
                            else
                            {
                                return Visibility.Visible;
                            }
                        }
                }
            }
            int resultPage;
            if (value != null && int.TryParse(value.ToString(), out resultPage))
            {
                if (resultPage.Equals(1))
                    return "Collapsed";
                if (resultPage.Equals(2))
                    return "Hidden";
                return "Visible";
            }
            return "Collapsed";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

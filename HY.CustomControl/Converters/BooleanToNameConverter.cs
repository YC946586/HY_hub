using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HY.CustomControl.Converters
{
    public class BooleanToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                switch (parameter.ToString())
                {
                    case "Purchased":
                        {
                            var dd = bool.Parse(value.ToString());
                            switch (dd)
                            {
                                case false:
                                    return "获取游戏";
                                default:
                                    return "开始游戏";
                            }
                        }
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace HY.CustomControl.Controls.Button
{
    public class IconButton : ButtonBase
    {
        public  readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(string), typeof(IconButton), new PropertyMetadata(""));

        public string IconText
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(double), typeof(IconButton), new PropertyMetadata(30));

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register(
            "DropDownContent", typeof(object), typeof(IconButton), new PropertyMetadata(default(object)));

        public object DropDownContent
        {
            get => GetValue(DropDownContentProperty);
            set => SetValue(DropDownContentProperty, value);
        }

       
      
    }
}

using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HY.CustomControl
{
     public class PreviewMouseWheel
    {
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        /// <summary>
        /// 鼠标移入listbox无法触发外面的滚动条事件
        /// </summary>
        private static RelayCommand<ListBox> _previewMouseWheelCommand;
        /// <summary>
        /// 鼠标移入listbox无法触发外面的滚动条事件
        /// </summary>
        public static RelayCommand<ListBox> PreviewMouseWheelCommand
        {
            get
            {
                if (_previewMouseWheelCommand == null)
                {
                    _previewMouseWheelCommand = new RelayCommand<ListBox>((s) =>
                    {
                        s.PreviewMouseWheel += S_PreviewMouseWheel;
                    });
                }
                return _previewMouseWheelCommand;
            }
            set
            {
                _previewMouseWheelCommand = value;
                if (StaticPropertyChanged != null)
                {
                    StaticPropertyChanged.Invoke(null, new PropertyChangedEventArgs("PreviewMouseWheelCommand"));
                }
            }
        }

        /// <summary>
        /// 鼠标移入事件冒泡给最外层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void S_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                // 内层ListBox拦截鼠标滚轮事件
                e.Handled = true;

                // 激发一个鼠标滚轮事件，冒泡给外层ListBox接收到
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    }
}

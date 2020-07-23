using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace HY.Client.Execute.Commons
{
    public  class Message
     {/// <summary>
      /// 错误    Msg.Error(ex.Message, false);
      /// </summary>
      /// <param name="msg"></param>
        public static void ErrorException(Exception ex, bool Host = true)
        {
            Show(Notify.Error, ex.Message, Host);
        }

        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string ex, bool Host = true)
        {
            Show(Notify.Info, ex, Host);
        }

        /// <summary>
        /// 真香警告
        /// </summary>
        /// <param name="msg"></param>
        public static void Warning(string ex, bool Host = true)
        {
            Show(Notify.Warning, ex, Host);
        }

        /// <summary>
        /// 真香询问  
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Question(string ex, bool Host = true)
        {
            return Show(Notify.Question, ex, Host);
        }

        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="notify">类型</param>
        /// <param name="msg">文本信息</param>
        /// <returns></returns>
        public static bool Show(Notify notify, string msg, bool Host = true)
        {
            MessageBoxImage Icon = MessageBoxImage.None;
           
            switch (notify)
            {
                case Notify.Error:
                    Icon = MessageBoxImage.Error;
                    break;
                case Notify.Warning:
                    Icon = MessageBoxImage.Warning;
                    break;
                case Notify.Info:
                    Icon = MessageBoxImage.Information;
                    break;
                case Notify.Question:
                    Icon = MessageBoxImage.Question;
                    break;
            }
            if (notify!= Notify.Question)
            {
                MessageBox.Show(msg, "温馨提示", MessageBoxButton.OK, Icon);
            }
            else
            {
                var dr = MessageBox.Show(msg, "温馨提示", MessageBoxButton.YesNo, Icon);
                if (dr == MessageBoxResult.Yes)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public enum Notify
    {
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error,
        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning,
        /// <summary>
        /// 提示信息
        /// </summary>
        [Description("提示信息")]
        Info,
        /// <summary>
        /// 询问信息
        /// </summary>
        [Description("询问信息")]
        Question,
    }
}

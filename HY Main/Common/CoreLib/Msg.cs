using HY.Application.Base;
using HY_Main.Common.Unity;
using HY_Main.ViewModel.Step;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.Common.CoreLib
{
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

    /// <summary>
    /// 消息类
    /// </summary>
    public class Msg
    {

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg"></param>
        public static async void Error(Exception ex, bool Host = true)
        {
            await Show(Notify.Error, ex.Message, Host);
        }

        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="msg"></param>
        public static async void Info(string ex, bool Host = true)
        {
            await Show(Notify.Info, ex, Host);
        }

        /// <summary>
        /// 真香警告
        /// </summary>
        /// <param name="msg"></param>
        public static async void Warning(string ex, bool Host = true)
        {
            await Show(Notify.Warning, ex, Host);
        }

        /// <summary>
        /// 真香询问
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static async Task<bool> Question(string ex, bool Host = true)
        {
            return await Show(Notify.Question, ex, Host);
        }

        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="notify">类型</param>
        /// <param name="msg">文本信息</param>
        /// <returns></returns>
        private static async Task<bool> Show(Notify notify, string msg, bool Host = true)
        {
            string Icon = string.Empty;
            string Color = string.Empty;
            bool Hide = true;
            switch (notify)
            {
                case Notify.Error:
                    Icon = "\ue66b";
                    Color = "#FF4500";
                    break;
                case Notify.Warning:
                    Icon = "\ue6b6";
                    Color = "#FF8247";
                    break;
                case Notify.Info:
                    Icon = "\ue65a";
                    Color = "#1C86EE";
                    break;
                case Notify.Question:
                    Icon = "\ue620";
                    Color = "#20B2AA";
                    Hide = false;
                    break;
            }
            var dialog = ServiceProvider.Instance.Get<IModelDialog>("MsgBoxViewDlg");
            dialog.BindViewModel(new MsgBoxViewModel() { Msg = msg, Icon = Icon, Color = Color, BtnHide = Hide });
            var TaskResult = await dialog.ShowDialog();
            if (TaskResult)
            {
                return true;
            }
            return false;

        }

       
    }
}

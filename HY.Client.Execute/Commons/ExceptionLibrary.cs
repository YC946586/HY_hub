using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons
{
    public class ExceptionLibrary
    {
        static ExceptionLibrary()
        {
            InitDictionarys(); //初始化异常字典
        }

        static List<ExceptionInfo> ExDictionarys = new List<ExceptionInfo>();

        /// <summary>
        /// 获取异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetErrorMsgByExpId(Exception ex)
        {
            var expMode = ExDictionarys.FirstOrDefault(t => t.ExpId.Equals(ex.HResult));
            if (expMode == null) return ex.Message;
            else
                return expMode.Msg;
        }

        private static void InitDictionarys()
        {
            //异常代码-用户异常提示-内部异常提示
            ExDictionarys.Add(new ExceptionInfo(-2146233087, "未能连接至远程服务器,请联系管理员!", "系统错误"));
        
        }

    }

    /// <summary>
    /// 异常字典
    /// </summary>
    public class ExceptionInfo
    {
        public ExceptionInfo(int expid, string msg, string msgexp)
        {
            _ExpId = expid;
            _Msg = msg;
            _MsgExp = msgexp;
        }

        private int _ExpId;
        private string _Msg;
        private string _MsgExp;

        public int ExpId { get { return _ExpId; } }

        public string Msg { get { return _Msg; } }

        public string MsgExp { get { return _MsgExp; } }

    }
}

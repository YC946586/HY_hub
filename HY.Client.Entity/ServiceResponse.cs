using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity
{
    /// <summary>
    /// 服务返回报文
    /// </summary>
    [Serializable]
    public class ServiceResponse
    {
        private string _code = "";
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回码
        /// </summary>
        public string code { get { return _code; } set { _code = value; } }

        /// <summary>
        /// 返回的列表结果
        /// </summary>
        public object result { get; set; }

        /// <summary>
        /// 结果总行数
        /// </summary>
        public int TotalCount { get; set; }
    }
}

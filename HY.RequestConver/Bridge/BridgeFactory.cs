using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.Bridge
{
    /// <summary>
    /// 数据桥接工厂
    /// </summary>
    public class BridgeFactory
    {
        static BridgeFactory()
        {
            BridgeManager = new FrameworkManager();
        }
        /// <summary>
        /// 桥接抽象层
        /// </summary>
        public static BridgeManager BridgeManager { get; private set; }

    }
}

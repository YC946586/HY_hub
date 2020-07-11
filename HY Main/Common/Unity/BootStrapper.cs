using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.Common.Unity
{
    /// <summary>
    /// Unity接口类
    /// </summary>
    class BootStrapper
    {
        /// <summary>
        /// 注册方法
        /// </summary>
        public static void Initialize()
        {
            ServiceProvider.RegisterServiceLocator(new UnityServiceLocator());
            ServiceProvider.Instance.Register();
        }
    }
}

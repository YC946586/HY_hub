using HY.RequestConver.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.Bridge
{
    public abstract class BridgeManager
    {
        /// <summary>
        /// 获取用户接口
        /// </summary>
        /// <returns></returns>
        public abstract IUser GetUserManager();

        /// <summary>
        /// 获取Home接口
        /// </summary>
        /// <returns></returns>
        public abstract IHome GetHomeManager();


        /// <summary>
        /// 获取Home接口
        /// </summary>
        /// <returns></returns>
        public abstract IStore GetStoreManager();

        /// <summary>
        /// 获取Home接口
        /// </summary>
        /// <returns></returns>
        public abstract ICommon GetCommonManager();

    }
}

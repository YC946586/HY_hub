﻿using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY.RequestConver.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver
{
    /// <summary>
    /// 请求管理器
    /// </summary>
    public class FrameworkManager : BridgeManager
    {
        /// <summary>
        /// 用户操作接口
        /// </summary>
        /// <returns></returns>
        public override IUser GetUserManager()
        {
            return new UserManager();
        }
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public override IHome GetHomeManager()
        {
            return new HomeManager();
        }
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public override IStore GetStoreManager()
        {
            return new StoreManager();
        }

        public override ICommon GetCommonManager()
        {
            return new CommonManager();
        }

    }

}

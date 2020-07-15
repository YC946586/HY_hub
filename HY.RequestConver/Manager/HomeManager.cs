using HY.Client.Entity;
using HY.RequestConver.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.Manager
{
    public class HomeManager : IHome
    {
        /// <summary>
        /// 获取推荐游戏
        /// </summary>
        /// <returns></returns>
        public Task<ServiceResponse> GetHomeGames()
        {
            try
            {
              
                var genrator = Task.Run(() => Network.ApiGet("home", "getHomeGames"));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

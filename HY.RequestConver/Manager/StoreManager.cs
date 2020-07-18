using HY.Client.Entity;
using HY.RequestConver.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.Manager
{
    public class StoreManager : IStore
    {
        /// <summary>
        /// 游戏分类
        /// </summary>
        /// <returns></returns>
        public Task<ServiceResponse> GetCates()
        {
            try
            {
                var genrator = Task.Run(() => Network.ApiGet("store", "getCates"));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        public Task<ServiceResponse> GetGames(int cateId, string filter, int pageIndex, int pageSize)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>
                {
                 
                    { "filter", filter},
                    { "pageIndex", pageIndex},
                    { "pageSize", pageSize},
                };
                if (cateId!=1212121211)
                {
                    dic.Add("cateId", cateId);
                }
                var genrator = Task.Run(() => Network.ApiGet("store", "getGames", dic));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

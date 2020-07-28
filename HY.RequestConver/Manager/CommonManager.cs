using HY.Client.Entity;
using HY.RequestConver.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.Manager
{
    public class CommonManager : ICommon
    {
        public Task<ServiceResponse> GetCommonDes()
        {
            try
            {
                var genrator = Task.Run(() => Network.ApiGet("common", "getCommonDes"));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<ServiceResponse> GetLoginFormBackGroundPics()
        {
            try
            {
                var genrator = Task.Run(() => Network.ApiGet("common", "getLoginFormBackGroundPics"));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }

       

        public Task<ServiceResponse> UseCoupon(string code)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>
                {
                    { "code", code },
                };
                var genrator = Task.Run(() => Network.ApiPost("coupon", "useCoupon", dic));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Task<ServiceResponse> GetVersion()
        {
            try
            {
                var genrator = Task.Run(() => Network.ApiGet("common", "getVersion"));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<ServiceResponse> GetTools()
        {
            try
            {
                var genrator = Task.Run(() => Network.ApiGet("common", "getTools"));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

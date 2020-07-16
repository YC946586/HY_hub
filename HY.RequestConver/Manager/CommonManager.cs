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
    }
}

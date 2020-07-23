using HY.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.InterFace
{
    public  interface ICommon
    {
        Task<ServiceResponse> GetCommonDes();

        Task<ServiceResponse> GetLoginFormBackGroundPics();

        Task<ServiceResponse> UseCoupon(string code);
    }
}

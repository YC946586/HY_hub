using HY.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.InterFace
{
     public interface IHome
    {
        Task<ServiceResponse> GetHomeGames();

        Task<ServiceResponse> GetCommonUseGames();

        Task<ServiceResponse> UpdateCommomUseGames(List<int> gameIds, string type);
    }
}

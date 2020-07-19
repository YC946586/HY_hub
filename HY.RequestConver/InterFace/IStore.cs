using HY.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.InterFace
{
    public interface IStore
    {
        Task<ServiceResponse> GetCates();

        Task<ServiceResponse> GetGames(int cateId,string filter,int pageIndex, int pageSize);

        Task<ServiceResponse> BuyGame(int gameId);

        Task<ServiceResponse> GetGameFiles(int gameId);
    }
}

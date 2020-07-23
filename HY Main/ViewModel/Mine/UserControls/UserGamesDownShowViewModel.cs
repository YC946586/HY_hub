using GalaSoft.MvvmLight.Command;
using HY.Client.Entity.UserEntitys;
using HY_Main.Common.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel.Mine.UserControls
{
    public class UserGamesDownShowViewModel: BaseOperation<UserGamesEntity>
    {

        public override void Edit<TModel>(TModel model)
        {
            var mod = model as UserGamesEntity;
            GameDwonloadViewModel.ResetTask(mod.gameId, mod.content);
            mod.content = "继续";
        }
    }
}

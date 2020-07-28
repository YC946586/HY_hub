using GalaSoft.MvvmLight.Command;
using HY.Client.Entity.UserEntitys;
using HY_Main.Common.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel.Mine.UserControls
{
    public class UserGamesDownShowViewModel: BaseOperation<UserGamesEntity>
    {
        //public static Assembly clientAssbly= Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "\\HY Main.exe");

        //private static Type typeofControl { get; set; } = clientAssbly.GetType("HY_Main.ViewModel.Mine.UserControls.GameDwonloadViewModel");
        public override void Edit<TModel>(TModel model)
        {
            var mod = model as UserGamesEntity;
            //object obj = Activator.CreateInstance(typeofControl);
            //var method = typeofControl.GetMethod("ResetTask");
            //object[] objPar = { mod.gameId, mod.content, mod };
            //method?.Invoke(obj, objPar);
            GameDwonloadViewModel model1 = new GameDwonloadViewModel();
            model1.ResetTask(mod.content, mod);
            mod.content = mod.content.Equals("继续") ? "暂停" : "继续";

        }
        public override void Del<TModel>(TModel model)
        {
            var mod = model as UserGamesEntity;
            GameDwonloadViewModel model1 = new GameDwonloadViewModel();
            model1.ResetTask("取消", mod);
        } 
    }
}

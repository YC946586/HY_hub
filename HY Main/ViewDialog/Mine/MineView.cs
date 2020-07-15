using HY.Application.Base;
using HY_Main.Common.CoreLib.Modules;
using HY_Main.Model.Mine;
using HY_Main.ViewModel.Mine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewDialog.Mine
{
    /// <summary>
    ///  
    /// </summary>
    [Module(ModuleType.Hander, "我的", "HY_Main.ViewDialog.Mine.MineView", "\xe6a9", 3)]
    public class MineView : BaseView<HY_Main.View.Mine.MaineView, MineViewModel, MineModel>, IModel
    {
    }
}

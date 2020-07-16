using HY.Application.Base;
using HY.Client.Entity.HomeEntitys;
using HY_Main.Common.CoreLib.Modules;
using HY_Main.ViewModel.ShopMall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewDialog.ShopMall
{
    /// <summary>
    ///  
    /// </summary>
    [Module(ModuleType.Hander, "商城", "HY_Main.ViewDialog.ShopMall.ShopMallView", "\xe607", 2)]
    public class ShopMallView : BaseView<View.ShopMall.ShopMallView, ShopMallViewModel, Recommendgame>, IModel
    {

    }
}

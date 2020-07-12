using HY.Application.Base;
using HY_Main.Common.CoreLib.Modules;
using HY_Main.Model.HomePage;
using HY_Main.ViewModel.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewDialog.HomePage
{
    /// <summary>
    ///  
    /// </summary>
    [Module(ModuleType.Hander,  "主页", "HY_Main.ViewDialog.HomePage.HomePageView", "\xe637",1)]
    public class HomePageView : BaseView<View.HomePage.HomePageView, HomePageViewModel, HomePageModel>, IModel
    {

    }
}

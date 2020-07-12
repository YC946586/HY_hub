using GalaSoft.MvvmLight;
using HY.Application.Base;
using HY_Main.Model.CoreLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HY_Main.Common.CoreLib.Modules
{
    /// <summary>
    /// 模块组
    /// </summary>
    public class ModuleManager : ViewModelBase
    {
        public ModuleManager()
        {

        }
        private ObservableCollection<HanderMenuModel> _ModulMenus = new ObservableCollection<HanderMenuModel>();

        /// <summary>
        /// 加载模块
        /// </summary>
        public ObservableCollection<HanderMenuModel> ModuleMenus
        {
            get { return _ModulMenus; }
        }
        /// <summary>
        /// 初始化模块组
        /// </summary>
        public async Task<object> LoadModulesAsync()
        {
            try
            {
                object currentPage = null;
                ModuleComponent loader = new ModuleComponent();
                var _IModule = await Task.Run(() => loader.GetModules());
                if (_IModule == null) return null;
                foreach (var m in _IModule.OrderBy(s=>s.Sort))
                {
                    HanderMenuModel handerMenu = new HanderMenuModel() { HeaderName = m.Name, HeaderIcon = m.ICON };
                    var ass = Assembly.GetExecutingAssembly();
                    if (ass.CreateInstance(m.ModuleNameSpace) is IModel dialog)
                    {
                        dialog.BindDefaultModel(m.Sort);
                        if (currentPage==null)
                        {
                            currentPage= dialog.GetView();
                        }
                        handerMenu.Body = dialog.GetView();
                    }
                    _ModulMenus.Add(handerMenu);
                }
                GC.Collect();
                return await Task.FromResult(currentPage);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal object IntiUi()
        {
            throw new NotImplementedException();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.Common.CoreLib.Modules
{
    public class ModuleComponent
    {
        protected Assembly _ModuleAssembly;

        /// <summary>
        /// 当前程序集
        /// </summary>
        public Assembly ModuleAssembly
        {
            get
            {
                if (_ModuleAssembly == null)
                {
                    _ModuleAssembly = Assembly.GetExecutingAssembly();
                }
                return _ModuleAssembly;
            }
        }

        
        /// <summary>
        /// 获取程序集自定义特性
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static ModuleAttribute GetModuleAttribute(Assembly asm)
        {
            ModuleAttribute temp = new ModuleAttribute(ModuleType.None, "", "", "", 0);
            if (asm == null) return temp;

            object[] list = asm.GetCustomAttributes(typeof(ModuleAttribute), false);
            if (list.Length > 0)
                return (ModuleAttribute)list[0];
            else
                return temp;
        }

        /// <summary>
        /// 获取类型自定义特性
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static ModuleAttribute GetModuleAttribute(TypeInfo asm)
        {
            ModuleAttribute temp = new ModuleAttribute(ModuleType.None, "", "","",0);
            if (asm == null) return temp;

            object[] list = asm.GetCustomAttributes(typeof(ModuleAttribute), false);
            if (list.Length > 0)
                return (ModuleAttribute)list[0];
            else
                return temp;
        }

        /// <summary>
        /// 获取程序集命名空间
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static string GetModuleNameSpace(Assembly asm)
        {
            return GetModuleAttribute(asm).ModuleNameSpace;
        }

        /// <summary>
        /// 获取当前程序集下模块
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<ModuleAttribute>> GetModules()
        {
            try
            {
                IList<ModuleAttribute> list = new List<ModuleAttribute>();
                await Task.Run(() =>
                {
                    var ModList = ModuleAssembly.DefinedTypes.Where(t => t.Name.Contains("View")).ToList();
                    ModList.ForEach(t =>
                    {
                        ModuleAttribute bute = GetModuleAttribute(t);
                        if (bute.ModuleType != ModuleType.None)
                            list.Add(bute);
                    });
                }); ;
                return list;
            }
            catch
            {
                return null;
            }
        }

       
 

        public void ResetAssembly()
        {
            _ModuleAssembly = null;
        }
    }
}

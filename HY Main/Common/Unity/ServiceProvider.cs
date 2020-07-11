using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.Common.Unity
{
    class ServiceProvider
    {
        public static IUnityLocator Instance { get; private set; }

        public static void RegisterServiceLocator(IUnityLocator s)
        {
            Instance = s;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AtmCommon
{
    public interface IAtmNavigation {
#if NETCOREAPP3_0_OR_GREATER
        static abstract string Path { get; }
        static abstract string LinkName { get; }
        static abstract bool IsNavigationEnabled { get;}
#endif
    }
}

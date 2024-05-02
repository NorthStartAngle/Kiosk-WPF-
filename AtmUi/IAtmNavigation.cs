using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AtmUi
{
    public interface IAtmNavigation {
        static abstract string Path { get; }
        static abstract string LinkName { get; }
        static abstract bool IsNavigationEnabled { get;}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace AtmCommon.ViewModels {
    public class InProgress : Base {

        private static readonly DependencyProperty button1_CommandProperty = DependencyProperty.Register("CashToPaperWallet_Command", typeof(ICommand), typeof(InProgress));


        public static DependencyProperty Button1_CommandProperty => button1_CommandProperty;


        public ICommand Button1_Command {
            get => (ICommand)GetValue(Button1_CommandProperty);
            set => SetValue(Button1_CommandProperty, value);
        }
    }
}

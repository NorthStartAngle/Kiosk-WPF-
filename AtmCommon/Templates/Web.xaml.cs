using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AtmCommon;

namespace AtmCommon.Templates {
    /// <summary>
    /// Interaction logic for Web.xaml
    /// </summary>
    public partial class Web : UserControl {
        public Web() {
            InitializeComponent();
        }

        public AtmCommon.ViewModels.Web ViewModel {
            get => (AtmCommon.ViewModels.Web)DataContext;
        }

        private void Base_Loaded(object sender, RoutedEventArgs e) {
            ViewModel.BackButton_Command = BackButton_Command;
        }

        public static RoutedCommand BackButton_Command = new RoutedCommand();

        private void BackButton_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            Navigator.GoBack(this);
        }

        private void BackButton_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }
    }
}

using AtmUi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using AtmCommon.ViewModels;

namespace AtmUi.Pages
{
    /// <summary>
    /// Interaction logic for MachineConfiguration.xaml
    /// </summary>
    public partial class MachineConfiguration : Page
    {
        public MachineConfiguration()
        {
            InitializeComponent();
        }

        public CustomBase ViewModel
        {
            get => (CustomBase)DataContext;
        }

        public MachineConfig ViewRes
        {
            get => (MachineConfig)this.FindResource("ViewRes");
        }

        public static string Path { get; } = $"Pages/MachineConfiguration.xaml";
        public static string LinkName { get; } = "MachineConfiguration";
        public static bool IsNavigationEnabled { get; } = true;

        delegate void RunPSDelegate(string script);

        private void CustomBase_Loaded(object sender, RoutedEventArgs e)
        {
            ViewRes.GetUsers();

            Task task1 = Task.Run(() =>
            {
                bool ret = ViewRes.RunPowerShellCommand();
                if (ret)
                {
                    MessageBox.Show("loading...");
                }
                else
                {
                    MessageBox.Show("loading error");
                }                
            });
        }
    }
}

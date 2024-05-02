using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AtmCommon.Controls;
using AtmLoader.ViewModels;
using AtmCommon;
using System.Diagnostics;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AtmLoader.Pages
{
    /// <summary>
    /// Interaction logic for LockDown.xaml
    /// </summary>
    /// 
     /*<viewmodels:LockDown>
            <viewmodels:LockDown.PageTitle>
                <sys:String>LockDown</sys:String>
            </viewmodels:LockDown.PageTitle>

            <viewmodels:LockDown.SystemButton_Visibility>
                <Visibility>Hidden</Visibility>
            </viewmodels:LockDown.SystemButton_Visibility>

            <viewmodels:LockDown.ShadeVisibility>
                <Visibility>Hidden</Visibility>
            </viewmodels:LockDown.ShadeVisibility>

            <viewmodels:LockDown.DialogText>
                <sys:String>Progressive stuff is progressing.Please wait...</sys:String>
            </viewmodels:LockDown.DialogText>
        </viewmodels:LockDown>*/

    public partial class LockDown : System.Windows.Controls.Page
    {
        private int loadingcounter = 4;
        private DataGrid dgSetting;
        
        public LockDown()
        {
            loadingcounter = 4;
            InitializeComponent();
            ViewModels.LockDown model = ViewModel;
            model.OnUpgradeItems += Model_OnUpgradeItems;
            // BusyBar.IsBusy = true;
            ProgressDialogVisible = true;
        }

        bool ProgressDialogVisible { 
            get => ViewModel.DialogVisibility == Visibility.Visible; 
            set {
                if (value) {
                    ViewModel.DialogVisibility = Visibility.Visible;
                }
                else {
                    ViewModel.DialogVisibility = Visibility.Hidden;
                }
            }
        }


        public ViewModels.LockDown ViewModel
        {
            get => (ViewModels.LockDown)DataContext;
        }

        public void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            DataGridColumnHeader _column = (sender as DataGridColumnHeader);
                 
            if ( _column != null)
            {
                ViewModel.selectAll();
            }
        }

        private void Upgrade_Click(object sender, RoutedEventArgs e)
        {
            if(dgSetting != null)
            {
                ViewModels.LockDown model = ViewModel;                
                model.UpgradingShow = Visibility.Visible;
                model.RunScriptsByUpgrade();
            }
            //string result =ViewModel.UpgradeSettingInfos();
            //MessageBox.Show(result);
        }

        private void RegisterKeyboardControl(object sender, RoutedEventArgs e)
        {
            if (sender is Control control)
            {
                TouchKeyboard.RegisterControl(control);
                control.Focus();
            }
        }

        private void Model_OnUpgradeItems(object source, ViewModels.MyEventArgs e)
        {
            if (e.GetInfo() == "Users")
            {

            }
            else if (e.GetInfo() == "Services")
            {

            }
            else if (e.GetInfo() == "Settings")
            {

            }
            else if (e.GetInfo() == "Installedapp")
            {

            }

            loadingcounter -= 1;

            ProgressDialogVisible = true;
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate 
            {
                ViewModel.ProgressValue = 100 - loadingcounter * 25;
                ViewModel.DialogText = $"Gathering info: {ViewModel.ProgressValue}%"; 
            }));

            if (loadingcounter == 0) {
                ProgressDialogVisible = false;
            }
        }

        public static string Path { get; } = $"Pages/LockDown.xaml";
        public static string LinkName { get; } = "System Lockdown";
        public static bool IsNavigationEnabled { get; } = true;

        private void collapsed_above(object sender, RoutedEventArgs e)
        {
            Expander me = (Expander)sender;
            Grid _grid = (Grid)me.Parent;

            _grid.RowDefinitions[0].Height = GridLength.Auto;

            IEnumerable<Expander> expanders = VisualTreeHelperExtensions.FindVisualChildren<Expander>(this);

            foreach (Expander item in expanders)
            {
                if(item != me)
                {
                    item.IsExpanded = true;

                }
            }
        }

        private void Expanded_above(object sender, RoutedEventArgs e)
        {
            Expander me = (Expander)sender;
            Grid _grid = (Grid)me.Parent;
            _grid.RowDefinitions[0].Height = new GridLength(1.0, GridUnitType.Star);

            IEnumerable<Expander> expanders = VisualTreeHelperExtensions.FindVisualChildren<Expander>(this);

            foreach (Expander item in expanders)
            {
                if (item != me)
                {
                    item.IsExpanded = false;

                }
            }
        }

        private void collapsed_below(object sender, RoutedEventArgs e)
        {
            Expander me = (Expander)sender;
            Grid _grid = (Grid)me.Parent;
            _grid.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Auto);

            IEnumerable<Expander> expanders = VisualTreeHelperExtensions.FindVisualChildren<Expander>(this);

            foreach (Expander item in expanders)
            {
                if (item != me)
                {
                    item.IsExpanded = true;

                }
            }
        }

        private void expanded_below(object sender, RoutedEventArgs e)
        {
            Expander me = (Expander)sender;
            Grid _grid = (Grid)me.Parent;
            _grid.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Star);
            IEnumerable<Expander> expanders = VisualTreeHelperExtensions.FindVisualChildren<Expander>(this);

            foreach (Expander item in expanders)
            {
                if (item != me)
                {
                    item.IsExpanded = false;

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_Initialized(object sender, EventArgs e)
        {
            dgSetting = (DataGrid)sender;
        }
    }

    public static class VisualTreeHelperExtensions
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T) yield return (T)ithChild;
                foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
            }
        }

        //List<Control> children = AllChildren(ItemsControl1);
        public static List<Control> AllChildren(DependencyObject parent)
        {
            var list = new List<Control> { };
            for (int count = 0; count < VisualTreeHelper.GetChildrenCount(parent); count++)
            {
                var child = VisualTreeHelper.GetChild(parent, count);
                if (child is Control)
                {
                    list.Add(child as Control);
                }
                list.AddRange(AllChildren(child));
            }
            return list;
        }

    }
}

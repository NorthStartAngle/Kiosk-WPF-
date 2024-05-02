using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;
using AtmUi.Pages;
using System.Windows.Media;
using System.Windows.Controls;

namespace AtmUi {
    public static class Navigator {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="queryParams"></param>
        /// <example>
        /// Navigator.Navigate(this, "DestinationPage.xaml", new Dictionary<string, string> {
        ///     { "param1", paramValue1 },
        ///     { "param2", paramValue2 }
        /// });

        /// </example>
        public static void Navigate(DependencyObject source, string path, IDictionary<string, string>? queryParams = null) {
            Uri uri = BuildUri(path, queryParams);
            NavigationService.GetNavigationService(source).Navigate(uri);
        }

        private static Uri BuildUri(string path, IDictionary<string, string>? queryParams) {
            if (queryParams is not null) {
                var pair = queryParams.FirstOrDefault();
                var key = Uri.EscapeDataString(pair.Key);
                var value = Uri.EscapeDataString(pair.Value);
                path += $"?{key}={value}";

                for (var i = 1; i < queryParams.Count; i++) {
                    pair = queryParams.ElementAt(i);
                    key = Uri.EscapeDataString(pair.Key);
                    value = Uri.EscapeDataString(pair.Value);
                    path += $"&{key}={value}";
                }
            }

            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            return uri;
        }

        public static void Navigate<T>(DependencyObject source, IDictionary<string, string>? queryParams = null) where T : IAtmNavigation {
            if (!T.IsNavigationEnabled) {
                return;
            }

            var uri = BuildUri<T>(queryParams);
            NavigationService.GetNavigationService(source).Navigate(uri);
        }

        public static Uri? BuildUri<T>(IDictionary<string, string>? queryParams = null) where T : IAtmNavigation {
            string path = T.Path;

            if (string.IsNullOrEmpty(path)) {
                return null;
            }

            if (queryParams is not null) {
                var pair = queryParams.FirstOrDefault();
                var key = Uri.EscapeDataString(pair.Key);
                var value = Uri.EscapeDataString(pair.Value);
                path += $"?{key}={value}";

                for (var i = 1; i < queryParams.Count; i++) {
                    pair = queryParams.ElementAt(i);
                    key = Uri.EscapeDataString(pair.Key);
                    value = Uri.EscapeDataString(pair.Value);
                    path += $"&{key}={value}";
                }
            }

            return new Uri(path, UriKind.RelativeOrAbsolute);
        }

        public static void GoBack(DependencyObject source) {
            NavigationService.GetNavigationService(source).GoBack();
        }

        public static bool? ShowDialog(Window dialogWindow, Page source) {
            /* This needs some work to position the dialog correctly when it's 
            not on the default screen. */
            var ownerWindow = NavigationWindow.GetWindow(source);
            // var psource = PresentationSource.FromVisual(source);
            dialogWindow.Owner = ownerWindow;
            dialogWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            var centerX = ownerWindow.Width / 2;
            var centerY = ownerWindow.Height / 2;

            var centerPoint = new Point(centerX, centerY);

            // Adjust the position of the dialog window
            dialogWindow.Left = centerPoint.X - (dialogWindow.Width / 2);
            dialogWindow.Top = centerPoint.Y - (dialogWindow.Height / 2);

            Point point = new Point(dialogWindow.Left, dialogWindow.Top);
            // point = ownerWindow.PointFromScreen(point);

            dialogWindow.Left = point.X;
            dialogWindow.Top = point.Y;

            ViewModels.TemplateViewModel? viewModel = null;

            viewModel = source.DataContext as ViewModels.TemplateViewModel;
            
            if (viewModel is not null) {
                viewModel.ShadeVisibility = Visibility.Visible;
            }

            bool? returnValue = dialogWindow.ShowDialog();

            if (viewModel is not null) {
                viewModel.ShadeVisibility = Visibility.Hidden;
            }

            return returnValue;
        }
    }
}

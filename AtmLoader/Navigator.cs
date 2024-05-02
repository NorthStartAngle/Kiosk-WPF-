using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows;

namespace AtmLoader {
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
        public static void Navigate(DependencyObject source, string path, IDictionary<string, string> queryParams = null) {
            Uri uri = BuildUri(path, queryParams);
            NavigationService.GetNavigationService(source).Navigate(uri);
        }

        private static Uri BuildUri(string path, IDictionary<string, string> queryParams) {
            if (queryParams != null) {
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

        public static void GoBack(DependencyObject source) {
            var navSvc = NavigationService.GetNavigationService(source);

            if (navSvc.CanGoBack) {
                navSvc.GoBack();
            }
        }
    }
}

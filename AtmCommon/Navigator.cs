using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace AtmCommon {
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
        public static void Navigate(DependencyObject source, string path) {
            Uri uri = BuildUri(path);
            NavigationService.GetNavigationService(source).Navigate(uri);
        }

        public static void Navigate(DependencyObject source, string path, IDictionary<string, string> queryParams) {
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

            return BuildUri(path);
        }

        private static Uri BuildUri(string path) {
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            return uri;
        }

        public static void GoBack(DependencyObject source) {
            NavigationService.GetNavigationService(source).GoBack();
        }

#if NETCOREAPP3_0_OR_GREATER
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
#endif
    }
}

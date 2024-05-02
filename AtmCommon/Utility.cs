using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Diagnostics;

namespace AtmCommon {
    public class Utility {
#if NETCOREAPP3_0_OR_GREATER
        public static T? FindVisualChild<T>(DependencyObject? parent) where T : DependencyObject {
#else
        public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject {
#endif
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                if (child is T typedChild) {
                    return typedChild;
                }
                else {
                    var result = FindVisualChild<T>(child);

                    if (result != null) {
                        return result;
                    }
                }
            }

            return null;
        }

#if NETCOREAPP3_0_OR_GREATER
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject? depObj) where T : DependencyObject {
#else
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject {
#endif
            if (depObj != null) {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T) {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child)) {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}

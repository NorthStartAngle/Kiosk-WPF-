using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace AtmCommon.Converters {
    public class BooleanToVisibilityCollapsedConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Visibility visibility) {
                switch (visibility) {
                    case Visibility.Visible:
                        return true;
                    case Visibility.Collapsed:
                        return false;
                    case Visibility.Hidden:
                        throw new InvalidOperationException("Cannot convert from Visibility.Hidden");
                }
            }

            return false;
        }
    }
}

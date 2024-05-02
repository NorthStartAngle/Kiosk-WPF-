using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace AtmCommon.Converters {
    public class BooleanToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Visibility visibility) {
                switch (visibility) {
                    case Visibility.Visible:
                        return true;
                    case Visibility.Hidden:
                        return false;
                    case Visibility.Collapsed:
                        throw new InvalidOperationException("Cannot convert from Visibility.Collapsed");
                }
            }

            return false;
        }
    }
}

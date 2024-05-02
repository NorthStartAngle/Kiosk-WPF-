using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Reflection;

namespace AtmCommon.Converters
{
    public class TypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return "null";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public class User
        {
            public User()
            {
            }
            public bool IsSelected { get; set; } = false;
            public string Name { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
            public string SuggestedValue { get; set; } = string.Empty;
        }
    }
}

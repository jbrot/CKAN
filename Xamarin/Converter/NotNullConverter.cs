using System;
using System.Globalization;
using Xamarin.Forms;

namespace CKAN.Xamarin.Converter
{
    public class NotNullConverter  : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new UnsupportedKraken("We do not support converting back.");
        }
    }
}

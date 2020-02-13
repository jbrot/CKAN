using System;
using System.Globalization;
using Xamarin.Forms;

namespace CKAN.Xamarin.Converter
{
    public class PELISelectedToColorConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool selected) {
                return selected ? Color.Blue : new Color(0.95, 0.95, 0.95);
            }
            throw new ArgumentException($"Tried to convert something other than a bool!");
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new UnsupportedKraken("We do not support converting back to bool.");
        }
    }
}

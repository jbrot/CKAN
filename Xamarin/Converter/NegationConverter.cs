using System;
using System.Globalization;
using Xamarin.Forms;

namespace CKAN.Xamarin.Converter
{
    public class NegationConverter  : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool selected) {
                return !selected;
            }
            throw new ArgumentException($"Tried to convert something other than a bool!");
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new UnsupportedKraken("We do not support converting back to bool.");
        }
    }
}

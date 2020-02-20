using System;
using System.Globalization;
using CKAN.Xamarin.View;
using Xamarin.Forms;

namespace CKAN.Xamarin.Converter
{
    public class StatusIndicatorConverter  : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool image = GetBool(parameter);

            if (value is Status status) {
                if (status == Status.Processing) {
                    return !image;
                } else {
                    return image;
                }
            }

            throw new ArgumentException("Invalid input type!");
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new UnsupportedKraken("We do not support converting back.");
        }

        private bool GetBool(object o)
        {
            if (o is bool b) {
                return b;
            }

            if (o is string s) {
                bool bb;
                bool.TryParse(s, out bb);
                return bb;
            }

            return false;
        }
    }
}

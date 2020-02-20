using System;
using System.Globalization;
using CKAN.Xamarin.View;
using Xamarin.Forms;

namespace CKAN.Xamarin.Converter
{
    public class StatusIndicatorMarginConverter  : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Status status) {
                if (status == Status.Valid) {
                    return 3;
                } else {
                    return 4;
                }
            }

            throw new ArgumentException("We only accept StatusIndicator.Status!");
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new UnsupportedKraken("We do not support converting back.");
        }
    }
}

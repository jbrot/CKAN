using System;
using System.Collections.Generic;
using System.Globalization;
using Autofac;
using CKAN.Xamarin.Model;
using CKAN.Xamarin.View;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;

namespace CKAN.Xamarin.Converter
{
    public class ModActionToText : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ModAction ma) {
                switch(value) {
                case ModAction.Install:
                    return "Install";
                case ModAction.Update:
                    return "Update";
                case ModAction.Remove:
                    return "Remove";
                case ModAction.Unknown:
                    return "N/A";
                }
            }

            throw new ArgumentException($"Tried to convert something other than a ModAction!");
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new UnsupportedKraken("We do not support converting back to ModAction.");
        }
    }
}

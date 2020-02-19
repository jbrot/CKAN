using System;
using System.Collections.Generic;
using System.Globalization;
using Autofac;
using CKAN.Xamarin.View;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;

namespace CKAN.Xamarin.Converter
{
    /// <summary>
    /// This class is used to link ViewModels to Views.
    /// </summary>
    public class ViewModelToViewConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) {
                return null;
            }

            if (value is BaseViewModel bvm) {
                return bvm.Scope.Resolve(typeof(IMvvmView<>).MakeGenericType(bvm.GetType()));
            }

            throw new ArgumentException($"Tried to convert type {value.GetType().ToString()}, which is not a subclass of {typeof(BaseViewModel).ToString()}!");
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new UnsupportedKraken("We do not support converting Views to ViewModels");
        }
    }
}

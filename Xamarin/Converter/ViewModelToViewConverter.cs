using System;
using System.Collections.Generic;
using System.Globalization;
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
        private IDictionary<Type, XView> map = new Dictionary<Type, XView>();

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type key = value?.GetType();
            if (!key.IsSubclassOf(typeof(BaseViewModel))) {
                throw new ArgumentException($"Tried to convert type {key.ToString()}, which is not a subclass of {typeof(BaseViewModel).ToString()}!");
            }

            // If we've already instantiated the corresponding view, return it.
            XView view;
            map.TryGetValue(key, out view);
            if (view != null)
                return view;

            // Otherwise, we need to create it.
            switch(value) {
            case BrowseViewModel _:
                view = new BrowseView();
                break;
            case InstalledViewModel _:
                view = new InstalledView();
                break;
            case SearchViewModel _:
                view = new SearchView();
                break;
            case SettingsViewModel _:
                view = new SettingsView();
                break;
            case UpdatesViewModel _:
                view = new UpdatesView();
                break;
            default:
                throw new ArgumentException($"Unsupported view model type {key.ToString()}!");
            }

            view.BindingContext = value;
            map.Add(key, view);
            return view;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new UnsupportedKraken("We do not support converting Views to ViewModels");
        }
    }
}

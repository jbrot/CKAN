using System;
using System.Collections.Generic;
using System.Globalization;
using CKAN.Xamarin.View;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.Converter
{
    /// <summary>
    /// This class is used to link ViewModels to Views.
    /// </summary>
    public class ViewModelToViewConverter : IValueConverter
    {
        private IDictionary<Type, Page> map = new Dictionary<Type, Page>(0);

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type key = value?.GetType();
            if (!key.IsSubclassOf(typeof(BaseViewModel))) {
                throw new ArgumentException($"Tried to convert type {key.ToString()}, which is not a subclass of {typeof(BaseViewModel).ToString()}!");
            }

            // If we've already instantiated the corresponding view, return it.
            Page page;
            map.TryGetValue(key, out page);
            if (page != null)
                return page;

            // Otherwise, we need to create it.
            switch(value) {
            case BrowsePageViewModel _:
                page = new BrowsePage();
                break;
            case InstalledPageViewModel _:
                page = new InstalledPage();
                break;
            case SearchPageViewModel _:
                page = new SearchPage();
                break;
            case SettingsPageViewModel _:
                page = new SettingsPage();
                break;
            case UpdatesPageViewModel _:
                page = new UpdatesPage();
                break;
            default:
                throw new ArgumentException($"Unsupported view model type {key.ToString()}!");
            }

            page.BindingContext = value;
            map.Add(key, page);
            return page;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new UnsupportedKraken("We do not support converting Views to ViewModels");
        }
    }
}

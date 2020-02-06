using System;
using System.Collections.Generic;
using System.Windows.Input;
using CKAN.Xamarin.Model;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private IList<NavigationItem> items;
        public IList<NavigationItem> Items {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        private BaseViewModel detailViewModel;
        /// <summary>
        /// DetailViewModel contains the ViewModel for the currently
        /// displayed View in the detail pane.
        ///
        /// This property is used to navigate the detail pane. That is, setting
        /// this value will cause the displayed view to be updated. This is
        /// accomplished using the ViewModelToViewConverter.
        /// </summary>
        public BaseViewModel DetailViewModel {
            get { return detailViewModel; }
            set { SetProperty(ref detailViewModel, value); }
        }

        public ICommand NavigationSelectedCommand { get; private set; }

        private IDictionary<Type, BaseViewModel> instantiatedViewModels = new Dictionary<Type, BaseViewModel>();

        public MainPageViewModel ()
        {
            // TODO: Put in proper icons
            Items = new List<NavigationItem> {
                new NavigationItem {
                    Label = "Search",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(SearchPageViewModel)
                },
                new NavigationItem {
                    Label = "Browse",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(BrowsePageViewModel)
                },
                new NavigationItem {
                    Label = "Installed",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(InstalledPageViewModel)
                },
                new NavigationItem {
                    Label = "Updates",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(UpdatesPageViewModel)
                },
                new NavigationItem {
                    Label = "Settings",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(SettingsPageViewModel)
                },
                new NavigationItem {
                    Label = "Run",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = null
                }
            };

            DetailViewModel = new SettingsPageViewModel();

            NavigationSelectedCommand = new Command<NavigationItem>(OnNavigationSelected);
        }

        private void OnNavigationSelected (NavigationItem item)
        {
            Type key = item?.ContentType;
            if (key == null) {
                return;
            }

            BaseViewModel vm;
            instantiatedViewModels.TryGetValue(key, out vm);
            if (vm == null) {
                vm = (BaseViewModel) Activator.CreateInstance(key);
                instantiatedViewModels.Add(key, vm);
            }

            DetailViewModel = vm;
        }
    }
}

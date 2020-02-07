using System;
using System.Collections.Generic;
using System.Windows.Input;
using Autofac;
using CKAN.Xamarin.Converter;
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

        public MainPageViewModel (ILifetimeScope scope)
            : base(scope)
        {
            // TODO: Put in proper icons
            Items = new List<NavigationItem> {
                new NavigationItem {
                    Label = "Search",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(SearchViewModel)
                },
                new NavigationItem {
                    Label = "Browse",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(BrowseViewModel)
                },
                new NavigationItem {
                    Label = "Installed",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(InstalledViewModel)
                },
                new NavigationItem {
                    Label = "Updates",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(UpdatesViewModel)
                },
                new NavigationItem {
                    Label = "Settings",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(SettingsViewModel)
                },
                new NavigationItem {
                    Label = "Run",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = null
                }
            };

            DetailViewModel = Scope.Resolve<SettingsViewModel>();

            NavigationSelectedCommand = new Command<NavigationItem>(OnNavigationSelected);
        }

        private void OnNavigationSelected (NavigationItem item)
        {
            Type type = item?.ContentType;
            if (type == null) {
                return;
            }

            DetailViewModel = (BaseViewModel) Scope.Resolve(type);
        }
    }
}

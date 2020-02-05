using System;
using System.Collections.Generic;
using CKAN.Xamarin.Model;
using CKAN.Xamarin.View;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    public class MasterPageModel : BaseViewModel
    {
        private IList<MasterPageItem> items;
        public IList<MasterPageItem> Items {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public MasterPageModel ()
        {
            // TODO: Put in proper icons
            // TODO: Re: discussion in MasterPageItem: Referring to the *Page types
            // here violates MVVM. This suggests that this list should be in the View
            // not the ViewModel, which also feels wrong. This seems to suggest to
            // me that the Enum approach is definitely the way to go.
            Items = new List<MasterPageItem> {
                new MasterPageItem {
                    Label = "Search",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(SearchPage)
                },
                new MasterPageItem {
                    Label = "Browse",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(BrowsePage)
                },
                new MasterPageItem {
                    Label = "Installed",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(InstalledPage)
                },
                new MasterPageItem {
                    Label = "Updates",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(UpdatesPage)
                },
                new MasterPageItem {
                    Label = "Settings",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = typeof(SettingsPage)
                },
                new MasterPageItem {
                    Label = "Run",
                    Icon = ImageSource.FromFile("Logo"),
                    ContentType = null
                }
            };
        }
    }
}

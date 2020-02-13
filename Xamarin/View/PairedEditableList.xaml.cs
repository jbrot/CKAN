using System;
using System.Collections.Generic;
using CKAN.Xamarin.Model;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public partial class PairedEditableList : ContentView
    {
        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList<PEListItemViewModel>), typeof(PairedEditableList));

        public IList<PEListItemViewModel> ItemsSource {
            get { return (IList<PEListItemViewModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public PairedEditableList ()
        {
            InitializeComponent();
        }

        private void OnItemSelected (object sender, SelectedItemChangedEventArgs args)
        {
            ListView lv = (ListView)sender;
            if (lv.SelectedItem == null) {
                return;
            }

            for (int i = 0; i < ItemsSource.Count; ++i) {
                ItemsSource[i].Selected = i == args.SelectedItemIndex;
            }
            lv.SelectedItem = null;
        }
    }
}

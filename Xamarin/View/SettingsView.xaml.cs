using System;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public partial class SettingsView : ContentView, IMvvmView<SettingsViewModel>
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsList SList { get => slist; }

        public SettingsView (SettingsViewModel vm)
        {
            ViewModel = vm;
            InitializeComponent();
            BindingContext = vm;
        }
    }
}

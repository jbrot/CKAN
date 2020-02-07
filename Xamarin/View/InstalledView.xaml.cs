using System;
using System.Collections.Generic;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public partial class InstalledView : ContentView, IMvvmView<InstalledViewModel>
    {
        public InstalledViewModel ViewModel { get; }

        public InstalledView (InstalledViewModel vm)
        {
            ViewModel = vm;
            InitializeComponent();
            BindingContext = vm;
        }
    }
}

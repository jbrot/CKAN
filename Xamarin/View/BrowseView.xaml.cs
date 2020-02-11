using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public partial class BrowseView : ContentView, IMvvmView<BrowseViewModel>
    {
        public BrowseViewModel ViewModel { get; }

        public BrowseView (BrowseViewModel vm)
        {
            ViewModel = vm;
            InitializeComponent();
            BindingContext = ViewModel;
        }
    }
}

using System;
using System.Collections.Generic;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public partial class SearchView : ContentView, IMvvmView<SearchViewModel>
    {
        public SearchViewModel ViewModel { get; }

        public SearchView (SearchViewModel vm)
        {
            ViewModel = vm;
            InitializeComponent();

            BindingContext = vm;
        }
    }
}

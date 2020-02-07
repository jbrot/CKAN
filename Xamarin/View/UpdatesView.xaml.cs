using System;
using System.Collections.Generic;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public partial class UpdatesView : ContentView, IMvvmView<UpdatesViewModel>
    {
        public UpdatesViewModel ViewModel { get; }

        public UpdatesView (UpdatesViewModel vm)
        {
            ViewModel = vm;
            InitializeComponent();
            BindingContext = vm;
        }
    }
}

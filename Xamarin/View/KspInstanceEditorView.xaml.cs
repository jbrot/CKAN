using System;
using System.Collections.Generic;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public partial class KspInstanceEditorView : ContentView, IMvvmView<KspInstanceEditorViewModel>
    {
        public KspInstanceEditorViewModel ViewModel { get; }

        public KspInstanceEditorView (KspInstanceEditorViewModel vm)
        {
            ViewModel = vm;
            InitializeComponent();
            BindingContext = vm;
        }
    }
}

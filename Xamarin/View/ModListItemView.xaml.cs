using System;
using System.Collections.Generic;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    /// <summary>
    /// This view is used to display a CkanModule in a ListView. Note that
    /// this view does not implement IMvvmView, as it is linked to
    /// ModListItemView by the ListView data template, and not through Autofac.
    ///
    /// <br />
    ///
    /// Furthermore, there will typically be several thousand of these
    /// constructed, meaning we would need to create several thousand lifetime
    /// scopes to use this class with Autofac. This is too resource intensive
    /// to be viable.
    /// </summary>
    public partial class ModListItemView : ContentView
    {
        public ModListItemView ()
        {
            InitializeComponent();
        }
    }
}

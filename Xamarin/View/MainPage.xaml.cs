using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public partial class MainPage : MasterDetailPage, IMvvmView<MainPageViewModel>
    {
        public MainPageViewModel ViewModel { get; }

        public MainPage (MainPageViewModel vm)
        {
            ViewModel = vm;
            InitializeComponent();
            BindingContext = vm;
        }
    }
}

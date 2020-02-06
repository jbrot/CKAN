using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public partial class MainPage : MasterDetailPage
    {
        public static readonly BindableProperty BindableDetailProperty = BindableProperty.Create(nameof(BindableDetail), typeof(Page), typeof(MainPage), null, propertyChanged: OnDetailChanged);

        public Page BindableDetail {
            get { return (Page)GetValue(BindableDetailProperty); }
            set { SetValue(BindableDetailProperty, value); }
        }

        static void OnDetailChanged (BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MainPage mdv && newValue is Page page) {
                mdv.Detail = page;
            }
        }

        public MainPage ()
        {
            InitializeComponent();
        }
    }
}

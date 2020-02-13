using System;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;

namespace CKAN.Xamarin.View
{
    /// <summary>
    /// SettingsList is a thin wrapper around SelectableList which provides
    /// a header, footer, and some extra graphical pizazz. It's used by
    /// SettingsView to display various lists.
    /// </summary>
    public partial class SettingsList : ContentView
    {
        public IList ItemsSource {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(SettingsList));

        public DataTemplate ItemTemplate {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(SettingsList));

        public ICommand ItemSelectedCommand {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }
        public static BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(SettingsList));

        public XView Header {
            get { return (XView)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(XView), typeof(SettingsList));


        public XView Footer {
            get { return (XView)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }
        public static BindableProperty FooterProperty = BindableProperty.Create(nameof(Footer), typeof(XView), typeof(SettingsList));


        public SettingsList ()
        {
            InitializeComponent();
        }
    }
}

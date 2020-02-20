using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public enum Status
    {
        Valid,
        Invalid,
        Processing
    }

    public partial class StatusIndicator : ContentView
    {
        public Status CurrentStatus {
            get { return (Status) GetValue(CurrentStatusProperty); }
            set { SetValue(CurrentStatusProperty, value); }
        }
        public static BindableProperty CurrentStatusProperty = BindableProperty.Create(nameof(CurrentStatus), typeof(Status), typeof(StatusIndicator));

        public string Message {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string), typeof(StatusIndicator));


        public StatusIndicator ()
        {
            InitializeComponent();
        }
    }
}

using System;
using Autofac;

namespace CKAN.Xamarin.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel (ILifetimeScope scope)
            : base(scope)
        {
        }
    }
}

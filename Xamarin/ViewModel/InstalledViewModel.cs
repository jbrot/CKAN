using System;
using Autofac;

namespace CKAN.Xamarin.ViewModel
{
    public class InstalledViewModel : BaseViewModel
    {
        public InstalledViewModel (ILifetimeScope scope)
            : base(scope)
        {
        }
    }
}

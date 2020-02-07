using System;
using Autofac;

namespace CKAN.Xamarin.ViewModel
{
    public class BrowseViewModel : BaseViewModel
    {
        public BrowseViewModel (ILifetimeScope scope)
            : base(scope)
        {
        }
    }
}

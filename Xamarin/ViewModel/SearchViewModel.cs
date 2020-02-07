using System;
using Autofac;

namespace CKAN.Xamarin.ViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel (ILifetimeScope scope)
            : base(scope)
        {
        }
    }
}

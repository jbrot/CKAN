using System;
namespace CKAN.Xamarin.Service
{
    public class RegistryService
    {
        private RegistryManager registry;
        public RegistryManager Registry {
            get { return registry; }
            set {
                registry = value;
            }
        }

        public RegistryService ()
        {
        }
    }
}

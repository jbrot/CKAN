using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Autofac;
using CKAN.Xamarin.Service;

namespace CKAN.Xamarin.ViewModel
{
    public class BrowseViewModel : BaseViewModel
    {
        private CkanService CkanService;

        public ObservableCollection<string> ModList { get; }

        public BrowseViewModel (ILifetimeScope scope, CkanService ckan)
            : base(scope)
        {
            CkanService = ckan;
            CkanService.PropertyChanged += OnServicePropertyChanged;
            ModList = new ObservableCollection<string>() {
                "Test 1",
                "Test 2",
                "Test 3",
                "Test 4",
                "Test 5"
            };

            if (CkanService.Registry != null) {
                UpdateModList();
            }
        }

        ~BrowseViewModel()
        {
            CkanService.PropertyChanged -= OnServicePropertyChanged;
        }

        private void OnServicePropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CkanService.Registry)) {
                UpdateModList();
            }
        }

        private void UpdateModList()
        {
            var ksp = CkanService.KSPManager.CurrentInstance;
            var registry = CkanService.Registry.registry;

            var allMods = new List<CkanModule>(registry.CompatibleModules(ksp.VersionCriteria()));
            foreach (InstalledModule im in registry.InstalledModules) {
                CkanModule m = null;
                try {
                    m = registry.LatestAvailable(im.identifier, ksp.VersionCriteria());
                } catch (ModuleNotFoundKraken) { }
                if (m == null) {
                    // Add unavailable installed mods to the list
                    allMods.Add(im.Module);
                }
            }

            ModList.Clear();
            foreach (CkanModule module in allMods) {
                ModList.Add(module.name);
            }
        }
    }
}

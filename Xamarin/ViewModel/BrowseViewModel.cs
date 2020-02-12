using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Autofac;
using CKAN.Xamarin.Model;
using CKAN.Xamarin.Service;

namespace CKAN.Xamarin.ViewModel
{
    public class BrowseViewModel : BaseViewModel
    {
        private CkanService CkanService;

        public ObservableCollection<ModListItemViewModel> ModList { get; }

        public BrowseViewModel (ILifetimeScope scope, CkanService ckan)
            : base(scope)
        {
            CkanService = ckan;
            CkanService.PropertyChanged += OnServicePropertyChanged;
            ModList = new ObservableCollection<ModListItemViewModel>();

            if (CkanService.Registry != null) {
                UpdateModList();
            } else {
                // TODO: Replace this testing code with a loading screen.
                ModList.Add(new ModListItemViewModel(new CkanModule {
                    name = "Test mod",
                    @abstract = "Test description",
                    version = new Versioning.ModuleVersion("1.2.3")
                }, ModAction.Install)); ;
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
            
            ModList.Clear();

            foreach (CkanModule module in registry.CompatibleModules(ksp.VersionCriteria())) {
                var act = ModAction.Install;
                if (registry.IsInstalled(module.identifier)) {
                    if (registry.HasUpdate(module.identifier, ksp.VersionCriteria())) {
                        act = ModAction.Update;
                    } else {
                        act = ModAction.Remove;
                    }
                }
                ModList.Add(new ModListItemViewModel(module, act));
            }

            foreach (InstalledModule im in registry.InstalledModules) {
                CkanModule m = null;
                try {
                    m = registry.LatestAvailable(im.identifier, ksp.VersionCriteria());
                } catch (ModuleNotFoundKraken) { }
                if (m == null) {
                    // Add unavailable installed mods to the list
                    ModList.Add(new ModListItemViewModel(im.Module, ModAction.Unknown));
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Autofac;
using CKAN.Xamarin.Model;
using CKAN.Xamarin.Service;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private KSPManager oldManager = null;
        private CkanService CkanService;

        public IList<KspListItemViewModel> KspInstances { get; } = new ObservableCollection<KspListItemViewModel>();

        public IList<PEListItemViewModel> TempItems { get; }

        public SettingsViewModel (ILifetimeScope scope, CkanService ckanService)
            : base(scope)
        {
            CkanService = ckanService;
            CkanService.PropertyChanged += OnServicePropertyChanged;

            if (CkanService.KSPManager != null) {
                oldManager = CkanService.KSPManager;
                CkanService.KSPManager.PropertyChanged += OnManagerPropertyChanged;
                UpdateKspInstances();
            }

            TempItems = new List<PEListItemViewModel>() {
                new PEListItemViewModel {
                    ColumnA="Key1",
                    ColumnB="Value1"
                },
                new PEListItemViewModel {
                    ColumnA="Key2",
                    ColumnB="Value2"
                }
            };
        }

        private void UpdateKspInstances ()
        {
            KspInstances.Clear();
            var manager = CkanService.KSPManager;
            foreach (KeyValuePair<string,KSP> entry in manager.Instances) {
                KspInstances.Add(new KspListItemViewModel {
                    Ksp = entry.Value,
                    Active = manager.CurrentInstance == entry.Value,
                    AutoStart = manager.AutoStartInstance == entry.Key,
                });
            }
        }

        private void UpdateActive()
        {
            foreach (KspListItemViewModel vm in KspInstances) {
                vm.Active = CkanService.KSPManager.CurrentInstance == vm.Ksp;
            }
        }

        private void OnServicePropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CkanService.KSPManager)) {
                UpdateKspInstances();
                if (oldManager != null) {
                    oldManager.PropertyChanged -= OnManagerPropertyChanged;
                }
                oldManager = CkanService.KSPManager;
                CkanService.KSPManager.PropertyChanged += OnManagerPropertyChanged;
            }
        }

        private void OnManagerPropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(KSPManager.CurrentInstance)) {
                Device.BeginInvokeOnMainThread(() => UpdateActive());
            }
        }
    }
}
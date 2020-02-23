using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using CKAN.Xamarin.Model;
using CKAN.Xamarin.Service;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private CkanService CkanService;
        private IDialogService DialogService;

        private KSPManager KSPManager {
            get => (KSPManager)GetValue(KSPManagerProperty);
            set => SetValue(KSPManagerProperty, value);
        }
        private static BindableProperty KSPManagerProperty = BindableProperty.Create(nameof(KSPManager), typeof(KSPManager), typeof(SettingsViewModel),
            propertyChanged: (obj, oldValue, newValue) => {
                SettingsViewModel svm = (SettingsViewModel) obj;
                if (oldValue is KSPManager old) {
                    old.PropertyChanged -= svm.OnManagerPropertyChanged;
                }
                if (newValue is KSPManager newm) {
                    newm.PropertyChanged += svm.OnManagerPropertyChanged;
                }
                svm.UpdateKspInstances();
            });

        public IList<KspListItemViewModel> KspInstances { get; } = new ObservableCollection<KspListItemViewModel>();
        public KspListItemViewModel SelectedKspInstance { get; set; }
        public ICommand NewKspInstance { get; private set; }
        public ICommand DeleteKspInstance { get; private set; }
        public ICommand EditKspInstance { get; private set; }
        public ICommand ActivateKspInstance { get; private set; }
        public ICommand MakeDefaultKspInstance { get; private set; }

        public IList<PEListItemViewModel> TempItems { get; }

        public SettingsViewModel (ILifetimeScope scope, CkanService ckanService, IDialogService dialogService)
            : base(scope)
        {
            CkanService = ckanService;
            DialogService = dialogService;

            SetBinding(KSPManagerProperty, new Binding(nameof(CkanService.KSPManager), source: CkanService));

            NewKspInstance = new Command(async() => await OnNewKspInstance());
            DeleteKspInstance = new Command(async () => await OnDeleteKspInstance());
            EditKspInstance = new Command(async () => await OnEditKspInstance());
            ActivateKspInstance = new Command(OnActivateKspInstance);
            MakeDefaultKspInstance = new Command(OnMakeDefaultKspInstance);

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
            foreach (KeyValuePair<string,KSP> entry in KSPManager.Instances) {
                KspInstances.Add(new KspListItemViewModel {
                    Ksp = entry.Value,
                    Active = KSPManager.CurrentInstance == entry.Value,
                    AutoStart = KSPManager.AutoStartInstance == entry.Key,
                });
            }
        }

        private void UpdateActive()
        {
            foreach (KspListItemViewModel vm in KspInstances) {
                vm.Active = KSPManager.CurrentInstance == vm.Ksp;
                vm.AutoStart = KSPManager.AutoStartInstance == vm.Ksp.Name;
            }
        }

        private async Task OnNewKspInstance() {
            ILifetimeScope scope = Scope.BeginLifetimeScope();
            KspInstanceEditorViewModel vm = scope.Resolve<KspInstanceEditorViewModel>();
            if (await RunModal(vm) is KspInstanceEditorViewModel.KspInstanceSpec spec) {
                KSPManager.AddInstance(new KSP(spec.Path, spec.Name, new NullUser()));
                UpdateKspInstances();
            }
        }

        private async Task OnDeleteKspInstance() {
            if (KSPManager.Instances.Count <= 1) {
                await DialogService.DisplayAlert("Can't delete instance!", "You cannot delete the last KSP instance.", "OK");
                return;
            }
            if (await DialogService.DisplayAlert("Are you sure?",
                    "Do you really want to remove this instance? This will not remove files, so you can always add it back later.",
                    "Delete", "Cancel")) {
                KSPManager.RemoveInstance(SelectedKspInstance.Ksp.Name);
                if (KSPManager.CurrentInstance == SelectedKspInstance.Ksp) {
                    KSPManager.SetCurrentInstance(KSPManager.Instances.Keys [0]);
                }
                UpdateKspInstances();
            }
        }

        private async Task OnEditKspInstance()
        {
            ILifetimeScope scope = Scope.BeginLifetimeScope(builder => {
                builder.RegisterInstance(SelectedKspInstance.Ksp).As(typeof(KSP)).ExternallyOwned();
            });
            KspInstanceEditorViewModel vm = scope.Resolve<KspInstanceEditorViewModel>();
            if (await RunModal(vm) is KspInstanceEditorViewModel.KspInstanceSpec spec) {
                string oldName = SelectedKspInstance.Ksp.Name;
                if (spec.Path != SelectedKspInstance.Path) {
                    KSPManager.RemoveInstance(oldName);
                    KSPManager.AddInstance(new KSP(spec.Path, spec.Name, new NullUser()));
                    UpdateKspInstances();
                } else if (spec.Name != SelectedKspInstance.Ksp.Name) {
                    KSPManager.RenameInstance(oldName, spec.Name);
                    UpdateKspInstances();
                }
            }
        }

        private void OnActivateKspInstance() {
            KSPManager.SetCurrentInstance(SelectedKspInstance.Ksp.Name);
        }

        private void OnMakeDefaultKspInstance()
        {
            KSPManager.SetAutoStart(SelectedKspInstance.Ksp.Name);
            UpdateActive();
        }

        private void OnManagerPropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(KSPManager.CurrentInstance)) {
                Device.BeginInvokeOnMainThread(() => UpdateActive());
            }
        }
    }
}

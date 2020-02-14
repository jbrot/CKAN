using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CKAN.Xamarin.Service
{
    /// <summary>
    /// The CkanService is responsible for setting up CKAN (i.e., the CKAN-core
    /// project) and presenting data from it to the GUI.
    ///
    /// <br />
    ///
    /// Classes which end with Service are automatically instantiated as
    /// singletons by Autofac (this is set up in App).
    /// </summary>
    public class CkanService : AbstractPropertyChangeNotifier, IDisposable
    {
        private KSPManager manager;
        public KSPManager KSPManager {
            get { return manager; }
        }
        public Task<bool> SetKSPManager(KSPManager mgr)
        {
            return Device.InvokeOnMainThreadAsync(() => SetProperty(ref manager, mgr, nameof(KSPManager)));
        }

        private RegistryManager registry;
        public RegistryManager Registry {
            get { return registry; }
        }
        public Task<bool> SetRegistry(RegistryManager rm)
        {
            return Device.InvokeOnMainThreadAsync(() => SetProperty(ref registry, rm, nameof(Registry)));
        }

        private IDialogService dialogService;

        public CkanService (IDialogService ds, KSPManager mgr = null)
        {
            dialogService = ds;

            // We don't want to martial to the main thread to set the
            // starting value.
            manager = mgr;
        }

        public void Dispose ()
        {
            KSPManager.Dispose();
        }

        /// <summary>
        /// This method will start asynchronously initializing CKAN. As things
        /// progress, the public properties will be updated (on the UI thread).
        /// This means that you can bind to these values and automatically
        /// display the correct information once it has loaded.
        /// </summary>
        public void Init()
        {
            Task.Run(async () => {
                if (KSPManager == null) {
                    var mgr = new KSPManager(new NullUser());
                    mgr.GetPreferredInstance();
                    await SetKSPManager(mgr);
                }

                KSP ksp = KSPManager.CurrentInstance ?? KSPManager.GetPreferredInstance();
                if (ksp != null && !await TryLoadKspInstance(ksp)) {
                    await Device.InvokeOnMainThreadAsync(() => Application.Current.Quit());
                }
            });
        }

        private async Task<bool> TryLoadKspInstance (KSP ksp)
        {
            bool retry;
            do {
                try {
                    retry = false;

                    var rm = RegistryManager.Instance(ksp);
                    await SetRegistry(rm);
                } catch (RegistryInUseKraken k) {
                    if (await dialogService.DisplayAlert("Lock File In User",
                        $"Lock file with live process ID found at {k.lockfilePath}\n\n"
                        + "This means that another instance of CKAN probably is accessing this instance."
                        + " You can delete the file to continue, but data corruption is very likely.\n\n"
                        + "Do you want to delete this lock file to force access?",
                        "Force", "Cancel")) {
                        File.Delete(k.lockfilePath);
                        retry = true;
                    } else {
                        // User cancelled, return failure
                        return false;
                    }

                } catch (NotKSPDirKraken k) {
                    await dialogService.DisplayAlert("Error",
                        $"Error loading {ksp.GameDir()}:\n{k.Message}",
                        "OK");
                    return false;

                } catch (Exception e) {
                    await dialogService.DisplayAlert("Error",
                        $"Error loading {Path.Combine(ksp.CkanDir(), "registry.json")}:\n{e.Message}",
                        "OK");
                    return false;
                }
            } while (retry);

            return true;
        }
    }
}

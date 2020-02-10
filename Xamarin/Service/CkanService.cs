using System;
using System.IO;
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
    public class CkanService : IDisposable
    {
        private KSPManager Manager;

        private IDialogService dialogService;
        private RegistryService registryService;

        public CkanService (RegistryService rs, IDialogService ds, KSPManager manager = null)
        {
            registryService = rs;
            dialogService = ds;
            Manager = manager;
        }

        public void Dispose ()
        {
            Manager.Dispose();
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
                if (Manager == null) {
                    var mgr = new KSPManager(new NullUser());
                    await Device.InvokeOnMainThreadAsync(() => Manager = mgr);
                }

                KSP ksp = Manager.CurrentInstance ?? Manager.GetPreferredInstance();
                if (!await TryLoadKspInstance(ksp)) {
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
                    registryService.Registry = rm;
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

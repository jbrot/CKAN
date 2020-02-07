using System;
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

        /// <summary>
        /// Autofac will use this constructor if no KSPManager is registered.
        /// This means we need to create our own.
        /// </summary>
        public CkanService()
        { }

        /// <summary>
        /// Autofac will use this constructor if a KSPManager is registered.
        /// In this case, we can jump straight to instance configuration.
        /// </summary>
        /// <param name="manager"></param>
        public CkanService (KSPManager manager)
        {
            manager = Manager;
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
            });
        }
    }
}

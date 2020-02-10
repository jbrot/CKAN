using System;
using System.Linq;
using System.Reflection;
using Autofac;
using CKAN.Xamarin.Service;
using CKAN.Xamarin.View;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CKAN.Xamarin
{
    public partial class App : Application
    {
        public KSPManager Manager;

        /// <summary>
        /// This LifetimeScope contains the registrations relevant for the
        /// Xamarin GUI.
        ///
        /// <br />
        ///
        /// In general, we want to try and avoid resolving dependencies directly,
        /// and use constructor injection instead. There are two exceptions to
        /// this rule:
        /// <list type="number">
        ///   <item><description>
        ///     ViewModels should directly resolve other ViewModels that they
        ///     are going to navigate to.
        ///   </description></item>
        ///   <item><description>
        ///     Glue code should use BaseViewModel.Scope to resolve the
        ///     affiliated View to display a ViewModel that has been navigated
        ///     to.
        ///   </description></item>
        /// </list>
        /// </summary>
        private ILifetimeScope guiScope;

        public App (KSPManager mgr)
        {
            InitializeComponent();

            guiScope = ServiceLocator.Container.BeginLifetimeScope(builder => {
                var asm = Assembly.GetExecutingAssembly();
                // Register subclasses of BaseViewModel
                builder.RegisterAssemblyTypes(asm)
                       .Where(t => t.IsSubclassOf(typeof(BaseViewModel)))
                       .InstancePerLifetimeScope();
                // Register implementers of IMvvmView<>
                builder.RegisterAssemblyTypes(asm)
                       .Where(t => t.GetInterfaces().Any(x =>
                            x.IsGenericType &&
                            x.GetGenericTypeDefinition() == typeof(IMvvmView<>)))
                       .AsImplementedInterfaces()
                       .InstancePerLifetimeScope();
                // Register services as singletons
                builder.RegisterAssemblyTypes(asm)
                       .Where(t => t.Name.EndsWith("Service"))
                       .AsImplementedInterfaces()
                       .AsSelf()
                       .SingleInstance();
                // If we have a KSPManager, register it so it will get passed
                // to CkanService. Otherwise, let CkanService construct its own.
                if (mgr != null) {
                    // We register KSPManager as externally owned so that
                    // CkanService can call its dispose in all situations.
                    builder.RegisterInstance(mgr)
                           .As<KSPManager>().ExternallyOwned();
                }

            });

            // Resolve the main pages in the outer LifetimeScope, as we
            // only need one of each.
            MainPage = (Page) guiScope.Resolve<IMvvmView<MainPageViewModel>>();

            // Start loading Ckan
            guiScope.Resolve<CkanService>().Init();
        }

        ~App ()
        {
            guiScope.Dispose();
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }

    }
}

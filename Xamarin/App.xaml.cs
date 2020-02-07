using System;
using System.Linq;
using System.Reflection;
using Autofac;
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
                builder.RegisterAssemblyTypes(asm)
                       .Where(t => t.IsSubclassOf(typeof(BaseViewModel)))
                       .InstancePerLifetimeScope();
                builder.RegisterAssemblyTypes(asm)
                       .Where(t => t.GetInterfaces().Any(x =>
                            x.IsGenericType &&
                            x.GetGenericTypeDefinition() == typeof(IMvvmView<>)))
                       .AsImplementedInterfaces()
                       .InstancePerLifetimeScope();
            });

            Manager = mgr ?? new KSPManager(new NullUser());

            // Resolve the main pages in the outer LifetimeScope, as we
            // only need one of each.
            MainPage = (Page) guiScope.Resolve<IMvvmView<MainPageViewModel>>();
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

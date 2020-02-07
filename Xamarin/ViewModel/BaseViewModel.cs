using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autofac;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    /// <summary>
    /// The base class for ViewModels.
    ///
    /// <br />
    ///
    /// ViewModels are instantiated using Autofac, so you can use request
    /// appropriate services in the constructor and have them automatically
    /// filled out.
    ///
    /// <br />
    ///
    /// Subclasses of BaseViewModel are automatically registered with Autofac
    /// by App, so you don't need to worry about registering yourself.
    ///
    /// <br />
    ///
    /// Subclasses of BaseViewModel are resolved per LifetimeScope. You should
    /// be mindful of which scope you are using when resolving a new ViewModel.
    /// In general, if it makes sense that the ViewModel you are resolving
    /// should last as long as the current ViewModel, resolve using the Scope
    /// property (for instance, MainPage resolving different navigation tabs).
    ///
    /// <br />
    ///
    /// However, if you are resolving a page that will not last as long,
    /// resolve the ViewModel in a new ILifetimeScope created from Scope, and
    /// then dispose of this scope after the ViewModel is no longer needed
    /// (for instance, when opening up a detail page for a mod, the detail page
    /// should be in a new scope, and when control returns from the detail page,
    /// the scope can be disposed)
    /// </summary>
    public abstract class BaseViewModel : BindableObject
    {
        /// <summary>
        /// The scope this ViewModel is affiliated with. Used to ensure the
        /// corresponding View is resolved with the same scope.
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
        /// </summary>
        public ILifetimeScope Scope { get; }

        public BaseViewModel (ILifetimeScope scope)
        {
            Scope = scope;
        }

        protected bool SetProperty<T> (ref T field, T value, [CallerMemberName]string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }

            OnPropertyChanging(name);
            field = value;
            OnPropertyChanged(name);
            return true;
        }
    }
}

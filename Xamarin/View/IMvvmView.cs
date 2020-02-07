using System;
using CKAN.Xamarin.ViewModel;

namespace CKAN.Xamarin.View
{
    /// <summary>
    /// This interface is used to link a view to its associated ViewModel.
    /// 
    /// <br />
    /// 
    /// Given a ViewModel of type <typeparamref name="T"/>, we use Autofac
    /// to resolve an instance of IMvvmView&lt;<typeparamref name="T"/>&gt; to
    /// locate the appropriate View to display.
    ///
    /// <br />
    /// 
    /// The main place this happens right now is in ViewModelToViewConverter,
    /// but this may change over time.
    ///
    /// <br />
    /// 
    /// Instances of IMvvmView are automatically
    /// registered with Autofac in App, so you don't have to worry about
    /// registering new classes yourself.
    ///
    /// <br />
    ///
    /// Views and ViewModels are resolved on a per lifetime scope basis,
    /// so you should create an appropriate lifetime scope before resolving
    /// them.
    /// </summary>
    /// <typeparam name="T">The associated ViewModel type</typeparam>
    public interface IMvvmView<T> where T : BaseViewModel
    {
        /// <summary>
        /// The associated ViewModel. Typically, you will have a parameter
        /// of type <typeparamref name="T"/> in your constructor, which you
        /// will use to set this.
        ///
        /// <br />
        ///
        /// Since instances of this class will be constructed by Autofac, all
        /// parameters to the constructor that Autofac knows about will be
        /// automatically filled in.
        /// </summary>
        T ViewModel { get; }
    }
}

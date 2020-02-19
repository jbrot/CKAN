using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    /// <summary>
    /// A ModalViewModel with an unspecified type variable.
    /// </summary>
    public abstract class ModalViewModel : BaseViewModel
    {
        public ModalViewModel (ILifetimeScope scope)
            : base(scope)
        { }
    }

    /// <summary>
    /// The base class for modal ViewModels. A modal view is a view which
    /// appears over the current view and prompts the user for action. When
    /// the user completes the view, control is returned to the parent view
    /// and a value is returned.
    ///
    /// <br />
    ///
    /// A ModalViewModel backs a ModalView, and should call Complete when
    /// the ModalView has completed its action.
    ///
    /// <br />
    ///
    /// Typically, ModalViewModels are resolved in their own ILifetimeScope.
    /// </summary>
    public abstract class ModalViewModel<T> : ModalViewModel
    {
        /// <summary>
        /// The TaskSource behind Task. You can use this to directly set the
        /// result of this ModalViewModel. You probably just want to use the
        /// Complete() and Fail() functions, but this is exposed in case you
        /// need to implement more complicated behavior.
        /// </summary>
        protected TaskCompletionSource<T> TaskSource = new TaskCompletionSource<T>();

        /// <summary>
        /// A Task representing the status of this ModalViewModel. You can await
        /// this task to get the result of this modal view.
        /// </summary>
        public Task<T> Task { get => TaskSource.Task; }

        public ModalViewModel (ILifetimeScope scope)
            : base(scope)
        { }

        /// <summary>
        /// Mark the ModalViewModel as successfully completed with the given
        /// result.
        /// </summary>
        protected void Complete (T value)
        {
            TaskSource.SetResult(value);
        }

        /// <summary>
        /// Mark the ModalViewModel as failed with the given exception.
        /// </summary>
        protected void Fail(Exception e)
        {
            TaskSource.SetException(e);
        }
    }
}

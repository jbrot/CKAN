using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CKAN
{
    /// <summary>
    /// This class provides some boilerplate code which makes implementing
    /// INotifyPropertyChanged much easier.
    /// </summary>
    /// 
    /// <example>
    /// To create a property that emits change notifications in a subclass,
    /// structure the property like this:
    /// <code>
    /// private Type myProperty;
    /// public Type MyProperty {
    ///     get => myProperty;
    ///     set { SetProperty(ref myProperty, value); }
    /// }
    /// </code>
    /// </example>
    public class AbstractPropertyChangeNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Set <paramref name="field"/> to <paramref name="value"/>, and if
        /// <paramref name="field"/> changes, emit PropertyChanged for the
        /// property specified in <paramref name="name"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// NOTE: <paramref name="name"/> will only be correctly set if this
        /// is called from a property setter. Otherwise, you need to specify
        /// the name of the property yourself. I recommend referring to the
        /// property via <code>nameof(MyProperty)</code> rather than the
        /// string literal <code>"MyProperty"</code> as that will cause
        /// things to be properly updated when refactoring (and emit a
        /// compiler error if the refactor misses this reference).</para>
        /// </remarks>
        /// 
        /// <example>
        /// <code>
        /// private Type myProperty;
        /// public Type MyProperty {
        ///     get => myProperty;
        ///     set { SetProperty(ref myProperty, value); }
        /// }
        /// </code>
        /// </example>
        protected bool SetProperty<T> (ref T field, T value, [CallerMemberName]string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }

            field = value;
            OnPropertyChanged(name);
            return true;
        }

        /// <summary>
        /// Invoke PropertyChanged for the specified property. You can override
        /// this in a subclass to add additional behavior, but if you do, make
        /// sure to call the base class implementation or PropertyChanged will
        /// not be emitted.
        /// </summary>
        /// <param name="name"></param>
        protected virtual void OnPropertyChanged (string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

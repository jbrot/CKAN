using System;
namespace CKAN.Xamarin.ViewModel
{
    /// <summary>
    /// Describes a ListItemViewModel which keeps track of whether or not it is
    /// selected. The inhabitants of SelectableListView.ItemsSource must
    /// implement this interface.
    /// </summary>
    public interface ISelectableListItemViewModel
    {
        /// <summary>
        /// Is this ListItemViewModel selected.
        /// </summary>
        bool Selected { get; set; }
    }
}

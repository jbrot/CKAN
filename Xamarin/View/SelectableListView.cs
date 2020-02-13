using System;
using System.Collections;
using System.Windows.Input;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    /// <summary>
    /// A SelectableListView is a ListView which implements its own selection
    /// mechanism. The ItemsSource of a SelectableListView <em>must</em> be
    /// inhabited by ISelectableListItemViewModels.
    /// </summary>
    /// <remarks>
    /// ISelectableListItemViewModels are aware of their selection status,
    /// allowing for the corresponding ViewCells to directly alter their
    /// appearance: as opposed to using the platform mechanism. This allows
    /// for greater flexibility and customizability.
    /// </remarks>
    public class SelectableListView : ListView
    {
        /// <summary>
        /// A Command fired whenever an item is selected.
        /// </summary>
        public ICommand ItemSelectedCommand {
            get { return (ICommand) GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value);  }
        }
        public static BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(SelectableListView));


        public int SelectedItemIndex {
            get { return (int)GetValue(SelectedItemIndexProperty); }
            protected set { SetValue(SelectedItemIndexProperty, value); }
        }
        public static BindableProperty SelectedItemIndexProperty = BindableProperty.Create(nameof(SelectedItemIndex), typeof(int), typeof(SelectableListView));


        public new ISelectableListItemViewModel SelectedItem {
            get { return (ISelectableListItemViewModel)GetValue(NewSelectedItemProperty); }
            protected set { SetValue(NewSelectedItemProperty, value); }
        }
        public static BindableProperty NewSelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(ISelectableListItemViewModel), typeof(SelectableListView));

        public SelectableListView ()
        {
            ItemTapped += OnItemTapped;
            SelectionMode = ListViewSelectionMode.None;
        }

        private void OnItemTapped (object sender, ItemTappedEventArgs args)
        {
            if (SelectedItemIndex == args.ItemIndex && SelectedItem == args.Item) {
                return;
            }

            SelectedItem = (ISelectableListItemViewModel)args.Item;
            SelectedItemIndex = args.ItemIndex;

            var list = (IList)ItemsSource;
            for (int i = 0; i < list.Count; i++) {
                ((ISelectableListItemViewModel) list [i]).Selected = i == args.ItemIndex;
            }

            ItemSelectedCommand?.Execute(args.Item);
        }
    }
}

using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public class CommandListView : ListView
    {
        public static BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(CommandListView));

        public ICommand ItemSelectedCommand {
            get { return (ICommand) GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value);  }
        }

        public CommandListView ()
        {
            ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ItemSelectedCommand?.Execute(args.SelectedItem);
        }
    }
}

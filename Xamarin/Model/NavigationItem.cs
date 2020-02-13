using System;
using CKAN.Xamarin.ViewModel;
using Xamarin.Forms;

namespace CKAN.Xamarin.Model
{
    public class NavigationItem : AbstractPropertyChangeNotifier, ISelectableListItemViewModel
    {
        public string Label { get; set; }
        public ImageSource Icon { get; set; }

        // TODO: Think about if Type is morally and practically the right
        // approach to link the item to its contents.
        //
        // I'm going with Type for now because that is what the MS sample
        // code uses.
        //
        // Originally, I was storing the View type here, which I didn't like
        // at all because it violated MVVM. Now, I'm storing the ViewModel
        // type which seems very reasonable.
        public Type ContentType;

        private bool selected;
        public bool Selected {
            get => selected;
            set { SetProperty(ref selected, value); }
        }
    }
}

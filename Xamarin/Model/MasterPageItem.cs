using System;
using Xamarin.Forms;

namespace CKAN.Xamarin.Model
{
    public class MasterPageItem
    {
        public string Label { get; set; }
        public ImageSource Icon { get; set; }
        // TODO: Think about if Type is morally and practically the right
        // approach to link the item to its contents.
        //
        // I'm going with Type for now because that is what the MS sample
        // code uses.
        //
        // It also could make sense to directly have the associated page
        // stored here---although I think that might violate MVVM.
        //
        // Perhaps a better solution would be to have an Enum of associated
        // actions, and then have them be resolved to pages in the ViewModel.
        // This is pretty similar to what happens with Type I guess: but I
        // the totality enforced by the enum.
        public Type ContentType;
    }
}

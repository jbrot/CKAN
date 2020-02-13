using System;

namespace CKAN.Xamarin.ViewModel
{
    public class PEListItemViewModel : AbstractPropertyChangeNotifier, ISelectableListItemViewModel
    {
        private string columnA;
        public string ColumnA {
            get => columnA;
            set { SetProperty(ref columnA, value); }
        }

        private string columnB;
        public string ColumnB {
            get => columnB;
            set { SetProperty(ref columnB, value); }
        }

        private bool selected;
        public bool Selected {
            get => selected;
            set { SetProperty(ref selected, value); }
        }
    }
}

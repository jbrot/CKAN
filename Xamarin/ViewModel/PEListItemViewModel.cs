using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CKAN.Xamarin.Model
{
    public class PEListItemViewModel : AbstractPropertyChangeNotifier
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

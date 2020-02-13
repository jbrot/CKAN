using System;
using System.Collections.Generic;
using Autofac;
using CKAN.Xamarin.Model;

namespace CKAN.Xamarin.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {

        public IEnumerable<PEListItemViewModel> TempItems { get; }

        public SettingsViewModel (ILifetimeScope scope)
            : base(scope)
        {

            TempItems = new List<PEListItemViewModel>() {
                new PEListItemViewModel {
                    ColumnA="Key1",
                    ColumnB="Value1"
                },
                new PEListItemViewModel {
                    ColumnA="Key2",
                    ColumnB="Value2"
                }
            };
        }
    }
}

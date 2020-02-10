using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CKAN.Xamarin.Service
{
    /// <summary>
    /// The current implementation of IDialogService. The easiest way to present
    /// dialogs is to violate Mvvm, which is why we have it wrapped in an opaque
    /// implementation of IDialogService.
    /// </summary>
    public class DialogService : IDialogService
    {
        public Task DisplayAlert (string title, string message, string cancel)
        {
            return Device.InvokeOnMainThreadAsync(
                () => Application.Current.MainPage.DisplayAlert(title, message, cancel));
        }

        public Task<bool> DisplayAlert (string title, string message, string accept, string cancel)
        {
            return Device.InvokeOnMainThreadAsync(
                () => Application.Current.MainPage.DisplayAlert(title, message, accept, cancel));
        }
    }
}

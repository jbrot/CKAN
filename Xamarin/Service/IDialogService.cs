using System;
using System.Threading.Tasks;

namespace CKAN.Xamarin.Service
{
    /// <summary>
    /// The Dialog service can be used by ViewModels to pop up alerts to the
    /// end user.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Show a prompt with one option.
        /// </summary>
        Task DisplayAlert (string title, string message, string cancel);

        /// <summary>
        /// Show a prompt with two options. Returns true on accept and false on
        /// cancel.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog prompt</param>
        /// <param name="accept">Accept message text (returns true if selected)</param>
        /// <param name="cancel">Cancel message text (returns false if selected)</param>
        /// <returns>True if the accept option is selected, false otherwise</returns>
        Task<bool> DisplayAlert (string title, string message, string accept, string cancel);
    }
}

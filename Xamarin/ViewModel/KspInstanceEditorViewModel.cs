using System;
using System.Windows.Input;
using Autofac;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    public class KspInstanceEditorViewModel : ModalViewModel<bool>
    {
        public ICommand CompleteCommand { get; private set; }

        public KspInstanceEditorViewModel (ILifetimeScope scope)
            : base(scope)
        {
            CompleteCommand = new Command(OnComplete);
        }

        public void OnComplete()
        {
            Complete(true);
        }
    }
}

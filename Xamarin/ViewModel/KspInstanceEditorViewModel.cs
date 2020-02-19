using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using CKAN.Xamarin.Service;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    public class KspInstanceEditorViewModel : ModalViewModel<KSP>
    {
        public ICommand CompleteCommand { get; private set; }
        public ICommand EditPathCommand { get; private set; }

        private bool isValid;
        public bool IsValid {
            get => isValid;
            set => SetProperty(ref isValid, value);
        }

        private string name;
        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string path;
        public string Path {
            get => path;
            set => SetProperty(ref path, value);
        }

        private KSP instance;
        public KSP Instance {
            get => instance;
            set => SetProperty(ref instance, instance);
        }

        private IFileService FileService;

        public KspInstanceEditorViewModel (ILifetimeScope scope, IFileService fileService, KSP inst = null)
            : base(scope)
        {
            FileService = fileService;

            CompleteCommand = new Command(OnComplete);
            EditPathCommand = new Command(EditPath);

            Instance = inst;
            Name = inst?.Name ?? "";
            Path = inst?.GameDir() ?? "";
        }

        private void OnComplete()
        {
            Complete(Instance);
        }

        private void EditPath()
        {
            var chosen = FileService.RunFileDialog(Path);
            if (chosen != null) {
                Path = chosen;
            }
        }
    }
}

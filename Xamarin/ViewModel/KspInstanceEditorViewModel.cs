using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using CKAN.Xamarin.Service;
using CKAN.Xamarin.View;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    public class KspInstanceEditorViewModel : ModalViewModel<KSP>
    {
        private static string MISSING_PATH = "You must provide a path.";
        private static string INVALID_PATH = "The given path is not a KSP instance.";
        private static string MISSING_NAME = "You must provide a name.";
        private static string TAKEN_NAME = "There is already an instance with this name.";


        public ICommand CompleteCommand { get; private set; }
        public ICommand EditPathCommand { get; private set; }

        public bool IsValid {
            get => NameStatus == Status.Valid &&
                   PathStatus == Status.Valid;
        }

        private Task NameValidator;
        private string name;
        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }
        private Status nameStatus;
        public Status NameStatus {
            get => nameStatus;
            set => SetProperty(ref nameStatus, value);
        }
        private string nameMessage;
        public string NameMessage {
            get => nameMessage;
            set => SetProperty(ref nameMessage, value);
        }

        private Task PathValidator;
        private string path;
        public string Path {
            get => path;
            set => SetProperty(ref path, value);
        }
        private Status pathStatus;
        public Status PathStatus {
            get => pathStatus;
            set => SetProperty(ref pathStatus, value);
        }
        private string pathMessage;
        public string PathMessage {
            get => pathMessage;
            set => SetProperty(ref pathMessage, value);
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
            if (inst != null) {
                Name = inst.Name;
                NameStatus = Status.Valid;
                Path = inst.GameDir();
                PathStatus = Status.Valid;
            } else {
                Name = "";
                NameStatus = Status.Invalid;
                NameMessage = MISSING_NAME;
                Path = "";
                PathStatus = Status.Invalid;
                PathMessage = MISSING_PATH;
            }
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

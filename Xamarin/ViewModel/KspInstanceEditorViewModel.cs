using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using CKAN.Xamarin.Service;
using CKAN.Xamarin.View;
using Xamarin.Forms;

using TTask = System.Threading.Tasks.Task;

namespace CKAN.Xamarin.ViewModel
{
    public class KspInstanceEditorViewModel : ModalViewModel<KSP>
    {
        private static string MISSING_PATH = "You must provide a path.";
        private static string INVALID_PATH = "The given path is not a KSP instance.";
        private static string TAKEN_PATH = "There is already an instance at this path.";
        private static string MISSING_NAME = "You must provide a name.";
        private static string TAKEN_NAME = "There is already an instance with this name.";

        public ICommand CompleteCommand { get; private set; }
        public ICommand EditPathCommand { get; private set; }

        public bool IsValid {
            get => NameStatus == Status.Valid &&
                   PathStatus == Status.Valid;
        }

        private string name;
        public string Name {
            get => name;
            set {
                if (SetProperty(ref name, value)) {
                    NameValidator.StartValidator(value);
                }
            }
        }
        private Status nameStatus;
        public Status NameStatus {
            get => nameStatus;
            set {
                if (SetProperty(ref nameStatus, value)) {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }
        private string nameMessage;
        public string NameMessage {
            get => nameMessage;
            set => SetProperty(ref nameMessage, value);
        }
        private Validator<string, Tuple<Status, string>> nameValidator;
        private Validator<string, Tuple<Status, string>> NameValidator {
            get {
                // We have to use lazy initialization here, otherwise the lambdas
                // won't be able to refer to instance variables.
                return nameValidator ?? new Validator<string, Tuple<Status, string>> {
                    Prelude = (input) => NameStatus = Status.Processing,
                    Validation = (input) => {
                        var status = Status.Valid;
                        var msg = "";
                        if (String.IsNullOrWhiteSpace(input)) {
                            status = Status.Invalid;
                            msg = MISSING_NAME;
                        } else if (CkanService.KSPManager.Instances.TryGetValue(input, out KSP taken) && taken != Instance) {
                            status = Status.Invalid;
                            msg = TAKEN_NAME;
                        }
                        return new Tuple<Status, string>(status, msg);
                    },
                    Publisher = (input) => {
                        NameStatus = input.Item1;
                        NameMessage = input.Item2;
                    }
                };
            }
        }

        private string path;
        public string Path {
            get => path;
            set {
                if (SetProperty(ref path, value)) {
                    PathValidator.StartValidator(value);
                }
            }
        }
        private Status pathStatus;
        public Status PathStatus {
            get => pathStatus;
            set {
                if (SetProperty(ref pathStatus, value)) {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }
        private string pathMessage;
        public string PathMessage {
            get => pathMessage;
            set => SetProperty(ref pathMessage, value);
        }
        private Validator<string, Tuple<Status, string>> pathValidator;
        private Validator<string, Tuple<Status, string>> PathValidator {
            get {
                // We have to use lazy initialization here, otherwise the lambdas
                // won't be able to refer to instance variables.
                return pathValidator ?? new Validator<string, Tuple<Status, string>> {
                    Prelude = (input) => PathStatus = Status.Processing,
                    Validation = (input) => {
                        var status = Status.Valid;
                        var msg = "";
                        if (String.IsNullOrWhiteSpace(input)) {
                            status = Status.Invalid;
                            msg = MISSING_PATH;
                        } else {
                            KSP temp = new KSP(input, "temp", new NullUser(), false);
                            if (!temp.Valid) {
                                status = Status.Invalid;
                                msg = INVALID_PATH;
                            } else {
                                foreach (KSP ksp in CkanService.KSPManager.Instances.Values) {
                                    // TODO: Ignoring case is a per-file-system decision, so this isn't always
                                    // the correct move. It would be nice to have a proper are-paths-equal function
                                    // in .NET, but I am not aware of any. As such, this is probably good enough for
                                    // the forseeable future.
                                    if (ksp.GameDir().Equals(temp.GameDir(), StringComparison.OrdinalIgnoreCase)) {
                                        if (ksp.Name != Instance?.Name) {
                                            status = Status.Invalid;
                                            msg = TAKEN_PATH;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        return new Tuple<Status, string>(status, msg);
                    },
                    Publisher = (input) => {
                        PathStatus = input.Item1;
                        PathMessage = input.Item2;
                    }
                };
            }
        }

        private KSP instance;
        public KSP Instance {
            get => instance;
            set => SetProperty(ref instance, value);
        }

        private CkanService CkanService;
        private IFileService FileService;

        public KspInstanceEditorViewModel (ILifetimeScope scope, CkanService ckanService, IFileService fileService, KSP inst = null)
            : base(scope)
        {
            CkanService = ckanService;
            FileService = fileService;

            CompleteCommand = new Command(OnComplete);
            EditPathCommand = new Command(EditPath);

            Instance = inst;
            if (inst != null) {
                Name = inst.Name;
                Path = inst.GameDir();
            } else {
                Name = "";
                Path = "";
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

        /// <summary>
        /// Validator encapsulates the boiler plate of asynchronous validation
        /// code while allowing for a new validation to begin before the previous
        /// one has finished.
        ///
        /// <br />
        ///
        /// Validator is mainly configured with two functions, one which performs the
        /// actual validation on a background thread, and one which publishes the
        /// result of the validation on the GUI thread.
        /// </summary>
        /// <typeparam name="I">The type of input passed to the validation function.</typeparam>
        /// <typeparam name="O">The type of data passed from the validation function to the publishing function.</typeparam>
        private class Validator<I, O>
        {
            private object IdLock = new object();
            private int Id = 0;

            /// <summary>
            /// This function will be invoked when the lock is first acquired.
            /// Typically, this will be invoked on the GUI thread and can be used
            /// to do initial configuration that might be in a race condition with
            /// publisher.
            /// </summary>
            public Action<I> Prelude { get; set; }

            /// <summary>
            /// This function will be invoked on a background thread to perform
            /// the actual validation. Its output will be passed to Publisher.
            /// </summary>
            public Func<I, O> Validation { get; set; }

            /// <summary>
            /// This function will be invoked on the GUI thread to publish the
            /// results of the validation.
            /// </summary>
            public Action<O> Publisher { get; set; }

            /// <summary>
            /// Start a validation on a given input. This function can be called
            /// multiple times without waiting for previous validations to finish.
            /// This is designed to be called from a property setter.
            /// </summary>
            /// 
            /// <example>
            /// <code>
            /// Validator&lt;string, T&gt; MyValidator = ...;
            /// string myProperty;
            /// string MyProperty {
            ///     get => myProperty;
            ///     set {
            ///         if (SetProperty(ref myProperty, value)) {
            ///             MyValidator.StartValidator(value);
            ///         }
            ///     }
            /// }
            /// </code>
            /// </example>
            public void StartValidator (I input)
            {
                // First, secure a new id.
                int nid;
                lock (IdLock) {
                    Id++;
                    nid = Id;
                    Prelude?.Invoke(input);
                }

                TTask.Run(async () => {
                    // If the validator is too fast, it's a bit disconscerting
                    // so we add in a delay.
                    await TTask.Delay(200).ConfigureAwait(false);

                    // After the delay, ensure we are still the most recent validator
                    // (it is likely we're not, so don't bother validating unless we
                    // have to.
                    lock (IdLock) {
                        if (nid != Id) {
                            return;
                        }
                    }

                    O result = Validation.Invoke(input);

                    // Publish validation results on the GUI thread.
                    Device.BeginInvokeOnMainThread(() => {
                        // Ensure we are still the current validator while we publish.
                        lock (IdLock) {
                            if (nid != Id) {
                                return;
                            }

                            Publisher.Invoke(result);
                        }
                    });
                });
            }
        }
    }
}

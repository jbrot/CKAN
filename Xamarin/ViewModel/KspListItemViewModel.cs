using System;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    public class KspListItemViewModel : AbstractPropertyChangeNotifier, ISelectableListItemViewModel
    {
        private KSP ksp;
        public KSP Ksp {
            get => ksp;
            set {
                if (SetProperty(ref ksp, value)) {
                    OnPropertyChanged(nameof(NameText));
                    OnPropertyChanged(nameof(Path));
                    OnPropertyChanged(nameof(Version));
                }
            }
        }

        public FormattedString NameText {
            get {
                var text = new FormattedString();
                if (Ksp != null) {
                    text.Spans.Add(new Span {
                        Text = Ksp.Name
                    });
                }
                if (Active) {
                    text.Spans.Add(new Span {
                        Text = " (Active)",
                        TextColor = Color.Gray
                    });
                }
                return text;
            }
        }

        public string Path {
            get {
                return ksp?.GameDir() ?? "";
            }
        }

        public string Version {
            get {
                return ksp?.Version().ToString();
            }
        }

        private bool active;
        public bool Active {
            get => active;
            set {
                if (SetProperty(ref active, value)) {
                    OnPropertyChanged(nameof(NameText));
                }
            }
        }

        private bool autoStart;
        public bool AutoStart {
            get => autoStart;
            set { SetProperty(ref autoStart, value); }
        }

        private bool selected;
        public bool Selected {
            get => selected;
            set { SetProperty(ref selected, value); }
        }
    }
}

using System;
using Xamarin.Forms;

namespace CKAN.Xamarin.ViewModel
{
    public class ModListItemViewModel : BaseViewModel
    {
        private CkanModule model;
        public CkanModule Model {
            get => model;
            set {
                if (SetProperty(ref model, value)) {
                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(Body));
                }
            }
        }

        public FormattedString Title {
            get {
                var fs = new FormattedString();
                if (Model != null) {
                    fs.Spans.Add(new Span() {
                        Text = $"{Model.name} ",
                        TextColor = Color.Black,
                        FontSize = 16
                    });
                    fs.Spans.Add(new Span() {
                        Text = $"({Model.version})",
                        TextColor = Color.Gray,
                        FontSize = 14
                    });
                }
                return fs;
            }
        }
        public FormattedString Body {
            get {
                var fs = new FormattedString();
                if (Model != null) {
                    fs.Spans.Add(new Span() {
                        Text = $"{Model.@abstract}",
                        FontSize = 12
                    });
                }
                return fs;
            }
        }

        public ModListItemViewModel (CkanModule mdl)
            : base(null)
        {
            Model = mdl;
        }
    }
}

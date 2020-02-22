using System;
using CKAN.Xamarin.Mac.Renderer;
using CKAN.Xamarin.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

[assembly: ExportRenderer(typeof(StatusIndicator), typeof(StatusIndicatorRenderer))]
namespace CKAN.Xamarin.Mac.Renderer
{
    public class StatusIndicatorRenderer : VisualElementRenderer<ContentView>
    {
        protected override void OnElementChanged (ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null) {
                e.NewElement.PropertyChanged += OnPropertyChanged;
                UpdateToolTip((StatusIndicator)e.NewElement);
            }

            if (e.OldElement != null) {
                e.OldElement.PropertyChanged -= OnPropertyChanged;
            }
        }

        private void OnPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StatusIndicator.Message)) {
                UpdateToolTip((StatusIndicator)sender);
            }
        }

        private void UpdateToolTip(StatusIndicator si)
        {
            if (si.CurrentStatus == Status.Invalid) {
                ToolTip = si.Message;
            } else {
                ToolTip = null;
            }
        }
    }
}

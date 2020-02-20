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

            StatusIndicator si = (StatusIndicator)e.NewElement;
            if (si?.CurrentStatus == Status.Invalid) {
                ToolTip = si.Message;
            } else {
                ToolTip = null;
            }
        }
    }
}

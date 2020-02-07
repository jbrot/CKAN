using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Xamarin.Forms;

namespace CKAN.Xamarin.View
{
    public class BindableContentPage : ContentPage

    {
        public BindableContentPage ()
        {
        }

        protected override void OnChildAdded (Element child)
        {
            base.OnChildAdded(child);

            // HACK: For some reason, after changing Content, the new content won't
            // render until we resize. So, we force an imperceptible resize to cause
            // it to show immediately.
            var mdv = Parent as MasterDetailPage;
            var delta = mdv.Y == 0 ? 1 : -1;
            mdv.Layout(new Rectangle(mdv.X, mdv.Y + delta, mdv.Width, mdv.Height - delta));
        }
    }
}

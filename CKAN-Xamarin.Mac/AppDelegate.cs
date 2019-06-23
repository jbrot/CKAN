using System.Diagnostics;
using AppKit;
using Foundation;

namespace CKAN_Xamarin.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.MacOS.FormsApplicationDelegate
    {

        NSWindow window;

        public AppDelegate ()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            window.Title = "Xamarin.Forms on Mac!";
            window.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override NSWindow MainWindow {
            get => window;
        }


        public override void DidFinishLaunching (NSNotification notification)
        {
            Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate (NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        public override bool ApplicationShouldHandleReopen (NSApplication sender, bool hasVisibleWindows)
        {
            if (hasVisibleWindows) {
                window.OrderFront(this);
            } else {
                window.MakeKeyAndOrderFront(this);
            }

            return true;
        }
    }
}

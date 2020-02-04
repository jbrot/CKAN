using AppKit;

namespace CKAN.Xamarin.Mac
{
    static class MainClass
    {
        static void Main (string [] args)
        {
            Logging.Initialize();

            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new AppDelegate();
            NSApplication.Main(args);
        }
    }
}

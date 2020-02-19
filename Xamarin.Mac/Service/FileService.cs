using System;
using System.Threading.Tasks;
using AppKit;
using CKAN.Xamarin.Service;

namespace CKAN.Xamarin.Mac.Service
{
    public class FileService : IFileService
    {

        public string RunFileDialog (string currentPath)
        {
            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = false;
            dlg.CanChooseDirectories = true;

            if (dlg.RunModal() == 1) {
                return dlg.Filename;
            }

            return null;
        }
    }
}

using System;
using System.Threading.Tasks;

namespace CKAN.Xamarin.Service
{
    /// <summary>
    /// IFileService provides a platform-independant interface to the file
    /// system. This is implemented in the platform-specific projects (e.g.,
    /// CKAN-Xamarin.Mac)
    /// </summary>
    public interface IFileService
    {
        string RunFileDialog (string currentPath = null);
    }
}

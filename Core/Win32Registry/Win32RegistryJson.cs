using System;
using System.Collections.Generic;

namespace CKAN.Win32Registry
{
    public class Win32RegistryJson : IWin32Registry
    {
        public Win32RegistryJson ()
        {
        }

        public string AutoStartInstance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DownloadCacheDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long? CacheSizeLimit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int RefreshRate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<string> GetAuthTokenHosts ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tuple<string, string>> GetInstances ()
        {
            throw new NotImplementedException();
        }

        public string GetKSPBuilds ()
        {
            throw new NotImplementedException();
        }

        public void SetAuthToken (string host, string token)
        {
            throw new NotImplementedException();
        }

        public void SetKSPBuilds (string buildMap)
        {
            throw new NotImplementedException();
        }

        public void SetRegistryToInstances (SortedList<string, KSP> instances)
        {
            throw new NotImplementedException();
        }

        public bool TryGetAuthToken (string host, out string token)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CKAN.Win32Registry
{
    public class Win32RegistryJson : IWin32Registry
    {
        private class ConfigFile
        {
            public string AutoStartInstance { get; set; }
            public string DownloadCacheDir { get; set; }
            public long? CacheSizeLimit { get; set; }
            public int? RefreshRate { get; set; }
            public string KSPBuilds { get; set; }
            public IList<KspInstance> KspInstances;
            public IList<AuthToken> AuthTokens;
        }

        private class KspInstance
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        private class AuthToken
        {
            public string Host { get; set; }
            public string Token { get; set; }
        }

        private static readonly string configFile =
            Environment.GetEnvironmentVariable("CKAN_CONFIG_FILE")
            ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CKAN",
                "config.json"
            );

        private static readonly string defaultDownloadCacheDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CKAN",
            "downloads"
        );

        private static readonly object _lock = new object();
        private static ConfigFile config = null;

        // Save the JSON configuration file. Only call this while you
        // own _lock.
        private static void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configFile, json);
        }

        public Win32RegistryJson ()
        {
            lock(_lock)
            {
                if (config != null)
                    return;
                try
                {
                    string json = File.ReadAllText(configFile);
                    config = JsonConvert.DeserializeObject<ConfigFile>(json);
                    if (config.KspInstances == null)
                    {
                        config.KspInstances = new List<KspInstance>();

                    }
                    if (config.AuthTokens == null)
                    {
                        config.AuthTokens = new List<AuthToken>();
                    }
                }
                catch (FileNotFoundException)
                {
                    config = new ConfigFile();
                    config.KspInstances = new List<KspInstance>();
                    config.AuthTokens = new List<AuthToken>();

                    SaveConfig();
                }
            }
        }

        public string DownloadCacheDir
        {
            get
            {
                lock (_lock)
                {
                    return config.DownloadCacheDir ?? defaultDownloadCacheDir;
                }
            }
            set
            {
                lock (_lock)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        config.DownloadCacheDir = null;
                    }
                    else
                    {
                        if (!Path.IsPathRooted(value))
                        {
                            value = Path.GetFullPath(value);
                        }
                        config.DownloadCacheDir = value;
                    }

                    SaveConfig();
                }
            }
        }

        public long? CacheSizeLimit
        {
            get
            {
                lock (_lock)
                {
                    return config.CacheSizeLimit;
                } 
            }

            set
            {
                lock (_lock)
                {
                    config.CacheSizeLimit = value;

                    SaveConfig();
                }
            }
        }

        public int RefreshRate
        {
            get
            {
                lock (_lock)
                {
                    return config.RefreshRate ?? 0;
                }
            }

            set
            {
                lock (_lock)
                {
                    if (value <= 0)
                    {
                        config.RefreshRate = null;
                    }
                    else
                    {
                        config.RefreshRate = value;
                    }

                    SaveConfig();
                }
            }
        }


        public string AutoStartInstance
        {
            get
            {
                lock (_lock)
                {
                    return config.AutoStartInstance ?? "";
                }
            }

            set
            {
                lock (_lock)
                {
                    config.AutoStartInstance = value ?? "";

                    SaveConfig();
                }
            }
        }


        public string GetKSPBuilds ()
        {
            lock (_lock)
            {
                return config.KSPBuilds;
            }
        }

        public void SetKSPBuilds (string buildMap)
        {
            lock (_lock)
            {
                config.KSPBuilds = buildMap;

                SaveConfig();
            }
        }

        public IEnumerable<Tuple<string, string>> GetInstances ()
        {
            lock (_lock)
            {
                return config.KspInstances.Select(instance =>
                    new Tuple<string, string>(instance.Name, instance.Path));
            }
        }

        public void SetRegistryToInstances(SortedList<string, KSP> instances)
        {
            lock (_lock)
            {
                config.KspInstances = instances.Select(instance => new KspInstance
                {
                    Name = instance.Key,
                    Path = instance.Value.GameDir()
                }).ToList();

                SaveConfig();
            }
        }

        public IEnumerable<string> GetAuthTokenHosts()
        {
            lock (_lock)
            {
                return config.AuthTokens.Select(token => token.Host);
            }
        }


        public bool TryGetAuthToken (string host, out string token)
        {
            lock (_lock)
            {
                foreach (AuthToken t in config.AuthTokens)
                {
                    if (t.Host == host)
                    {
                        token = t.Token;
                        return true;
                    }
                }

                token = "";
                return false;
            }
        }

        public void SetAuthToken (string host, string token)
        {
            lock (_lock)
            {
                bool found = false;
                foreach (AuthToken t in config.AuthTokens)
                {
                    if (t.Host == host)
                    {
                        found = true;
                        t.Token = token;
                    }
                }

                if (!found)
                {
                    config.AuthTokens.Add(new AuthToken
                    {
                        Host = host,
                        Token = token
                    });
                }

                SaveConfig();
            }
        }
    }
}

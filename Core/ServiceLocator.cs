using System;
using Autofac;
using CKAN.GameVersionProviders;
using CKAN.Win32Registry;

namespace CKAN
{
    /// <summary>
    /// This class exists as a really obvious place for our service locator (ie: Autofac container)
    /// to live.
    /// </summary>
    public static class ServiceLocator
    {
        private static IContainer _container;
        public static IContainer Container
        {
            // NB: Totally not thread-safe.
            get
            {
                if (_container == null)
                {
                    Init();
                }

                return _container;
            }

            set
            {
                _container = value;
            }
        }

        private static void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<GrasGameComparator>()
                .As<IGameComparator>();

            // See https://stackoverflow.com/questions/5116977/how-to-check-the-os-version-at-runtime-e-g-windows-or-linux-without-using-a-con
            // for why we need to check for 128. We could also use RuntimeInformation.IsOSPlatform, but that would require bringing in an extra
            // dependency.
            //
            // Also, we'll make this a SingleInstance, which will make life much simpler when dealing with the JSON registry.
            var platform = Environment.OSVersion.Platform;
            if (platform == PlatformID.MacOSX || platform == PlatformID.Unix || ((int)platform) == 128)
                builder.RegisterType<Win32RegistryJson>().As<IWin32Registry>().SingleInstance();
            else
                builder.RegisterType<Win32RegistryReal>().As<IWin32Registry>().SingleInstance();

                builder.RegisterType<KspBuildMap>()
                .As<IKspBuildMap>()
                .SingleInstance(); // Since it stores cached data we want to keep it around

            builder.RegisterType<KspBuildIdVersionProvider>()
                .As<IGameVersionProvider>()
                .Keyed<IGameVersionProvider>(KspVersionSource.BuildId);

            builder.RegisterType<KspReadmeVersionProvider>()
                .As<IGameVersionProvider>()
                .Keyed<IGameVersionProvider>(KspVersionSource.Readme);

            _container = builder.Build();
        }
    }
}

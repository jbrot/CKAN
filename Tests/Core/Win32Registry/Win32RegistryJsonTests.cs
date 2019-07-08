using CKAN.Win32Registry;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Tests.Data;

namespace Tests.Core.Win32Registry
{
    [TestFixture] public class Win32RegistryJsonTests
    {
        // We want to make sure that the config file is pointed to the
        // right place for the other tests.
        private string configFileLoc;

        [SetUp]
        public void SetUp()
        {
            configFileLoc = new Win32RegistryJson().ConfigFile;
        }

        [TearDown]
        public void TearDown()
        {
            _ = new Win32RegistryJson(configFileLoc);
        }

        [Test]
        public void CreatesNewConfig()
        {
            string tmpFile = Path.GetTempFileName();
            File.Delete(tmpFile);

            _ = new Win32RegistryJson(tmpFile);

            Assert.IsTrue(File.Exists(tmpFile));

            File.Delete(tmpFile);
        }

        [Test]
        public void CreatesNewConfigAndDirectory()
        {
            string tmpDir = Path.GetTempFileName();
            File.Delete(tmpDir);

            string tmpFile = Path.Combine(tmpDir, "config.json");

            _ = new Win32RegistryJson(tmpFile);

            Assert.IsTrue(File.Exists(tmpFile));

            Directory.Delete(tmpDir, true);
        }

        [Test]
        public void LoadsGoodConfig()
        {
            string tmpFile = Path.GetTempFileName();
            File.WriteAllText(tmpFile, TestData.GoodJsonConfig());

            var reg = new Win32RegistryJson(tmpFile);

            CollectionAssert.AreEquivalent(new List<Tuple<string, string>> ()
            {
                new Tuple<string, string>("instance1", "instance1_path"),
                new Tuple<string, string>("instance2", "instance2_path")
            }, reg.GetInstances());

            CollectionAssert.AreEquivalent(new List<string>()
            {
                "host1",
                "host2",
                "host3"
            }, reg.GetAuthTokenHosts());

            var token = "";
            Assert.IsTrue(reg.TryGetAuthToken("host1", out token));
            Assert.AreEqual("token1", token);
            Assert.IsTrue(reg.TryGetAuthToken("host2", out token));
            Assert.AreEqual("token2", token);
            Assert.IsTrue(reg.TryGetAuthToken("host3", out token));
            Assert.AreEqual("token3", token);

            Assert.AreEqual("asi", reg.AutoStartInstance);
            Assert.AreEqual("dci", reg.DownloadCacheDir);
            Assert.AreEqual(2, reg.CacheSizeLimit);
            Assert.AreEqual(4, reg.RefreshRate);
            Assert.AreEqual("build_string", reg.GetKSPBuilds());

            File.Delete(tmpFile);
        }

        [Test]
        public void LoadsMissingJsonConfig()
        {
            string tmpFile = Path.GetTempFileName();
            File.WriteAllText(tmpFile, TestData.MissingJsonConfig());

            var reg = new Win32RegistryJson(tmpFile);

            CollectionAssert.AreEquivalent(new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("instance1", "instance1_path"),
                new Tuple<string, string>("instance2", "instance2_path")
            }, reg.GetInstances());

            CollectionAssert.AreEquivalent(new List<string>(), reg.GetAuthTokenHosts());

            Assert.AreEqual("", reg.AutoStartInstance);
            Assert.AreEqual(Win32RegistryJson.DefaultDownloadCacheDir, reg.DownloadCacheDir);
            Assert.AreEqual(null, reg.CacheSizeLimit);
            Assert.AreEqual(4, reg.RefreshRate);
            Assert.AreEqual("build_string", reg.GetKSPBuilds());

            File.Delete(tmpFile);
        }

        [Test]
        public void LoadsExtraConfig()
        {
            string tmpFile = Path.GetTempFileName();
            File.WriteAllText(tmpFile, TestData.ExtraJsonConfig());

            var reg = new Win32RegistryJson(tmpFile);

            CollectionAssert.AreEquivalent(new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("instance1", "instance1_path"),
                new Tuple<string, string>("instance2", "instance2_path")
            }, reg.GetInstances());

            CollectionAssert.AreEquivalent(new List<string>()
            {
                "host1",
                "host2",
                "host3"
            }, reg.GetAuthTokenHosts());

            var token = "";
            Assert.IsTrue(reg.TryGetAuthToken("host1", out token));
            Assert.AreEqual("token1", token);
            Assert.IsTrue(reg.TryGetAuthToken("host2", out token));
            Assert.AreEqual("token2", token);
            Assert.IsTrue(reg.TryGetAuthToken("host3", out token));
            Assert.AreEqual("token3", token);

            Assert.AreEqual("asi", reg.AutoStartInstance);
            Assert.AreEqual("dci", reg.DownloadCacheDir);
            Assert.AreEqual(2, reg.CacheSizeLimit);
            Assert.AreEqual(4, reg.RefreshRate);
            Assert.AreEqual("build_string", reg.GetKSPBuilds());

            File.Delete(tmpFile);
        }

        [Test]
        public void FailsToLoadBadConfig()
        {
            string tmpFile = Path.GetTempFileName();
            File.WriteAllText(tmpFile, TestData.BadJsonConfig());

            Assert.Catch<JsonException>(delegate
            {
                _ = new Win32RegistryJson(tmpFile);
            });

            File.Delete(tmpFile);
        }

#if !NETCOREAPP
        // TODO: Migration tests.

        // I don't see any good way to do these without overwriting the
        // registry values, which is fine for the build server but may be
        // annoying for devs.
#endif

    }
}

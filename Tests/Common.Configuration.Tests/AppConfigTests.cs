using NUnit.Framework;

namespace Common.Configuration.Tests
{
    [TestFixture]
    public class AppConfigTests
    {
        class AppConfigSection1 : AppConfigBase
        {
            public AppConfigSection1(string appConfigFileName)
                :base("Section1", appConfigFileName)
            {
            }

            public string Prm1 => GetValue<string>("Prm1");
            public int Prm2 => GetValue<int>("Prm2");
            public string NonExistPrm => GetValue<string>("NonExistPrm");
        }
        class AppConfig : AppConfigBase
        {
            public AppConfig(string appConfigFileName)
                : base(null, appConfigFileName)
            {
            }
        }

        private const string AppConfigFile = "appsettings1.json";

        [Test]
        public void Section_Parameter_exists()
        {
            var cfg = new AppConfigSection1(AppConfigFile);
            Assert.AreEqual("Val1", cfg.Prm1);
            Assert.AreEqual(10, cfg.Prm2);
        }

        [Test]
        public void Section_Parameter_does_not_exist()
        {
            var cfg = new AppConfigSection1(AppConfigFile);
            Assert.IsNull(cfg.NonExistPrm);
        }

        [Test]
        public void GetConnectionString_Test()
        {
            var cfg = new AppConfigSection1(AppConfigFile);
            Assert.AreEqual("Some connection string", cfg.GetConnectionString("DbConnection1"));

            cfg = new AppConfigSection1(AppConfigFile);
            Assert.AreEqual("Some connection string", cfg.GetConnectionString("DbConnection1"));
        }

        [Test]
        public void ConnectionString_Test()
        {
            var cfg = new AppConfigSection1("appsettings1.json");
            Assert.AreEqual("Some connection string", cfg.ConnectionString);
        }

        [Test]
        public void UnityConfiguration_Test()
        {
            var cfg = new AppConfigSection1("appsettings1.json");
            var unity = cfg.Unity;
            Assert.IsNotNull(unity);
            Assert.IsNotNull(unity.Containers);
            Assert.AreEqual(2, unity.Containers.Count);
            Assert.AreEqual("Container1", unity.Containers[0].Container);
            Assert.IsNotNull(unity.Containers[0].Mappings);
            Assert.AreEqual(2, unity.Containers[0].Mappings.Count);
            Assert.AreEqual("type11", unity.Containers[0].Mappings[0].Type);
            Assert.AreEqual("mapTo11", unity.Containers[0].Mappings[0].MapTo);
        }

    }
}

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Configuration.Menu;
using NUnit.Framework;

namespace Common.Configuration.Tests
{
    [TestFixture]
    public class MenuConfigurationTests
    {
        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        [Test]
        public void LoadTest()
        {
            MenuConfiguration menuConfig = MenuConfiguration.Load(AssemblyDirectory + @"\Data\TestMenu.xml");
            Assert.NotNull(menuConfig);
            Assert.NotNull(menuConfig.Menus);
            Assert.AreEqual(2, menuConfig.Menus.Count());
            Menu.Menu menu = menuConfig.Menus[0];
            Assert.NotNull(menu);
            Assert.AreEqual("Menu1", menu.Id);
            Assert.NotNull(menu.Items);
            Assert.AreEqual(2, menu.Items.Count());
            Assert.AreEqual("Item1", menu.Items[0].Title);
            Assert.NotNull(menu.Items[0].Items);
            Assert.AreEqual(2, menu.Items[0].Items.Count());
            MenuItem mi = menu.Items[0].Items[0];
            Assert.AreEqual("Item11", mi.Title);
            Assert.AreEqual("controller11", mi.Controller);
            Assert.AreEqual("Action11", mi.Action);

            Assert.NotNull(mi.DependencyActions);
            Assert.AreEqual(1, mi.DependencyActions.Count());
            Assert.AreEqual("DepAction111", mi.DependencyActions[0].Action);
            Assert.AreEqual("DepController111", mi.DependencyActions[0].Controller);
        }
    }
}

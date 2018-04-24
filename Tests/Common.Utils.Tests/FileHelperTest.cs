using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Common.Utils.Tests
{
    [TestFixture]
    public class FileHelperTest
    {
        private string _testFolder;
        private string _testTempFolder;

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        [OneTimeSetUp]
        public void InitAllTests()
        {
            _testFolder = AssemblyDirectory + @"\Data\FileHelper";
            _testTempFolder = AssemblyDirectory + @"\Data\FileHelper\Temp";

            if (Directory.Exists(_testTempFolder))
                Directory.Delete(_testTempFolder, true);

            Directory.CreateDirectory(_testTempFolder);
        }

        [Test]
        public void DirectoryCopyTest()
        {
            string srcDir = Path.Combine(_testFolder, "Dir1");

            Assert.DoesNotThrow(() => FileHelper.DirectoryCopy(srcDir, _testTempFolder, true));
            Assert.True(Directory.Exists(Path.Combine(_testTempFolder, "Dir11")));
            Assert.True(File.Exists(Path.Combine(_testTempFolder, "TextFile1.txt")));
            Assert.True(File.Exists(Path.Combine(_testTempFolder, "Dir11", "TextFile11.txt")));

            Assert.Throws<IOException>(() => FileHelper.DirectoryCopy(srcDir, _testTempFolder, true, false));
            Assert.DoesNotThrow(() => FileHelper.DirectoryCopy(srcDir, _testTempFolder, true, true));

            Directory.Delete(_testTempFolder, true);
            Directory.CreateDirectory(_testTempFolder);

            Assert.DoesNotThrow(() => FileHelper.DirectoryCopy(srcDir, _testTempFolder, false));
            Assert.False(Directory.Exists(Path.Combine(_testTempFolder, "Dir11")));
            Assert.True(File.Exists(Path.Combine(_testTempFolder, "TextFile1.txt")));
        }

        [Test]
        public void CopyFilesToDirectoryTest()
        {
            string dstPath = Path.Combine(_testTempFolder, "F1");
            string[] files1 =  Directory.GetFiles(Path.Combine(_testFolder, "Dir1"));
            FileHelper.CopyFilesToDirectory(files1, dstPath, true);

            string[] files2 = Directory.GetFiles(dstPath);
            CollectionAssert.AreEqual(files1.Select(Path.GetFileName), files2.Select(Path.GetFileName));
        }

        [Test]
        public void GetRandomFolderNameTest()
        {
            string name1 = FileHelper.GetRandomFolderName();
            string name2 = FileHelper.GetRandomFolderName();
            string name3 = FileHelper.GetRandomFolderName();
            Assert.AreNotEqual(name1, name2);
            Assert.AreNotEqual(name1, name3);
            Assert.AreNotEqual(name2, name3);
        }

        [Test]
        public void RenameTest()
        {
            string filePath = Path.Combine(_testTempFolder, "FileToReaname.txt");
            FileStream fs = File.Create(filePath);
            fs.Close();

            string filePath2 = FileHelper.Rename(filePath, "RenamedFile.txt");
            Assert.False(File.Exists(filePath));
            Assert.True(File.Exists(filePath2));
        }
    }
}

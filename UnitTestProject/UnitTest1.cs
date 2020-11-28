using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Program.RunAsync().GetAwaiter().GetResult();
            var filePath = Program.filePath;
            var imageFolderPath = Program.imageFolderPath;
            Assert.AreEqual(true, File.Exists(filePath));
            Assert.AreEqual(true, Directory.Exists(imageFolderPath)); 
            string[] fileEntries = Directory.GetFiles(imageFolderPath);
            Assert.AreEqual(true, fileEntries.Length > 0);
        }
    }
}

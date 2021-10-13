using NUnit.Framework;
using System.Reflection;

namespace SourceModelsProviderJsonTests
{
    public sealed class FileHelperTests
    {
        //private string GetCurrentAssemblyLocation()
        //{ 
        //    string assemblyLoc = Assembly.GetExecutingAssembly().Location;
        //    string assemblyFolder = assemblyLoc.Remove(assemblyLoc.IndexOf("Digitteck.Gateway.Tests.dll"), "Digitteck.Gateway.Tests.dll".Length);
        //    return assemblyFolder;
        //}

        //[Test]
        //public void file_helper_get_file_path_relative_simple()
        //{
        //    string path = FileHelper.GetFilePathInCurrentAssembly("test");
        //    string assemblyFolder = GetCurrentAssemblyLocation();
        //    Assert.AreEqual(path, $"{assemblyFolder}test");
        //}

        //[Test]
        //public void file_helper_get_file_path_relative_nested()
        //{
        //    string path = FileHelper.GetFilePathInCurrentAssembly("test\\test2.json");
        //    string assemblyFolder = GetCurrentAssemblyLocation();
        //    Assert.AreEqual(path, $"{assemblyFolder}test\\test2.json");
        //}

        //[Test]
        //public void file_helper_get_file_path_relative_nested_not_clean()
        //{
        //    string assemblyFolder = GetCurrentAssemblyLocation();
        //    string path1 = FileHelper.GetFilePathInCurrentAssembly("\\test\\test2.json");
        //    string path2 = FileHelper.GetFilePathInCurrentAssembly("/test/test2.json");

        //    Assert.AreEqual(path1, $"{assemblyFolder}test\\test2.json");
        //    Assert.AreEqual(path2, $"{assemblyFolder}test\\test2.json");
        //}
    }
}

using GenericModEngine.Core.Types.Assemblies;

namespace GenericModEngine.Tests;

public class UnloadedAssemblyWrapperTest
{
    [Fact]
    public void Test_Constructor_Invalid()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            new UnloadedAssemblyWrapper("C:/Test/NotADll.txt");
        });
    }

    [Fact]
    public void Test_Constructor_Mock()
    {
        string path = "C:/Test/aDLL.dll";
        UnloadedAssemblyWrapper wrapper = new UnloadedAssemblyWrapper(path, true);
        Assert.Equal("MockAssembly", wrapper.AssemblyName.Name);
        Assert.Equal(path, wrapper.AssemblyPath);
    }
}

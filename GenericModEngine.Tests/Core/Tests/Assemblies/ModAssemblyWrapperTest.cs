using System.Reflection;
using GenericModEngine.Core.Types.Assemblies;
using GenericModEngine.Tests.Core.Utils.Data.Implementations;
using Moq;

namespace GenericModEngine.Tests;

public class ModAssemblyWrapperTest
{
    private Mock<Assembly> assemblyOneEntryPoint = new Mock<Assembly>();
    private Mock<Assembly> assemblyTwoEntryPoints = new Mock<Assembly>();
    private Mock<Assembly> assemblyNoEntryPoint = new Mock<Assembly>();

    [Fact]
    public void Test_ModAssemblyWrapper_EntryPoint()
    {
        assemblyOneEntryPoint
            .Setup(a => a.GetTypes())
            .Returns(new Type[] { typeof(EntryPoint1), typeof(GenericClass1) });
        assemblyTwoEntryPoints
            .Setup(a => a.GetTypes())
            .Returns(new Type[] { typeof(EntryPoint1), typeof(EntryPoint2) });
        assemblyNoEntryPoint.Setup(a => a.GetTypes()).Returns(new Type[] { typeof(GenericClass2) });

        ModAssemblyWrapper wrapper1 = new ModAssemblyWrapper(assemblyOneEntryPoint.Object);
        ModAssemblyWrapper wrapper2 = new ModAssemblyWrapper(assemblyNoEntryPoint.Object);
        Assert.Throws<InvalidOperationException>(() =>
        {
            new ModAssemblyWrapper(assemblyTwoEntryPoints.Object);
        });

        Assert.NotNull(wrapper1.EntryPoint);
        Assert.IsType<EntryPoint1>(wrapper1.EntryPoint);
        Assert.Equal(assemblyOneEntryPoint.Object, wrapper1.Assembly);
        Assert.Null(wrapper2.EntryPoint);
        Assert.Equal(assemblyNoEntryPoint.Object, wrapper2.Assembly);
    }
}

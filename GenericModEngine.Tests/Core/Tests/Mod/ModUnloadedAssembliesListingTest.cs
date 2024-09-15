using System.IO.Abstractions.TestingHelpers;
using GenericModEngine.Core.Types;
using GenericModEngine.Core.Types.Assemblies;
using GenericModEngine.Tests.Core.Utils.Data.Manifests;

namespace GenericModEngine.Tests;

public class ModUnloadedAssembliesListingTest
{
    private static readonly string Manifest1Path = Path.Combine(
        "C:",
        "Game",
        "Mods",
        "Mod1",
        "About",
        "manifest.json"
    );
    private static readonly string Manifest2Path = Path.Combine(
        "C:",
        "Game",
        "Mods",
        "Mod2",
        "About",
        "manifest.json"
    );

    private static readonly string Assembly1Path = Path.Combine(
        "C:",
        "Game",
        "Mods",
        "Mod1",
        "Assemblies",
        "Mod1.dll"
    );
    private static readonly string Assembly2Path = Path.Combine(
        "C:",
        "Game",
        "Mods",
        "Mod1",
        "Assemblies",
        "Mod2.dll"
    );
    private static readonly string NotAssemblyPath = Path.Combine(
        "C:",
        "Game",
        "Mods",
        "Mod1",
        "Assemblies",
        "Mod3.txt"
    );

    private MockFileSystem _fileSystem = new MockFileSystem(
        new Dictionary<string, MockFileData>()
        {
            { Manifest1Path, new MockFileData(MockManifestsJSON.Manifest1) },
            { Manifest2Path, new MockFileData(MockManifestsJSON.Manifest2) },
            { Assembly1Path, new MockFileData(new byte[] { 0x00 }) },
            { Assembly2Path, new MockFileData(new byte[] { 0x00 }) },
            { NotAssemblyPath, new MockFileData(new byte[] { 0x00 }) },
        }
    );

    [Fact]
    public void Test_ListUnloadedAssemblies_Mod1()
    {
        Mod mod = new Mod(Path.Combine("C:", "Game", "Mods", "Mod1"), _fileSystem);
        List<UnloadedAssemblyWrapper> unloadedAssemblies = mod.UnloadedAssemblies;
        Assert.Equal(2, unloadedAssemblies.Count);
        Assert.Contains(
            unloadedAssemblies,
            a => Path.GetFullPath(a.AssemblyPath) == Path.GetFullPath(Assembly1Path)
        );
        Assert.Contains(
            unloadedAssemblies,
            a => Path.GetFullPath(a.AssemblyPath) == Path.GetFullPath(Assembly2Path)
        );
    }

    [Fact]
    public void Test_ListUnloadedAssemblies_Mod2()
    {
        Mod mod = new Mod(Path.Combine("C:", "Game", "Mods", "Mod2"), _fileSystem);
        List<UnloadedAssemblyWrapper> unloadedAssemblies = mod.UnloadedAssemblies;
        Assert.Empty(unloadedAssemblies);
    }
}

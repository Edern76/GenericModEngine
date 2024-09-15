using System.IO.Abstractions.TestingHelpers;
using GenericModEngine.Core.Types;
using GenericModEngine.Tests.Core.Utils.Data.Manifests;

namespace GenericModEngine.Tests;

public class ManifestParsingTest
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
    private static readonly string Manifest3Path = Path.Combine(
        "C:",
        "Game",
        "Mods",
        "Mod3",
        "About",
        "manifest.json"
    );
    private static readonly string Manifest4Path = Path.Combine(
        "C:",
        "Game",
        "Mods",
        "Mod4",
        "About",
        ".gitkeep"
    );
    private MockFileSystem _fileSystem = new MockFileSystem(
        new Dictionary<string, MockFileData>()
        {
            { Manifest1Path, new MockFileData(MockManifestsJSON.Manifest1) },
            { Manifest2Path, new MockFileData(MockManifestsJSON.Manifest2) },
            { Manifest3Path, new MockFileData(MockManifestsJSON.Manifest3) },
            { Manifest4Path, new MockFileData(new byte[] { 0x00 }) },
        }
    );

    [Fact]
    public void Test_Parse_Manifest_Minimal()
    {
        Mod mod = new Mod(Path.Combine("C:", "Game", "Mods", "Mod1"), _fileSystem);
        Assert.Equal("Mod1", mod.Manifest.Name);
        Assert.Equal("MockDev.Mods.Mod1", mod.Manifest.ID);
        Assert.Empty(mod.Manifest.Author);
        Assert.Empty(mod.Manifest.Version);
        Assert.Empty(mod.Manifest.Description);
        Assert.Empty(mod.Manifest.Website);
        Assert.Empty(mod.Manifest.Dependencies);
        Assert.Empty(mod.Manifest.Incompatibilities);
        Assert.Empty(mod.Manifest.LoadAfter);
    }

    [Fact]
    public void Test_Parse_Manifest_Full()
    {
        Mod mod = new Mod(Path.Combine("C:", "Game", "Mods", "Mod2"), _fileSystem);
        Assert.Equal("Mod2", mod.Manifest.Name);
        Assert.Equal("MockDev.Mods.Mod2", mod.Manifest.ID);
        Assert.Equal("MockDev", mod.Manifest.Author);
        Assert.Equal("1.0.0", mod.Manifest.Version);
        Assert.Equal("This is a mock mod", mod.Manifest.Description);
        Assert.Equal("https://www.mockdev.com", mod.Manifest.Website);
        Assert.Equal(new string[] { "MockDev.Mods.Mod1" }, mod.Manifest.Dependencies);
        Assert.Equal(
            new string[] { "OtherDev.Mods.IncompatibleMess" },
            mod.Manifest.Incompatibilities
        );
        Assert.Equal(new string[] { "Core.Wrappers.Harmony" }, mod.Manifest.LoadAfter);
    }

    [Fact]
    public void Test_Parse_Manifest_Invalid()
    {
        Assert.Throws<FormatException>(
            () => new Mod(Path.Combine("C:", "Game", "Mods", "Mod3"), _fileSystem)
        );
    }

    [Fact]
    public void Test_Parse_Manifest_NoManifest()
    {
        Assert.Throws<InvalidOperationException>(
            () => new Mod(Path.Combine("C:", "Game", "Mods", "Mod4"), _fileSystem)
        );
    }
}

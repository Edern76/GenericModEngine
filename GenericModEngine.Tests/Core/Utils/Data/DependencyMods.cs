using GenericModEngine.Core.Types;

namespace GenericModEngine.Tests.Core.Utils.Data;

public class DependencyMods
{
    public static readonly Mod Dependency1 = new Mod("_", new ModManifest() { ID = "Dependency1" });
    public static readonly Mod Dependency2 = new Mod("_", new ModManifest() { ID = "Dependency2" });
    public static readonly Mod Dependency3 = new Mod("_", new ModManifest() { ID = "Dependency3" });
    public static readonly Mod Dependency4 = new Mod("_", new ModManifest() { ID = "Dependency4" });
    public static readonly Mod Dependent1 = new Mod("_", new ModManifest() { ID = "Dependent1", Dependencies = new string[] { "Dependency1", } });
    public static readonly Mod Dependent2 = new Mod("_", new ModManifest() { ID = "Dependent2", Dependencies = new string[] { "Dependency2", "Dependency3"} });
    public static readonly Mod Dependent3 = new Mod("_", new ModManifest() { ID = "Dependent3", Dependencies = new string[] { "Dependency4" } });
    public static readonly Mod Dependent4 = new Mod("_", new ModManifest() { ID = "Dependent4", Dependencies = new string[] { "Dependent1"}, LoadAfter = new string[] { "Dependent1" } });
    
    public static readonly Mod CircularDependency1 = new Mod("_", new ModManifest() { ID = "CircularDependency1", Dependencies = new string[] { "CircularDependency2" } });
    public static readonly Mod CircularDependency2 = new Mod("_", new ModManifest() { ID = "CircularDependency2", Dependencies = new string[] { "CircularDependency1" } });
}
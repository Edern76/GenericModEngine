using GenericModEngine.Core.Types;

namespace GenericModEngine.Tests.Core.Utils.Data;

public static class LoadAfterMods
{
    public static readonly Mod Basic1 = new Mod("_", new ModManifest() { ID = "Basic1" });
    public static readonly Mod Basic2 = new Mod("_", new ModManifest() { ID = "Basic2" });
    public static readonly Mod Before1 = new Mod("_", new ModManifest() { ID = "Before1" });
    public static readonly Mod Before2 = new Mod("_", new ModManifest() { ID = "Before2" });
    public static readonly Mod Before3 = new Mod("_", new ModManifest() { ID = "Before3" });
    public static readonly Mod Before3AfterBefore1 = new Mod("_", new ModManifest() { ID = "Before3", LoadAfter = new string[] { "Before1" } });
    public static readonly Mod AfterBefore2 = new Mod("_", new ModManifest() { ID = "After2", LoadAfter = new string[] { "Before2" } });
    public static readonly Mod AfterBefore3 = new Mod("_", new ModManifest() { ID = "After3", LoadAfter = new string[] { "Before3" } });
    public static readonly Mod AfterAfterBefore3 = new Mod("_", new ModManifest() { ID = "After4", LoadAfter = new string[] { "After3" } });
    public static readonly Mod AfterAfterBefore2 = new Mod("_", new ModManifest() { ID = "After5", LoadAfter = new string[] { "After2" } });
    public static readonly Mod AfterBasic1AndBefore2 = new Mod("_", new ModManifest() { ID = "After6", LoadAfter = new string[] { "Basic1", "Before2" } });

    public static readonly Mod AfterAfterAfterBefore3 =
        new Mod("_", new ModManifest() { ID = "After7", LoadAfter = new string[] { "After4" } });
}
using GenericModEngine.Core.Types;

namespace GenericModEngine.Tests.Core.Utils.Data;

public static class CompatibilityMods
{
    public static readonly Mod Compatible1 = new Mod("_", new ModManifest() { ID = "Compatible1" });
    public static readonly Mod Compatible2 = new Mod("_", new ModManifest() { ID = "Compatible2" });
    public static readonly Mod Incompatible1 = new Mod("_", new ModManifest() { ID = "Incompatible1", Incompatibilities = new string[] { "Compatible1" } });
    public static readonly Mod Incompatible2 = new Mod("_", new ModManifest() { ID = "Incompatible2", Incompatibilities = new string[] { "Compatible2" } });
}
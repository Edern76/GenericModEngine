using Newtonsoft.Json;

namespace GenericModEngine.Core.Types;

public class Mod
{
    public ModManifest Manifest { get; init; }
    public string BasePath { get; init; }

    public string GetAboutPath() => Path.Join(BasePath, "About");
    public string GetImagePath() => Path.Join(GetAboutPath(), "Cover.png");
    public string GetAssembliesFolder => Path.Join(BasePath, "Assemblies");
    public string GetDataFolder => Path.Join(BasePath, "Data");
    public string GetPatchesFolder => Path.Join(BasePath, "Patches");
    public string GetAssetFolder => Path.Join(BasePath, "Assets");

    public Mod(string basePath)
    {
        this.BasePath = basePath;
        string manifestPath = Path.Join(basePath, "manifest.json");
        if (!File.Exists(manifestPath))
            throw new InvalidOperationException($"Failed to find manifest at {manifestPath}");
        this.Manifest = JsonConvert.DeserializeObject<ModManifest>(File.ReadAllText(manifestPath)) ?? throw new InvalidOperationException($"Failed to load manifest from {manifestPath}");
    }

    // Only for testing
    internal Mod(string basePath, ModManifest manifest)
    {
        this.BasePath = basePath;
        this.Manifest = manifest;
    }

    public bool IsCompatibleWith(Mod otherMod)
    {
        return !Manifest.Incompatibilities.Contains(otherMod.Manifest.ID);
    }
}
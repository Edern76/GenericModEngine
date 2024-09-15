using GenericModEngine.Core.Types.Assemblies;
using Newtonsoft.Json;

namespace GenericModEngine.Core.Types;

public class Mod
{
    public ModManifest Manifest { get; init; }
    public string BasePath { get; init; }

    public List<UnloadedAssemblyWrapper> UnloadedAssemblies { get; init; } =
        new List<UnloadedAssemblyWrapper>();

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
        this.Manifest =
            JsonConvert.DeserializeObject<ModManifest>(File.ReadAllText(manifestPath))
            ?? throw new InvalidOperationException($"Failed to load manifest from {manifestPath}");
        if (Directory.Exists(GetAssembliesFolder))
        {
            DirectoryInfo assembliesFolder = new DirectoryInfo(GetAssembliesFolder);
            List<FileInfo> files = new List<FileInfo>(
                assembliesFolder.GetFiles("*.dll", SearchOption.TopDirectoryOnly)
            );
            UnloadedAssemblies.AddRange(files.Select(f => new UnloadedAssemblyWrapper(f.FullName)));
        }
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

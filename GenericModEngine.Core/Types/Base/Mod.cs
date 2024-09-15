using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using GenericModEngine.Core.Types.Assemblies;
using Newtonsoft.Json;

namespace GenericModEngine.Core.Types;

public class Mod
{
    private readonly IFileSystem _fileSystem;
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
        : this(basePath, new FileSystem()) { }

    public Mod(string basePath, IFileSystem fileSystem)
    {
        this.BasePath = basePath;
        this._fileSystem = fileSystem;
        string manifestPath = Path.Join(GetAboutPath(), "manifest.json");
        if (!_fileSystem.File.Exists(manifestPath))
        {
            throw new InvalidOperationException($"Failed to find manifest at {manifestPath}");
        }
        this.Manifest =
            JsonConvert.DeserializeObject<ModManifest>(_fileSystem.File.ReadAllText(manifestPath))
            ?? throw new InvalidOperationException($"Failed to load manifest from {manifestPath}");
        if (String.IsNullOrEmpty(Manifest.ID) || String.IsNullOrEmpty(Manifest.Name))
        {
            throw new FormatException($"Manifest at {manifestPath} is invalid");
        }
        if (fileSystem.Directory.Exists(GetAssembliesFolder))
        {
            IDirectoryInfo assembliesFolder = fileSystem.DirectoryInfo.New(GetAssembliesFolder);
            List<IFileInfo> files = new List<IFileInfo>(
                assembliesFolder.GetFiles("*.dll", SearchOption.TopDirectoryOnly)
            );
            bool mock = fileSystem is MockFileSystem;
            UnloadedAssemblies.AddRange(
                files.Select(f => new UnloadedAssemblyWrapper(f.FullName, mock))
            );
        }
    }

    // Only for testing
    internal Mod(string basePath, ModManifest manifest)
    {
        this.BasePath = basePath;
        this.Manifest = manifest;
        this._fileSystem = new FileSystem();
    }

    public bool IsCompatibleWith(Mod otherMod)
    {
        return !Manifest.Incompatibilities.Contains(otherMod.Manifest.ID);
    }
}

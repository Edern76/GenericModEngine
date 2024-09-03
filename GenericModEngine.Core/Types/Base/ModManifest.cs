namespace GenericModEngine.Core.Types;

public record ModManifest()
{
    public string Name { get; init; }
    public string ID { get; init; }
    public string Author { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Website { get; init; } = string.Empty;
    public string[] Dependencies { get; init; } = Array.Empty<string>();
    public string[] Incompatibilities { get; init; } = Array.Empty<string>();
    public string[] LoadAfter { get; init; } = Array.Empty<string>();
};
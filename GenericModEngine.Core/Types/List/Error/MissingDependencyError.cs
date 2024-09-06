namespace GenericModEngine.Core.Types.Error;

public class MissingDependencyError : IModListError
{
    public string Source { get; init; }
    public string? Target { get; init; }
    
    public ModListErrorSeverity Severity => ModListErrorSeverity.FIXABLE;

    public MissingDependencyError(string source, string? target)
    {
        this.Source = source;
        this.Target = target;
    }
    
    public string ToString() => $"{Source} is missing dependency {Target}";
}
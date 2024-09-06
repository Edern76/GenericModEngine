namespace GenericModEngine.Core.Types.Error;

public class CircularLoadAfterError : IModListError
{
    public string Source { get; init; }
    public string? Target { get; init; }
    
    public ModListErrorSeverity Severity => ModListErrorSeverity.FATAL;

    public CircularLoadAfterError(string source, string? target)
    {
        this.Source = source;
        this.Target = target;
    }
    
    public string ToString() => $"{Source} needs to be loaded after {Target} which also needs to be loaded after the former. The mod engine cannot therefore load either of them.";
    
}
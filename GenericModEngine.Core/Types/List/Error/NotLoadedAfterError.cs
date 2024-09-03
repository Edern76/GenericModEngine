namespace GenericModEngine.Core.Types.Error;

public class NotLoadedAfterError : IModListError
{
    public string Source { get; init; }
    public string? Target { get; init; }

    public NotLoadedAfterError(string source, string? target)
    {
        this.Source = source;
        this.Target = target;
    }
    
    public string ToString() => $"{Source} is not loaded after {Target}";
}